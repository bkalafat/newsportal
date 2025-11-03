import { Metadata } from "next";
import Link from "next/link";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Cpu,
  TrendingUp,
  ArrowRight,
  Sparkles,
  Bot,
  Brain,
  Github,
  Code2,
} from "lucide-react";
import { Header } from "@/components/layout/header";
import { Footer } from "@/components/layout/footer";

export const metadata: Metadata = {
  title: "Kategoriler - Teknoloji Haberleri",
  description:
    "Yapay zeka, GitHub Copilot, OpenAI, robotik ve daha fazla teknoloji kategorisini keşfedin.",
  keywords: [
    "yapay zeka",
    "github copilot",
    "openai",
    "claude ai",
    "deepseek",
    "robotik",
    "dotnet",
    "mcp",
    "teknoloji haberleri",
  ],
  alternates: {
    canonical: "/categories",
  },
  openGraph: {
    title: "Kategoriler - Teknoloji Haberleri",
    description:
      "Yapay zeka, GitHub Copilot ve teknoloji haberlerini kategorilere göre keşfedin.",
    type: "website",
    url: "/categories",
  },
  twitter: {
    card: "summary",
    title: "Kategoriler - Teknoloji Haberleri",
    description: "Yapay zeka ve teknoloji haberlerini keşfedin.",
  },
};

// Force revalidation on every request to show updated categories
export const revalidate = 0; // Disable ISR cache

const categories = [
  {
    id: "popular",
    icon: TrendingUp,
    color: "text-rose-600 dark:text-rose-400",
    bgColor: "bg-rose-50 dark:bg-rose-950",
    description: "En popüler ve trend olan haberler",
    displayName: "Popüler",
  },
  {
    id: "artificialintelligence",
    icon: Brain,
    color: "text-purple-600 dark:text-purple-400",
    bgColor: "bg-purple-50 dark:bg-purple-950",
    description: "Yapay zeka ve makine öğrenimi haberleri",
    displayName: "Yapay Zeka",
  },
  {
    id: "githubcopilot",
    icon: Github,
    color: "text-gray-800 dark:text-gray-300",
    bgColor: "bg-gray-50 dark:bg-gray-950",
    description: "GitHub Copilot güncellemeleri ve haberleri",
    displayName: "GitHub Copilot",
  },
  {
    id: "openai",
    icon: Sparkles,
    color: "text-emerald-600 dark:text-emerald-400",
    bgColor: "bg-emerald-50 dark:bg-emerald-950",
    description: "OpenAI, ChatGPT ve GPT modelleri",
    displayName: "OpenAI",
  },
  {
    id: "claudeai",
    icon: Bot,
    color: "text-orange-600 dark:text-orange-400",
    bgColor: "bg-orange-50 dark:bg-orange-950",
    description: "Anthropic Claude AI haberleri",
    displayName: "Claude AI",
  },
  {
    id: "dotnet",
    icon: Code2,
    color: "text-indigo-600 dark:text-indigo-400",
    bgColor: "bg-indigo-50 dark:bg-indigo-950",
    description: ".NET framework ve C# geliştirme",
    displayName: ".NET",
  },
  {
    id: "mcp",
    icon: Cpu,
    color: "text-cyan-600 dark:text-cyan-400",
    bgColor: "bg-cyan-50 dark:bg-cyan-950",
    description: "Model Context Protocol haberleri",
    displayName: "MCP",
  },
  {
    id: "robotics",
    icon: Bot,
    color: "text-blue-600 dark:text-blue-400",
    bgColor: "bg-blue-50 dark:bg-blue-950",
    description: "Robotik ve otomasyon sistemleri",
    displayName: "Robotik",
  },
  {
    id: "deepseek",
    icon: Sparkles,
    color: "text-violet-600 dark:text-violet-400",
    bgColor: "bg-violet-50 dark:bg-violet-950",
    description: "DeepSeek AI modelleri ve haberleri",
    displayName: "DeepSeek",
  },
];

export default function CategoriesPage() {
  return (
    <div className="flex min-h-screen flex-col">
      <Header />
      <main className="flex-1">
        <div className="container mx-auto max-w-6xl px-4 py-12">
          {/* Header */}
          <div className="mb-12 text-center">
            <div className="mb-4 flex items-center justify-center gap-2">
              <TrendingUp className="text-primary h-8 w-8" />
              <h1 className="text-4xl font-bold md:text-5xl">Kategoriler</h1>
            </div>
            <p className="text-muted-foreground mx-auto max-w-2xl text-lg">
              Yapay zeka, GitHub Copilot ve teknoloji haberlerini kategorilere göre keşfedin
            </p>
          </div>

          {/* Categories Grid */}
          <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
            {categories.map((category) => {
              const Icon = category.icon;
              return (
                <Link key={category.id} href={`/category/${category.id}`} className="group">
                  <Card className="h-full cursor-pointer transition-all duration-300 hover:-translate-y-1 hover:shadow-lg">
                    <CardHeader>
                      <div
                        className={`h-16 w-16 rounded-lg ${category.bgColor} mb-4 flex items-center justify-center transition-transform group-hover:scale-110`}
                      >
                        <Icon className={`h-8 w-8 ${category.color}`} />
                      </div>
                      <div className="flex items-center justify-between">
                        <CardTitle className="capitalize">
                          {category.displayName}
                        </CardTitle>
                        <ArrowRight className="text-muted-foreground h-5 w-5 transition-transform group-hover:translate-x-1" />
                      </div>
                    </CardHeader>
                    <CardContent>
                      <CardDescription>{category.description}</CardDescription>
                    </CardContent>
                  </Card>
                </Link>
              );
            })}
          </div>

          {/* Stats Section */}
          <div className="mt-16 text-center">
            <Card className="mx-auto max-w-2xl">
              <CardHeader>
                <CardTitle>Her Gün Yeni Haberler</CardTitle>
                <CardDescription>
                  Yapay zeka, GitHub Copilot ve teknoloji dünyasından güncel haberler
                </CardDescription>
              </CardHeader>
              <CardContent>
                <div className="mt-4 grid grid-cols-3 gap-4">
                  <div className="text-center">
                    <div className="text-primary text-3xl font-bold">9</div>
                    <div className="text-muted-foreground text-sm">Teknoloji Kategorisi</div>
                  </div>
                  <div className="text-center">
                    <div className="text-primary text-3xl font-bold">Günlük</div>
                    <div className="text-muted-foreground text-sm">Otomatik Güncelleme</div>
                  </div>
                  <div className="text-center">
                    <div className="text-primary text-3xl font-bold">Güncel</div>
                    <div className="text-muted-foreground text-sm">Son Haberler</div>
                  </div>
                </div>
              </CardContent>
            </Card>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
