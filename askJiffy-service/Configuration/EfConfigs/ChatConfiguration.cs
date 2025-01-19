﻿using askJiffy_service.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace askJiffy_service.Configuration.EfConfigs
{
    public class ChatConfiguration : IEntityTypeConfiguration<ChatDTO>
    {
        public void Configure(EntityTypeBuilder<ChatDTO> builder)
        {
           builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).HasMaxLength(255);
            builder.Property(t => t.CreatedAt).IsRequired().HasColumnType("Date").HasDefaultValueSql("GETDATE()");
            
            //Update Date has to be > DateCreatedAt
            builder.Property(t => t.UpdatedAt).IsRequired().HasColumnType("Date");
        }
    }
}
