using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsApi.Application.DTOs;

namespace NewsApi.Infrastructure.Services;

/// <summary>
/// Service for fetching news data from external NewsAPI.org
/// </summary>
public interface INewsDataFetcherService
{
    /// <summary>
    /// Fetches latest news articles from NewsAPI.org
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of news articles as DTOs ready to be saved</returns>
    Task<List<CreateNewsArticleDto>> FetchLatestNewsAsync(CancellationToken cancellationToken = default);
}

internal sealed class NewsDataFetcherService : INewsDataFetcherService
{
    private readonly HttpClient _httpClient;
    private readonly NewsApiSettings _settings;
    private readonly ILogger<NewsDataFetcherService> _logger;
    private readonly TranslationService _translationService;

    public NewsDataFetcherService(
        HttpClient httpClient,
        IOptions<NewsApiSettings> settings,
        ILogger<NewsDataFetcherService> logger,
        TranslationService translationService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _translationService = translationService ?? throw new ArgumentNullException(nameof(translationService));
    }

    public async Task<List<CreateNewsArticleDto>> FetchLatestNewsAsync(CancellationToken cancellationToken = default)
    {
        var allArticles = new List<CreateNewsArticleDto>();

        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            _logger.LogWarning("NewsAPI ApiKey is not configured. Skipping news fetch.");
            return allArticles;
        }

        foreach (var category in _settings.Categories)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                _logger.LogInformation("Fetching {Category} news from NewsAPI.org", category);

                var articles = await FetchNewsByCategoryAsync(category, cancellationToken);
                allArticles.AddRange(articles);

                _logger.LogInformation("Fetched {Count} articles for {Category}", articles.Count, category);

