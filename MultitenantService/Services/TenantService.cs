using Microsoft.Extensions.Options;
using MultitenantService.Options;

namespace MultitenantService.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSetting _tenantSetting;
        private HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(IOptions<TenantSetting> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSetting = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;
            if (_httpContext!=null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant",out var tenantId))
                {
                    SetTenant(tenantId);
                }
                else
                {
                    throw new Exception("Invalid Tenant!");
                }

            }
        }

        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSetting.Tenants.Where(a => a.TID == tenantId).FirstOrDefault();
            if (_currentTenant == null) throw new Exception("Invalid Tenant!");
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }
        }

        private void SetDefaultConnectionStringToCurrentTenant()
        {
            _currentTenant.ConnectionString = _tenantSetting.Defaults.ConnectionString;
        }

        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            return _tenantSetting.Defaults?.ConnectionString;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}
