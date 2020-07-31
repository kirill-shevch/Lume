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
            services.AddSingleton<IEventLogic, EventLogic>();
            services.AddSingleton<IPersonLogic, PersonLogic>();
            services.AddSingleton<IChatLogic, ChatLogic>();
            services.AddSingleton<ICityLogic, CityLogic>();
        }

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationRepository, AuthorizationRepository>();
            services.AddSingleton<IImageRepository, ImageRepository>();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IChatRepository, ChatRepository>();
            services.AddSingleton<IEventRepository, EventRepository>();
            services.AddSingleton<ICityRepository, CityRepository>();

        }

        public static void RegisterFactories(this IServiceCollection services)
        {
            services.AddSingleton<CoreContextFactory>();
            services.AddSingleton<AuthorizationContextFactory>();
        }

        public static void RegisterValidations(this IServiceCollection services)
        {
            services.AddTransient<IAuthorizationValidation, AuthorizationValidation>();
            services.AddTransient<IImageValidation, ImageValidation>();
            services.AddTransient<IEventValidation, EventValidation>();
            services.AddTransient<IPersonValidation, PersonValidation>();
            services.AddTransient<IChatValidation, ChatValidation>();
            services.AddTransient<IFriendValidation, FriendValidation>();
        }
    }
}
