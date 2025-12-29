namespace vehicle_management_backend.Core.DTOs
{
    public class ModelDTO
    {
        public Guid ModelId { get; set; }
        public string ModelName { get; set; }
        public Guid BrandId { get; set; }
    }
}