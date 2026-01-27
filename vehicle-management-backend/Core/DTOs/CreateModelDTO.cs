using System.ComponentModel.DataAnnotations;
namespace vehicle_management_backend.Core.DTOs
{
    public class CreateModelDTO
    {
        [Required]
        public string BrandCode { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
    }
}