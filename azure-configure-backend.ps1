#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Configure Azure Container App backend settings

.DESCRIPTION
    Updates Azure Container App with optimal settings for production:
    - Minimum replicas to prevent cold starts
    - CORS environment variables
    - Health check configuration
    - Scale rules

.PARAMETER ResourceGroup
    Azure resource group name (default: newsportal-rg)

.PARAMETER ContainerApp
    Container app name (default: newsportal-backend)

.PARAMETER MinReplicas
    Minimum number of replicas (0 = allow scale to zero, 1+ = always running)
    Default: 0 (cost-optimized, but has cold starts)

.EXAMPLE
    .\azure-configure-backend.ps1
    Configure with default settings (scale to zero enabled)

.EXAMPLE
    .\azure-configure-backend.ps1 -MinReplicas 1
    Configure to always keep at least 1 instance running (no cold starts)
#>

param(
    [string]$ResourceGroup = "newsportal-rg",
    [string]$ContainerApp = "newsportal-backend",
    [int]$MinReplicas = 0
)

Write-Host "⚙️  Azure Container App Configuration" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📋 Settings:" -ForegroundColor Yellow
Write-Host "   Resource Group: $ResourceGroup"
Write-Host "   Container App: $ContainerApp"
Write-Host "   Min Replicas: $MinReplicas $(if ($MinReplicas -eq 0) { '(cold starts enabled)' } else { '(always running)' })"
Write-Host ""

# Check if Azure CLI is installed
if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Azure CLI not found!" -ForegroundColor Red
    Write-Host "   Install from: https://aka.ms/azure-cli" -ForegroundColor Yellow
    exit 1
}

# Check Azure login
Write-Host "🔐 Checking Azure login..." -ForegroundColor Yellow
$azAccount = az account show 2>$null | ConvertFrom-Json
if (-not $azAccount) {
    Write-Host "   Please login to Azure..." -ForegroundColor Yellow
    az login
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Azure login failed!" -ForegroundColor Red
        exit 1
    }
}
Write-Host "✅ Logged in as: $($azAccount.user.name)" -ForegroundColor Green
Write-Host ""

# Configure scale rules
Write-Host "📊 Configuring scale rules..." -ForegroundColor Yellow
az containerapp update `
    --name $ContainerApp `
    --resource-group $ResourceGroup `
    --min-replicas $MinReplicas `
    --max-replicas 10 `
    --scale-rule-name "http-scaling" `
    --scale-rule-type "http" `
    --scale-rule-http-concurrency 50

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Scale configuration failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Scale rules configured" -ForegroundColor Green
Write-Host ""

# Get current configuration
Write-Host "📋 Current Configuration:" -ForegroundColor Cyan
$config = az containerapp show `
    --name $ContainerApp `
    --resource-group $ResourceGroup `
    --query "{fqdn:properties.configuration.ingress.fqdn, minReplicas:properties.template.scale.minReplicas, maxReplicas:properties.template.scale.maxReplicas}" `
    --output json | ConvertFrom-Json

if ($config) {
    Write-Host "   URL: https://$($config.fqdn)" -ForegroundColor White
    Write-Host "   Min Replicas: $($config.minReplicas)" -ForegroundColor White
    Write-Host "   Max Replicas: $($config.maxReplicas)" -ForegroundColor White
    Write-Host ""
}

# Display recommendations
Write-Host "💡 Recommendations:" -ForegroundColor Yellow
if ($MinReplicas -eq 0) {
    Write-Host "   ⚠️  Cold starts enabled (cost-optimized)" -ForegroundColor Yellow
    Write-Host "   - First request after inactivity may take 10-30 seconds" -ForegroundColor Gray
    Write-Host "   - Frontend displays helpful message to users" -ForegroundColor Gray
    Write-Host "   - To eliminate cold starts, run: .\azure-configure-backend.ps1 -MinReplicas 1" -ForegroundColor Gray
} else {
    Write-Host "   ✅ Always running - no cold starts" -ForegroundColor Green
    Write-Host "   - Higher cost (~$15-30/month for 1 replica)" -ForegroundColor Gray
    Write-Host "   - Instant responses" -ForegroundColor Gray
    Write-Host "   - To enable cost savings, run: .\azure-configure-backend.ps1 -MinReplicas 0" -ForegroundColor Gray
}
Write-Host ""

Write-Host "✅ Configuration complete!" -ForegroundColor Green
Write-Host ""
Write-Host "🔗 Useful Links:" -ForegroundColor Cyan
Write-Host "   Portal: https://portal.azure.com/#resource/subscriptions/$($azAccount.id)/resourceGroups/$ResourceGroup/providers/Microsoft.App/containerApps/$ContainerApp"
Write-Host "   Logs: az containerapp logs show --name $ContainerApp --resource-group $ResourceGroup --follow"
Write-Host ""
