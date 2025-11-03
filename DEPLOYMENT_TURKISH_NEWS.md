# Türkçe Haberler - Hızlı Dağıtım Kılavuzu

> **Amaç:** Veritabanındaki tüm haberleri Türkçe'ye çevirmek ve düzgün formatlamak

## 🚀 Hızlı Başlangıç (3 Adım)

### 1️⃣ Backend'i Başlat

```bash
# Docker ile (Önerilen)
cd newsportal
docker compose up -d

# Backend'in hazır olmasını bekle (30-60 saniye)
docker compose logs -f newsportal-backend
# "Now listening on: http://[::]:8080" mesajını bekle
```

### 2️⃣ Türkçe Haberleri Yükle

**Windows PowerShell:**
```powershell
.\reseed-turkish-news.ps1
```

**Linux/Mac:**
```bash
./reseed-turkish-news.sh
```

**Manuel (cURL):**
```bash
# 1. İngilizce haberleri temizle
curl -X POST http://localhost:5000/api/seed/cleanup-low-quality

# 2. Ana haberleri yükle (15 adet)
curl -X POST http://localhost:5000/api/seed/news

# 3. Reddit haberlerini yükle (5 adet)
curl -X POST http://localhost:5000/api/seed/reddit
```

### 3️⃣ Doğrula

```bash
# Tüm haberleri listele
curl http://localhost:5000/api/newsarticle | jq .

# Kategori bazlı
curl http://localhost:5000/api/newsarticle?category=githubcopilot
curl http://localhost:5000/api/newsarticle?category=popular
```

**Tarayıcıda:**
- Backend API: http://localhost:5000/swagger
- Frontend: http://localhost:3000

## 📊 Yüklenen Haberler

### Ana Haberler (15 adet)

| Kategori | Haber Sayısı | Konular |
|----------|--------------|---------|
| `githubcopilot` | 6 | GitHub Enterprise, Copilot kullanımı, güvenlik |
| `artificialintelligence` | 4 | AI araçları, LinkedIn AI, Meta AI |
| `popular` | 5 | Twitter/X, Instagram, TikTok, YouTube |

### Reddit Haberleri (5 adet)

Tümü `githubcopilot` kategorisinde:
- GitHub Enterprise faturalandırma
- Copilot kullanıcı deneyimi
- Ücretsiz erişim politikası
- Hesap güvenliği
- UI değişiklikleri

## 🔧 Script Seçenekleri

### PowerShell Script

```powershell
# Normal kullanım (cleanup + seed)
.\reseed-turkish-news.ps1

# Cleanup atla, sadece seed
.\reseed-turkish-news.ps1 -SkipCleanup

# Sadece cleanup yap
.\reseed-turkish-news.ps1 -OnlyCleanup

# Farklı backend URL
.\reseed-turkish-news.ps1 -BackendUrl "http://your-server:5000"
```

### Bash Script

```bash
# Normal kullanım (cleanup + seed)
./reseed-turkish-news.sh

# Cleanup atla, sadece seed
./reseed-turkish-news.sh --skip-cleanup

# Sadece cleanup yap
./reseed-turkish-news.sh --only-cleanup

# Farklı backend URL
./reseed-turkish-news.sh --backend-url "http://your-server:5000"
```

## 📋 İçerik Formatı

### ✅ Türkçe İçerik
- **Başlıklar (Caption):** Tamamen Türkçe
- **Özetler (Summary):** Tamamen Türkçe
- **İçerikler (Content):** Türkçe HTML formatında

### 📝 HTML Formatı
Tüm haberler düzgün HTML yapısıyla:
```html
<h2>Başlık</h2>
<p>Paragraf metni...</p>

<ul>
  <li>Liste öğesi 1</li>
  <li>Liste öğesi 2</li>
</ul>

<blockquote>Alıntı metni</blockquote>

<table>
  <tr><th>Başlık</th><td>Veri</td></tr>
</table>
```

## 🎯 Kategoriler

Tüm kategoriler **lowercase İngilizce** (teknik gereksinim):

| Kategori | Türkçe Anlamı | Kullanım |
|----------|---------------|----------|
| `popular` | Popüler | Sosyal medya, genel haberler |
| `artificialintelligence` | Yapay Zeka | AI araçları, machine learning |
| `githubcopilot` | GitHub Copilot | GitHub, Copilot, kod geliştirme |
| `openai` | OpenAI | OpenAI, ChatGPT |
| `robotics` | Robotik | Robot, otomasyon |
| `deepseek` | DeepSeek | DeepSeek AI |
| `dotnet` | .NET | .NET, C# |
| `claudeai` | Claude AI | Anthropic Claude |
| `mcp` | MCP | Model Context Protocol |

