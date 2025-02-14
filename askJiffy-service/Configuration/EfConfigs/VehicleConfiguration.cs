using askJiffy_service.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace askJiffy_service.Configuration.EfConfigs
{
    public class VehicleConfiguration : IEntityTypeConfiguration<VehicleDTO>
    {
        public void Configure(EntityTypeBuilder<VehicleDTO> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Make).IsRequired().HasMaxLength(50);
            builder.Property(v => v.Model).IsRequired().HasMaxLength(50);
            builder.Property(v => v.Year).IsRequired().HasColumnType("int"); // For validation in app logic
            builder.Property(v => v.Transmission).HasColumnType("int").HasDefaultValue(0);
            builder.Property(v => v.Mileage).HasColumnType("int");
        }
    }
}
