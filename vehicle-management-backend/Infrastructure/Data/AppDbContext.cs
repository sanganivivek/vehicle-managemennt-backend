using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Core.Models;
namespace vehicle_management_backend.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<VehicleMaster> Vehicles { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<VehicleMaster>()
                .HasKey(v => v.VehicleId);

            // Fix Multiple Cascade Path: Vehicle -> Brand (Restrict)
            modelBuilder.Entity<VehicleMaster>()
                .HasOne(v => v.Brand)
                .WithMany()
                .HasForeignKey(v => v.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vehicle -> Model (Restrict or Cascade is fine, usually Cascade if Model is deleted)
            modelBuilder.Entity<VehicleMaster>()
                .HasOne(v => v.Model)
                .WithMany()
                .HasForeignKey(v => v.ModelId)
                .OnDelete(DeleteBehavior.Restrict); 
                
            // Brand -> Model (Cascade is standard)
            modelBuilder.Entity<Model>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.Models)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}