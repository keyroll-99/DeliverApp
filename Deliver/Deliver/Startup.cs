﻿using Deliver.Setup;
using Models;

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

            services.RegisterService();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
