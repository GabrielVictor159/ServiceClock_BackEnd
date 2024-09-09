
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Infraestructure.Data.Map;

public class AppointmentMap : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToContainer("Appointments");
        builder.HasPartitionKey(c => c.Status);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.ServiceId)
            .IsRequired();

        builder.Property(c => c.ClientId)
            .IsRequired();

        builder.Property(c => c.Date)
            .IsRequired();

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasOne(c => c.Client)
            .WithMany()
            .HasForeignKey(c => c.ClientId);

        builder.HasOne(c => c.Service)
           .WithMany()
           .HasForeignKey(c => c.ServiceId);
    }
}

