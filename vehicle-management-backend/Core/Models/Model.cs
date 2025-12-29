namespace vehicle_management_backend.Core.Models
{
    public class Model
    {
            public int ModelId { get; set; }
        public string ModelName { get; set; }

        public Guid BrandId { get; set; }
        public Brand? Brand { get; set; }
    }
}
