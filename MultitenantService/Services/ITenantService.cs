using MultitenantService.Options;

namespace MultitenantService.Services
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();
        public string GetConnectionString();
        public Tenant GetTenant();
    }
}
