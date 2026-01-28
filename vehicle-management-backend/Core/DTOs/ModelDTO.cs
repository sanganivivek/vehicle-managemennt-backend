namespace vehicle_management_backend.Core.DTOs
{
    public class ModelDTO
    {
        public Guid ModelId { get; set; }
        public string? ModelCode { get; set; }
        public string? ModelName { get; set; }
        public string? ModelType { get; set; }
        public string? Description { get; set; }
        public Guid BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? BrandCode { get; set; }
    }
}