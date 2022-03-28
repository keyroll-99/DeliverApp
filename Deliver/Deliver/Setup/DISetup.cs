using Repository.Repository.Impl;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;

namespace Deliver.Setup;

public static class DISetup
{
    public static IServiceCollection RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        return services;
    }

    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtUtils, JwtUtils>();
        return services;
    }
}
