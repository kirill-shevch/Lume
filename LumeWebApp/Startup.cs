using AutoMapper;
using BLL.Core;
using BLL.Core.Mappings;
using Constants;
using Lume.DI;
using LumeWebApp.Middleware;
using LumeWebApp.SwaggerAttributes;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Utils.TelemetryInitializers;

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
            services.AddMvc();

            services.AddAutoMapper(typeof(MappingProfile));

            services.RegisterLogics();
            
            services.RegisterRepositories();

            services.RegisterFactories();

            services.RegisterValidations();

			services.AddControllers();
            services.AddSwaggerGen(swagger =>
            {
                swagger.OperationFilter<CustomHeaderSwaggerAttribute>();
                swagger.SwaggerDoc("v2", new OpenApiInfo { Title = "Lume API" });
            });
            services.AddSingleton<ITelemetryInitializer, RequestBodyInitializer>();
            services.AddHostedService<BackgroundJobService>();
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
