namespace vehicle_management_backend.Core.Models
{
    public class Brand
    {
        public Guid BrandId { get; set; }
        public string BrandName { get; set; }

        public required ICollection<Model> Models { get; set; }
    }
}
