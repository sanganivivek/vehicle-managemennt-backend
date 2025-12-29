namespace vehicle_management_backend.Core.DTOs
{
    public class VehicleDTO
    {
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }
        // Optional: Add logic for 'isActive' if you need it
    }
}