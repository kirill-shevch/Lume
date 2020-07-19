using BLL.Authorization;
using BLL.Authorization.Interfaces;
using BLL.Core;
using BLL.Core.Interfaces;
using DAL.Authorization;
using DAL.Core;
using DAL.Core.Interfaces;
using DAL.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lume.DI
{
    public static class DependencyInjection
    {
        public static void RegisterLogics(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationLogic, AuthorizationLogic>();
            services.AddSingleton<IImageLogic, ImageLogic>();
            services.AddSingleton<ICoreLogic, CoreLogic>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationRepository, AuthorizationRepository>();
            services.AddSingleton<IImageRepository, ImageRepository>();
            services.AddSingleton<ICoreRepository, CoreRepository>();
        }

        public static void RegisterFactories(this IServiceCollection services)
        {
            services.AddSingleton<CoreContextFactory>();
            services.AddSingleton<AuthorizationContextFactory>();
        }

        public static void RegisterValidations(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationValidation, AuthorizationValidation>();
            services.AddSingleton<IImageValidation, ImageValidation>();
            services.AddSingleton<ICoreValidation, CoreValidation>();
        }
    }
}
