
using Microsoft.EntityFrameworkCore;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Infraestructure.Data.Map;

namespace ServiceClock_BackEnd.Infraestructure.Data;

public partial class Context : DbContext
{
    public Context()
    {
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Log> Logs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseCosmos(
                Environment.GetEnvironmentVariable("COSMOS_URL")!,
                Environment.GetEnvironmentVariable("COSMOS_KEY")!,
                databaseName: Environment.GetEnvironmentVariable("COSMOS_DATABASE")!);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyMap());
        modelBuilder.ApplyConfiguration(new ClientMap());
        modelBuilder.ApplyConfiguration(new LogMap());
        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

