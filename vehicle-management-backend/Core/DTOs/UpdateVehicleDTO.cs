namespace vehicle_management_backend.Core.DTOs
{
    public class UpdateVehicleDTO : CreateVehicleDTO1
    {
        public string RegNo { get; set; } = string.Empty;
        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }
        public int? ModelYear { get; set; }
        public bool IsActive { get; set; }
        public int CurrentStatus { get; set; } = 0;
    }
}
