using System.ComponentModel.DataAnnotations;
using System.Security;
namespace vehicle_management_backend.Core.DTOs
{
    public class VehicleDTO
    {
        public Guid VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string RegNo { get; set; }

        [Required]
        public string BrandCode { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public int? ModelYear { get; set; }
        public bool IsActive { get; set; }
        public int CurrentStatus { get; set; }
    }
}