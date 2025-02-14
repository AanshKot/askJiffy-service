using askJiffy_service.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace askJiffy_service.Configuration.EfConfigs
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDTO>
    {
        public void Configure(EntityTypeBuilder<UserDTO> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            //index on email
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.ProfilePictureUrl).IsRequired().HasMaxLength(500).HasDefaultValue("https://dummyimage.com/100x100/000/fff&text=Set+Your+Profile+Picture");
            
            builder.Property(u => u.Provider).IsRequired().HasMaxLength(100); // Consider adding regex validation
            builder.Property(u => u.Role).IsRequired(); // Enum is generally constrained automatically.
        }
    }
}
