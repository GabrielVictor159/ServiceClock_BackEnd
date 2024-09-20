using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceClock_BackEnd.Domain.Helpers;
using ServiceClock_BackEnd.Domain.Models;

namespace ServiceClock_BackEnd.Infraestructure.Data.Map;

public class CompanyMap : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToContainer("Companies");
        builder.HasPartitionKey(c => c.Country);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Password)
           .IsRequired()
           .HasMaxLength(256)
           .HasConversion(
               v => v.PasswordEncryption(),
               v => v);

        builder.Property(c => c.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Country)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.PostalCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .IsRequired()
            .IsUnicode(true)
            .HasMaxLength(100);

        builder.Property(c => c.EstablishedDate)
            .IsRequired();
    }
}


