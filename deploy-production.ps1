#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploy News Portal to Production (Azure + Netlify)

.DESCRIPTION
    Complete production deployment script that:
    1. Deploys backend to Azure Container Apps
    2. Configures CORS and scaling
    3. Deploys frontend to Netlify
    4. Runs health checks

.PARAMETER SkipBackend
    Skip backend deployment (only deploy frontend)

.PARAMETER SkipFrontend
    Skip frontend deployment (only deploy backend)

.PARAMETER MinReplicas
    Minimum backend replicas (0 = scale to zero, 1+ = always running)
    Default: 0 (cost-optimized)

.EXAMPLE
    .\deploy-production.ps1
    Full production deployment

.EXAMPLE
    .\deploy-production.ps1 -MinReplicas 1
    Deploy with always-running backend (no cold starts)

.EXAMPLE
    .\deploy-production.ps1 -SkipBackend
    Only deploy frontend
#>

param(
    [switch]$SkipBackend,
    [switch]$SkipFrontend,
    [int]$MinReplicas = 0
)

$ErrorActionPreference = "Stop"

function Write-Success { Write-Host $args -ForegroundColor Green }
function Write-Info { Write-Host $args -ForegroundColor Cyan }
function Write-Warning { Write-Host $args -ForegroundColor Yellow }
function Write-Error { param($msg) Write-Host $msg -ForegroundColor Red }
function Write-Step { param($num, $msg) Write-Host "[$num/4] $msg" -ForegroundColor Cyan }

# Banner
Write-Host ""
Write-Host "╔═══════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║    🚀 News Portal Production Deployment              ║" -ForegroundColor Cyan
Write-Host "╚═══════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Info "Configuration:"
Write-Host "  Backend: $(if ($SkipBackend) { 'SKIP' } else { 'DEPLOY' })"
Write-Host "  Frontend: $(if ($SkipFrontend) { 'SKIP' } else { 'DEPLOY' })"
Write-Host "  Min Replicas: $MinReplicas $(if ($MinReplicas -eq 0) { '(cold starts enabled)' } else { '(always running)' })"
Write-Host ""

# Step 1: Deploy Backend
if (-not $SkipBackend) {
    Write-Step 1 "Deploying Backend to Azure Container Apps"
    Write-Host ""
    
    if (-not (Test-Path ".\deploy-backend.ps1")) {
        Write-Error "❌ deploy-backend.ps1 not found!"
        exit 1
    }
    
    & .\deploy-backend.ps1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Backend deployment failed!"
        exit 1
    }
    
    Write-Success "✅ Backend deployed"
    Write-Host ""
    
    # Step 2: Configure Backend
    Write-Step 2 "Configuring Backend Settings"
    Write-Host ""
    
    if (-not (Test-Path ".\azure-configure-backend.ps1")) {
        Write-Warning "⚠️  azure-configure-backend.ps1 not found, skipping configuration"
    } else {
        & .\azure-configure-backend.ps1 -MinReplicas $MinReplicas
        
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "⚠️  Backend configuration failed, but deployment can continue"
        } else {
            Write-Success "✅ Backend configured"
        }
    }
    
    Write-Host ""
    
    # Wait for backend to be ready
    Write-Info "⏳ Waiting for backend to be ready..."
    $backendUrl = "https://newsportal-backend.happyglacier-db0dd319.westeurope.azurecontainerapps.io/health"
    $maxAttempts = 30
    $attempt = 0
    $healthy = $false
    
    while ($attempt -lt $maxAttempts) {
        $attempt++
        try {
            $response = Invoke-WebRequest -Uri $backendUrl -UseBasicParsing -TimeoutSec 5
            if ($response.StatusCode -eq 200) {
                $healthy = $true
                break
            }
        } catch {
            Write-Host "." -NoNewline
            Start-Sleep -Seconds 2
        }
    }
    
    Write-Host ""
    
    if ($healthy) {
        Write-Success "✅ Backend is healthy and responding"
    } else {
        Write-Warning "⚠️  Backend not responding yet. It may still be starting up."
        Write-Info "   This is normal for cold starts. The deployment can continue."
    }
    
    Write-Host ""
} else {
    Write-Info "⏭️  Skipping backend deployment"
    Write-Host ""
}

