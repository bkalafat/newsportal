#!/usr/bin/env pwsh
# Türkçe Haber Veritabanı Yenileme Script
# Bu script veritabanındaki İngilizce haberleri temizler ve Türkçe haberleri yükler

param(
    [string]$BackendUrl = "http://localhost:5000",
    [switch]$SkipCleanup,
    [switch]$OnlyCleanup
)

$ErrorActionPreference = "Stop"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Türkçe Haber Veritabanı Yenileme" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Function to make HTTP requests
function Invoke-ApiRequest {
    param(
        [string]$Endpoint,
        [string]$Method = "POST",
        [string]$Description
    )
    
    Write-Host "⏳ $Description..." -ForegroundColor Yellow
    
    try {
        $url = "$BackendUrl$Endpoint"
        $response = Invoke-RestMethod -Uri $url -Method $Method -TimeoutSec 300
        
        Write-Host "✅ $Description - Başarılı!" -ForegroundColor Green
        
        if ($response.message) {
            Write-Host "   📝 $($response.message)" -ForegroundColor Gray
        }
        
        if ($response.fetched) {
            Write-Host "   📊 Getirilen: $($response.fetched)" -ForegroundColor Gray
        }
        if ($response.created) {
            Write-Host "   ✨ Oluşturulan: $($response.created)" -ForegroundColor Gray
        }
        if ($response.saved) {
            Write-Host "   💾 Kaydedilen: $($response.saved)" -ForegroundColor Gray
        }
        if ($response.totalDeleted) {
            Write-Host "   🗑️  Silinen: $($response.totalDeleted)" -ForegroundColor Gray
        }
        if ($response.totalFixed) {
            Write-Host "   🔧 Düzeltilen: $($response.totalFixed)" -ForegroundColor Gray
        }
        
        Write-Host ""
        return $true
    }
    catch {
        Write-Host "❌ $Description - Hata!" -ForegroundColor Red
        Write-Host "   $($_.Exception.Message)" -ForegroundColor Red
        Write-Host ""
        return $false
    }
}

# Test backend connection
Write-Host "🔍 Backend bağlantısı kontrol ediliyor..." -ForegroundColor Yellow
try {
    $healthCheck = Invoke-RestMethod -Uri "$BackendUrl/health" -Method GET -TimeoutSec 10
    Write-Host "✅ Backend hazır!" -ForegroundColor Green
    Write-Host ""
}
catch {
    Write-Host "❌ Backend'e bağlanılamıyor: $BackendUrl" -ForegroundColor Red
    Write-Host "   Lütfen backend'in çalıştığından emin olun:" -ForegroundColor Yellow
    Write-Host "   docker compose up -d" -ForegroundColor Cyan
    Write-Host ""
    exit 1
}

# Step 1: Cleanup low-quality/English content
if (-not $SkipCleanup) {
    $success = Invoke-ApiRequest `
        -Endpoint "/api/seed/cleanup-low-quality" `
        -Description "İngilizce ve düşük kaliteli haberler temizleniyor"
    
    if (-not $success) {
        Write-Host "⚠️  Cleanup başarısız oldu, ama devam ediyoruz..." -ForegroundColor Yellow
        Write-Host ""
    }
}

if ($OnlyCleanup) {
    Write-Host "================================================" -ForegroundColor Cyan
    Write-Host "✅ Cleanup tamamlandı!" -ForegroundColor Green
    Write-Host "================================================" -ForegroundColor Cyan
    exit 0
}

# Step 2: Fix dates (if any articles have 1970 dates)
$success = Invoke-ApiRequest `
    -Endpoint "/api/seed/fix-dates" `
    -Description "Tarihler düzeltiliyor"

# Step 3: Seed main Turkish news
$success = Invoke-ApiRequest `
    -Endpoint "/api/seed/news" `
    -Description "Türkçe ana haberler yükleniyor (15 adet)"

if (-not $success) {
    Write-Host "⚠️  Ana haberler yüklenemedi!" -ForegroundColor Red
    Write-Host ""
}

# Step 4: Seed Reddit Turkish news
$success = Invoke-ApiRequest `
    -Endpoint "/api/seed/reddit" `
    -Description "Türkçe Reddit haberleri yükleniyor (5 adet)"

if (-not $success) {
    Write-Host "⚠️  Reddit haberleri yüklenemedi!" -ForegroundColor Red
    Write-Host ""
}

# Final verification
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "🔍 Veritabanı durumu kontrol ediliyor..." -ForegroundColor Yellow
Write-Host ""

try {
    $allNews = Invoke-RestMethod -Uri "$BackendUrl/api/newsarticle" -Method GET
    $newsCount = $allNews.Count
    
    Write-Host "📊 Toplam haber sayısı: $newsCount" -ForegroundColor Green
    Write-Host ""
    
    # Count by category
    $categories = $allNews | Group-Object -Property category | Sort-Object Count -Descending
    
    Write-Host "📁 Kategorilere göre dağılım:" -ForegroundColor Cyan
    foreach ($cat in $categories) {
        Write-Host "   - $($cat.Name): $($cat.Count) haber" -ForegroundColor Gray
    }
    Write-Host ""
    
    # Check if any news is in Turkish
    $turkishNews = $allNews | Where-Object { 
        $_.caption -match '[ığüşöçİĞÜŞÖÇ]' -or 
        $_.summary -match '[ığüşöçİĞÜŞÖÇ]'
    }
    
    $turkishCount = $turkishNews.Count
    $turkishPercentage = [math]::Round(($turkishCount / $newsCount) * 100, 1)
    
    if ($turkishPercentage -gt 80) {
        Write-Host "✅ Haberler Türkçe: $turkishCount / $newsCount (%$turkishPercentage)" -ForegroundColor Green
    }
    elseif ($turkishPercentage -gt 50) {
        Write-Host "⚠️  Haberler kısmen Türkçe: $turkishCount / $newsCount (%$turkishPercentage)" -ForegroundColor Yellow
    }
    else {
        Write-Host "❌ Çoğu haber Türkçe değil: $turkishCount / $newsCount (%$turkishPercentage)" -ForegroundColor Red
    }
    Write-Host ""
}
catch {
    Write-Host "⚠️  Haber sayısı alınamadı" -ForegroundColor Yellow
    Write-Host ""
}

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "✅ İşlem tamamlandı!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "🌐 Frontend'i kontrol edin:" -ForegroundColor Cyan
Write-Host "   http://localhost:3000" -ForegroundColor Gray
Write-Host ""
Write-Host "📖 Backend API'yi kontrol edin:" -ForegroundColor Cyan
Write-Host "   $BackendUrl/swagger" -ForegroundColor Gray
Write-Host ""

# Examples
Write-Host "📝 Örnek API çağrıları:" -ForegroundColor Cyan
Write-Host "   # Tüm haberleri listele" -ForegroundColor Gray
Write-Host "   curl $BackendUrl/api/newsarticle" -ForegroundColor DarkGray
Write-Host ""
Write-Host "   # Kategoriye göre listele" -ForegroundColor Gray
Write-Host "   curl $BackendUrl/api/newsarticle?category=githubcopilot" -ForegroundColor DarkGray
Write-Host "   curl $BackendUrl/api/newsarticle?category=popular" -ForegroundColor DarkGray
Write-Host ""
