using askJiffy_service.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace askJiffy_service.Configuration.EfConfigs
{
    public class ChatMessageConfiguration: IEntityTypeConfiguration<ChatMessageDTO>
    {
        public void Configure(EntityTypeBuilder<ChatMessageDTO> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Question).IsRequired().HasMaxLength(7000);
        }
    }
}
