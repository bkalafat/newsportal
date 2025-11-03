import { Metadata } from "next";
import { Newspaper, Code, Zap, Shield } from "lucide-react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Header } from "@/components/layout/header";
import { Footer } from "@/components/layout/footer";

export const metadata: Metadata = {
  title: "Hakkımızda - Teknoloji Haberleri",
  description:
    "Yapay zeka, robotik ve yazılım geliştirme alanlarında güncel haberleri bir araya getiren, Türkçe teknoloji haber platformu. Reddit'ten otomatik toplanan kaliteli içerikler.",
  alternates: {
    canonical: "/about",
  },
  openGraph: {
    title: "Hakkımızda - Teknoloji Haberleri",
    description:
      "Yapay zeka, robotik ve yazılım geliştirme alanlarında güncel haberleri bir araya getiren, Türkçe teknoloji haber platformu.",
    type: "website",
    url: "/about",
  },
  twitter: {
    card: "summary",
    title: "Hakkımızda - Teknoloji Haberleri",
    description:
      "Yapay zeka, robotik ve yazılım geliştirme alanlarında güncel haberleri bir araya getiren, Türkçe teknoloji haber platformu.",
  },
};

// ISR: Revalidate every hour for profile updates
export const revalidate = 3600; // 1 hour

export default function AboutPage() {
  const categories = [
    "Yapay Zeka",
    "GitHub Copilot",
    "MCP",
    "OpenAI",
    "Robotik",
    "DeepSeek",
    ".NET",
    "Claude AI",
  ];

  const features = [
    {
      icon: Newspaper,
      title: "Otomatik Haber Toplama",
      description: "Reddit'in en popüler teknoloji subreddit'lerinden güncel içerikler otomatik olarak toplanır ve Türkçe okuyuculara sunulur",
    },
    {
      icon: Zap,
      title: "Günlük Güncelleme",
      description: "Her gün belirlenen saatte otomatik olarak yeni haberler eklenir, hiçbir önemli gelişmeyi kaçırmazsınız",
    },
    {
      icon: Code,
      title: "Kaliteli İçerik",
      description: "Yapay zeka, robotik ve yazılım geliştirme alanlarında en çok etkileşim alan içerikler önceliklenir",
    },
    {
      icon: Shield,
      title: "Güvenilir Kaynaklar",
      description: "Tüm haberler doğrudan kaynak bağlantılarıyla sunulur, orijinal tartışmalara kolayca erişebilirsiniz",
    },
  ];

  return (
    <div className="flex min-h-screen flex-col">
      <Header />
      <main className="flex-1">
        <div className="container mx-auto max-w-5xl px-4 py-12">
          {/* Header */}
          <div className="mb-12 text-center">
            <h1 className="mb-4 text-4xl font-bold md:text-5xl">Hakkımızda</h1>
            <p className="text-muted-foreground mx-auto max-w-2xl text-xl">
              Teknoloji dünyasının nabzını tutan, Türkçe haber platformu
            </p>
          </div>

          {/* Mission */}
          <Card className="mb-8">
            <CardHeader>
              <CardTitle>Misyonumuz</CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <p className="text-muted-foreground">
                Teknoloji Haberleri, dünya çapında teknoloji topluluklarında en çok konuşulan 
                gelişmeleri Türkçe okuyuculara ulaştırmayı amaçlayan bir haber platformudur. 
                Özellikle yapay zeka, robotik ve yazılım geliştirme alanlarındaki güncel 
                haberleri takip ederek, Türk teknoloji meraklılarının ve profesyonellerinin 
                bilgi kaynaklarına kolay erişimini sağlıyoruz.
              </p>
              <p className="text-muted-foreground">
                Reddit gibi global teknoloji topluluklarındaki en popüler ve etkileşimli içerikleri 
                otomatik olarak toplayarak, dil bariyerini ortadan kaldırıyor ve değerli bilgileri 
                Türkçe konuşan teknoloji tutkunu kişilere sunuyoruz. Amacımız, teknoloji dünyasındaki 
                en son gelişmeleri takip etmenizi kolaylaştırmak ve bilgiye erişimi demokratikleştirmektir.
              </p>
            </CardContent>
          </Card>

          {/* What We Cover */}
          <Card className="mb-8">
            <CardHeader>
              <CardTitle>Kapsadığımız Konular</CardTitle>
              <CardDescription>
                Platform üzerinde düzenli olarak yayınlanan haber kategorileri
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="flex flex-wrap gap-2">
                {categories.map((category) => (
                  <Badge key={category} variant="secondary" className="text-sm">
                    {category}
                  </Badge>
                ))}
              </div>
              <div className="mt-6 space-y-4">
                <div>
                  <h3 className="mb-2 font-semibold">Yapay Zeka ve Makine Öğrenmesi</h3>
                  <p className="text-muted-foreground text-sm">
                    ChatGPT, Claude, Gemini gibi büyük dil modellerinden, makine öğrenmesi 
                    algoritmalarına, yapay zeka uygulamalarına ve etik tartışmalara kadar 
                    geniş bir yelpazede haberler
                  </p>
                </div>
                <div>
                  <h3 className="mb-2 font-semibold">Yazılım Geliştirme</h3>
                  <p className="text-muted-foreground text-sm">
                    Programlama dilleri, framework'ler, geliştirme araçları, en iyi pratikler 
                    ve yazılım mühendisliği trendleri
                  </p>
                </div>
                <div>
                  <h3 className="mb-2 font-semibold">Robotik ve Otomasyon</h3>
                  <p className="text-muted-foreground text-sm">
                    İnsansı robotlar, endüstriyel otomasyon, drone teknolojisi ve robotik 
                    sistemlerdeki yenilikler
                  </p>
                </div>
              </div>
            </CardContent>
          </Card>

          {/* Features */}
          <div className="mb-12">
            <h2 className="mb-6 text-center text-3xl font-bold">Özellikler</h2>
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              {features.map((feature) => {
                const Icon = feature.icon;
                return (
                  <Card key={feature.title}>
                    <CardHeader>
          {/* Features */}
          <div className="mb-12">
            <h2 className="mb-6 text-center text-3xl font-bold">Nasıl Çalışır?</h2>
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              {features.map((feature) => {
                const Icon = feature.icon;
                return (
                  <Card key={feature.title}>
                    <CardHeader>
                      <div className="flex items-center gap-3">
                        <div className="bg-primary/10 rounded-lg p-2">
                          <Icon className="text-primary h-6 w-6" />
                        </div>
                        <CardTitle>{feature.title}</CardTitle>
                      </div>
                    </CardHeader>
                    <CardContent>
                      <CardDescription>{feature.description}</CardDescription>
                    </CardContent>
                  </Card>
                );
              })}
            </div>
          </div>

          {/* Value Proposition */}
          <Card>
            <CardHeader>
              <CardTitle>Neden Teknoloji Haberleri?</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                <div>
                  <h3 className="mb-2 font-semibold">📰 Tek Noktadan Erişim</h3>
                  <p className="text-muted-foreground text-sm">
                    Farklı subreddit'leri ve forumları tek tek takip etmenize gerek yok. 
                    En önemli teknoloji haberleri burada bir araya geliyor.
                  </p>
                </div>
                <div>
                  <h3 className="mb-2 font-semibold">🎯 Filtrelenmiş İçerik</h3>
                  <p className="text-muted-foreground text-sm">
                    Topluluk tarafından beğenilen ve yorum alan içerikler öncelikleniyor. 
                    Zamanınızı en değerli haberlere ayırabilirsiniz.
                  </p>
                </div>
                <div>
                  <h3 className="mb-2 font-semibold">🔗 Doğrudan Kaynak Erişimi</h3>
                  <p className="text-muted-foreground text-sm">
                    Her haberin orijinal kaynağına tek tıkla ulaşabilir, detaylı tartışmaları 
                    okuyabilir ve toplulukla etkileşime geçebilirsiniz.
                  </p>
                </div>
                <div>
                  <h3 className="mb-2 font-semibold">⚡ Güncel ve Hızlı</h3>
                  <p className="text-muted-foreground text-sm">
                    Otomatik güncelleme sistemi sayesinde, teknoloji dünyasındaki gelişmeleri 
                    neredeyse gerçek zamanlı takip edebilirsiniz.
                  </p>
                </div>
                <div>
                  <h3 className="mb-2 font-semibold">📱 Her Cihazda Erişilebilir</h3>
                  <p className="text-muted-foreground text-sm">
                    Masaüstü, tablet veya mobil cihazınızdan, istediğiniz yerden 
                    haberlere erişebilirsiniz. Modern ve kullanıcı dostu arayüz 
                    her platformda mükemmel çalışır.
                  </p>
                </div>
              </div>
            </CardContent>
          </Card>
          </div>
          {/* Footer */}
          <div className="text-muted-foreground mt-12 text-center">
            <p className="text-sm">
              Teknoloji dünyasındaki gelişmeleri takip etmek hiç bu kadar kolay olmamıştı.
            </p>
            <p className="mt-2 text-sm">
              Her gün yeni haberler için sitemizi ziyaret edin.
            </p>
            <p className="mt-2 text-sm">
              &copy; {new Date().getFullYear()} Teknoloji Haberleri. Tüm hakları saklıdır.
            </p>
          </div>