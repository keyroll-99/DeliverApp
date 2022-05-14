using Microsoft.EntityFrameworkCore;
using Models.Db;

namespace Repository;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Company> Company { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Deliver> Delivers { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<User>()
            .HasOne(x => x.Company)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Company>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Company)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Location>()
            .HasOne(x => x.Company)
            .WithMany(x => x.Locations)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Company>()
            .HasMany(x => x.Locations)
            .WithOne(x => x.Company)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Car>()
            .HasOne(x => x.Driver)
            .WithOne(x => x.Car)
            .HasForeignKey<Car>(x => x.DriverId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Deliver>()
            .HasOne(x => x.To)
            .WithMany(x => x.Pickup)
            .HasForeignKey(x => x.ToId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Deliver>()
            .HasOne(x => x.From)
            .WithMany(x => x.Send)
            .HasForeignKey(x => x.FromId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<UserRole>()
            .HasOne(x => x.User)
            .WithMany(x => x.UserRole)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<UserRole>()
            .HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<RefreshToken>()
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
