#!/bin/bash
# Türkçe Haber Veritabanı Yenileme Script
# Bu script veritabanındaki İngilizce haberleri temizler ve Türkçe haberleri yükler

set -e

# Default values
BACKEND_URL="${BACKEND_URL:-http://localhost:5000}"
SKIP_CLEANUP=false
ONLY_CLEANUP=false

# Parse arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --backend-url)
            BACKEND_URL="$2"
            shift 2
            ;;
        --skip-cleanup)
            SKIP_CLEANUP=true
            shift
            ;;
        --only-cleanup)
            ONLY_CLEANUP=true
            shift
            ;;
        *)
            echo "Unknown option: $1"
            echo "Usage: $0 [--backend-url URL] [--skip-cleanup] [--only-cleanup]"
            exit 1
            ;;
    esac
done

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
GRAY='\033[0;90m'
NC='\033[0m' # No Color

echo -e "${CYAN}================================================${NC}"
echo -e "${CYAN}Türkçe Haber Veritabanı Yenileme${NC}"
echo -e "${CYAN}================================================${NC}"
echo ""

# Function to make HTTP requests
make_request() {
    local endpoint="$1"
    local description="$2"
    
    echo -e "${YELLOW}⏳ ${description}...${NC}"
    
    local url="${BACKEND_URL}${endpoint}"
    local response
    
    if response=$(curl -s -f -X POST "$url" -w "\nHTTP_CODE:%{http_code}" 2>&1); then
        local http_code=$(echo "$response" | grep "HTTP_CODE:" | cut -d: -f2)
        local body=$(echo "$response" | sed '/HTTP_CODE:/d')
        
        echo -e "${GREEN}✅ ${description} - Başarılı!${NC}"
        
        # Parse JSON response (basic parsing)
        if echo "$body" | grep -q "message"; then
            local message=$(echo "$body" | grep -o '"message":"[^"]*"' | cut -d'"' -f4)
            echo -e "   ${GRAY}📝 ${message}${NC}"
        fi
        
        if echo "$body" | grep -q "fetched"; then
            local fetched=$(echo "$body" | grep -o '"fetched":[0-9]*' | cut -d: -f2)
            echo -e "   ${GRAY}📊 Getirilen: ${fetched}${NC}"
        fi
        
        if echo "$body" | grep -q "created"; then
            local created=$(echo "$body" | grep -o '"created":[0-9]*' | cut -d: -f2)
            echo -e "   ${GRAY}✨ Oluşturulan: ${created}${NC}"
        fi
        
        if echo "$body" | grep -q "saved"; then
            local saved=$(echo "$body" | grep -o '"saved":[0-9]*' | cut -d: -f2)
            echo -e "   ${GRAY}💾 Kaydedilen: ${saved}${NC}"
        fi
        
        if echo "$body" | grep -q "totalDeleted"; then
            local deleted=$(echo "$body" | grep -o '"totalDeleted":[0-9]*' | cut -d: -f2)
            echo -e "   ${GRAY}🗑️  Silinen: ${deleted}${NC}"
        fi
        
        if echo "$body" | grep -q "totalFixed"; then
            local fixed=$(echo "$body" | grep -o '"totalFixed":[0-9]*' | cut -d: -f2)
            echo -e "   ${GRAY}🔧 Düzeltilen: ${fixed}${NC}"
        fi
        
        echo ""
        return 0
    else
        echo -e "${RED}❌ ${description} - Hata!${NC}"
        echo -e "   ${RED}${response}${NC}"
        echo ""
        return 1
    fi
}

# Test backend connection
echo -e "${YELLOW}🔍 Backend bağlantısı kontrol ediliyor...${NC}"
if curl -s -f "${BACKEND_URL}/health" > /dev/null 2>&1; then
    echo -e "${GREEN}✅ Backend hazır!${NC}"
    echo ""
else
    echo -e "${RED}❌ Backend'e bağlanılamıyor: ${BACKEND_URL}${NC}"
    echo -e "   ${YELLOW}Lütfen backend'in çalıştığından emin olun:${NC}"
    echo -e "   ${CYAN}docker compose up -d${NC}"
    echo ""
    exit 1
fi

# Step 1: Cleanup low-quality/English content
if [ "$SKIP_CLEANUP" = false ]; then
    if ! make_request "/api/seed/cleanup-low-quality" "İngilizce ve düşük kaliteli haberler temizleniyor"; then
        echo -e "${YELLOW}⚠️  Cleanup başarısız oldu, ama devam ediyoruz...${NC}"
        echo ""
    fi
fi

if [ "$ONLY_CLEANUP" = true ]; then
    echo -e "${CYAN}================================================${NC}"
    echo -e "${GREEN}✅ Cleanup tamamlandı!${NC}"
    echo -e "${CYAN}================================================${NC}"
    exit 0
fi

# Step 2: Fix dates (if any articles have 1970 dates)
make_request "/api/seed/fix-dates" "Tarihler düzeltiliyor" || true

# Step 3: Seed main Turkish news
if ! make_request "/api/seed/news" "Türkçe ana haberler yükleniyor (15 adet)"; then
    echo -e "${RED}⚠️  Ana haberler yüklenemedi!${NC}"
    echo ""
fi

# Step 4: Seed Reddit Turkish news
if ! make_request "/api/seed/reddit" "Türkçe Reddit haberleri yükleniyor (5 adet)"; then
    echo -e "${RED}⚠️  Reddit haberleri yüklenemedi!${NC}"
    echo ""
fi

# Final verification
echo -e "${CYAN}================================================${NC}"
echo -e "${YELLOW}🔍 Veritabanı durumu kontrol ediliyor...${NC}"
echo ""

if response=$(curl -s -f "${BACKEND_URL}/api/newsarticle" 2>&1); then
    # Count news (basic JSON array counting)
    news_count=$(echo "$response" | grep -o '"id"' | wc -l)
    
    echo -e "${GREEN}📊 Toplam haber sayısı: ${news_count}${NC}"
    echo ""
    
    # Check for Turkish characters
    if echo "$response" | grep -q '[ığüşöçİĞÜŞÖÇ]'; then
        echo -e "${GREEN}✅ Haberler Türkçe içeriyor${NC}"
    else
        echo -e "${YELLOW}⚠️  Türkçe karakter tespit edilemedi${NC}"
    fi
    echo ""
else
    echo -e "${YELLOW}⚠️  Haber sayısı alınamadı${NC}"
    echo ""
fi

echo -e "${CYAN}================================================${NC}"
echo -e "${GREEN}✅ İşlem tamamlandı!${NC}"
echo -e "${CYAN}================================================${NC}"
echo ""
echo -e "${CYAN}🌐 Frontend'i kontrol edin:${NC}"
echo -e "   ${GRAY}http://localhost:3000${NC}"
echo ""
echo -e "${CYAN}📖 Backend API'yi kontrol edin:${NC}"
echo -e "   ${GRAY}${BACKEND_URL}/swagger${NC}"
echo ""

# Examples
echo -e "${CYAN}📝 Örnek API çağrıları:${NC}"
echo -e "   ${GRAY}# Tüm haberleri listele${NC}"
echo -e "   ${GRAY}curl ${BACKEND_URL}/api/newsarticle${NC}"
echo ""
echo -e "   ${GRAY}# Kategoriye göre listele${NC}"
echo -e "   ${GRAY}curl ${BACKEND_URL}/api/newsarticle?category=githubcopilot${NC}"
echo -e "   ${GRAY}curl ${BACKEND_URL}/api/newsarticle?category=popular${NC}"
echo ""
