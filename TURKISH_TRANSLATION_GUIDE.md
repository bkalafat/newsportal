# Türkçe Haber Çevirisi ve Formatlama Rehberi

## Genel Bakış

Bu proje, tüm haber içeriklerinin **tamamen Türkçe** olması ve **düzgün HTML formatında** sunulması için güncellendi.

## Yapılan Değişiklikler

### 1. Kategori İsimleri Düzeltildi ✅

**Önceki Durum:**
- Kategoriler Türkçe büyük harfle yazılıyordu: `"Teknoloji"`, `"İş"`, `"Eğlence"`
- Bu, validator ile uyumsuzdu ve hatalara neden oluyordu

**Yeni Durum:**
- Tüm kategoriler artık geçerli lowercase İngilizce teknik isimlerle:
  - `githubcopilot` - GitHub Copilot ile ilgili haberler
  - `artificialintelligence` - Yapay zeka haberleri
  - `popular` - Popüler haberler (sosyal medya, genel)
  - `openai` - OpenAI haberleri
  - `robotics` - Robotik haberleri
  - `deepseek` - DeepSeek haberleri
  - `dotnet` - .NET haberleri
  - `claudeai` - Claude AI haberleri
  - `mcp` - Model Context Protocol haberleri

### 2. İçerik Tamamen Türkçe ✅

Tüm haber içerikleri zaten Türkçe:
- ✅ **Caption** (Başlık): Türkçe
- ✅ **Summary** (Özet): Türkçe
- ✅ **Content** (İçerik): Türkçe HTML formatında
- ✅ **Keywords** (Anahtar kelimeler): Türkçe
- ✅ **Social Tags** (Sosyal etiketler): Türkçe/hashtag

### 3. HTML Formatı Düzenlendi ✅

Tüm haber içerikleri düzgün HTML yapısıyla:
- `<p>` - Paragraflar
- `<h2>`, `<h3>` - Başlıklar (hiyerarşik)
- `<ul>`, `<ol>`, `<li>` - Listeler
- `<blockquote>` - Alıntılar
- `<strong>` - Vurgular
- `<table>` - Tablolar (gerektiğinde)
- `<img>` - Görseller (gerektiğinde)

## Veritabanını Türkçe Verilerle Güncelleme

### Yöntem 1: Docker Ortamında (Önerilen)

1. **Docker servislerini başlat:**
```bash
cd /path/to/newsportal
docker compose up -d
```

2. **Backend'in hazır olmasını bekle (30-60 saniye):**
```bash
docker compose logs -f newsportal-backend
# "Now listening on: http://[::]:8080" mesajını görene kadar bekle
```

3. **Eski İngilizce içerikleri temizle:**
```bash
curl -X POST http://localhost:5000/api/seed/cleanup-low-quality
```

Bu endpoint:
- İçeriği 500 karakterden kısa olan haberleri siler
- Türkçe karakter içermeyen haberleri siler
- İngilizce kelime kalıpları içeren haberleri siler
- Görsel olmayan haberleri siler

4. **Yeni Türkçe haberleri yükle:**
```bash
# Ana haberler (15 adet - sosyal medya, teknoloji)
curl -X POST http://localhost:5000/api/seed/news

# Reddit haberleri (5 adet - GitHub Copilot)
curl -X POST http://localhost:5000/api/seed/reddit
```

5. **Tarihleri düzelt (eğer 1970 tarihleri varsa):**
```bash
curl -X POST http://localhost:5000/api/seed/fix-dates
```

6. **Sonuçları doğrula:**
```bash
# Tüm haberleri listele
curl http://localhost:5000/api/newsarticle

# Kategoriye göre listele
curl http://localhost:5000/api/newsarticle?category=githubcopilot
curl http://localhost:5000/api/newsarticle?category=popular
```

### Yöntem 2: Production (Azure/Heroku)

Eğer production ortamındaysanız:

```bash
# Cleanup endpoint'ini çağır
curl -X POST https://your-production-url.com/api/seed/cleanup-low-quality

# Seed endpoint'ini çağır
curl -X POST https://your-production-url.com/api/seed/news
curl -X POST https://your-production-url.com/api/seed/reddit
```

## Seed Verileri İçeriği

### SeedNewsData.cs (15 Haber)

1. **GitHub Copilot Kategorisi (6 haber):**
   - GitHub Enterprise Cloud Çift Ücretlendirme Sorunu
   - GitHub Ana Sayfasında Activity Bölümü Kayboldu
   - GitHub Copilot Actions PR'larda Çöktü mü?
   - Copilot'u Kapattıktan Sonra Kodlamanın Ne Kadar Stresli Olduğunu Fark Ettim
   - Kişisel vs Kurumsal GitHub Hesapları Güvenlik Sorunu
   - GitHub Copilot Pro Ücretsiz Erişimi Kaybedilebilir mi?

2. **Artificial Intelligence Kategorisi (4 haber):**
   - Yapay Zeka Kodlama Araçları: Copilot vs Cursor vs Cline
   - LinkedIn'de AI Powered İş İlanları Dönemi Başladı
   - LinkedIn Learning: 2025'te En Çok Talep Gören 10 Skill
   - Meta AI Artık WhatsApp, Instagram ve Facebook'ta Entegre

3. **Popular Kategorisi (5 haber):**
   - X (Twitter) Yeni Algoritma Güncellemesi: Uzun İçerikler Ön Planda
   - Twitter Spaces'te Yeni Özellik: Kayıt ve Tekrar İzleme
   - Instagram Reels Artık 10 Dakikaya Kadar Uzun Olabiliyor
   - TikTok Shop Türkiye'de Açılıyor: E-Ticaretin Yeni Dönemi
   - YouTube Premium Türkiye'de Fiyat Artışı: Yeni Tarifeler Açıklandı

