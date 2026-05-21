using BootStrapper.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Data;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<UserConfig> UserConfigs { get; set; }

    private static readonly ValueConverter<string[], string> JsonConverter = new ValueConverter<string[], string>(
        templates => System.Text.Json.JsonSerializer.Serialize(templates, (System.Text.Json.JsonSerializerOptions?)null),
        templates => System.Text.Json.JsonSerializer.Deserialize<string[]>(templates, (System.Text.Json.JsonSerializerOptions?)null) ?? Array.Empty<string>()
    );

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=bootstrapper.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Project>()
            .Property(e => e.Templates) // String Array -> Json string
            .HasConversion(JsonConverter);

        modelBuilder
            .Entity<Project>()
            .Property(e => e.ChangeHistory) 
            .HasConversion(JsonConverter);
    }
}