                // Rate limiting: Wait 1 second between requests to avoid API throttling
                await Task.Delay(1000, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news for category {Category}", category);
            }
        }

        return allArticles;
    }

    private async Task<List<CreateNewsArticleDto>> FetchNewsByCategoryAsync(
        string category,
        CancellationToken cancellationToken)
    {
        var articles = new List<CreateNewsArticleDto>();

        try
        {
            // Build request URL for top headlines
            var country = _settings.Countries.FirstOrDefault() ?? "tr";
            var url = $"{_settings.BaseUrl}/top-headlines?country={country}&category={category}&pageSize={_settings.MaxArticlesPerCategory}&apiKey={_settings.ApiKey}";

            var response = await _httpClient.GetAsync(new Uri(url), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "NewsAPI returned status {StatusCode} for category {Category}",
                    response.StatusCode, category);
                return articles;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<NewsApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            if (result?.Articles == null || result.Articles.Length == 0)
            {
                _logger.LogInformation("No articles found for category {Category}", category);
                return articles;
            }

            // Convert NewsAPI articles to our DTOs and filter Turkish only
            foreach (var article in result.Articles)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    // Convert article to DTO and translate to Turkish if needed
                    var dto = await MapToCreateDtoAsync(article, category, cancellationToken);
                    if (dto != null)
                    {
                        articles.Add(dto);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to map article: {Title}", article.Title);
                }
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching news for category {Category}", category);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for category {Category}", category);
        }

        return articles;
    }

    private async Task<CreateNewsArticleDto?> MapToCreateDtoAsync(
        NewsApiArticle article,
        string category,
        CancellationToken cancellationToken)
    {
        // Skip articles without title or content
        if (string.IsNullOrWhiteSpace(article.Title) ||
            string.IsNullOrWhiteSpace(article.Description) ||
            string.Equals(article.Title, "[Removed]", StringComparison.Ordinal))
        {
            return null;
        }

        // Detect language and translate to Turkish if needed
        var sourceLanguage = _translationService.DetectLanguage(article.Title);
        string turkishTitle = article.Title;
        string turkishDescription = article.Description ?? article.Title;
        string turkishContent = article.Content ?? article.Description ?? article.Title;

        // Always translate non-Turkish content to Turkish
        if (sourceLanguage != "tr")
        {
            try
            {
                _logger.LogInformation("Translating article to Turkish: {Title}", article.Title);
                
                turkishTitle = await _translationService.TranslateToTurkishAsync(article.Title, sourceLanguage);
                
                if (!string.IsNullOrEmpty(article.Description))
                {
                    turkishDescription = await _translationService.TranslateToTurkishAsync(article.Description, sourceLanguage);
                }
                
                if (!string.IsNullOrEmpty(article.Content))
                {
                    turkishContent = await _translationService.TranslateToTurkishAsync(article.Content, sourceLanguage);
                }

                // Validate translation produced Turkish content
                if (!_translationService.IsTurkish(turkishTitle))
                {
                    _logger.LogWarning("Translation failed to produce Turkish content for: {Title}", article.Title);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Translation failed for article: {Title}", article.Title);
                return null;
            }
        }

        // Map NewsAPI category to our internal categories
        var mappedCategory = MapCategory(category);

        return new CreateNewsArticleDto
        {
            Category = mappedCategory,
            Type = "Genel",
            Caption = TruncateString(turkishTitle, 500),
            Keywords = string.Join(", ", ExtractKeywords(turkishTitle)),
            SocialTags = string.Empty,
            Summary = TruncateString(turkishDescription, 2000),
            ImgPath = string.Empty,
            ImgAlt = TruncateString(turkishTitle, 200),
            ImageUrl = article.UrlToImage ?? string.Empty,
            ThumbnailUrl = article.UrlToImage ?? string.Empty,
            Content = BuildTurkishContent(turkishContent, article),
            Subjects = ExtractKeywords(turkishTitle),
            Authors = string.IsNullOrWhiteSpace(article.Author)
                ? ["NewsAPI"]
                : [TruncateString(article.Author, 100)],
            ExpressDate = article.PublishedAt != default
                ? article.PublishedAt
                : DateTime.UtcNow,
            Priority = 5,
            IsActive = true,
            IsSecondPageNews = false,
        };
    }

    private static string BuildTurkishContent(string translatedContent, NewsApiArticle article)
    {
        var content = translatedContent;

        // NewsAPI often truncates content with [+X chars], so add source link
        var sourceLink = $"\n\n<p>Kaynak: <a href=\"{article.Url}\" target=\"_blank\" rel=\"noopener noreferrer\">{article.Source?.Name ?? "Haber Kaynağı"}</a></p>";

        return content + sourceLink;
    }

    private static string MapCategory(string newsApiCategory)
    {
        return newsApiCategory.ToLowerInvariant() switch
        {
            "technology" => "Teknoloji",
            "business" => "Ekonomi",
            "sports" => "Spor",
            "science" => "Bilim",
            "health" => "Sağlık",
            "entertainment" => "Magazin",
            "general" => "Genel",
            _ => "Genel",
        };
    }

    private static string[] ExtractKeywords(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        // Simple keyword extraction: split by common separators
        return text.Split([' ', ',', '-', ':', '|'], StringSplitOptions.RemoveEmptyEntries)
            .Where(w => w.Length > 3)
            .Take(5)
            .ToArray();
    }

    private static string TruncateString(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        return text.Length <= maxLength
            ? text
            : string.Concat(text.AsSpan(0, maxLength - 3), "...");
    }
}

/// <summary>
/// Response model from NewsAPI.org
/// </summary>
internal sealed class NewsApiResponse
{
    public string Status { get; set; } = string.Empty;

    public int TotalResults { get; set; }

    public NewsApiArticle[] Articles { get; set; } = [];
}

/// <summary>
/// Article model from NewsAPI.org
/// </summary>
internal sealed class NewsApiArticle
{
    public NewsApiSource? Source { get; set; }

    public string Author { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string UrlToImage { get; set; } = string.Empty;

    public DateTime PublishedAt { get; set; }

    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// Source model from NewsAPI.org
/// </summary>
internal sealed class NewsApiSource
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
