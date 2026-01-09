
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vehicle_management_backend.Infrastructure.Data;
#nullable disable
namespace vehicle_management_backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20251227173448_RebuildSchemaWithGuids")]
    partial class RebuildSchemaWithGuids
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);
            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);
            modelBuilder.Entity("vehicle_management_backend.Core.Models.Brand", b =>
                {
                    b.Property<Guid>("BrandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");
                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.HasKey("BrandId");
                    b.ToTable("Brands");
                });
            modelBuilder.Entity("vehicle_management_backend.Core.Models.Model", b =>
                {
                    b.Property<Guid>("ModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");
                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier");
                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.HasKey("ModelId");
                    b.HasIndex("BrandId");
                    b.ToTable("Models");
                });
            modelBuilder.Entity("vehicle_management_backend.Core.Models.VehicleMaster", b =>
                {
                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier");
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");
                    b.Property<Guid>("ModelId")
                        .HasColumnType("uniqueidentifier");
                    b.Property<string>("VehicleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");
                    b.ToTable("Vehicles");
                });
            modelBuilder.Entity("vehicle_management_backend.Core.Models.Model", b =>
                {
                    b.HasOne("vehicle_management_backend.Core.Models.Brand", "Brand")
                        .WithMany("Models")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                    b.Navigation("Brand");
                });
            modelBuilder.Entity("vehicle_management_backend.Core.Models.Brand", b =>
                {
                    b.Navigation("Models");
                });
#pragma warning restore 612, 618
        }
    }
}