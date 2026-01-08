using System.ComponentModel.DataAnnotations;

namespace vehicle_management_backend.Core.DTOs
{
    public class CreateVehicleWithoutNameDTO
    {
        [Required]
        public string RegNo { get; set; } = string.Empty;
        
        [Required]
        public Guid BrandId { get; set; }
        
        [Required]
        public Guid ModelId { get; set; }
        
        public int? ModelYear { get; set; }
        
        public bool IsActive { get; set; } = true;
        public int CurrentStatus { get; set; }
    }
}