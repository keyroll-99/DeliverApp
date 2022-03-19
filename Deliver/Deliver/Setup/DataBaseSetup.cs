using Microsoft.EntityFrameworkCore;
using Repository;

namespace Deliver.Setup
{
    public static class DataBaseSetup
    {
        public static IServiceCollection SetupDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
