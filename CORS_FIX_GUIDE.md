# CORS & Cold Start Fix Guide

## Issues Fixed

### 1. CORS Error
**Error**: `Access to XMLHttpRequest has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present`

**Root Cause**: Backend CORS configuration had `AllowCredentials()` which is incompatible with wildcard subdomain patterns.

**Fix Applied**:
- Removed `AllowCredentials()` from CORS policy
- Added wildcard support for Netlify preview deployments (`*.netlify.app`)
- Added preflight caching (10 minutes) to reduce CORS overhead
- Disabled `withCredentials` in frontend API client

### 2. 503 Service Unavailable (Cold Start)
**Error**: `GET https://newsportal-backend...azurecontainerapps.io/api/NewsArticle/ net::ERR_FAILED 503`

**Root Cause**: Azure Container Apps scaled to zero (min replicas = 0). First request after inactivity takes 10-30 seconds to spin up a new instance.

**Fixes Applied**:
- Increased frontend timeout to 90 seconds (allows time for cold start)
- Added user-friendly error message for 503 errors
- Created configuration script to adjust min replicas

## Deployment Steps

### Option 1: Quick Fix (Keep Cold Starts, Cost-Optimized)

This keeps the backend scaled to zero when idle (saves money), but shows helpful messages to users during cold starts.

```powershell
# 1. Deploy updated backend
.\deploy-backend.ps1

# 2. Deploy updated frontend
cd frontend
npm run build
netlify deploy --prod

# 3. Test
Start-Process "https://teknohaber.netlify.app"
```

### Option 2: Eliminate Cold Starts (Always Running)

This keeps at least 1 backend instance always running (~$15-30/month additional cost).

```powershell
# 1. Deploy updated backend
.\deploy-backend.ps1

# 2. Configure to always run (no cold starts)
.\azure-configure-backend.ps1 -MinReplicas 1

# 3. Deploy frontend
cd frontend
npm run build
netlify deploy --prod

# 4. Test
Start-Process "https://teknohaber.netlify.app"
```

## Testing the Fix

### 1. Test CORS
```powershell
# Should return 200 with CORS headers
curl -i -H "Origin: https://teknohaber.netlify.app" `
     -H "Access-Control-Request-Method: GET" `
     -X OPTIONS `
     https://newsportal-backend.happyglacier-db0dd319.westeurope.azurecontainerapps.io/api/NewsArticle/
```

Expected headers:
```
access-control-allow-origin: https://teknohaber.netlify.app
access-control-allow-methods: GET,POST,PUT,DELETE
access-control-allow-headers: *
access-control-max-age: 600
```

### 2. Test Backend Health
```powershell
curl https://newsportal-backend.happyglacier-db0dd319.westeurope.azurecontainerapps.io/health
```

Expected: `Healthy` or `200 OK`

### 3. Test Frontend
1. Open https://teknohaber.netlify.app
2. Open browser console (F12)
3. Check for errors
4. Verify news articles load

## What Changed

### Backend Changes
**File**: `backend/Presentation/Extensions/ServiceCollectionExtensions.cs`
- ✅ Added wildcard support for `*.netlify.app` domains
- ✅ Removed `AllowCredentials()` (incompatible with wildcards)
- ✅ Added preflight caching (10 min)
- ✅ Added exposed headers for better debugging

### Frontend Changes
**File**: `frontend/lib/api/client.ts`
- ✅ Increased timeout from 60s to 90s (allows cold start time)
- ✅ Disabled `withCredentials` (not needed, simplifies CORS)
- ✅ Added specific error handling for 503 (cold start message)
- ✅ Added network error handling with user-friendly messages

**File**: `frontend/netlify.toml`
- ✅ Added security headers (XSS Protection, Permissions-Policy)

### New Scripts
**File**: `azure-configure-backend.ps1`
- Configure Azure Container App scaling behavior
- Choose between cost-optimized (cold starts) or always-running (instant)

## Monitoring

### Check Backend Status
```powershell
az containerapp show `
  --name newsportal-backend `
  --resource-group newsportal-rg `
  --query "{status:properties.runningStatus, replicas:properties.template.scale.minReplicas}"
```

### View Backend Logs
```powershell
az containerapp logs show `
  --name newsportal-backend `
  --resource-group newsportal-rg `
  --follow
```

### Check Frontend Deployment
```powershell
netlify status
netlify open:site
```

## Cost Comparison

| Configuration | Monthly Cost | Cold Start Time | Response Time |
|--------------|--------------|-----------------|---------------|
| **Min 0 replicas** (current) | $5-10 | 10-30 seconds | 100-300ms after warm |
| **Min 1 replica** (always on) | $20-40 | None | 100-300ms always |

## Troubleshooting

### CORS Still Failing
1. Check backend logs for CORS errors
2. Verify Netlify environment variable: `NEXT_PUBLIC_API_URL`
3. Test CORS preflight manually (see "Test CORS" above)
4. Ensure backend is deployed: `.\deploy-backend.ps1`

### Backend Still Returns 503
1. Wait 30-60 seconds and refresh (cold start in progress)
2. Check if backend is running: `az containerapp show --name newsportal-backend --resource-group newsportal-rg`
3. If stuck, restart: `az containerapp restart --name newsportal-backend --resource-group newsportal-rg`
4. Consider setting min replicas to 1: `.\azure-configure-backend.ps1 -MinReplicas 1`

### Frontend Shows Error Message
- If message says "Backend is starting up", wait 30 seconds and refresh
- If message says "Unable to connect", check backend health endpoint
- Check browser console for detailed error

## Verification Checklist

- [ ] Backend deployed successfully
- [ ] CORS preflight returns correct headers
- [ ] Backend health endpoint returns 200
- [ ] Frontend builds without errors
- [ ] Frontend deployed to Netlify
- [ ] Website loads at https://teknohaber.netlify.app
- [ ] News articles display correctly
- [ ] No CORS errors in browser console
- [ ] No 503 errors (or shows helpful message and recovers)

## Support

If issues persist:
1. Check backend logs: `az containerapp logs show --name newsportal-backend --resource-group newsportal-rg --tail 100`
2. Check Netlify deploy logs: https://app.netlify.com/sites/teknohaber/deploys
3. Test backend directly: `curl https://newsportal-backend.happyglacier-db0dd319.westeurope.azurecontainerapps.io/health`
4. Open GitHub issue with error details
