namespace vehicle_management_backend.Core.DTOs
{
    public class CreateVehicleDTO
    {
        public string RegNo { get; set; }
        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }
        public int ModelYear { get; set; }
        public bool IsActive { get; set; }
    }
}
