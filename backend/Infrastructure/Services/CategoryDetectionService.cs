using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace NewsApi.Infrastructure.Services;

/// <summary>
/// Service for intelligently detecting news categories based on content, source, and engagement
/// </summary>
internal sealed class CategoryDetectionService
{
    private readonly ILogger<CategoryDetectionService> _logger;

    // Valid categories from frontend: popular, artificialintelligence, githubcopilot, mcp, openai, robotics, deepseek, dotnet, claudeai
    // Category keywords with weights
    private static readonly Dictionary<string, (string Category, List<string> Keywords, int Weight)> CategoryPatterns = new()
    {
        // OpenAI - ChatGPT, GPT models
        ["openai"] = ("openai", new List<string>
        {
            "openai", "chatgpt", "gpt-4", "gpt-5", "gpt", "sam altman",
            "dall-e", "whisper", "sora", "o1"
        }, 100),

        // Claude AI - Anthropic
        ["claudeai"] = ("claudeai", new List<string>
        {
            "claude", "anthropic", "claude ai", "claude 3", "claude sonnet",
            "claude opus", "dario amodei"
        }, 100),

        // GitHub Copilot - AI coding assistant
        ["githubcopilot"] = ("githubcopilot", new List<string>
        {
            "github copilot", "copilot", "copilot x", "copilot chat",
            "github ai", "code completion", "ai pair programming"
        }, 100),

        // General AI/ML - When specific AI service not mentioned
        ["artificialintelligence"] = ("artificialintelligence", new List<string>
        {
            "artificial intelligence", "ai", "machine learning", "ml", "deep learning",
            "neural network", "llm", "large language model", "generative ai",
            "transformer", "bert", "nlp", "computer vision", "ai model"
        }, 90),

        // Robotics
        ["robotics"] = ("robotics", new List<string>
        {
            "robot", "robotics", "automation", "autonomous", "drone",
            "tesla bot", "boston dynamics", "humanoid", "industrial robot"
        }, 95),

        // DeepSeek - Chinese AI company
        ["deepseek"] = ("deepseek", new List<string>
        {
            "deepseek", "deep seek", "deepseek ai", "deepseek coder",
            "deepseek v2", "chinese ai"
        }, 100),

        // .NET - Microsoft development platform
        ["dotnet"] = ("dotnet", new List<string>
        {
            ".net", "dotnet", "c#", "csharp", "asp.net", "blazor",
            "entity framework", "maui", ".net core", "visual studio"
        }, 95),

        // MCP - Model Context Protocol
        ["mcp"] = ("mcp", new List<string>
        {
            "mcp", "model context protocol", "context protocol",
            "llm context", "ai context"
        }, 100),
    };

    public CategoryDetectionService(ILogger<CategoryDetectionService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Detect the most appropriate category based on title, content, source, and engagement metrics
    /// </summary>
    /// <param name="title">News title</param>
    /// <param name="content">News content</param>
    /// <param name="source">News source (e.g., "Reddit - r/programming")</param>
    /// <param name="tags">Tags associated with the news</param>
    /// <param name="score">Engagement score (upvotes, likes, etc.)</param>
    /// <returns>The detected category</returns>
    public string DetectCategory(
        string title,
        string content,
        string source,
        List<string> tags,
        int score)
    {
        var categoryScores = new Dictionary<string, int>();

        // Combine all text for analysis
        var combinedText = $"{title} {content} {source} {string.Join(" ", tags)}".ToLowerInvariant();

        // Score based on keyword matching
        foreach (var (patternKey, (category, keywords, weight)) in CategoryPatterns)
        {
            var matches = keywords.Count(keyword =>
                Regex.IsMatch(combinedText, $@"\b{Regex.Escape(keyword)}\b", RegexOptions.IgnoreCase));

            if (matches > 0)
            {
                if (!categoryScores.ContainsKey(category))
                {
                    categoryScores[category] = 0;
                }
                categoryScores[category] += matches * weight;
            }
        }

        // Boost category based on source
        var sourceCategory = GetCategoryFromSource(source);
        if (!string.IsNullOrEmpty(sourceCategory) && categoryScores.ContainsKey(sourceCategory))
        {
            categoryScores[sourceCategory] += 50; // Source boost
        }

        // Boost category based on high engagement (viral content)
        if (score > 1000)
        {
            // High engagement usually indicates important tech or world news
            if (categoryScores.ContainsKey("Technology"))
            {
                categoryScores["Technology"] += 30;
            }
            if (categoryScores.ContainsKey("World"))
            {
                categoryScores["World"] += 20;
            }
        }

        // Select category with highest score
        if (categoryScores.Any())
        {
            var detectedCategory = categoryScores.OrderByDescending(x => x.Value).First().Key;
            _logger.LogDebug(
                "Detected category '{Category}' for '{Title}' (scores: {Scores})",
                detectedCategory,
                title.Length > 50 ? title[..50] + "..." : title,
                string.Join(", ", categoryScores.Select(x => $"{x.Key}:{x.Value}")));
            return detectedCategory;
        }

        // Default to popular for general tech news or when category cannot be determined
        var defaultCategory = GetDefaultCategoryFromSource(source);
        _logger.LogDebug("Using default category '{Category}' for source '{Source}'", defaultCategory, source);
        return defaultCategory;
    }

    /// <summary>
    /// Get category hint from news source
    /// </summary>
    private static string GetCategoryFromSource(string source)
    {
        var lowerSource = source.ToLowerInvariant();

        // Reddit subreddit mapping
        if (lowerSource.Contains("r/artificial") || lowerSource.Contains("r/machinelearning"))
        {
            return "artificialintelligence";
        }

        if (lowerSource.Contains("r/openai"))
        {
            return "openai";
        }

        if (lowerSource.Contains("r/claudeai"))
        {
            return "claudeai";
        }

        if (lowerSource.Contains("r/github") || lowerSource.Contains("github trending"))
        {
            return "githubcopilot";
        }

        if (lowerSource.Contains("r/dotnet") || lowerSource.Contains("r/csharp"))
        {
            return "dotnet";
        }

        if (lowerSource.Contains("r/robotics"))
        {
            return "robotics";
        }

        return string.Empty;
    }

    /// <summary>
    /// Get default category when no strong signals detected
    /// </summary>
    private static string GetDefaultCategoryFromSource(string source)
    {
        var lowerSource = source.ToLowerInvariant();

        // Check for specific sources
        if (lowerSource.Contains("openai") || lowerSource.Contains("chatgpt"))
        {
            return "openai";
        }

        if (lowerSource.Contains("anthropic") || lowerSource.Contains("claude"))
        {
            return "claudeai";
        }

        if (lowerSource.Contains("github copilot") || lowerSource.Contains("copilot"))
        {
            return "githubcopilot";
        }

        // For most tech sources, default to popular (general tech news)
        return "popular";
    }

    /// <summary>
    /// Get top trending categories from a list of aggregated news items
    /// </summary>
    public Dictionary<string, int> GetTrendingCategories(IEnumerable<AggregatedNewsItem> newsItems)
    {
        var categoryEngagement = new Dictionary<string, int>();

        foreach (var item in newsItems)
        {
            var category = DetectCategory(item.Title, item.Content, item.Source, item.Tags, item.Score);

            if (!categoryEngagement.ContainsKey(category))
            {
                categoryEngagement[category] = 0;
            }
            categoryEngagement[category] += item.Score;
        }

        return categoryEngagement.OrderByDescending(x => x.Value)
            .ToDictionary(x => x.Key, x => x.Value);
    }
}
