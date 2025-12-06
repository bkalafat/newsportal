#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Fixes existing news articles by adding images and correcting categories
.DESCRIPTION
    This script:
    1. Fetches all existing news articles from MongoDB
    2. Downloads and uploads images from Unsplash for each article
    3. Updates categories from 'teknohaber' to proper categories
    4. Updates the database with corrected data
#>

param(
    [string]$BackendUrl = "http://localhost:5000",
    [switch]$DryRun
)

Write-Host "=== Fix Existing News Articles ===" -ForegroundColor Cyan
Write-Host ""

# Check if backend is running
try {
    $health = Invoke-RestMethod -Uri "$BackendUrl/health" -Method Get -ErrorAction Stop
    Write-Host "✓ Backend is healthy" -ForegroundColor Green
}
catch {
    Write-Host "✗ Backend is not responding at $BackendUrl" -ForegroundColor Red
    Write-Host "  Please ensure Docker containers are running: docker compose up -d" -ForegroundColor Yellow
    exit 1
}

# Login to get JWT token
Write-Host ""
Write-Host "Logging in..." -ForegroundColor Cyan
$loginBody = @{
    username = "admin"
    password = "admin123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$BackendUrl/api/Auth/login" -Method Post -Body $loginBody -ContentType "application/json"
    $token = $loginResponse.token
    Write-Host "✓ Logged in successfully" -ForegroundColor Green
}
catch {
    Write-Host "✗ Login failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# Get all news articles
Write-Host ""
Write-Host "Fetching existing news articles..." -ForegroundColor Cyan
try {
    $news = Invoke-RestMethod -Uri "$BackendUrl/api/NewsArticle" -Method Get
    Write-Host "✓ Found $($news.Count) articles" -ForegroundColor Green
}
catch {
    Write-Host "✗ Failed to fetch news: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Category mapping based on title/content
function Get-ProperCategory {
    param([string]$Title, [string]$Content)
    
    $combined = "$Title $Content".ToLower()
    
    # Check for specific AI platforms
    if ($combined -match "openai|chatgpt|gpt-4|gpt-5") {
        return "openai"
    }
    if ($combined -match "claude|anthropic") {
        return "claudeai"
    }
    if ($combined -match "github\s+copilot|copilot") {
        return "githubcopilot"
    }
    if ($combined -match "deepseek") {
        return "deepseek"
    }
    if ($combined -match "\.net|dotnet|c#|csharp") {
        return "dotnet"
    }
    if ($combined -match "\brobot|\brobotic") {
        return "robotics"
    }
    if ($combined -match "\bmcp\b|model\s+context\s+protocol") {
        return "mcp"
    }
    if ($combined -match "\bai\b|artificial\s+intelligence|machine\s+learning|\bml\b") {
        return "artificialintelligence"
    }
    
    # Default to popular
    return "popular"
}

# Get Unsplash image URL for category
function Get-UnsplashImageUrl {
    param([string]$Category)
    
    $query = switch ($Category) {
        "openai" { "artificial+intelligence,chatgpt,openai" }
        "claudeai" { "artificial+intelligence,claude,ai+assistant" }
        "githubcopilot" { "programming,coding,developer" }
        "artificialintelligence" { "artificial+intelligence,technology,AI" }
        "robotics" { "robot,robotics,automation" }
        "deepseek" { "artificial+intelligence,deep+learning" }
        "dotnet" { "programming,code,software+development" }
        "mcp" { "technology,network,communication" }
        default { "technology,innovation,digital" }
    }
    
    return "https://source.unsplash.com/1920x1080/?$query"
}

# Process each article
$updatedCount = 0
$skippedCount = 0
$failedCount = 0

Write-Host ""
Write-Host "Processing articles..." -ForegroundColor Cyan
Write-Host ""

foreach ($article in $news) {
    Write-Host "Article: $($article.caption)" -ForegroundColor Yellow
    Write-Host "  ID: $($article.id)"
    Write-Host "  Current Category: $($article.category)"
    Write-Host "  Has Image: $(-not [string]::IsNullOrEmpty($article.imageUrl))"
    
    # Determine proper category
    $properCategory = Get-ProperCategory -Title $article.caption -Content $article.content
    Write-Host "  Detected Category: $properCategory" -ForegroundColor Cyan
    
    # Check if needs update
    $needsCategoryFix = $article.category -ne $properCategory
    $needsImage = [string]::IsNullOrEmpty($article.imageUrl)
    
    if (-not $needsCategoryFix -and -not $needsImage) {
        Write-Host "  → Skipping (no changes needed)" -ForegroundColor Gray
        $skippedCount++
        Write-Host ""
        continue
    }
    
    if ($DryRun) {
        Write-Host "  → [DRY RUN] Would update:" -ForegroundColor Magenta
        if ($needsCategoryFix) {
            Write-Host "    - Category: $($article.category) → $properCategory"
        }
        if ($needsImage) {
            Write-Host "    - Add image from Unsplash"
        }
        $updatedCount++
        Write-Host ""
        continue
    }
    
    # Upload image if needed
    $imageUploaded = $false
    if ($needsImage) {
        try {
            $imageUrl = Get-UnsplashImageUrl -Category $properCategory
            Write-Host "  → Downloading image from Unsplash..." -ForegroundColor Cyan
            
            # Download image
            $tempImagePath = Join-Path $env:TEMP "news-image-$($article.id).jpg"
            Invoke-WebRequest -Uri $imageUrl -OutFile $tempImagePath
            
            # Upload to backend
            $boundary = [System.Guid]::NewGuid().ToString()
            $fileContent = [System.IO.File]::ReadAllBytes($tempImagePath)
            $fileEnc = [System.Text.Encoding]::GetEncoding('iso-8859-1').GetString($fileContent)
            
            $bodyLines = @(
                "--$boundary",
                "Content-Disposition: form-data; name=`"category`"",
                "",
                $properCategory,
                "--$boundary",
                "Content-Disposition: form-data; name=`"file`"; filename=`"image.jpg`"",
                "Content-Type: image/jpeg",
                "",
                $fileEnc,
                "--$boundary--"
            )
            
            $body = $bodyLines -join "`r`n"
            
            $uploadResponse = Invoke-RestMethod `
                -Uri "$BackendUrl/api/NewsArticle/$($article.id)/upload-image" `
                -Method Post `
                -Headers @{
                    "Authorization" = "Bearer $token"
                    "Content-Type" = "multipart/form-data; boundary=$boundary"
                } `
                -Body $body
            
            Remove-Item $tempImagePath -ErrorAction SilentlyContinue
            
            Write-Host "  ✓ Image uploaded successfully" -ForegroundColor Green
            $imageUploaded = $true
        }
        catch {
            Write-Host "  ✗ Failed to upload image: $($_.Exception.Message)" -ForegroundColor Red
            # Continue with category update even if image fails
        }
    }
    
    # Update category if needed
    if ($needsCategoryFix) {
        try {
            # Create update DTO
            $updateDto = @{
                category = $properCategory
                type = $article.type
                caption = $article.caption
                summary = $article.summary
                content = $article.content
                keywords = $article.keywords
                socialTags = $article.socialTags
                imgAlt = $article.imgAlt
                expressDate = $article.expressDate
                priority = $article.priority
                isActive = $article.isActive
                showComment = $article.showComment
            } | ConvertTo-Json
            
            $updateResponse = Invoke-RestMethod `
                -Uri "$BackendUrl/api/NewsArticle/$($article.id)" `
                -Method Put `
                -Headers $headers `
                -Body $updateDto
            
            Write-Host "  ✓ Category updated: $($article.category) → $properCategory" -ForegroundColor Green
        }
        catch {
            Write-Host "  ✗ Failed to update category: $($_.Exception.Message)" -ForegroundColor Red
            $failedCount++
            Write-Host ""
            continue
        }
    }
    
    $updatedCount++
    Write-Host "  ✓ Article updated successfully" -ForegroundColor Green
    Write-Host ""
}

# Summary
Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Updated: $updatedCount" -ForegroundColor Green
Write-Host "Skipped: $skippedCount" -ForegroundColor Yellow
Write-Host "Failed: $failedCount" -ForegroundColor Red

if ($DryRun) {
    Write-Host ""
    Write-Host "This was a dry run. Re-run without -DryRun to apply changes." -ForegroundColor Magenta
}
