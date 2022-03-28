﻿using Microsoft.EntityFrameworkCore;
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

            var adminRole = await roles.FirstOrDefaultAsync(x => x.Name == "admin");
            var admin = await users.FirstOrDefaultAsync(x => x.Name == "admin");

            if (adminRole is null)
            {
                adminRole = new Role
                {
                    Name = "admin",
                    CreateTime = DateTime.Now,
                };
                roles.Add(adminRole);
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
                    Password = BCrypt.Net.BCrypt.HashPassword("password")
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
