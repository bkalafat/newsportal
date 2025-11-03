import axios, { AxiosInstance, AxiosError } from "axios";
import { News, CreateNewsDto, UpdateNewsDto, ApiError } from "./types";

/**
 * News API Client
 * Handles all HTTP requests to the News API backend
 */
class NewsApiClient {
  private client: AxiosInstance;

  constructor() {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";
    const basePath = process.env.NEXT_PUBLIC_API_BASE_PATH || "/api/NewsArticle";
    const baseURL = `${apiUrl}${basePath}`;

    this.client = axios.create({
      baseURL,
      headers: {
        "Content-Type": "application/json",
      },
      timeout: 90000, // 90 seconds - allows time for Azure Container Apps to scale from zero
      withCredentials: false, // Ensure CORS preflight works correctly
    });

    // Response interceptor for error handling
    this.client.interceptors.response.use(
      (response) => response,
      async (error: AxiosError<ApiError>) => {
        // Handle 503 Service Unavailable (Azure Container Apps cold start)
        if (error.response?.status === 503) {
          const apiError: ApiError = {
            message: "Backend is starting up (cold start). This usually takes 10-30 seconds. Please refresh the page.",
            statusCode: 503,
            errors: error.response?.data?.errors,
          };
          console.warn("Backend is scaling from zero. Cold start detected.");
          return Promise.reject(apiError);
        }

        // Handle network errors (CORS, connection refused, etc.)
        if (!error.response) {
          const apiError: ApiError = {
            message: error.code === "ECONNABORTED" 
              ? "Request timeout. Backend might be starting up or experiencing high load."
              : "Unable to connect to backend. Please check your internet connection or try again later.",
            statusCode: 0,
            errors: undefined, // No response means no error details
          };
          console.error("Network error:", error.message);
          return Promise.reject(apiError);
        }

        const apiError: ApiError = {
          message: error.response?.data?.message || error.message || "An error occurred",
          statusCode: error.response?.status || 500,
          errors: error.response?.data?.errors,
        };
        return Promise.reject(apiError);
      }
    );
  }

  /**
   * Get all news articles
   */
  async getAllNews(): Promise<News[]> {
    const response = await this.client.get<News[]>("/");
    // Backend returns array directly
    const data = Array.isArray(response.data) ? response.data : [];
    return data.map(this.parseNewsDate);
  }

  /**
   * Get a single news article by ID
   */
  async getNewsById(id: string): Promise<News> {
    const response = await this.client.get<News>(`/${id}`);
    return this.parseNewsDate(response.data);
  }

  /**
   * Create a new news article
   * Note: This might require authentication in the backend
   */
  async createNews(data: CreateNewsDto): Promise<News> {
    const response = await this.client.post<News>("/", data);
    return this.parseNewsDate(response.data);
  }

  /**
   * Update an existing news article
   * Note: This might require authentication in the backend
   */
  async updateNews(id: string, data: UpdateNewsDto): Promise<News> {
    const response = await this.client.put<News>(`/${id}`, data);
    return this.parseNewsDate(response.data);
  }

  /**
   * Delete a news article
   * Note: This might require authentication in the backend
   */
  async deleteNews(id: string): Promise<void> {
    await this.client.delete(`/${id}`);
  }

  /**
   * Get news by category
   */
  async getNewsByCategory(category: string): Promise<News[]> {
    // Lowercase to match backend's lowercase categories
    // (popular, artificialintelligence, githubcopilot, mcp, openai, robotics, deepseek, dotnet, claudeai)
    const lowercaseCategory = category.toLowerCase();
    const response = await this.client.get<News[]>(`/?category=${lowercaseCategory}`);
    // Backend returns array directly
    const data = Array.isArray(response.data) ? response.data : [];
    return data.map(this.parseNewsDate);
  }

  /**
   * Parse date strings to Date objects
   * Handles invalid dates gracefully
   */
  private parseNewsDate(news: News): News {
    let publishedAt: Date;

    try {
      // Backend returns expressDate, fallback to publishedAt for backward compatibility
      const dateValue = news.expressDate || news.publishedAt;
      if (!dateValue) {
        // If no date provided, use current date
        publishedAt = new Date();
      } else if (dateValue instanceof Date) {
        // Already a Date object
        publishedAt = dateValue;
      } else {
        // Parse string date
        publishedAt = new Date(dateValue);
        // Check if date is valid
        if (isNaN(publishedAt.getTime())) {
          publishedAt = new Date();
        }
      }
    } catch {
      // Fallback to current date if parsing fails
      publishedAt = new Date();
    }

    return {
      ...news,
      publishedAt,
    };
  }

  /**
   * Get API health status
   */
  async getHealth(): Promise<{ status: string }> {
    const healthUrl = `${process.env.NEXT_PUBLIC_API_URL}/health`;
    const response = await axios.get(healthUrl);
    return response.data;
  }
}

// Export a singleton instance
export const newsApi = new NewsApiClient();
