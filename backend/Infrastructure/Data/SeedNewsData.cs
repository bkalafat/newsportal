using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using NewsApi.Common;
using NewsApi.Domain.Entities;

namespace NewsApi.Infrastructure.Data;

internal static class SeedNewsData
{
    public static async Task SeedAsync(MongoDbContext context)
    {
        var newsCollection = context.News;

        // Check if we already have data and clear it
        var existingCount = await newsCollection
            .CountDocumentsAsync(FilterDefinition<NewsArticle>.Empty)
            .ConfigureAwait(false);
        if (existingCount > 0)
        {
            Console.WriteLine($"Database already contains {existingCount} news articles. Clearing old data...");
            await newsCollection.DeleteManyAsync(FilterDefinition<NewsArticle>.Empty).ConfigureAwait(false);
            Console.WriteLine("Old data cleared successfully!");
        }

        var now = DateTime.UtcNow;
        var newsArticles = new List<NewsArticle>
        {
            // POPULAR CATEGORY - En Popüler Haberler
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "2025'in En İyi Teknoloji Trendleri",
                Slug = SlugHelper.GenerateSlug("2025'in En İyi Teknoloji Trendleri"),
                Keywords = "teknoloji, trend, AI, yapay zeka, gelecek",
                SocialTags = "#Teknoloji #Trend #2025 #AI",
                Summary = "2025 yılında dünyayı değiştirecek en önemli teknoloji trendleri açıklandı. Yapay zeka, kuantum bilişim ve sürdürülebilir teknolojiler ön planda.",
                ImgPath = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ImgAlt = "2025 Teknoloji Trendleri",
                ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=400&q=80",
                Content = @"<p>2025 yılı teknoloji dünyası için devrim niteliğinde gelişmelere sahne oluyor. <strong>Yapay zeka</strong>, kuantum bilişim ve sürdürülebilir teknolojiler en çok konuşulan konular arasında.</p>

<h2>1. Yapay Zeka ve Otomasyonun Yükselişi</h2>
<p>Yapay zeka artık günlük hayatımızın her alanına nüfuz ediyor. ChatGPT benzeri büyük dil modelleri, kod yazımından müşteri hizmetlerine kadar birçok sektörde devrim yaratıyor.</p>

<h2>2. Kuantum Bilişim Gerçekleşiyor</h2>
<p>IBM ve Google'ın öncülük ettiği kuantum bilgisayarlar, karmaşık problemleri klasik bilgisayarlardan milyonlarca kat hızlı çözebiliyor.</p>

<h2>3. Sürdürülebilir Teknoloji</h2>
<p>Yeşil enerji ve karbon-nötr veri merkezleri, teknoloji şirketlerinin yeni önceliği haline geldi.</p>

<blockquote>""2025, teknolojinin insanlığa en çok fayda sağladığı yıl olacak."" - Tech Futurist Report</blockquote>",
                Subjects = new[] { "Teknoloji", "Gelecek", "Trendler" },
                Authors = new[] { "Teknoloji Editörü" },
                ExpressDate = now.AddHours(-2),
                CreateDate = now.AddHours(-2),
                UpdateDate = now.AddHours(-2),
                Priority = 1,
                IsActive = true,
                ViewCount = 5800,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "Türkiye'de Teknoloji Girişimleri Rekor Kırıyor",
                Slug = SlugHelper.GenerateSlug("Türkiye'de Teknoloji Girişimleri Rekor Kırıyor"),
                Keywords = "türkiye, startup, girişim, teknoloji, yatırım",
                SocialTags = "#Türkiye #Startup #Girişim #Teknoloji",
                Summary = "Türkiye'deki teknoloji girişimleri 2025'in ilk çeyreğinde 2.5 milyar dolar yatırım aldı. Bu, bir önceki yıla göre %87 artış anlamına geliyor.",
                ImgPath = "https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&q=80",
                ImgAlt = "Türkiye Startup Ekosistemi",
                ImageUrl = "https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1559136555-9303baea8ebd?w=400&q=80",
                Content = @"<p>Türkiye teknoloji ekosistemi için tarihi bir döneme tanıklık ediyor. <strong>2025'in ilk çeyreğinde 2.5 milyar dolar</strong> yatırım alan Türk girişimleri, bölgenin en hızlı büyüyen pazarı haline geldi.</p>

<h2>En Çok Yatırım Alan Sektörler</h2>
<ul>
<li><strong>Fintech:</strong> $890 milyon</li>
<li><strong>E-ticaret:</strong> $650 milyon</li>
<li><strong>AI/Yazılım:</strong> $540 milyon</li>
<li><strong>Sağlık Teknolojisi:</strong> $420 milyon</li>
</ul>

<h2>Unicorn Sayısı Artıyor</h2>
<p>Türkiye'nin unicorn (1 milyar dolar değerlemeli) şirket sayısı 12'ye ulaştı. Yeni katılanlar arasında bir yapay zeka girişimi ve bir siber güvenlik şirketi bulunuyor.</p>

<blockquote>""Türkiye, Avrupa'nın en dinamik teknoloji merkezlerinden biri olmaya devam ediyor."" - Venture Capital Association</blockquote>",
                Subjects = new[] { "Girişimcilik", "Yatırım", "Ekonomi" },
                Authors = new[] { "İş Dünyası Editörü" },
                ExpressDate = now.AddHours(-5),
                CreateDate = now.AddHours(-5),
                UpdateDate = now.AddHours(-5),
                Priority = 2,
                IsActive = true,
                ViewCount = 4200,
                IsSecondPageNews = false,
            },

            // ARTIFICIAL INTELLIGENCE CATEGORY
            new NewsArticle
            {
                Category = "artificialintelligence",
                Type = "haber",
                Caption = "OpenAI GPT-5 Duyurusu: İnsan Düzeyinde Akıl Yürütme",
                Slug = SlugHelper.GenerateSlug("OpenAI GPT-5 Duyurusu İnsan Düzeyinde Akıl Yürütme"),
                Keywords = "openai, gpt5, yapay zeka, AI, makine öğrenmesi",
                SocialTags = "#OpenAI #GPT5 #YapayZeka #AI",
                Summary = "OpenAI, GPT-5 modelini tanıttı. Yeni model, karmaşık problemlerde insan düzeyinde akıl yürütme ve muhakeme yapabiliyor.",
                ImgPath = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ImgAlt = "OpenAI GPT-5",
                ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=400&q=80",
                Content = @"<p>OpenAI, beklenen <strong>GPT-5</strong> modelini resmi olarak tanıttı. Yeni nesil yapay zeka modeli, karmaşık problemlerde insan düzeyinde akıl yürütme ve muhakeme yapabiliyor.</p>

<h2>GPT-5'in Yeni Özellikleri</h2>
<ul>
<li><strong>Gelişmiş Muhakeme:</strong> Çok adımlı problem çözme</li>
<li><strong>Multimodal:</strong> Metin, görsel, ses ve video işleme</li>
<li><strong>Uzun Bağlam:</strong> 1 milyon token context window</li>
<li><strong>Tutarlılık:</strong> %96 fact-checking accuracy</li>
</ul>

<h2>Performans Karşılaştırması</h2>
<p>GPT-5, GPT-4'e göre matematik ve kodlamada %87 daha iyi performans gösteriyor. Özellikle bilimsel makale yazımı ve veri analizi konularında çığır açıyor.</p>

<blockquote>""GPT-5, yapay genel zekanın (AGI) gerçekleştirilmesine en yakın modelimiz."" - Sam Altman, OpenAI CEO</blockquote>

<h2>Kullanım Alanları</h2>
<ul>
<li>Bilimsel araştırma asistanı</li>
<li>Tıbbi teşhis desteği</li>
<li>Yazılım geliştirme</li>
<li>Eğitim ve öğretim</li>
<li>Yaratıcı içerik üretimi</li>
</ul>",
                Subjects = new[] { "Yapay Zeka", "OpenAI", "Teknoloji" },
                Authors = new[] { "AI Editörü" },
                ExpressDate = now.AddHours(-3),
                CreateDate = now.AddHours(-3),
                UpdateDate = now.AddHours(-3),
                Priority = 1,
                IsActive = true,
                ViewCount = 6700,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "artificialintelligence",
                Type = "haber",
                Caption = "Yapay Zeka Etik Kuralları: AB'den Yeni Düzenleme",
                Slug = SlugHelper.GenerateSlug("Yapay Zeka Etik Kuralları AB'den Yeni Düzenleme"),
                Keywords = "AI, etik, AB, düzenleme, yapay zeka yasası",
                SocialTags = "#AI #Etik #AB #Yapay Zeka",
                Summary = "Avrupa Birliği, yapay zeka sistemleri için kapsamlı etik kurallar ve düzenlemeler getiriyor. AI Act, 2025'ten itibaren yürürlüğe giriyor.",
                ImgPath = "https://images.unsplash.com/photo-1620712943543-bcc4688e7485?w=1200&q=80",
                ImgAlt = "AI Etik Kuralları",
                ImageUrl = "https://images.unsplash.com/photo-1620712943543-bcc4688e7485?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1620712943543-bcc4688e7485?w=400&q=80",
                Content = @"<p>Avrupa Birliği'nin <strong>AI Act</strong> yasası, yapay zeka teknolojilerinin etik ve güvenli kullanımını garantilemek için 2025'te resmen yürürlüğe giriyor.</p>

<h2>Temel Kurallar</h2>
<ul>
<li><strong>Şeffaflık:</strong> AI sistemlerin açıklanabilir olması</li>
<li><strong>Veri Gizliliği:</strong> GDPR uyumluluğu</li>
<li><strong>İnsan Denetimi:</strong> Kritik kararlarda insan onayı</li>
<li><strong>Adalet:</strong> Algoritmik önyargıların önlenmesi</li>
</ul>

<h2>Risk Kategorileri</h2>
<p>AI Act, yapay zeka sistemlerini risk seviyelerine göre kategorize ediyor:</p>
<ol>
<li><strong>Kabul Edilemez Risk:</strong> Yasaklı (sosyal skorlama, manipülasyon)</li>
<li><strong>Yüksek Risk:</strong> Sıkı düzenleme (sağlık, güvenlik, finans)</li>
<li><strong>Düşük Risk:</strong> Şeffaflık gereksinimleri (chatbot'lar)</li>
<li><strong>Minimal Risk:</strong> Serbest kullanım (spam filtreleri)</li>
</ol>

<blockquote>""Bu düzenleme, yapay zekanın insanlığa faydalı kalmasını garanti edecek."" - Ursula von der Leyen, AB Komisyon Başkanı</blockquote>",
                Subjects = new[] { "Yapay Zeka", "Etik", "Düzenleme" },
                Authors = new[] { "Hukuk Editörü" },
                ExpressDate = now.AddHours(-7),
                CreateDate = now.AddHours(-7),
                UpdateDate = now.AddHours(-7),
                Priority = 2,
                IsActive = true,
                ViewCount = 3800,
                IsSecondPageNews = false,
            },

            // GITHUB COPILOT CATEGORY
            new NewsArticle
            {
                Category = "githubcopilot",
                Type = "haber",
                Caption = "GitHub Enterprise Cloud Çift Ücretlendirme Sorunu",
                Slug = SlugHelper.GenerateSlug("GitHub Enterprise Cloud Çift Ücretlendirme Sorunu"),
                Keywords = "github, enterprise, billing, support, cloud",
                SocialTags = "#GitHub #Enterprise #Billing",
                Summary = "Bir geliştirici, GitHub Enterprise Cloud hesabında $168 yerine $84 faturalaşma sorunu yaşıyor ve 3 haftadır destekten yanıt alamıyor.",
                ImgPath = "https://images.unsplash.com/photo-1618044733300-9472054094ee?w=1200&q=80",
                ImgAlt = "GitHub Enterprise Cloud",
                ImageUrl = "https://images.unsplash.com/photo-1618044733300-9472054094ee?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1618044733300-9472054094ee?w=400&q=80",
                Content = @"<p>Reddit kullanıcısı stepanokdev, <strong>GitHub Enterprise Cloud</strong> hesabında yaşadığı faturalama sorununu paylaştı. Normalde 4 aktif kullanıcı için aylık $84 ödeyen şirket, Ekim ayı faturasında $168 ücretlendirilmiş.</p>

<h2>Destek Ekibi Yanıt Vermiyor</h2>
<p>Kullanıcı, 3 hafta önce açtığı destek talebine hala yanıt alamadığını belirtiyor. Enterprise hesapların 24 saat içinde yanıt alması beklenirken, bu durum hayal kırıklığı yarattı.</p>

<blockquote>""Enterprise hesapların 24 saat içinde yanıt alması gerekmiyor mu? Neredeyse bir aydır bekliyorum.""</blockquote>

<h2>Detaylar</h2>
<ul>
<li>Fatura No: INV102226125</li>
<li>Beklenen Ücret: ~$84</li>
<li>Çekilen Ücret: $168</li>
<li>GitHub Actions: $0 (tamamen indirimli)</li>
<li>Copilot: Devre dışı</li>
</ul>

<p>Kullanıcı, şirketin yalnızca doğru miktarı geri ödeyeceğini ve çift ücretlendirmeyi ödeyemeyeceğini belirtiyor. GitHub Enterprise ekibinden bir açıklama bekleniyor.</p>",
                Subjects = new[] { "GitHub", "Enterprise", "Billing" },
                Authors = new[] { "Reddit Community" },
                ExpressDate = now.AddDays(-2),
                CreateDate = now.AddDays(-2),
                UpdateDate = now.AddDays(-2),
                Priority = 1,
                IsActive = true,
                ViewCount = 2500,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "githubcopilot",
                Type = "haber",
                Caption = "GitHub Ana Sayfasında Activity Bölümü Kayboldu",
                Slug = SlugHelper.GenerateSlug("GitHub Ana Sayfasında Activity Bölümü Kayboldu"),
                Keywords = "github, activity, sidebar, bug, SSO",
                SocialTags = "#GitHub #Bug #Activity",
                Summary = "Kullanıcılar GitHub ana sayfasındaki 'Activity' bölümünün kaybolduğunu bildiriyor. Sorun şirket SSO eklendiğinde başlamış olabilir.",
                ImgPath = "https://images.unsplash.com/photo-1556075798-4825dfaaf498?w=1200&q=80",
                ImgAlt = "GitHub Dashboard",
                ImageUrl = "https://images.unsplash.com/photo-1556075798-4825dfaaf498?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1556075798-4825dfaaf498?w=400&q=80",
                Content = @"<p>Reddit kullanıcısı superl2, GitHub ana sayfasının sidebar'ında bulunan <strong>Activity</strong> bölümünün ortadan kaybolduğunu paylaştı.</p>

<h2>Sorunun Detayları</h2>
<p>Activity bölümü normalde kullanıcının son issue'ları ve pull request'lerini gösteriyor. Kullanıcı, sorununun şirket SSO login'i ekledikten sonra başladığını düşünüyor ancak bunun tesadüf olabileceğini belirtiyor.</p>

<h2>Topluluk Tepkileri</h2>
<p>Benzer sorunları yaşayan kullanıcılar, GitHub'ın son UI güncellemelerinden sonra çeşitli hataların ortaya çıktığını belirtiyor. Özellikle SSO entegrasyonundan sonra bazı özelliklerin kaybolması bilinen bir sorun.</p>

<p><strong>Geçici Çözüm:</strong> Kullanıcılar cache temizleme ve farklı tarayıcı kullanmayı öneriyorlar.</p>",
                Subjects = new[] { "GitHub", "Bug", "UI" },
                Authors = new[] { "Reddit Community" },
                ExpressDate = now.AddDays(-3),
                CreateDate = now.AddDays(-3),
                UpdateDate = now.AddDays(-3),
                Priority = 2,
                IsActive = true,
                ViewCount = 1000,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "githubcopilot",
                Type = "haber",
                Caption = "GitHub Copilot Actions PR'larda Çöktü mü?",
                Slug = SlugHelper.GenerateSlug("GitHub Copilot Actions PR'larda Çöktü mü"),
                Keywords = "github, copilot, actions, billing, error",
                SocialTags = "#GitHubCopilot #Actions #Bug",
                Summary = "Kullanıcılar PR'larda @copilot etiketlendiğinde 'billing error' hatası alıyorlar. Hesaplar güncel ve limitler aşılmamış durumda.",
                ImgPath = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ImgAlt = "GitHub Copilot Error",
                ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=400&q=80",
                Content = @"<p>Reddit kullanıcısı SoCalChrisW, Pull Request'lerde <strong>@copilot</strong> etiketlendiğinde hata aldığını bildirdi.</p>

<h2>Hata Mesajı</h2>
<blockquote>""Copilot has encountered an error. See logs for additional details.""</blockquote>

<p>Action log'larında ise şu hata görülüyor:</p>

<blockquote>""The job was not started because recent account payments have failed or your spending limit needs to be increased. Please check the 'Billing & plans' section in your settings""</blockquote>

<h2>Gerçek Durum</h2>
<ul>
<li>Kullanım limitlerin çok altında</li>
<li>Hesap güncel</li>
<li>Son ödeme denemesi yok</li>
<li>Billing cycle ortasında</li>
</ul>

<p>Birçok kullanıcı aynı hatayı alıyor. GitHub'ın Copilot Actions altyapısında genel bir sorun olduğu tahmin ediliyor.</p>",
                Subjects = new[] { "GitHub", "Copilot", "Actions" },
                Authors = new[] { "Reddit Community" },
                ExpressDate = now.AddDays(-1),
                CreateDate = now.AddDays(-1),
                UpdateDate = now.AddDays(-1),
                Priority = 1,
                IsActive = true,
                ViewCount = 750,
                IsSecondPageNews = false,
            },

            // MCP CATEGORY - Model Context Protocol
            new NewsArticle
            {
                Category = "mcp",
                Type = "haber",
                Caption = "Model Context Protocol (MCP): AI Entegrasyonunun Geleceği",
                Slug = SlugHelper.GenerateSlug("Model Context Protocol MCP AI Entegrasyonunun Geleceği"),
                Keywords = "MCP, model context protocol, AI, entegrasyon, standart",
                SocialTags = "#MCP #AI #Protocol #Standart",
                Summary = "Anthropic'in geliştirdiği Model Context Protocol, AI modellerinin veri kaynaklarıyla standart bir şekilde iletişim kurmasını sağlıyor.",
                ImgPath = "https://images.unsplash.com/photo-1558494949-ef010cbdcc31?w=1200&q=80",
                ImgAlt = "Model Context Protocol",
                ImageUrl = "https://images.unsplash.com/photo-1558494949-ef010cbdcc31?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1558494949-ef010cbdcc31?w=400&q=80",
                Content = @"<p><strong>Model Context Protocol (MCP)</strong>, Anthropic tarafından geliştirilen ve AI modellerinin çeşitli veri kaynaklarıyla güvenli ve standart bir şekilde iletişim kurmasını sağlayan açık kaynak bir protokol.</p>

<h2>MCP Nedir?</h2>
<p>MCP, AI asistanlarının veritabanları, API'ler ve dosya sistemleri gibi kaynaklara erişimini standartlaştırır. Bu sayede geliştiriciler, her AI modeli için ayrı entegrasyon kodu yazmak zorunda kalmaz.</p>

<h2>Temel Özellikler</h2>
<ul>
<li><strong>Evrensel Standart:</strong> Tüm AI modelleri için tek protokol</li>
<li><strong>Güvenlik:</strong> Granular erişim kontrolü</li>
<li><strong>Bağlam Yönetimi:</strong> Akıllı context window kullanımı</li>
<li><strong>Açık Kaynak:</strong> Topluluk katkılarına açık</li>
</ul>

<h2>Destekleyen Platformlar</h2>
<ul>
<li>Anthropic Claude</li>
<li>OpenAI GPT serisi</li>
<li>Google Gemini</li>
<li>Microsoft Copilot</li>
</ul>

<blockquote>""MCP, AI entegrasyonlarını HTTP'nin web için yaptığı gibi standartlaştıracak."" - Anthropic Blog</blockquote>",
                Subjects = new[] { "AI", "Protocol", "Standart" },
                Authors = new[] { "Teknoloji Editörü" },
                ExpressDate = now.AddHours(-4),
                CreateDate = now.AddHours(-4),
                UpdateDate = now.AddHours(-4),
                Priority = 1,
                IsActive = true,
                ViewCount = 2800,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "mcp",
                Type = "haber",
                Caption = "MCP ile Claude Desktop Entegrasyonu: Adım Adım Kılavuz",
                Slug = SlugHelper.GenerateSlug("MCP ile Claude Desktop Entegrasyonu Adım Adım Kılavuz"),
                Keywords = "MCP, claude, desktop, entegrasyon, tutorial",
                SocialTags = "#MCP #Claude #Tutorial #AI",
                Summary = "Claude Desktop uygulamasında MCP kullanarak yerel dosyalarınıza ve veritabanlarınıza güvenli erişim sağlayın.",
                ImgPath = "https://images.unsplash.com/photo-1516116216624-53e697fedbea?w=1200&q=80",
                ImgAlt = "Claude MCP Integration",
                ImageUrl = "https://images.unsplash.com/photo-1516116216624-53e697fedbea?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1516116216624-53e697fedbea?w=400&q=80",
                Content = @"<p>Claude Desktop uygulaması, <strong>Model Context Protocol (MCP)</strong> sayesinde yerel sisteminizdeki dosyalara ve veritabanlarına güvenli erişim sağlayabiliyor.</p>

<h2>Kurulum Adımları</h2>
<ol>
<li>Claude Desktop'ı indirin ve kurun</li>
<li>MCP server'ı npm ile yükleyin: <code>npm install -g @anthropic-ai/mcp-server</code></li>
<li>Config dosyasını düzenleyin: <code>~/.config/claude/mcp.json</code></li>
<li>İzinleri ayarlayın</li>
<li>Claude'u yeniden başlatın</li>
</ol>

<h2>Örnek Kullanım Senaryoları</h2>
<ul>
<li><strong>Kod Analizi:</strong> Tüm projenizi Claude'a yüklemeden analiz edin</li>
<li><strong>Veritabanı Sorguları:</strong> SQL sorguları çalıştırın</li>
<li><strong>Dosya Operasyonları:</strong> Dosya okuma/yazma işlemleri</li>
<li><strong>API Çağrıları:</strong> REST API'leri test edin</li>
</ul>

<blockquote>""MCP ile Claude artık gerçek bir yerel asistan gibi çalışıyor."" - Claude Desktop Kullanıcısı</blockquote>",
                Subjects = new[] { "Claude", "MCP", "Tutorial" },
                Authors = new[] { "Dev Tutorial Team" },
                ExpressDate = now.AddHours(-6),
                CreateDate = now.AddHours(-6),
                UpdateDate = now.AddHours(-6),
                Priority = 2,
                IsActive = true,
                ViewCount = 1900,
                IsSecondPageNews = false,
            },

            // OPENAI CATEGORY
            new NewsArticle
            {
                Category = "openai",
                Type = "haber",
                Caption = "Copilot'u Kapattıktan Sonra Kodlamanın Ne Kadar Stresli Olduğunu Fark Ettim",
                Slug = SlugHelper.GenerateSlug("Copilot'u Kapattıktan Sonra Kodlamanın Ne Kadar Stresli Olduğunu Fark Ettim"),
                Keywords = "copilot, AI, coding, stress, productivity",
                SocialTags = "#Copilot #AI #Coding #WebDev",
                Summary = "6 yıllık bir geliştirici, Copilot'u kapattıktan sonra kodlamanın ne kadar rahatladığını paylaşıyor. AI'nın sürekli öneri yapması dikkat dağıtıcı olabiliyor.",
                ImgPath = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=1200&q=80",
                ImgAlt = "Coding without AI",
                ImageUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=400&q=80",
                Content = @"<p>Reddit kullanıcısı xSypRo, 6 yıldır yazılım geliştirdiğini ve AI'nın son 3 yıldır hayatının bir parçası olduğunu belirtiyor. Ancak Copilot'u kapattıktan sonra şaşırtıcı bir keşif yaptı.</p>

<h2>AI ile Kodlama Stresi</h2>
<blockquote>""Copilot'u daha iyi bir IntelliSense gibi kullanıyordum. Ne yazmak istediğimi biliyordum ama bazen çok fazla yazı gerekliydi ve Copilot kısayol sağlıyordu. Ama bazen 'SUS!!! Düzenlemeyi bırak, odağımı dağıtıyorsun!!' diye düşünüyordum.""</blockquote>

<h2>TikTok/Reels Benzeri Davranış</h2>
<p>Kullanıcı, Copilot'un davranışını sosyal medya algoritmalarına benzetiyor:</p>
<ul>
<li>Sürekli ekranda değişiklikler</li>
<li>Yanıp sönen öneriler</li>
<li>Dikkat dağıtıcı görseller</li>
<li>Odaklanmayı zorlaştıran sürekli hareket</li>
</ul>

<blockquote>""Bu sadece bir text editör, bu şekilde davranmamalı.""</blockquote>

<h2>Yeni Yaklaşım</h2>
<p>Geliştirici şimdi varsayılan olarak kapalı tutup sadece template kod veya uzun yazı işlerinde açmayı deniyor. Topluluktan 222 upvote alan post, birçok geliştiricinin benzer hissettiğini gösteriyor.</p>

<p><strong>Sonuç:</strong> AI araçları üretkenliği artırabilir ama her zaman daha iyi değil. Kişisel tercih ve çalışma tarzı önemli.</p>",
                Subjects = new[] { "Web Development", "AI Tools", "Productivity" },
                Authors = new[] { "xSypRo" },
                ExpressDate = now.AddHours(-12),
                CreateDate = now.AddHours(-12),
                UpdateDate = now.AddHours(-12),
                Priority = 1,
                IsActive = true,
                ViewCount = 3200,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "openai",
                Type = "haber",
                Caption = "OpenAI DevDay 2025: Yeni Araçlar ve API Güncellemeleri",
                Slug = SlugHelper.GenerateSlug("OpenAI DevDay 2025 Yeni Araçlar ve API Güncellemeleri"),
                Keywords = "openai, devday, API, developer, tools",
                SocialTags = "#OpenAI #DevDay #API #Developer",
                Summary = "OpenAI DevDay 2025'te geliştiriciler için yeni araçlar ve API güncellemeleri duyuruldu. Function calling ve vision API'leri geliştirildi.",
                ImgPath = "https://images.unsplash.com/photo-1587440871875-191322ee64b0?w=1200&q=80",
                ImgAlt = "OpenAI DevDay 2025",
                ImageUrl = "https://images.unsplash.com/photo-1587440871875-191322ee64b0?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1587440871875-191322ee64b0?w=400&q=80",
                Content = @"<p>OpenAI, yıllık geliştirici konferansı <strong>DevDay 2025</strong>'te çığır açan yenilikler duyurdu.</p>

<h2>Yeni Özellikler</h2>
<ul>
<li><strong>Structured Outputs:</strong> JSON çıktıları garanti ediliyor</li>
<li><strong>Vision API v2:</strong> Daha hızlı ve ucuz görsel işleme</li>
<li><strong>Function Calling 2.0:</strong> Paralel fonksiyon çağrıları</li>
<li><strong>Realtime API:</strong> WebSocket üzerinden streaming</li>
</ul>

<h2>Fiyat İndirimleri</h2>
<table>
<tr><td>GPT-4 Turbo</td><td>%40 indirim</td></tr>
<tr><td>GPT-3.5</td><td>%60 indirim</td></tr>
<tr><td>Embedding</td><td>%70 indirim</td></tr>
</table>

<blockquote>""Amacımız AI'yı her geliştirici için erişilebilir kılmak."" - OpenAI Developer Relations</blockquote>",
                Subjects = new[] { "OpenAI", "API", "Developer" },
                Authors = new[] { "Dev News Team" },
                ExpressDate = now.AddHours(-8),
                CreateDate = now.AddHours(-8),
                UpdateDate = now.AddHours(-8),
                Priority = 1,
                IsActive = true,
                ViewCount = 4100,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "githubcopilot",
                Type = "tartışma",
                Caption = "Kişisel vs Kurumsal GitHub Hesapları Güvenlik Sorunu",
                Slug = SlugHelper.GenerateSlug("Kişisel vs Kurumsal GitHub Hesapları Güvenlik Sorunu"),
                Keywords = "github, security, DLP, enterprise, personal account",
                SocialTags = "#GitHub #Security #Enterprise",
                Summary = "Güvenlik ekipleri, geliştiricilerin kişisel GitHub hesaplarını iş için kullanmasını risk olarak işaretliyor. DLP politikaları atlanabilir.",
                ImgPath = "https://images.unsplash.com/photo-1563986768609-322da13575f3?w=1200&q=80",
                ImgAlt = "GitHub Security",
                ImageUrl = "https://images.unsplash.com/photo-1563986768609-322da13575f3?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1563986768609-322da13575f3?w=400&q=80",
                Content = @"<p>Reddit kullanıcısı hashkent, şirketindeki güvenlik ekibinin tespit ettiği riski paylaştı: Geliştiriciler kişisel GitHub hesaplarıyla çalışırken şirket kodunu kendi hesaplarına push edebilir ve DLP politikalarını atlayabilir.</p>

<h2>Denenen Çözüm</h2>
<p>Kullanıcı iş için ayrı bir GitHub hesabı oluşturmaya çalışmış ancak GitHub'ın <strong>one-account-per-user</strong> politikası nedeniyle hesap suspend edilmiş.</p>

<h2>Şirket Durumu</h2>
<ul>
<li>Primarily GitLab shop</li>
<li>~120 mühendis için GitHub Copilot Enterprise SSO</li>
<li>Sadece 3 mobile developer GitHub'da kod tutuyor</li>
<li>Çoğu geliştirici katkı grafiği umursamıyor (kod GitLab'da)</li>
</ul>

<h2>Tartışma Noktası</h2>
<blockquote>""Özel iş hesabıyla bile, geliştiriciler 'john-acme' gibi kişisel repo'lara push edebilir ve ayrılmadan önce gerçek kişisel hesaplarına transfer edebilir. Bu biraz anlamsız bir sorun.""</blockquote>

<p>Topluluk, benzer kurulumda diğer şirketlerin nasıl yönettiğini tartışıyor.</p>",
                Subjects = new[] { "Security", "GitHub", "Enterprise" },
                Authors = new[] { "hashkent" },
                ExpressDate = now.AddDays(-4),
                CreateDate = now.AddDays(-4),
                UpdateDate = now.AddDays(-4),
                Priority = 2,
                IsActive = true,
                ViewCount = 900,
                IsSecondPageNews = false,
            },
            // ROBOTICS CATEGORY
            new NewsArticle
            {
                Category = "robotics",
                Type = "haber",
                Caption = "Tesla Optimus Gen 3: Üretim Hattında İlk Robotlar",
                Slug = SlugHelper.GenerateSlug("Tesla Optimus Gen 3 Üretim Hattında İlk Robotlar"),
                Keywords = "tesla, optimus, robot, humanoid, üretim",
                SocialTags = "#Tesla #Optimus #Robot #Humanoid",
                Summary = "Tesla'nın insansı robotu Optimus Gen 3, şirketin üretim tesislerinde çalışmaya başladı. Robot, basit montaj işlerini insanlarla birlikte yapıyor.",
                ImgPath = "https://images.unsplash.com/photo-1561557944-6e7860d1a7eb?w=1200&q=80",
                ImgAlt = "Tesla Optimus Robot",
                ImageUrl = "https://images.unsplash.com/photo-1561557944-6e7860d1a7eb?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1561557944-6e7860d1a7eb?w=400&q=80",
                Content = @"<p>Tesla'nın <strong>Optimus Gen 3</strong> insansı robotu, şirketin Fremont fabrikasında üretime katıldı. Bu, ticari insansı robotların seri üretime geçişinde önemli bir kilometre taşı.</p>

<h2>Teknik Özellikler</h2>
<ul>
<li><strong>Boy:</strong> 173 cm</li>
<li><strong>Ağırlık:</strong> 73 kg</li>
<li><strong>Batarya Ömrü:</strong> 8 saat</li>
<li><strong>Taşıma Kapasitesi:</strong> 20 kg</li>
<li><strong>Hız:</strong> 1.4 m/s (yürüme)</li>
</ul>

<h2>Yapabildiği İşler</h2>
<ul>
<li>Parça montajı</li>
<li>Malzeme taşıma</li>
<li>Kalite kontrol</li>
<li>Temizlik işleri</li>
</ul>

<blockquote>""Optimus, fabrikalarımızda insanlarla yan yana çalışan ilk robot."" - Elon Musk</blockquote>",
                Subjects = new[] { "Robotik", "Tesla", "Otomasyon" },
                Authors = new[] { "Robotik Editörü" },
                ExpressDate = now.AddHours(-9),
                CreateDate = now.AddHours(-9),
                UpdateDate = now.AddHours(-9),
                Priority = 1,
                IsActive = true,
                ViewCount = 5300,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "robotics",
                Type = "haber",
                Caption = "Tıp Robotları: Ameliyat Masasındaki Yeni Asistanlar",
                Slug = SlugHelper.GenerateSlug("Tıp Robotları Ameliyat Masasındaki Yeni Asistanlar"),
                Keywords = "tıp, robot, cerrahi, sağlık, teknoloji",
                SocialTags = "#TıpRobotları #Sağlık #Cerrahi",
                Summary = "Cerrahi robotlar, ameliyatlarda doktorlara yardım ederek daha hassas ve güvenli operasyonlar yapılmasını sağlıyor.",
                ImgPath = "https://images.unsplash.com/photo-1538108149393-fbbd81895907?w=1200&q=80",
                ImgAlt = "Cerrahi Robot",
                ImageUrl = "https://images.unsplash.com/photo-1538108149393-fbbd81895907?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1538108149393-fbbd81895907?w=400&q=80",
                Content = @"<p><strong>Cerrahi robotlar</strong>, modern tıbbın vazgeçilmez araçları haline geldi. 2025'te dünya genelinde 10,000'den fazla hastanede robotik cerrahi sistemi kullanılıyor.</p>

<h2>Avantajları</h2>
<ul>
<li><strong>Hassasiyet:</strong> Mikrometre düzeyinde kesinlik</li>
<li><strong>Minimal İnvaziv:</strong> Daha küçük kesiler</li>
<li><strong>Hızlı İyileşme:</strong> Hastanede kalış süresi %50 azalıyor</li>
<li><strong>Titreşim Yok:</strong> Robot eli asla titremez</li>
</ul>

<h2>Kullanım Alanları</h2>
<ul>
<li>Kalp cerrahisi</li>
<li>Kanser ameliyatları</li>
<li>Organ nakli</li>
<li>Nöroşirurji</li>
</ul>

<blockquote>""Robotik cerrahi, tıbbın geleceğini şekillendiriyor."" - American College of Surgeons</blockquote>",
                Subjects = new[] { "Sağlık", "Robotik", "Tıp" },
                Authors = new[] { "Sağlık Editörü" },
                ExpressDate = now.AddHours(-11),
                CreateDate = now.AddHours(-11),
                UpdateDate = now.AddHours(-11),
                Priority = 2,
                IsActive = true,
                ViewCount = 3600,
                IsSecondPageNews = false,
            },

            // DEEPSEEK CATEGORY
            new NewsArticle
            {
                Category = "deepseek",
                Type = "haber",
                Caption = "DeepSeek V3: Açık Kaynak AI Modeli ChatGPT'ye Rakip",
                Slug = SlugHelper.GenerateSlug("DeepSeek V3 Açık Kaynak AI Modeli ChatGPT'ye Rakip"),
                Keywords = "deepseek, AI, açık kaynak, chatgpt, yapay zeka",
                SocialTags = "#DeepSeek #AI #OpenSource",
                Summary = "Çin merkezli DeepSeek, GPT-4 seviyesinde performans gösteren açık kaynak AI modelini yayınladı. Model tamamen ücretsiz kullanılabiliyor.",
                ImgPath = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ImgAlt = "DeepSeek V3",
                ImageUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1677442136019-21780ecad995?w=400&q=80",
                Content = @"<p><strong>DeepSeek V3</strong>, GPT-4 seviyesinde performans gösteren tamamen açık kaynak bir AI modeli olarak piyasaya sürüldü.</p>

<h2>Öne Çıkan Özellikler</h2>
<ul>
<li><strong>Açık Kaynak:</strong> Model ağırlıkları tamamen açık</li>
<li><strong>Yüksek Performans:</strong> Birçok benchmark'ta GPT-4'e yakın</li>
<li><strong>Çok Dilli:</strong> 50+ dil desteği</li>
<li><strong>Ücretsiz:</strong> API kullanımı sınırsız</li>
</ul>

<h2>Benchmark Sonuçları</h2>
<table>
<tr><td>MMLU</td><td>86.7%</td><td>(GPT-4: 87.2%)</td></tr>
<tr><td>HumanEval</td><td>84.2%</td><td>(GPT-4: 85.4%)</td></tr>
<tr><td>GSM8K</td><td>92.1%</td><td>(GPT-4: 92.0%)</td></tr>
</table>

<blockquote>""DeepSeek, AI demokratikleşmesinde önemli bir adım."" - AI Research Community</blockquote>",
                Subjects = new[] { "AI", "Açık Kaynak", "DeepSeek" },
                Authors = new[] { "AI Editörü" },
                ExpressDate = now.AddHours(-10),
                CreateDate = now.AddHours(-10),
                UpdateDate = now.AddHours(-10),
                Priority = 1,
                IsActive = true,
                ViewCount = 4800,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "deepseek",
                Type = "haber",
                Caption = "DeepSeek Coder V2: Kod Yazımında Yeni Standart",
                Slug = SlugHelper.GenerateSlug("DeepSeek Coder V2 Kod Yazımında Yeni Standart"),
                Keywords = "deepseek, coder, programming, AI, açık kaynak",
                SocialTags = "#DeepSeek #Coder #Programming #AI",
                Summary = "DeepSeek Coder V2, programlama dillerinde uzmanlaşmış açık kaynak AI modeli. 100+ programlama dilini destekliyor ve GitHub Copilot'a alternatif oluyor.",
                ImgPath = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=1200&q=80",
                ImgAlt = "DeepSeek Coder",
                ImageUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=400&q=80",
                Content = @"<p><strong>DeepSeek Coder V2</strong>, özellikle kod yazımı için optimize edilmiş açık kaynak bir AI model ailesi. Model, 100+ programlama dilini destekliyor.</p>

<h2>Teknik Özellikler</h2>
<ul>
<li><strong>Model Boyutları:</strong> 1B, 7B, 33B parametreli versiyonlar</li>
<li><strong>Context Window:</strong> 16K token</li>
<li><strong>Dil Desteği:</strong> Python, JavaScript, Java, C++, Go, Rust ve 94+ dil</li>
<li><strong>Açık Kaynak:</strong> MIT lisansı ile tamamen ücretsiz</li>
</ul>

<h2>Performans</h2>
<table>
<tr><th>Benchmark</th><th>DeepSeek V2</th><th>GPT-3.5</th></tr>
<tr><td>HumanEval</td><td>78.6%</td><td>72.5%</td></tr>
<tr><td>MBPP</td><td>75.4%</td><td>71.2%</td></tr>
<tr><td>MultiPL-E</td><td>69.8%</td><td>65.3%</td></tr>
</table>

<h2>Kullanım Alanları</h2>
<ul>
<li>Code completion (VS Code, JetBrains IDE'ler)</li>
<li>Bug fixing ve refactoring</li>
<li>Code explanation</li>
<li>Unit test generation</li>
</ul>

<blockquote>""DeepSeek Coder, açık kaynak kod asistanı alanında yeni bir standart belirliyor."" - Developer Community</blockquote>",
                Subjects = new[] { "DeepSeek", "Coding", "AI" },
                Authors = new[] { "Dev Tools Team" },
                ExpressDate = now.AddHours(-14),
                CreateDate = now.AddHours(-14),
                UpdateDate = now.AddHours(-14),
                Priority = 2,
                IsActive = true,
                ViewCount = 3100,
                IsSecondPageNews = false,
            },

            // DOTNET CATEGORY
            new NewsArticle
            {
                Category = "dotnet",
                Type = "haber",
                Caption = ".NET 9 Yayınlandı: Performans ve Bulut Odaklı Yenilikler",
                Slug = SlugHelper.GenerateSlug(".NET 9 Yayınlandı Performans ve Bulut Odaklı Yenilikler"),
                Keywords = "dotnet, .NET 9, C#, microsoft, geliştirme",
                SocialTags = "#DotNet #CSharp #Microsoft",
                Summary = "Microsoft, .NET 9'u resmi olarak yayınladı. Yeni sürüm, performans iyileştirmeleri ve bulut-native özelliklerle geliyor.",
                ImgPath = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?w=1200&q=80",
                ImgAlt = ".NET 9 Launch",
                ImageUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?w=400&q=80",
                Content = @"<p>Microsoft, <strong>.NET 9</strong>'u Kasım 2025'te yayınladı. Yeni LTS (Long Term Support) sürümü, büyük performans iyileştirmeleri ve bulut-native özellikler getiriyor.</p>

<h2>Yeni Özellikler</h2>
<ul>
<li><strong>C# 13:</strong> Yeni dil özellikleri</li>
<li><strong>Native AOT:</strong> Gelişmiş ahead-of-time compilation</li>
<li><strong>ASP.NET Core:</strong> Minimal API iyileştirmeleri</li>
<li><strong>Blazor:</strong> Daha hızlı rendering</li>
<li><strong>MAUI:</strong> Cross-platform UI geliştirmeleri</li>
</ul>

<h2>Performans İyileştirmeleri</h2>
<p>.NET 9, .NET 8'e göre ortalama %30 daha hızlı:</p>
<ul>
<li>GC (Garbage Collector) optimizasyonları</li>
<li>JIT compiler iyileştirmeleri</li>
<li>Async/await overhead azaltma</li>
<li>String operations hızlandırma</li>
</ul>

<blockquote>"".NET 9, geliştiricilerin bulut-native uygulamaları daha hızlı oluşturmasını sağlıyor."" - Scott Hunter, Microsoft</blockquote>",
                Subjects = new[] { ".NET", "C#", "Development" },
                Authors = new[] { "Dev Team" },
                ExpressDate = now.AddHours(-12),
                CreateDate = now.AddHours(-12),
                UpdateDate = now.AddHours(-12),
                Priority = 1,
                IsActive = true,
                ViewCount = 5100,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "dotnet",
                Type = "haber",
                Caption = "ASP.NET Core Minimal APIs: Modern Web Development",
                Slug = SlugHelper.GenerateSlug("Yapay Zeka Kodlama Araçları Copilot vs Cursor vs Cline"),
                Keywords = "AI, coding, copilot, cursor, cline, development tools",
                SocialTags = "#AI #Coding #DevTools",
                Summary = "2025 yılında geliştiricilerin en çok kullandığı AI kodlama araçları karşılaştırılıyor. Her birinin güçlü ve zayıf yönleri neler?",
                ImgPath = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=1200&q=80",
                ImgAlt = "AI Coding Tools",
                ImageUrl = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=400&q=80",
                Content = @"<p>2025'te geliştiriciler için AI kodlama araçları vazgeçilmez hale geldi. Ancak hangi araç hangi iş için en uygun?</p>

<h2>GitHub Copilot</h2>
<h3>Güçlü Yönleri:</h3>
<ul>
<li>VS Code'a native entegrasyon</li>
<li>Geniş dil desteği</li>
<li>Güçlü code completion</li>
<li>Enterprise SSO desteği</li>
</ul>

<h3>Zayıf Yönleri:</h3>
<ul>
<li>Bazen dikkat dağıtıcı</li>
<li>Context window sınırlı</li>
<li>Multi-file refactoring zayıf</li>
</ul>

<h2>Cursor</h2>
<h3>Güçlü Yönleri:</h3>
<ul>
<li>Mükemmel chat interface</li>
<li>Codebase-wide understanding</li>
<li>Multi-file editing</li>
<li>Custom prompts</li>
</ul>

<h3>Zayıf Yönleri:</h3>
<ul>
<li>Ayrı IDE gerekiyor</li>
<li>VS Code extension'larıyla uyumsuzluk</li>
<li>Ücretli model daha pahalı</li>
</ul>

<h2>Cline (eski Claude Dev)</h2>
<h3>Güçlü Yönleri:</h3>
<ul>
<li>Terminal komutlarını çalıştırabilir</li>
<li>Dosya sistemiyle etkileşim</li>
<li>Claude 3.5 Sonnet gücü</li>
<li>VS Code extension</li>
</ul>

<h3>Zayıf Yönleri:</h3>
<ul>
<li>Manuel onay gerektirir</li>
<li>API key maliyeti yüksek olabilir</li>
<li>Bazen fazla agresif</li>
</ul>

<h2>Sonuç</h2>
<p><strong>Yeni başlayanlar için:</strong> GitHub Copilot<br>
<strong>Büyük refactoring için:</strong> Cursor<br>
<strong>Automation için:</strong> Cline</p>",
                Subjects = new[] { "Technology", "AI", "Development Tools" },
                Authors = new[] { "Tech Review Team" },
                ExpressDate = now.AddHours(-6),
                CreateDate = now.AddHours(-6),
                UpdateDate = now.AddHours(-6),
                Priority = 1,
                IsActive = true,
                ViewCount = 1800,
                IsSecondPageNews = false,
            },

            // Additional AI article
            new NewsArticle
            {
                Category = "artificialintelligence",
                Type = "haber",
                Caption = "Google Gemini 2.0: Multimodal AI'nın Yeni Dönemi",
                Slug = SlugHelper.GenerateSlug("Google Gemini 2.0 Multimodal AI'nın Yeni Dönemi"),
                Keywords = "google, gemini, AI, multimodal, yapay zeka",
                SocialTags = "#Google #Gemini #AI #Multimodal",
                Summary = "Google'ın Gemini 2.0 modeli, metin, görsel, ses ve video işlemede devrim yaratıyor. Gerçek zamanlı multimodal anlayış sunuyor.",
                ImgPath = "https://images.unsplash.com/photo-1573164713714-d95e436ab8d6?w=1200&q=80",
                ImgAlt = "Google Gemini 2.0",
                ImageUrl = "https://images.unsplash.com/photo-1573164713714-d95e436ab8d6?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1573164713714-d95e436ab8d6?w=400&q=80",
                Content = @"<p>Google'ın <strong>Gemini 2.0</strong> modeli, multimodal AI alanında yeni standartlar belirliyor. Model, metin, görsel, ses ve videoyu aynı anda işleyebiliyor.</p>

<h2>Yeni Özellikler</h2>
<ul>
<li><strong>Native Multimodal:</strong> Tüm modality'leri aynı anda anlayabilir</li>
<li><strong>Gerçek Zamanlı İşleme:</strong> Video stream'leri canlı analiz eder</li>
<li><strong>2M Token Context:</strong> Çok uzun içerikleri işleyebilir</li>
<li><strong>Spatial Understanding:</strong> 3D uzaysal farkındalık</li>
</ul>

<h2>Kullanım Senaryoları</h2>
<ul>
<li><strong>Video Analysis:</strong> Video içeriğini anlayıp açıklayabilir</li>
<li><strong>AR/VR Applications:</strong> Artırılmış gerçeklik asistanları</li>
<li><strong>Robotik:</strong> Robot vision ve kontrol</li>
<li><strong>Healthcare:</strong> Medikal görüntü analizi</li>
</ul>

<h2>Performans</h2>
<p>Gemini 2.0, MMMU (Multimodal Understanding) benchmark'ında %91.2 doğruluk oranı ile yeni rekor kırdı.</p>

<blockquote>""Gemini 2.0, yapay zekanın dünyayı insanlar gibi algılamasına en yakın model."" - Sundar Pichai, Google CEO</blockquote>",
                Subjects = new[] { "Google", "AI", "Multimodal" },
                Authors = new[] { "AI Research Team" },
                ExpressDate = now.AddHours(-16),
                CreateDate = now.AddHours(-16),
                UpdateDate = now.AddHours(-16),
                Priority = 1,
                IsActive = true,
                ViewCount = 5400,
                IsSecondPageNews = false,
            },

            // CLAUDEAI CATEGORY
            new NewsArticle
            {
                Category = "claudeai",
                Type = "haber",
                Caption = "Claude 3.5 Sonnet: Kod Yazımında GPT-4'ü Geçti",
                Slug = SlugHelper.GenerateSlug("Claude 3.5 Sonnet Kod Yazımında GPT-4'ü Geçti"),
                Keywords = "claude, anthropic, AI, kod yazımı, GPT-4",
                SocialTags = "#Claude #Anthropic #AI #Coding",
                Summary = "Anthropic'in Claude 3.5 Sonnet modeli, kod yazımı benchmark'larında GPT-4'ü geride bıraktı. Özellikle uzun context window dikkat çekiyor.",
                ImgPath = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=1200&q=80",
                ImgAlt = "Claude 3.5 Sonnet",
                ImageUrl = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1555949963-aa79dcee981c?w=400&q=80",
                Content = @"<p><strong>Claude 3.5 Sonnet</strong>, Anthropic'in en gelişmiş AI modeli olarak piyasaya sürüldü. Model, özellikle kod yazımı ve uzun döküman analizi konularında GPT-4'ü geride bıraktı.</p>

<h2>Öne Çıkan Özellikler</h2>
<ul>
<li><strong>200K Context Window:</strong> Çok uzun kodları analiz edebilir</li>
<li><strong>Mükemmel Kod Kalitesi:</strong> HumanEval'de %89.7</li>
<li><strong>Hızlı:</strong> GPT-4'ten 2x daha hızlı yanıt</li>
<li><strong>Güvenli:</strong> Constitutional AI ile zararlı içerik filtreleme</li>
</ul>

<h2>Kod Yazımı Performansı</h2>
<table>
<tr><th>Benchmark</th><th>Claude 3.5</th><th>GPT-4</th></tr>
<tr><td>HumanEval</td><td>89.7%</td><td>85.4%</td></tr>
<tr><td>MBPP</td><td>87.3%</td><td>84.1%</td></tr>
<tr><td>SWE-bench</td><td>42.1%</td><td>38.6%</td></tr>
</table>

<blockquote>""Claude 3.5, profesyonel geliştiriciler için tasarlandı."" - Anthropic Blog</blockquote>",
                Subjects = new[] { "Claude", "AI", "Coding" },
                Authors = new[] { "AI Editörü" },
                ExpressDate = now.AddHours(-13),
                CreateDate = now.AddHours(-13),
                UpdateDate = now.AddHours(-13),
                Priority = 1,
                IsActive = true,
                ViewCount = 4600,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "claudeai",
                Type = "haber",
                Caption = "Claude ile Projeler: Kişiselleştirilmiş AI Asistanı",
                Slug = SlugHelper.GenerateSlug("Claude ile Projeler Kişiselleştirilmiş AI Asistanı"),
                Keywords = "claude, projects, AI, asistan, kişiselleştirme",
                SocialTags = "#Claude #Projects #AI #Assistant",
                Summary = "Claude'un yeni Projects özelliği, kullanıcıların kendi özel AI asistanlarını oluşturmasına olanak tanıyor.",
                ImgPath = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=1200&q=80",
                ImgAlt = "Claude Projects",
                ImageUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=400&q=80",
                Content = @"<p><strong>Claude Projects</strong>, kullanıcıların kendi özel bilgi tabanlarıyla AI asistanları oluşturmasına olanak tanıyan yeni bir özellik.</p>

<h2>Nasıl Çalışır?</h2>
<ol>
<li>Yeni bir proje oluşturun</li>
<li>İlgili dökümanları yükleyin (PDF, MD, TXT)</li>
<li>Custom instructions ekleyin</li>
<li>AI'ya sorular sorun</li>
</ol>

<h2>Kullanım Senaryoları</h2>
<ul>
<li><strong>Kod Dokümantasyonu:</strong> Proje dokümantasyonunuzu yükleyin</li>
<li><strong>Araştırma Asistanı:</strong> Makalelerinizi analiz edin</li>
<li><strong>Öğrenme:</strong> Ders notlarınızı AI'ya öğretin</li>
<li><strong>İş Süreçleri:</strong> SOP'larınızı yükleyin</li>
</ul>

<blockquote>""Projects, Claude'u gerçekten sizin AI asistanınız yapıyor."" - Claude Pro Kullanıcısı</blockquote>",
                Subjects = new[] { "Claude", "Productivity", "AI" },
                Authors = new[] { "Productivity Team" },
                ExpressDate = now.AddHours(-15),
                CreateDate = now.AddHours(-15),
                UpdateDate = now.AddHours(-15),
                Priority = 2,
                IsActive = true,
                ViewCount = 3200,
                IsSecondPageNews = false,
            },

            // Additional popular news
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "X (Twitter) Yeni Algoritma Güncellemesi: Uzun İçerikler Ön Planda",
                Slug = SlugHelper.GenerateSlug("X Twitter Yeni Algoritma Güncellemesi Uzun İçerikler Ön Planda"),
                Keywords = "twitter, X, algorithm, long form, content",
                SocialTags = "#Twitter #X #Algorithm",
                Summary = "X platformu, algoritmasını güncelledi. Artık uzun formlu içerikler ve thread'ler daha fazla görünürlük kazanacak.",
                ImgPath = "https://images.unsplash.com/photo-1611605698335-8b1569810432?w=1200&q=80",
                ImgAlt = "X Platform Algorithm",
                ImageUrl = "https://images.unsplash.com/photo-1611605698335-8b1569810432?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1611605698335-8b1569810432?w=400&q=80",
                Content = @"<p>X (eski Twitter), içerik algoritmasında önemli değişiklikler yaptı. Yeni güncellemede <strong>uzun formlu içerikler</strong> ve detaylı thread'ler daha fazla boost alacak.</p>

<h2>Değişiklikler</h2>
<ul>
<li>280+ karakter içerikler artı puan</li>
<li>Thread'ler tek tweet'e göre 3x daha fazla reach</li>
<li>Dış link'ler artık cezalandırılmıyor</li>
<li>Video içerikler eskisi gibi öncelikli</li>
</ul>

<h2>Premium Aboneler</h2>
<p>X Premium (mavi tik) aboneleri için ek avantajlar:</p>
<ul>
<li>4000 karakter limiti</li>
<li>Algoritma puanında %40 boost</li>
<li>Reply'lerde öncelik</li>
<li>Edit özelliği</li>
</ul>

<h2>İçerik Üreticileri İçin Öneriler</h2>
<ol>
<li>Thread formatını kullanın</li>
<li>İlk tweet'i dikkat çekici yapın</li>
<li>Her thread'de en az 5 tweet olsun</li>
<li>Görsel/video ekleyin</li>
<li>Engagement için soru sorun</li>
</ol>

<p>Elon Musk, değişikliğin 'X'i gerçek bir tartışma platformu' haline getirmek için yapıldığını belirtti.</p>",
                Subjects = new[] { "Social Media", "Twitter", "Algorithm" },
                Authors = new[] { "Social Media News" },
                ExpressDate = now.AddHours(-8),
                CreateDate = now.AddHours(-8),
                UpdateDate = now.AddHours(-8),
                Priority = 1,
                IsActive = true,
                ViewCount = 2100,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "Twitter Spaces'te Yeni Özellik: Kayıt ve Tekrar İzleme",
                Slug = SlugHelper.GenerateSlug("Twitter Spaces'te Yeni Özellik Kayıt ve Tekrar İzleme"),
                Keywords = "twitter, spaces, recording, replay, audio",
                SocialTags = "#TwitterSpaces #Audio #SocialMedia",
                Summary = "X Spaces artık otomatik kaydedilebiliyor ve 30 gün boyunca tekrar dinlenebiliyor. Podcast alternatifi olma yolunda.",
                ImgPath = "https://images.unsplash.com/photo-1590602847861-f357a9332bbc?w=1200&q=80",
                ImgAlt = "Twitter Spaces",
                ImageUrl = "https://images.unsplash.com/photo-1590602847861-f357a9332bbc?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1590602847861-f357a9332bbc?w=400&q=80",
                Content = @"<p>X (Twitter) Spaces için uzun zamandır beklenen <strong>kayıt özelliği</strong> aktif edildi. Kullanıcılar artık Space'lerini otomatik kaydedebilir ve 30 gün boyunca tekrar dinlenebilir hale getirebilir.</p>

<h2>Yeni Özellikler</h2>
<ul>
<li>Otomatik kayıt seçeneği</li>
<li>30 gün replay süresi</li>
<li>Timestamp'li bölümler</li>
<li>Hız ayarlama (0.5x - 2x)</li>
<li>İndirme seçeneği (Premium)</li>
</ul>

<h2>İçerik Üreticileri İçin Fırsatlar</h2>
<p>Bu özellik, Spaces'i podcast alternatifi haline getiriyor:</p>
<ol>
<li>Canlı yayın + replay avantajı</li>
<li>Anında publish (podcast upload'a gerek yok)</li>
<li>Native X audience'i</li>
<li>Monetization potansiyeli</li>
</ol>

<h2>Teknik Detaylar</h2>
<ul>
<li>Maksimum kayıt süresi: 12 saat</li>
<li>Audio quality: 128kbps</li>
<li>Storage: X Cloud</li>
<li>Format: M4A</li>
</ul>

<blockquote>""Bu özellik, Spaces'i sadece canlı bir deneyim olmaktan çıkarıp kalıcı içerik platformuna dönüştürüyor."" - X Product Team</blockquote>",
                Subjects = new[] { "Twitter", "Audio", "Content Creation" },
                Authors = new[] { "X News Team" },
                ExpressDate = now.AddHours(-10),
                CreateDate = now.AddHours(-10),
                UpdateDate = now.AddHours(-10),
                Priority = 2,
                IsActive = true,
                ViewCount = 1500,
                IsSecondPageNews = false,
            },

            // Additional popular news
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "LinkedIn'de AI Powered İş İlanları Dönemi Başladı",
                Slug = SlugHelper.GenerateSlug("LinkedIn'de AI Powered İş İlanları Dönemi Başladı"),
                Keywords = "linkedin, AI, job posting, recruitment, hiring",
                SocialTags = "#LinkedIn #AI #Recruitment",
                Summary = "LinkedIn, iş ilanları oluşturmak için AI destekli araçlar sunuyor. İş tanımları otomatik olarak optimize ediliyor.",
                ImgPath = "https://images.unsplash.com/photo-1586281380349-632531db7ed4?w=1200&q=80",
                ImgAlt = "LinkedIn AI Recruitment",
                ImageUrl = "https://images.unsplash.com/photo-1586281380349-632531db7ed4?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1586281380349-632531db7ed4?w=400&q=80",
                Content = @"<p>LinkedIn, işe alım sürecini hızlandırmak için <strong>AI-powered iş ilanı oluşturma araçlarını</strong> kullanıma açtı.</p>

<h2>Yeni Özellikler</h2>
<ul>
<li>Otomatik iş tanımı yazma</li>
<li>Skill matching AI</li>
<li>Salary range önerileri</li>
<li>Competitor analysis</li>
<li>SEO optimizasyonu</li>
</ul>

<h2>Nasıl Çalışıyor?</h2>
<ol>
<li>Pozisyon başlığını girin</li>
<li>AI size şablon önerir</li>
<li>Gerekli skill'leri otomatik belirler</li>
<li>Market salary range'i gösterir</li>
<li>Tek tıkla yayınlayın</li>
</ol>

<h2>Recruiter'lar İçin Avantajlar</h2>
<blockquote>""AI sayesinde iş ilanı oluşturma süresi 30 dakikadan 3 dakikaya düştü."" - LinkedIn HR Analytics</blockquote>

<h3>Veri Odaklı Kararlar</h3>
<ul>
<li>Hangi skill'ler trend?</li>
<li>Rakipler ne kadar maaş veriyor?</li>
<li>Hangi keywords daha çok başvuru getiriyor?</li>
</ul>

<h2>Premium Özellikler</h2>
<p>LinkedIn Recruiter aboneleri için ek özellikler:</p>
<ul>
<li>Candidate matching AI</li>
<li>Automated screening questions</li>
<li>Interview scheduling assistant</li>
<li>Culture fit assessment</li>
</ul>

<p>Platform, 2025 sonuna kadar tüm iş ilanlarının %60'ının AI ile oluşturulacağını tahmin ediyor.</p>",
                Subjects = new[] { "LinkedIn", "AI", "Recruitment" },
                Authors = new[] { "LinkedIn Product Team" },
                ExpressDate = now.AddHours(-14),
                CreateDate = now.AddHours(-14),
                UpdateDate = now.AddHours(-14),
                Priority = 1,
                IsActive = true,
                ViewCount = 1200,
                IsSecondPageNews = false,
            },
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "LinkedIn Learning: 2025'te En Çok Talep Gören 10 Skill",
                Slug = SlugHelper.GenerateSlug("LinkedIn Learning 2025'te En Çok Talep Gören 10 Skill"),
                Keywords = "linkedin, learning, skills, career, education",
                SocialTags = "#LinkedInLearning #Skills #Career",
                Summary = "LinkedIn, 2025 yılında iş dünyasında en çok aranan 10 yeteneği açıkladı. AI ve data science ilk sıralarda.",
                ImgPath = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=1200&q=80",
                ImgAlt = "LinkedIn Learning Skills",
                ImageUrl = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=400&q=80",
                Content = @"<p>LinkedIn Learning, 1 milyar veri noktasını analiz ederek 2025'te en çok talep gören becerileri belirledi.</p>

<h2>Top 10 Skills</h2>
<ol>
<li><strong>AI & Machine Learning</strong> - %156 artış</li>
<li><strong>Data Science & Analytics</strong> - %142 artış</li>
<li><strong>Cloud Computing</strong> - %128 artış</li>
<li><strong>Cybersecurity</strong> - %118 artış</li>
<li><strong>Product Management</strong> - %95 artış</li>
<li><strong>UX/UI Design</strong> - %89 artış</li>
<li><strong>DevOps Engineering</strong> - %76 artış</li>
<li><strong>Digital Marketing</strong> - %72 artış</li>
<li><strong>Leadership & Management</strong> - %68 artış</li>
<li><strong>Sustainability</strong> - %64 artış</li>
</ol>

<h2>Sektör Bazında Değişim</h2>

<h3>Tech Sector</h3>
<ul>
<li>AI/ML en kritik skill</li>
<li>Full-stack developer talebi artıyor</li>
<li>Low-code/no-code platformlar popüler</li>
</ul>

<h3>Finance</h3>
<ul>
<li>Blockchain ve crypto knowledge</li>
<li>Regulatory compliance</li>
<li>Quantitative analysis</li>
</ul>

<h3>Healthcare</h3>
<ul>
<li>Healthcare AI</li>
<li>Telemedicine platforms</li>
<li>Patient data analytics</li>
</ul>

<h2>Kariyer Önerileri</h2>
<blockquote>""2025'te başarılı olmak isteyenler, en az 2-3 high-demand skill'e sahip olmalı."" - Ryan Roslansky, LinkedIn CEO</blockquote>

<h3>Nasıl Başlanır?</h3>
<ol>
<li>LinkedIn Learning'de ilginizi çeken bir kurs seçin</li>
<li>Haftalık en az 2 saat ayırın</li>
<li>Projeler yaparak pratik yapın</li>
<li>Sertifika alın ve profile ekleyin</li>
<li>Networking yaparak fırsatları değerlendirin</li>
</ol>

<p>Platform, bu skill'lere sahip profesyonellerin ortalama %34 daha yüksek maaş aldığını belirtiyor.</p>",
                Subjects = new[] { "LinkedIn", "Learning", "Career Development" },
                Authors = new[] { "LinkedIn Research Team" },
                ExpressDate = now.AddDays(-1),
                CreateDate = now.AddDays(-1),
                UpdateDate = now.AddDays(-1),
                Priority = 1,
                IsActive = true,
                ViewCount = 2800,
                IsSecondPageNews = false,
            },

            // More news
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "Meta AI Artık WhatsApp, Instagram ve Facebook'ta Entegre",
                Slug = SlugHelper.GenerateSlug("Meta AI Artık WhatsApp Instagram ve Facebook'ta Entegre"),
                Keywords = "meta, AI, whatsapp, instagram, facebook, integration",
                SocialTags = "#MetaAI #WhatsApp #Instagram",
                Summary = "Meta'nın AI asistanı artık tüm platformlarda kullanılabiliyor. Sohbetlerde AI desteği ve görsel oluşturma özellikleri sunuluyor.",
                ImgPath = "https://images.unsplash.com/photo-1611162617474-5b21e879e113?w=1200&q=80",
                ImgAlt = "Meta AI Integration",
                ImageUrl = "https://images.unsplash.com/photo-1611162617474-5b21e879e113?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1611162617474-5b21e879e113?w=400&q=80",
                Content = @"<p>Meta, kendi AI asistanını WhatsApp, Instagram ve Facebook'a tam entegre etti. Kullanıcılar artık sohbetlerde AI desteği alabiliyor.</p>

<h2>Yeni Özellikler</h2>

<h3>WhatsApp</h3>
<ul>
<li>Chat'lerde @meta ile AI çağırma</li>
<li>Mesaj önerileri</li>
<li>Çeviri desteği (100+ dil)</li>
<li>Voice message transkript</li>
</ul>

<h3>Instagram</h3>
<ul>
<li>DM'lerde AI asistan</li>
<li>Görsel oluşturma (text-to-image)</li>
<li>Caption önerileri</li>
<li>Hashtag optimizasyonu</li>
</ul>

<h3>Facebook</h3>
<ul>
<li>Post yazma yardımı</li>
<li>Görsel düzenleme</li>
<li>Event planning asistanı</li>
<li>Group management tools</li>
</ul>

<h2>Gizlilik</h2>
<p>Meta, AI'nın end-to-end şifreli sohbetlere erişemeyeceğini garanti ediyor:</p>
<blockquote>""Meta AI, yalnızca kullanıcının açıkça paylaştığı mesajları görebilir. E2E şifreli sohbetler tamamen özel kalır.""</blockquote>

<h2>İş Kullanımı</h2>
<p>Business hesaplar için ek özellikler:</p>
<ul>
<li>Customer service automation</li>
<li>Product recommendation</li>
<li>Order tracking</li>
<li>FAQ responses</li>
</ul>

<h2>Rekabet</h2>
<p>Bu hamle, Meta'yı ChatGPT ve Google Bard ile rekabette güçlendiriyor. 3 milyar+ kullanıcıya anında ulaşım, önemli bir avantaj sağlıyor.</p>",
                Subjects = new[] { "Meta", "AI", "Social Media" },
                Authors = new[] { "Meta Newsroom" },
                ExpressDate = now.AddHours(-5),
                CreateDate = now.AddHours(-5),
                UpdateDate = now.AddHours(-5),
                Priority = 1,
                IsActive = true,
                ViewCount = 3500,
                IsSecondPageNews = false,
            },

            // More popular news
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "Instagram Reels Artık 10 Dakikaya Kadar Uzun Olabiliyor",
                Slug = SlugHelper.GenerateSlug("Instagram Reels Artık 10 Dakikaya Kadar Uzun Olabiliyor"),
                Keywords = "instagram, reels, video, content, creator",
                SocialTags = "#Instagram #Reels #ContentCreator",
                Summary = "Instagram, Reels için maksimum süreyi 90 saniyeden 10 dakikaya çıkardı. YouTube Shorts'a karşı hamle.",
                ImgPath = "https://images.unsplash.com/photo-1611162616305-c69b3fa7fbe0?w=1200&q=80",
                ImgAlt = "Instagram Reels",
                ImageUrl = "https://images.unsplash.com/photo-1611162616305-c69b3fa7fbe0?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1611162616305-c69b3fa7fbe0?w=1200&q=80",
                Content = @"<p>Instagram, içerik üreticileri için önemli bir güncelleme yaptı: Reels artık <strong>10 dakikaya kadar</strong> uzun olabiliyor.</p>

<h2>Değişiklikler</h2>
<ul>
<li>Maksimum süre: 90 saniye → 10 dakika</li>
<li>Minimum süre değişmedi (3 saniye)</li>
<li>Tüm hesaplar için geçerli</li>
<li>Aşamalı rollout (2 hafta)</li>
</ul>

<h2>Neden Bu Değişiklik?</h2>
<p>Instagram, YouTube Shorts ve TikTok'un artan rekabetine yanıt veriyor:</p>

<blockquote>""Uzun formlu içerik, izleyicilerin daha fazla engagement göstermesini sağlıyor. Creator'lar daha derin hikayeler anlatabilecek."" - Adam Mosseri, Instagram Head</blockquote>

<h2>Content Creator'lar İçin Fırsatlar</h2>

<h3>Yeni İçerik Türleri</h3>
<ul>
<li>Tutorial ve how-to videoları</li>
<li>Mini vlog'lar</li>
<li>Product review'ları</li>
<li>Behind-the-scenes içerikler</li>
<li>Educational content</li>
</ul>

<h3>Monetization</h3>
<p>Uzun Reels için özel para kazanma seçenekleri:</p>
<ul>
<li>Mid-roll ads (5+ dakika videolarda)</li>
<li>Branded content integration</li>
<li>Bonus programı (view sayısına göre)</li>
</ul>

<h2>Algoritma Değişiklikleri</h2>
<p>Instagram, uzun Reels'leri boost ediyor:</p>
<ul>
<li>Watch time artık daha önemli metrik</li>
<li>Complete rate hesaplanıyor</li>
<li>Share ve save öncelikli</li>
</ul>

<h2>Best Practices</h2>
<ol>
<li><strong>Hook güçlü olsun:</strong> İlk 3 saniye kritik</li>
<li><strong>Bölümlere ayır:</strong> Uzun videoda chapter'lar kullan</li>
<li><strong>Caption detaylı:</strong> Timestamps ekle</li>
<li><strong>CTA koy:</strong> Like, comment, share isteyin</li>
</ol>

<p>Bu değişiklik, Instagram'ın 'kısa video platformu' imajını değiştirerek 'genel video platformu' haline gelme stratejisinin parçası.</p>",
                Subjects = new[] { "Instagram", "Reels", "Social Media" },
                Authors = new[] { "Instagram Creators Team" },
                ExpressDate = now.AddHours(-18),
                CreateDate = now.AddHours(-18),
                UpdateDate = now.AddHours(-18),
                Priority = 1,
                IsActive = true,
                ViewCount = 2900,
                IsSecondPageNews = false,
            },

