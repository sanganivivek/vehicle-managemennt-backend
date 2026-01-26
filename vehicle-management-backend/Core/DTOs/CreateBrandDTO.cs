namespace vehicle_management_backend.Core.DTOs
{
    public class BrandDTO
    {
        public Guid? BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandCode { get; set; }
        public bool IsActive { get; set;  }
    }
}