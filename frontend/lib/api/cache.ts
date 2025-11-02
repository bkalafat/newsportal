import { unstable_cache } from "next/cache";

/**
 * Server-side cache utilities for API data fetching
 * Uses Next.js unstable_cache for optimal performance
 */

interface News {
  id: string;
  category: string;
  caption: string;
  slug: string;
  summary: string;
  content: string;
  imageUrl?: string;
  thumbnailUrl?: string;
  expressDate: string;
  [key: string]: any;
}

/**
 * Fetch all news with server-side caching
 * Revalidates every 5 minutes
 */
export const getCachedAllNews = unstable_cache(
  async (): Promise<News[]> => {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";
    const response = await fetch(`${apiUrl}/api/NewsArticle`, {
      next: { revalidate: 300 }, // 5 minutes
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      console.error(`Failed to fetch news: ${response.status}`);
      return [];
    }

    const data = await response.json();
    return Array.isArray(data) ? data : [];
  },
  ["all-news"],
  {
    revalidate: 300, // 5 minutes
    tags: ["news", "all-news"],
  }
);

/**
 * Fetch news by category with server-side caching
 * Revalidates every 10 minutes
 */
export const getCachedNewsByCategory = unstable_cache(
  async (category: string): Promise<News[]> => {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";
    const response = await fetch(
      `${apiUrl}/api/NewsArticle?category=${category.toLowerCase()}`,
      {
        next: { revalidate: 600 }, // 10 minutes
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!response.ok) {
      console.error(`Failed to fetch news by category: ${response.status}`);
      return [];
    }

    const data = await response.json();
    return Array.isArray(data) ? data : [];
  },
  ["news-by-category"],
  {
    revalidate: 600, // 10 minutes
    tags: ["news", "news-by-category"],
  }
);

/**
 * Fetch news by slug with server-side caching
 * Revalidates every 1 hour
 */
export const getCachedNewsBySlug = unstable_cache(
  async (slug: string): Promise<News | null> => {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";
    const response = await fetch(
      `${apiUrl}/api/NewsArticle/by-slug?slug=${encodeURIComponent(slug)}`,
      {
        next: { revalidate: 3600 }, // 1 hour
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (!response.ok) {
      console.error(`Failed to fetch news by slug: ${response.status}`);
      return null;
    }

    return await response.json();
  },
  ["news-by-slug"],
  {
    revalidate: 3600, // 1 hour
    tags: ["news", "news-by-slug"],
  }
);

/**
 * Fetch trending news with server-side caching
 * Revalidates every 15 minutes
 */
export const getCachedTrendingNews = unstable_cache(
  async (): Promise<News[]> => {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";
    const response = await fetch(`${apiUrl}/api/NewsArticle/trending`, {
      next: { revalidate: 900 }, // 15 minutes
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      console.error(`Failed to fetch trending news: ${response.status}`);
      return [];
    }

    const data = await response.json();
    return Array.isArray(data) ? data : [];
  },
  ["trending-news"],
  {
    revalidate: 900, // 15 minutes
    tags: ["news", "trending"],
  }
);

/**
 * Generate static params for top news articles
 * Used for pre-rendering pages at build time
 */
export async function getStaticNewsParams(limit: number = 100): Promise<string[]> {
  try {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";
    const response = await fetch(`${apiUrl}/api/NewsArticle?pageSize=${limit}`, {
      next: { revalidate: 86400 }, // 24 hours
    });

    if (!response.ok) return [];

    const news: News[] = await response.json();
    return news.map((item) => item.slug);
  } catch (error) {
    console.error("Error generating static params:", error);
    return [];
  }
}
