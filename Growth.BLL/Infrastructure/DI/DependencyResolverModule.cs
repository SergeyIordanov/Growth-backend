using Growth.DAL.Context;
using Growth.DAL.Interfaces;
using Growth.DAL.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Growth.BLL.Infrastructure.DI
{
    public class DependencyResolverModule
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var connectionstring = configuration["ConnectionStrings:MongoDb"];

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDbContext>(provider => new DbContext(connectionstring));
        }
    }
}