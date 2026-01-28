namespace vehicle_management_backend.Core.Models
{
    public class Brand
    {
        [System.ComponentModel.DataAnnotations.Key]
        public Guid BrandId { get; set; }
        public string? BrandCode { get; set; }
        public string? BrandName { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Model>? Models { get; set; }
    }
}