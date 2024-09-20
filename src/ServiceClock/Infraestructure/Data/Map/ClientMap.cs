using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceClock_BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceClock_BackEnd.Domain.Helpers;

namespace ServiceClock_BackEnd.Infraestructure.Data.Map;
public class ClientMap : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToContainer("Clients");
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

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

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

        builder.Property(c => c.BirthDate)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.Active)
            .IsRequired();

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.HasOne(c => c.Company)
            .WithMany()
            .HasForeignKey(c => c.CompanyId);
    }
}
