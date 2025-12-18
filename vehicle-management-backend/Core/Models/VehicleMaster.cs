namespace vehicle_management_backend.Core.Models
{
    public class VehicleMaster
    {
        public Guid VehicleId { get; set; }
        public string RegNo { get; set; }

        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }

        public Guid ModelId { get; set; }
        public Model Model { get; set; }
    }
}
    