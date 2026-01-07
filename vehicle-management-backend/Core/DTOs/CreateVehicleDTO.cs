using System.Security;

namespace vehicle_management_backend.Core.DTOs
{
    public class VehicleDTO
    {
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string RegNo { get; set; }
        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public int? ModelYear { get; set; }
        public bool IsActive { get; set; }
        public int CurrentStatus { get; set; }
    }
}