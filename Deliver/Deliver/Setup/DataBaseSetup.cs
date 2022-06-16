using Microsoft.EntityFrameworkCore;
using Models.Db;
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

        public async static Task<IServiceCollection> BaseInsert(this IServiceCollection services)
        {
            var appDbContext = services.BuildServiceProvider().GetService<AppDbContext>()!;
            var users = appDbContext.Users;
            var roles = appDbContext.Roles;
            var userRoles = appDbContext.UserRoles;
            var companies = appDbContext.Company;

            var adminRole = await roles.FirstOrDefaultAsync(x => x.Name == "Admin");
            var admin = await users.FirstOrDefaultAsync(x => x.Name == "Admin");
            var company = await companies.FirstOrDefaultAsync(x => x.Name == "admin-company");

            if(company is null)
            {
                company = new Company
                {
                    Name = "admin-company"
                };

                companies.Add(company);
            }

            if (admin is null)
            {
                admin = new User
                {
                    CreateTime = DateTime.Now,
                    Email = "Admin@admin.com",
                    Name = "Admin",
                    Hash = Guid.NewGuid(),
                    Username = "admin",
                    Surname = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"),
                    CompanyId = company.Id,
                };
                users.Add(admin);
            }

            await appDbContext.SaveChangesAsync();
            
            var userRole = await userRoles.FirstOrDefaultAsync(x => x.RoleId == adminRole.Id && x.UserId == admin.Id);
            if (userRole is null)
            {
                userRole = new UserRole
                {
                    RoleId = adminRole.Id,
                    UserId = admin.Id,
                    Role = adminRole,
                    User = admin,
                    CreateTime = DateTime.Now,
                };
                userRoles.Add(userRole);

                await appDbContext.SaveChangesAsync();
            }

            return services;

        }
    }
}
