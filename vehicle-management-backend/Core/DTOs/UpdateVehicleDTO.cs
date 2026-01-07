namespace vehicle_management_backend.Core.DTOs
{
    public class UpdateVehicleDTO
    {
        public string RegNo { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }
        public int? ModelYear { get; set; }
        public bool IsActive { get; set; }

        // ✅ ADD THIS LINE
        public int CurrentStatus { get; set; }
    }
}
