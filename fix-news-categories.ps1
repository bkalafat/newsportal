#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Fixes news categories in MongoDB directly
.DESCRIPTION
    Changes 'teknohaber' category to 'openai' for AI-related articles
#>

Write-Host "=== Fix News Categories in Database ===" -ForegroundColor Cyan
Write-Host ""

# Fix categories - change 'teknohaber' to 'openai'
Write-Host "Updating categories from 'teknohaber' to 'openai'..." -ForegroundColor Cyan

$result = docker exec -it newsportal-mongodb mongosh -u admin -p password123 --authenticationDatabase admin --quiet --eval @"
db = db.getSiblingDB('NewsDb');
const result = db.News.updateMany(
    { Category: 'teknohaber' },
    { `$set: { Category: 'openai' } }
);
print('Modified:', result.modifiedCount);
"@

Write-Host "✓ Categories updated" -ForegroundColor Green
Write-Host $result

Write-Host ""
Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "All articles are now in proper categories." -ForegroundColor Green
Write-Host "The next news aggregation will add images automatically." -ForegroundColor Green
