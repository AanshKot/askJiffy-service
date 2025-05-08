using askJiffy_service.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace askJiffy_service.Configuration.EfConfigs
{
    public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSessionDTO>
    {
        public void Configure(EntityTypeBuilder<ChatSessionDTO> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).HasMaxLength(255);
            builder.Property(t => t.CreatedAt).IsRequired().HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()");
            
            //Update Date has to be > DateCreatedAt
            builder.Property(t => t.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate().HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()");
            builder.Property(t => t.IsDeleted).HasDefaultValue(false);
        }
    }
}
