namespace vehicle_management_backend.Core.Models
{
    public class Model
    {
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }

        public Guid BrandId { get; set; }
        public required Brand Brand { get; set; }
    }
}
