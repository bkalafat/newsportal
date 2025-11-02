"use client";

import { useMemo } from "react";
import { useTranslations } from "next-intl";
import { NewsCategory } from "@/lib/api/types";
import { Card, CardHeader, CardTitle } from "@/components/ui/card";
import Link from "next/link";
import {
  Cpu,
  TrendingUp,
} from "lucide-react";
import { useAllNews } from "@/lib/api/hooks";

const categoryIcons = {
  [NewsCategory.Popular]: TrendingUp,
  [NewsCategory.ArtificialIntelligence]: Cpu,
  [NewsCategory.GithubCopilot]: Cpu,
  [NewsCategory.MCP]: Cpu,
  [NewsCategory.OpenAI]: Cpu,
  [NewsCategory.Robotics]: Cpu,
  [NewsCategory.DeepSeek]: Cpu,
  [NewsCategory.DotNet]: Cpu,
  [NewsCategory.ClaudeAI]: Cpu,
};

export function CategoriesSection() {
  const t = useTranslations("categories");
  const { data: news } = useAllNews();

  // All available categories in priority order
  const allCategories = [
    NewsCategory.Popular,
    NewsCategory.ArtificialIntelligence,
    NewsCategory.OpenAI,
    NewsCategory.GithubCopilot,
    NewsCategory.ClaudeAI,
    NewsCategory.DotNet,
    NewsCategory.MCP,
    NewsCategory.Robotics,
    NewsCategory.DeepSeek,
  ];

  // Filter categories that have at least one news item
  const categoriesWithNews = useMemo(() => {
    if (!news || news.length === 0) {
      return [];
    }

    // Count news per category
    const categoryCounts = new Map<string, number>();
    news.forEach((item) => {
      const category = item.category?.toLowerCase();
      if (category) {
        categoryCounts.set(category, (categoryCounts.get(category) || 0) + 1);
      }
    });

    // Filter categories that have news and take top 6
    return allCategories
      .filter((category) => {
        const count = categoryCounts.get(category) || 0;
        return count > 0;
      })
      .slice(0, 6);
  }, [news]);

  // Don't show section if no categories have news
  if (categoriesWithNews.length === 0) {
    return null;
  }

  return (
    <section className="space-y-6">
      <h2 className="text-3xl font-bold">{t("all")}</h2>
      <div className="grid grid-cols-2 gap-4 md:grid-cols-3 lg:grid-cols-6">
        {categoriesWithNews.map((category) => {
          const Icon = categoryIcons[category];
          return (
            <Card key={category} className="group cursor-pointer transition-shadow hover:shadow-md">
              <Link href={`/category/${category}`}>
                <CardHeader className="pb-3">
                  <div className="mb-2 flex justify-center">
                    <div className="bg-primary/10 group-hover:bg-primary/20 flex h-12 w-12 items-center justify-center rounded-full transition-colors">
                      <Icon className="text-primary h-6 w-6" />
                    </div>
                  </div>
                  <CardTitle className="text-center text-sm font-medium">{t(category)}</CardTitle>
                </CardHeader>
              </Link>
            </Card>
          );
        })}
      </div>
    </section>
  );
}
