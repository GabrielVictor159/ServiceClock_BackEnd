
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Log> Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment.GetEnvironmentVariable("DBCONN") != null)
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DBCONN"), options =>
            {
                options.EnableRetryOnFailure(2, TimeSpan.FromSeconds(5), new List<string>());
                options.MigrationsHistoryTable("_MigrationHistory", "ServiceClock");
            });
        else
            optionsBuilder.UseInMemoryDatabase("ServiceClockInMemory");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyMap());
        modelBuilder.ApplyConfiguration(new ClientMap());
        modelBuilder.ApplyConfiguration(new AppointmentMap());
        modelBuilder.ApplyConfiguration(new ServiceMap());
        modelBuilder.ApplyConfiguration(new LogMap());
        modelBuilder.ApplyConfiguration(new MessageMap());

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
           v => DateTime.SpecifyKind(v.ToUniversalTime(), DateTimeKind.Utc),
           v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); 

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(DateTime)))
            {
                property.SetValueConverter(dateTimeConverter);
            }
        }


        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

