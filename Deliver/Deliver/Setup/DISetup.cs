using Integrations.Impl;
using Integrations.Interface;
using Repository.Repository.Impl;
using Repository.Repository.Interface;
using Services.Impl;
using Services.Interface;
using System.Net.Mail;

namespace Deliver.Setup;

public static class DISetup
{
    public static IServiceCollection RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        return services;
    }

    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtUtils, JwtUtils>();
        services.AddScoped<ICompanyService, CompanyService>();
        return services;
    }

    public static IServiceCollection RegisterIntegrations(this IServiceCollection services)
    {
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<SmtpClient>();
        return services;
    }
}