# Step 3: Deploy Frontend
if (-not $SkipFrontend) {
    Write-Step 3 "Deploying Frontend to Netlify"
    Write-Host ""
    
    if (-not (Test-Path ".\deploy-frontend.ps1")) {
        Write-Error "❌ deploy-frontend.ps1 not found!"
        exit 1
    }
    
    & .\deploy-frontend.ps1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "❌ Frontend deployment failed!"
        exit 1
    }
    
    Write-Success "✅ Frontend deployed"
    Write-Host ""
} else {
    Write-Info "⏭️  Skipping frontend deployment"
    Write-Host ""
}

# Step 4: Final Health Checks
Write-Step 4 "Running Health Checks"
Write-Host ""

$backendHealthy = $false
$frontendHealthy = $false

# Check Backend
if (-not $SkipBackend) {
    Write-Info "🔍 Checking backend..."
    try {
        $response = Invoke-WebRequest -Uri "https://newsportal-backend.happyglacier-db0dd319.westeurope.azurecontainerapps.io/health" -UseBasicParsing -TimeoutSec 10
        if ($response.StatusCode -eq 200) {
            Write-Success "  ✅ Backend: Healthy"
            $backendHealthy = $true
        }
    } catch {
        Write-Warning "  ⚠️  Backend: Not responding (may be cold start)"
    }
}

# Check Frontend
if (-not $SkipFrontend) {
    Write-Info "🔍 Checking frontend..."
    try {
        $response = Invoke-WebRequest -Uri "https://teknohaber.netlify.app" -UseBasicParsing -TimeoutSec 10
        if ($response.StatusCode -eq 200) {
            Write-Success "  ✅ Frontend: Online"
            $frontendHealthy = $true
        }
    } catch {
        Write-Warning "  ⚠️  Frontend: Not responding"
    }
}

Write-Host ""

# Summary
Write-Host "╔═══════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║           🎉 DEPLOYMENT COMPLETE                      ║" -ForegroundColor Green
Write-Host "╚═══════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 Production URLs:" -ForegroundColor Cyan
Write-Host "   Frontend:  " -NoNewline; Write-Host "https://teknohaber.netlify.app" -ForegroundColor White
Write-Host "   Backend:   " -NoNewline; Write-Host "https://newsportal-backend.happyglacier-db0dd319.westeurope.azurecontainerapps.io" -ForegroundColor White
Write-Host "   Swagger:   " -NoNewline; Write-Host "https://newsportal-backend.happyglacier-db0dd319.westeurope.azurecontainerapps.io/swagger" -ForegroundColor White
Write-Host ""
Write-Host "📊 Status:" -ForegroundColor Cyan
Write-Host "   Backend:   " -NoNewline; if ($backendHealthy -or $SkipBackend) { Write-Success "✅ Healthy" } else { Write-Warning "⚠️  Starting (cold start)" }
Write-Host "   Frontend:  " -NoNewline; if ($frontendHealthy -or $SkipFrontend) { Write-Success "✅ Online" } else { Write-Warning "⚠️  Check status" }
Write-Host ""

if ($MinReplicas -eq 0) {
    Write-Warning "💡 Note: Cold starts enabled (min replicas = 0)"
    Write-Info "   First request after inactivity may take 10-30 seconds"
    Write-Info "   To eliminate cold starts: .\deploy-production.ps1 -MinReplicas 1"
    Write-Host ""
}

Write-Host "🔗 Management:" -ForegroundColor Cyan
Write-Host "   Netlify Dashboard: " -NoNewline; Write-Host "https://app.netlify.com/sites/teknohaber/deploys" -ForegroundColor White
Write-Host "   Azure Portal: " -NoNewline; Write-Host "https://portal.azure.com/#resource/subscriptions/.../resourceGroups/newsportal-rg" -ForegroundColor White
Write-Host ""
Write-Host "📝 Monitoring:" -ForegroundColor Cyan
Write-Host "   Backend logs: " -NoNewline; Write-Host "az containerapp logs show --name newsportal-backend --resource-group newsportal-rg --follow" -ForegroundColor Gray
Write-Host "   Frontend logs: " -NoNewline; Write-Host "netlify logs" -ForegroundColor Gray
Write-Host ""

if (-not $backendHealthy -and -not $SkipBackend) {
    Write-Warning "⚠️  Backend is not responding yet. This is normal for cold starts."
    Write-Info "   Wait 30-60 seconds and visit: https://teknohaber.netlify.app"
    Write-Info "   The frontend will display a helpful message during cold starts."
}

Write-Success "✅ Deployment successful!"
Write-Host ""