## 🔍 Doğrulama Adımları

Script otomatik olarak kontrol eder, ama manuel doğrulama için:

### 1. Haber Sayısı
```bash
curl -s http://localhost:5000/api/newsarticle | jq length
# Beklenen: 20 (15 ana + 5 reddit)
```

### 2. Türkçe İçerik
```bash
# Başlıklar Türkçe mi?
curl -s http://localhost:5000/api/newsarticle | jq '.[].caption'

# İlk haberin içeriği
curl -s http://localhost:5000/api/newsarticle | jq '.[0]'
```

### 3. Kategoriler Geçerli mi?
```bash
# Tüm kategorileri listele
curl -s http://localhost:5000/api/newsarticle | jq -r '.[].category' | sort | uniq

# Beklenen kategoriler:
# artificialintelligence
# githubcopilot
# popular
```

### 4. HTML Formatı Doğru mu?
```bash
# İlk haberin içeriğinde HTML tagları var mı?
curl -s http://localhost:5000/api/newsarticle | jq -r '.[0].content' | head -20

# <h2>, <p>, <ul>, <li> gibi taglar görmelisiniz
```

## ⚠️ Sorun Giderme

### Backend Başlamıyor
```bash
# Logları kontrol et
docker compose logs newsportal-backend

# SSL sertifika hatası varsa
docker compose down
docker compose build --no-cache
docker compose up -d
```

### Cleanup Tüm Haberleri Siliyor
**Normal davranış!** Cleanup endpoint şunları siler:
- 500 karakterden kısa içerikler
- Türkçe karakter içermeyen içerikler
- İngilizce kelime kalıpları içeren başlıklar
- Görsel olmayan haberler

Çözüm: Seed endpoint'lerini tekrar çağırın.

### "Category must be one of..." Hatası
**Eski seed verisi.** Bu PR'daki güncellemeler kategorileri düzeltti:
```bash
# Yeni kodu çek
git pull origin copilot/translate-news-to-turkish

# Backend'i yeniden başlat
docker compose restart newsportal-backend
```

### Tarihler 1970 Gösteriyor
```bash
# Tarihleri düzelt
curl -X POST http://localhost:5000/api/seed/fix-dates
```

### Cache Sorunları
Backend restart ederken cache otomatik temizlenir:
```bash
docker compose restart newsportal-backend
```

## 🌐 Production Deployment

### Azure/Heroku
```bash
# Environment variable'ları ayarla
BACKEND_URL="https://your-production-backend.azurewebsites.net"

# Script'i çalıştır
./reseed-turkish-news.sh --backend-url "$BACKEND_URL"
```

### Netlify Frontend
Netlify otomatik ISR revalidation destekliyor:
- Backend seed işlemi sonrası otomatik trigger
- Yapılandırma: `appsettings.json` → `NetlifySettings`

## 📚 Ek Kaynaklar

- **Detaylı Rehber:** [TURKISH_TRANSLATION_GUIDE.md](./TURKISH_TRANSLATION_GUIDE.md)
- **Deployment Genel:** [DEPLOYMENT_QUICK_START.md](./DEPLOYMENT_QUICK_START.md)
- **API Dokümantasyonu:** http://localhost:5000/swagger

## ✅ Checklist

Son kontrol listesi:

- [ ] Backend çalışıyor (`docker compose ps`)
- [ ] Cleanup başarılı (`cleanup-low-quality`)
- [ ] Ana haberler yüklendi (`seed/news` - 15 adet)
- [ ] Reddit haberleri yüklendi (`seed/reddit` - 5 adet)
- [ ] Toplam ~20 haber var
- [ ] Tüm başlıklar Türkçe
- [ ] Kategoriler geçerli (lowercase)
- [ ] HTML formatı doğru
- [ ] Frontend'de haberler görünüyor

## 🎉 Başarı!

Script başarıyla çalıştıysa:

```
✅ İşlem tamamlandı!
🌐 Frontend'i kontrol edin: http://localhost:3000
📖 Backend API'yi kontrol edin: http://localhost:5000/swagger
```

Tüm haberler artık **tamamen Türkçe** ve **düzgün formatlanmış**! 🇹🇷