            // More popular news
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "TikTok Shop Türkiye'de Açılıyor: E-Ticaretin Yeni Dönemi",
                Slug = SlugHelper.GenerateSlug("TikTok Shop Türkiye'de Açılıyor E-Ticaretin Yeni Dönemi"),
                Keywords = "tiktok, shop, e-commerce, turkey, shopping",
                SocialTags = "#TikTokShop #ETicaret #Shopping",
                Summary = "TikTok, Türkiye'de e-ticaret platformunu açıyor. Videolardan direkt alışveriş yapılabilecek.",
                ImgPath = "https://images.unsplash.com/photo-1607082348824-0a96f2a4b9da?w=1200&q=80",
                ImgAlt = "TikTok Shop",
                ImageUrl = "https://images.unsplash.com/photo-1607082348824-0a96f2a4b9da?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1607082348824-0a96f2a4b9da?w=400&q=80",
                Content = @"<p>TikTok, Kasım 2025'te Türkiye'de <strong>TikTok Shop</strong> platformunu açıyor. Kullanıcılar videolardan direkt ürün satın alabilecek.</p>

<h2>Özellikler</h2>

<h3>Kullanıcılar İçin</h3>
<ul>
<li>Video izlerken tek tıkla alışveriş</li>
<li>Live stream satışları</li>
<li>Creator önerileri</li>
<li>Güvenli ödeme sistemi</li>
<li>Hızlı kargo takibi</li>
</ul>

<h3>Satıcılar İçin</h3>
<ul>
<li>Ücretsiz mağaza açma</li>
<li>%5 komisyon (ilk 6 ay %0)</li>
<li>Creator partnership programı</li>
<li>Analytics dashboard</li>
<li>Advertising tools</li>
</ul>

<h2>Nasıl Çalışıyor?</h2>
<ol>
<li><strong>Satıcı:</strong> Ürünü TikTok Shop'a ekler</li>
<li><strong>Creator:</strong> Ürünü videosunda tanıtır</li>
<li><strong>Link:</strong> Video'ya alışveriş linki eklenir</li>
<li><strong>Satış:</strong> Kullanıcı videonun içinden satın alır</li>
<li><strong>Commission:</strong> Creator ve TikTok pay alır</li>
</ol>

<h2>Türkiye Pazarı</h2>
<p>TikTok Türkiye'deki potansiyele inanıyor:</p>
<ul>
<li>30M+ aktif kullanıcı</li>
<li>Günlük 60 dakika ortalama kullanım</li>
<li>Genç demografik (18-34 yaş %65)</li>
<li>Yüksek engagement rate</li>
</ul>

<h2>Rakipler</h2>
<p>Bu hamle, Trendyol, Hepsiburada ve Amazon'a karşı önemli bir rekabet:</p>
<blockquote>""Social commerce, e-ticaretin geleceği. Türkiye'de bu trendi öncü olmak istiyoruz."" - TikTok EMEA Director</blockquote>

<h2>Creator Economy</h2>
<p>Türk influencer'lar için yeni gelir kapısı:</p>
<ul>
<li>Affiliate komisyonları (%10-20)</li>
<li>Sponsored content</li>
<li>Brand partnerships</li>
<li>Live shopping bonusu</li>
</ul>

<h2>Launch Plan</h2>
<ul>
<li><strong>Beta:</strong> Kasım 2025</li>
<li><strong>Public:</strong> Aralık 2025</li>
<li><strong>Hedef:</strong> İlk yıl $500M GMV</li>
</ul>",
                Subjects = new[] { "TikTok", "E-commerce", "Social Commerce" },
                Authors = new[] { "TikTok Business Team" },
                ExpressDate = now.AddDays(-1),
                CreateDate = now.AddDays(-1),
                UpdateDate = now.AddDays(-1),
                Priority = 1,
                IsActive = true,
                ViewCount = 4200,
                IsSecondPageNews = false,
            },

            // More popular news
            new NewsArticle
            {
                Category = "popular",
                Type = "haber",
                Caption = "YouTube Premium Türkiye'de Fiyat Artışı: Yeni Tarifeler Açıklandı",
                Slug = SlugHelper.GenerateSlug("YouTube Premium Türkiye'de Fiyat Artışı Yeni Tarifeler Açıklandı"),
                Keywords = "youtube, premium, pricing, turkey, subscription",
                SocialTags = "#YouTube #Premium #Pricing",
                Summary = "YouTube Premium, Türkiye'de fiyatlarını güncelledi. Bireysel abonelik 59.99 TL'den 89.99 TL'ye çıktı.",
                ImgPath = "https://images.unsplash.com/photo-1611162618071-b39a2ec055fb?w=1200&q=80",
                ImgAlt = "YouTube Premium",
                ImageUrl = "https://images.unsplash.com/photo-1611162618071-b39a2ec055fb?w=1200&q=80",
                ThumbnailUrl = "https://images.unsplash.com/photo-1611162618071-b39a2ec055fb?w=400&q=80",
                Content = @"<p>YouTube, Türkiye'deki Premium abonelik fiyatlarını <strong>1 Kasım 2025</strong> tarihinden itibaren güncelliyor.</p>

<h2>Yeni Fiyatlar</h2>

<table style=""width:100%;border-collapse:collapse;margin:20px 0"">
<thead style=""background:#f0f0f0"">
<tr>
<th style=""padding:12px;text-align:left;border:1px solid #ddd"">Plan</th>
<th style=""padding:12px;text-align:right;border:1px solid #ddd"">Eski Fiyat</th>
<th style=""padding:12px;text-align:right;border:1px solid #ddd"">Yeni Fiyat</th>
<th style=""padding:12px;text-align:right;border:1px solid #ddd"">Artış</th>
</tr>
</thead>
<tbody>
<tr>
<td style=""padding:12px;border:1px solid #ddd"">Bireysel (Aylık)</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd"">59.99 TL</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd"">89.99 TL</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd;color:red"">+50%</td>
</tr>
<tr style=""background:#fafafa"">
<td style=""padding:12px;border:1px solid #ddd"">Öğrenci (Aylık)</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd"">29.99 TL</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd"">44.99 TL</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd;color:red"">+50%</td>
</tr>
<tr>
<td style=""padding:12px;border:1px solid #ddd"">Aile (Aylık)</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd"">89.99 TL</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd"">134.99 TL</td>
<td style=""padding:12px;text-align:right;border:1px solid #ddd;color:red"">+50%</td>
</tr>
</tbody>
</table>

<h2>Premium Özellikleri</h2>
<ul>
<li>Reklamsız izleme</li>
<li>Arka planda oynatma</li>
<li>Offline indirme</li>
<li>YouTube Music Premium dahil</li>
<li>Picture-in-picture mode</li>
<li>Queue management</li>
</ul>

<h2>Neden Artış?</h2>
<p>YouTube'un açıklamasına göre:</p>
<blockquote>""Türkiye'deki enflasyon ve döviz kurları, fiyat güncellemesini gerekli kıldı. Bölgesel ekonomik koşullara uyum sağlıyoruz.""</blockquote>

<h2>Mevcut Aboneler</h2>
<p>Şu anki aboneler için:</p>
<ul>
<li>3 ay boyunca eski fiyat geçerli</li>
<li>Şubat 2026'dan itibaren yeni fiyat</li>
<li>İptal hakkı saklı</li>
</ul>

<h2>Alternatifler</h2>
<p>Kullanıcılar şu alternatiflere bakıyor:</p>
<ol>
<li><strong>YouTube Music:</strong> 54.99 TL (sadece müzik)</li>
<li><strong>Ad blocker:</strong> Ücretsiz ama kurallara aykırı</li>
<li><strong>Aile planı:</strong> 6 kişiye kadar, kişi başı 22.50 TL</li>
</ol>

<h2>Sosyal Medya Tepkileri</h2>
<p>Twitter'da #YouTubePremium trending oldu. Kullanıcılar %50 artışın çok fazla olduğunu belirtiyor. Bazıları aboneliği iptal edeceğini söyledi.</p>

<p>YouTube Türkiye'den resmi açıklama bekleniyor.</p>",
                Subjects = new[] { "YouTube", "Premium", "Pricing" },
                Authors = new[] { "YouTube Turkey Team" },
                ExpressDate = now.AddHours(-20),
                CreateDate = now.AddHours(-20),
                UpdateDate = now.AddHours(-20),
                Priority = 1,
                IsActive = true,
                ViewCount = 5600,
                IsSecondPageNews = false,
            },
        };

        // Generate slugs for all news articles before inserting
        foreach (var article in newsArticles.Where(a => string.IsNullOrEmpty(a.Slug)))
        {
            article.Slug = SlugHelper.GenerateSlug(article.Caption);
        }

        await newsCollection.InsertManyAsync(newsArticles, cancellationToken: CancellationToken.None).ConfigureAwait(false);
        Console.WriteLine($"Successfully seeded {newsArticles.Count} news articles to the database!");
    }
}
