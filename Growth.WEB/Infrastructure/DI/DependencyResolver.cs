using Growth.BLL.Infrastructure.CryptoProviders;
using Growth.BLL.Infrastructure.DI;
using Growth.BLL.Interfaces;
using Growth.BLL.Services;
using Growth.WEB.Authentication;
using Growth.WEB.Authentication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Growth.WEB.Infrastructure.DI
{
    /// <summary>
    /// Class for all dependency resolutions
    /// </summary>
    public static class DependencyResolver
    {
        /// <summary>
        /// Resolves specified dependency injections
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Resolve(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IKidService, KidService>();
            services.AddTransient<IPathService, PathService>();
            services.AddTransient<ICryptoProvider, MD5CryptoProvider>();
            services.AddTransient<IIdentityProvider, IdentityProvider>();

            DependencyResolverModule.Configure(services, configuration);
        }
    }
}