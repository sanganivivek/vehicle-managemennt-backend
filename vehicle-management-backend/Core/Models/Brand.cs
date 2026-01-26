namespace vehicle_management_backend.Core.Models
{
    public class Brand
    {
        public Guid brandId { get; set; }
        public string? BrandCode { get; set; }
        public string? BrandName { get; set; }
        public bool IsActive { get; set; } = true;
        public required ICollection<Model> Models { get; set; }
    }
}