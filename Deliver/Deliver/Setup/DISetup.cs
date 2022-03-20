using Repository.Repository.Impl;
using Repository.Repository.Interface;

namespace Deliver.Setup;

public static class DISetup
{
    public static IServiceCollection RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
