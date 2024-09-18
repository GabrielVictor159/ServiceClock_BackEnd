
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Infraestructure.Data.Map;

public class MessageMap : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToContainer("Messages");
        builder.HasPartitionKey(c => c.Type);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (MessageType)Enum.Parse(typeof(MessageType), v)
            );

        builder.Property(c => c.ClientId)
            .IsRequired();

        builder.Property(c => c.CompanyId)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .IsRequired();

        builder.Property(c => c.MessageContent)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.CreateAt)
            .IsRequired();

        builder.HasOne(c => c.Client)
            .WithMany()
            .HasForeignKey(c => c.ClientId);

        builder.HasOne(c => c.Company)
            .WithMany()
            .HasForeignKey(c => c.CompanyId);

        builder.Property(c => c.Active)
            .IsRequired();

    }
}
