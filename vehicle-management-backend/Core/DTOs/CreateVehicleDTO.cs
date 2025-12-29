namespace vehicle_management_backend.Core.DTOs
{
    public class VehicleDTO
    {
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string RegNo { get; set; }
        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }
        public int? ModelYear { get; set; }
        public bool IsActive { get; set; }
    }
}