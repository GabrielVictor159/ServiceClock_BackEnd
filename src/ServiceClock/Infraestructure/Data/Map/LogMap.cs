
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceClock_BackEnd.Domain.Models;
using System.Reflection.Emit;
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Infraestructure.Data.Map;

public class LogMap : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {       
        builder.ToContainer("Log");
        builder.HasPartitionKey(l => l.Type);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.LogDate)
            .IsRequired();

        builder.Property(c => c.Class)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Message)
            .IsRequired()
            .HasMaxLength(100);
    }
}

