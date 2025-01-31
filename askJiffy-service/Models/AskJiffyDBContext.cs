using askJiffy_service.Configuration.EfConfigs;
using askJiffy_service.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace askJiffy_service.Models
{
    public class AskJiffyDBContext : DbContext
    {
        public AskJiffyDBContext(DbContextOptions<AskJiffyDBContext> options)
        : base(options)
        {

        }

        //add Foreign key references to Chat and Vehicles
        DbSet<ChatSessionDTO> ChatSessions { get; set; }
        DbSet<ChatMessageDTO> ChatMessages { get; set; }
        DbSet<VehicleDTO> Vehicles { get; set; }
        DbSet<UserDTO> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            
            //Define entity specific configuration in EFConfig
            modelBuilder.ApplyConfiguration(new ChatSessionConfiguration());
            
            
        }
    }
}
