using Microsoft.EntityFrameworkCore;
using MultitenantService.Contracts;
using MultitenantService.Entities;
using MultitenantService.Services;

namespace MultitenantService.Data
{
    public class MultitenantDbContext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;

        public MultitenantDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetTenant()?.TID;
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasQueryFilter(a => a.TenantId == TenantId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "sharedTenant")
                {
                    optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
                }
            }
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entity in ChangeTracker.Entries<ITenant>().ToList())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entity.Entity.TenantId = TenantId;
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
