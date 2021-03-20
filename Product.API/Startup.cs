using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product.API.DbConnection;
using Product.API.Models;
using Product.API.Repositories;
using Product.API.Repositories.Mappers;
using Product.API.Services;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Product.API
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();

            services.AddSwaggerGen(action =>
            {
                action.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Product API",
                    Description = "By: Thanh Pham"
                });
                action.IncludeXmlComments(Debugger.IsAttached ?
                    Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Product.API.xml")
                    : Path.Combine("../../..", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Product.API.xml"));
            });

            services.AddOptions();
            services.Configure<SqliteDatabaseSettings>(Configuration.GetSection("SqliteDatabaseSettings"));

            services.AddScoped<IDbConnectionFactory, SqliteDbConnectionFactory>();
            services.AddScoped<IDbConnectionStrategy, DbConnectionStrategy>();
            services.AddTransient<IRepository<Models.Product>, ProductRepository>();
            services.AddTransient<IRepository<ProductOption>, ProductOptionRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductOptionService, ProductOptionService>();
            services.AddTransient<IProductMapper, ProductMapper>();

            services.AddTransient<IOptimisticLockingResolver, OptimisticLockingResolver>();

            services.AddHttpContextAccessor();
        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
            });

            app.UseSwagger();
            app.UseSwaggerUI(u => u.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API"));
        }
    }
}
