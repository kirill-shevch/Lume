using BLL.Authorization;
using BLL.Authorization.Interfaces;
using BLL.Core;
using BLL.Core.Interfaces;
using Constants;
using DAL.Authorization;
using DAL.Core;
using DAL.Core.Interfaces;
using DAL.Core.Repositories;
using LumeWebApp.Middleware;
using LumeWebApp.SwaggerAttributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LumeWebApp
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
			services.AddSingleton<AuthorizationContextFactory>();
			services.AddSingleton<IAuthorizationRepository, AuthorizationRepository>();
			services.AddSingleton<IAuthorizationLogic, AuthorizationLogic>();
            services.AddSingleton<IAuthorizationValidation, AuthorizationValidation>();
            services.AddSingleton<CoreContextFactory>();
            services.AddSingleton<ICoreRepository, CoreRepository>();
            services.AddSingleton<ICoreValidation, CoreValidation>();
            services.AddSingleton<ICoreLogic, CoreLogic>();
			services.AddControllers();
            services.AddSwaggerGen(swagger =>
            {
                swagger.OperationFilter<CustomHeaderSwaggerAttribute>();
                swagger.SwaggerDoc("v2", new OpenApiInfo { Title = "Lume API" });
            });
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseWhen(context => AuthProtectedRoutes.ProtectedRoutes.Contains(context.Request.Path),
                builder =>
                {
                    builder.UseMiddleware<AuthorizationMiddleware>();
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "Lume API"));
        }
    }
}