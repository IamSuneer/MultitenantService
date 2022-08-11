using Microsoft.OpenApi.Models;
using MultitenantService.Data;
using MultitenantService.Extensions;
using MultitenantService.Options;
using MultitenantService.Services;

namespace MultitenantService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            config = configuration;
        }

        public IConfiguration config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Multitenant", Version = "v1" });
            });
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IProductService, ProductService>();
            services.Configure<TenantSetting>(config.GetSection(nameof(TenantSetting)));
            services.AddAndMigrateTenantDatabases(config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multitenant v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
