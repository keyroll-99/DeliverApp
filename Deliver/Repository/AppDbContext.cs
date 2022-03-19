﻿using Microsoft.EntityFrameworkCore;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<User>()
            .HasOne(x => x.Company)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.CompanyId);

        modelBuilder
            .Entity<Company>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Company);

        modelBuilder
            .Entity<Location>()
            .HasOne(x => x.Company)
            .WithMany(x => x.Locations)
            .HasForeignKey(x => x.CompanyId);

        modelBuilder
            .Entity<Company>()
            .HasMany(x => x.Locations)
            .WithOne(x => x.Company);
    }
}
