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
            modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            //For DB model refer to db context puml diagram

            //ChatSession relationships
            // ChatSession - Vehicle, ChatSession - User
            modelBuilder.Entity<ChatSessionDTO>(b =>
            {
                b.HasOne(cs => cs.Vehicle)  
                 .WithMany(v => v.ChatSessions) 
                 .HasForeignKey("VehicleId") 
                 .IsRequired();

                b.HasOne(cs => cs.User) 
                .WithMany(u => u.ChatSessions) 
                .HasForeignKey("UserId")
                .IsRequired(false);
            });

            //User - Vehicle Relationship
            //only need to define relationship from one side
            modelBuilder.Entity<VehicleDTO>(b =>
            {
                b.HasOne(v => v.User)
                 .WithMany(u => u.Vehicles)
                 .HasForeignKey("UserId") 
                 .IsRequired(false); 
            });

            //ChatMessage - ChatSession Relationship
            modelBuilder.Entity<ChatMessageDTO>(b =>
            {
                b.HasOne(cm => cm.ChatSession)  
                 .WithMany(cs => cs.ChatMessages)
                 .HasForeignKey("ChatSessionId") 
                 .IsRequired(); 
            });

        }
    }
}
