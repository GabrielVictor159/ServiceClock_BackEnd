﻿
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceClock_BackEnd.Domain.Models;
using ServiceClock_BackEnd.Domain.Enums;

namespace ServiceClock_BackEnd.Infraestructure.Data.Map;

public class MessageMap : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Message", "ServiceClock");

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

        builder.Property(c => c.Active)
            .IsRequired();

    }
}
