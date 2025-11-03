using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsApi.Application.Services;
using NewsApi.Infrastructure.Services;

namespace NewsApi.Infrastructure.BackgroundJobs;

/// <summary>
/// Background service that fetches social media posts daily
/// </summary>
internal sealed class SocialMediaFetcherService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SocialMediaFetcherService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24); // Run daily
    private readonly TimeSpan _startDelay = TimeSpan.FromMinutes(2); // Initial delay after startup

    public SocialMediaFetcherService(
        IServiceProvider serviceProvider,
        ILogger<SocialMediaFetcherService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Social Media Fetcher Service is starting");

        // Wait before first execution to avoid startup load
        await Task.Delay(_startDelay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Social Media Fetcher Service is running at: {Time}", DateTimeOffset.UtcNow);

                await FetchAndStoreSocialMediaPostsAsync(stoppingToken);

                _logger.LogInformation("Social Media Fetcher Service completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching social media posts");
            }

            // Wait for next interval
            _logger.LogInformation("Next run scheduled in {Hours} hours", _interval.TotalHours);
            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Social Media Fetcher Service is stopping");
    }

    private async Task FetchAndStoreSocialMediaPostsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var redditService = scope.ServiceProvider.GetRequiredService<RedditService>();
        var socialMediaService = scope.ServiceProvider.GetRequiredService<ISocialMediaPostService>();
        var translationService = scope.ServiceProvider.GetRequiredService<TranslationService>();

        int totalImported = 0;
        int totalSkipped = 0;
        int totalFiltered = 0;

        // Fetch from multiple subreddits
        var subreddits = new[]
        {
            // GitHub Copilot
            ("github", "copilot", "GitHub Copilot discussions"),
            ("programming", "copilot OR \"github copilot\"", "Programming community"),
            ("webdev", "copilot", "Web development"),
            
            // Artificial Intelligence
            ("artificial", string.Empty, "AI general discussions"),
            ("MachineLearning", string.Empty, "Machine Learning community"),
            ("ArtificialInteligence", string.Empty, "AI specific community"),
            ("singularity", string.Empty, "AI singularity discussions"),
            
            // OpenAI
            ("OpenAI", string.Empty, "OpenAI official community"),
            ("ChatGPT", string.Empty, "ChatGPT discussions"),
            ("ChatGPTCoding", string.Empty, "ChatGPT for coding"),
            
            // Claude AI
            ("ClaudeAI", string.Empty, "Claude AI discussions"),
            ("Anthropic", string.Empty, "Anthropic/Claude community"),
            
            // General AI/LLM
            ("LocalLLaMA", string.Empty, "Local LLM discussions"),
            ("Oobabooga", string.Empty, "LLM tools and models"),
        };

        foreach (var (subreddit, query, description) in subreddits)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                List<CreateSocialMediaPostDto> posts;

                if (string.IsNullOrEmpty(query))
                {
                    _logger.LogInformation("Fetching top posts from r/{Subreddit}", subreddit);
                    posts = await redditService.GetTopPostsAsync(
                        subreddit,
                        "day", // Last 24 hours
                        25);
                }
                else
                {
                    _logger.LogInformation("Fetching posts from r/{Subreddit} about '{Query}'", subreddit, query);
                    posts = await redditService.SearchPostsAsync(
                        subreddit,
                        query,
                        "top",
                        "day", // Last 24 hours
                        25);
                }

                _logger.LogInformation("Fetched {Count} posts from r/{Subreddit}", posts.Count, subreddit);

                foreach (var post in posts)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        // Check if content is Turkish
                        var isTitleTurkish = translationService.IsTurkish(post.Title);
                        var isContentTurkish = string.IsNullOrEmpty(post.Content) || translationService.IsTurkish(post.Content);

                        // Skip if content is English (not Turkish)
                        if (!isTitleTurkish && !isContentTurkish)
                        {
                            totalFiltered++;
                            _logger.LogDebug("Filtered English post: {Title}", post.Title);
                            continue;
                        }

                        // Translate if needed
                        var translatedPost = post;
                        if (!isTitleTurkish)
                        {
                            var translatedTitle = await translationService.TranslateToTurkishAsync(post.Title);
                            translatedPost = post with { Title = translatedTitle, Language = "tr" };
                        }

                        if (!string.IsNullOrEmpty(post.Content) && !isContentTurkish)
                        {
                            var translatedContent = await translationService.TranslateToTurkishAsync(post.Content);
                            translatedPost = translatedPost with { Content = translatedContent, Language = "tr" };
                        }

                        await socialMediaService.CreatePostAsync(translatedPost);
                        totalImported++;
                        _logger.LogDebug("Imported post: {Title} from r/{Subreddit}", translatedPost.Title, subreddit);
                    }
                    catch (InvalidOperationException ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
                    {
                        totalSkipped++;
                        _logger.LogDebug("Skipped duplicate post: {ExternalId}", post.ExternalId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to import post {ExternalId} from r/{Subreddit}",
                            post.ExternalId, subreddit);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching posts from r/{Subreddit}", subreddit);
            }
        }

        _logger.LogInformation(
            "Social media fetch completed: {Imported} posts imported, {Skipped} skipped, {Filtered} English posts filtered",
            totalImported, totalSkipped, totalFiltered);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Social Media Fetcher Service is stopping gracefully");
        return base.StopAsync(cancellationToken);
    }
}