### SeedRedditNewsData.cs (5 Haber)

Tümü **GitHub Copilot Kategorisi:**
- GitHub Enterprise Cloud Faturalandırma Sorunu: İki Kat Ödeme
- Geliştirici: 'GitHub Copilot Olmadan Kodlamak Daha Az Stresli'
- GitHub Copilot Pro Ücretsiz Erişimi Kaybetme Endişesi
- GitHub Hesap Politikası: Kişisel mi İş için mi?
- GitHub Ana Sayfa Kenar Çubuğunda Aktivite Bölümü Kayboldu

## HTML İçerik Formatı Örnekleri

### Paragraf ve Başlık
```html
<p>Reddit kullanıcısı stepanokdev, <strong>GitHub Enterprise Cloud</strong> hesabında yaşadığı faturalama sorununu paylaştı.</p>

<h2>Destek Ekibi Yanıt Vermiyor</h2>
<p>Kullanıcı, 3 hafta önce açtığı destek talebine hala yanıt alamadığını belirtiyor.</p>
```

### Alıntı (Blockquote)
```html
<blockquote>"Enterprise hesapların 24 saat içinde yanıt alması gerekmiyor mu? Neredeyse bir aydır bekliyorum."</blockquote>
```

### Liste
```html
<h2>Detaylar</h2>
<ul>
<li>Fatura No: INV102226125</li>
<li>Beklenen Ücret: ~$84</li>
<li>Çekilen Ücret: $168</li>
<li>GitHub Actions: $0 (tamamen indirimli)</li>
<li>Copilot: Devre dışı</li>
</ul>
```

### Tablo (LinkedIn Skills örneği)
```html
<h2>Top 10 Skills</h2>
<ol>
<li><strong>AI & Machine Learning</strong> - %156 artış</li>
<li><strong>Data Science & Analytics</strong> - %142 artış</li>
<li><strong>Cloud Computing</strong> - %128 artış</li>
</ol>
```

## Doğrulama Checklist

Veritabanı güncellendikten sonra kontrol edin:

- [ ] Tüm haber başlıkları Türkçe mi?
- [ ] Tüm haber özetleri Türkçe mi?
- [ ] Tüm haber içerikleri Türkçe mi?
- [ ] Kategoriler geçerli mi? (lowercase: githubcopilot, popular, vb.)
- [ ] HTML formatı düzgün mü? (açılan taglar kapatılmış mı?)
- [ ] Görsel URL'leri çalışıyor mu?
- [ ] Tarihler doğru mu? (1970 değil)
- [ ] Her haberde en az 500 karakter içerik var mı?

## Frontend Entegrasyonu

Frontend zaten Türkçe desteğine sahip:
- `SlugHelper.GenerateSlug()` fonksiyonu Türkçe karakterleri düzgün handle ediyor
- Frontend bileşenleri Türkçe içeriği doğru şekilde gösteriyor
- SEO meta tagları Türkçe karakterlerle uyumlu

## Sorun Giderme

### "Category must be one of: popular, artificialintelligence..." hatası
- **Sebep:** Kategori ismi geçersiz
- **Çözüm:** Seed dosyalarında kategori isimlerini kontrol edin, lowercase ve İngilizce olmalı

### "Non-Turkish content detected" ve silinme
- **Sebep:** cleanup-low-quality endpoint İngilizce içerik tespit etti
- **Çözüm:** Normal, İngilizce içerikler temizleniyor. Yeni Türkçe seed verilerini yükleyin

### Tarihler 1970 gösteriyor
- **Sebep:** Bazı eski kayıtlar default DateTime değerine sahip
- **Çözüm:** `POST /api/seed/fix-dates` endpoint'ini çağırın

### Kategoriler frontend'de görünmüyor
- **Sebep:** Cache sorunları
- **Çözüm:** Backend'i restart edin veya cache'i temizleyin (cleanup çağrısı zaten temizler)

## Teknik Detaylar

### Kategori Validasyonu
```csharp
var allowedCategories = new[] { 
    "popular", 
    "artificialintelligence", 
    "githubcopilot", 
    "mcp", 
    "openai", 
    "robotics", 
    "deepseek", 
    "dotnet", 
    "claudeai" 
};
```

### Türkçe Karakter Tespiti
```csharp
var turkishChars = new[] { 'ı', 'ğ', 'ü', 'ş', 'ö', 'ç', 'İ', 'Ğ', 'Ü', 'Ş', 'Ö', 'Ç' };
```

### Slug Oluşturma (Türkçe Desteği)
```csharp
// "GitHub Copilot Actions PR'larda Çöktü mü?" 
// -> "github-copilot-actions-prlarda-coktu-mu"
```

## Özet

✅ **Tamamlandı:**
- Tüm seed verileri Türkçe
- HTML formatları düzenlendi
- Kategori isimleri validator ile uyumlu hale getirildi
- Cleanup endpoint mevcut

🎯 **Yapılması Gereken:**
1. Docker compose ile backend'i başlat
2. cleanup-low-quality endpoint'ini çağır
3. news ve reddit seed endpoint'lerini çağır
4. Frontend'de doğrula

Bu adımları tamamladıktan sonra tüm haberler **tamamen Türkçe** ve **düzgün formatlanmış** olacaktır! 🇹🇷
