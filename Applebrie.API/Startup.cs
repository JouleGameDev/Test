using Applebrie.BLL.Cache.CacheRepository;
using Applebrie.BLL.Cache.Interface;
using Applebrie.BLL.Engines;
using Applebrie.BLL.Interfaces;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using Applebrie.DAL.Interfaces;
using Applebrie.DAL.Options;
using Applebrie.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Applebrie.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc(op => op.EnableEndpointRouting = false);
            //Configure Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Applebrie API", Version = "v1" });
            });

            services.Configure<SQLDBContextOptions>(Configuration.GetSection("DbConnectionString"));
            
            // Repositories
           // services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserRepository, EFUserRepository>();

            // Engines
            services.AddScoped<IUserEngine, UserEngine>();

            // Cache
            services.AddSingleton<ICache<RequestUserFiltredListModel, ResponseUserPagedResult>, UserDictionaryCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc();
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
