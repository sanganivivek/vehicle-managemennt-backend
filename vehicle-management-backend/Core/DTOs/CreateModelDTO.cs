using System.ComponentModel.DataAnnotations;

namespace vehicle_management_backend.Core.DTOs
{
    public class CreateModelDTO
    {
        [Required]
        public Guid BrandId { get; set; }

        [StringLength(50)]
        public string? ModelCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }



        [StringLength(500)]
        public string? Description { get; set; }
    }
}