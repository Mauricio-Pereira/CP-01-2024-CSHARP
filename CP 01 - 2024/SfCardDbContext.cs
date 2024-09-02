using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CP_01___2024;

public class SfCardDbContext : DbContext
{
    public SfCardDbContext() : base()
    {
    }
    public DbSet<SfCard> SFCARDS { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseOracle(configuration.GetConnectionString("FiapOracleConnection"), options =>
        {
            options.CommandTimeout(120);
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SfCard>()
            .Property(c => c.ID)
            .ValueGeneratedOnAdd();
    }
}