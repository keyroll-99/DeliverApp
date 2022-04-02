using Deliver.Middleware;
using Deliver.Setup;
using Models;
using Models.Intefrations;

namespace Deliver
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _environment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.SetupDb(_configuration);

            services.RegisterRepository();


            services.AddControllers();

            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"))
                .AddScoped<AppSettings>();


            services.Configure<MailSettings>(_configuration.GetSection("Mail"))
                .AddScoped<MailSettings>();

            services.Configure<LoggedUser>(_configuration.GetSection("AppUser"))
                .AddScoped<LoggedUser>();

            services.RegisterService();
            services.RegisterIntegrations();

            if (_environment.IsDevelopment())
            {
                var baseInsertTask = services.BaseInsert();
                baseInsertTask.Wait();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors(x => x
                 .SetIsOriginAllowed(o => true)
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
