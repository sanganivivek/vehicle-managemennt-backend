using System.ComponentModel.DataAnnotations;

namespace vehicle_management_backend.Core.Models
{
    public class VehicleMaster
    {
        [Key]
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }

        public string VehicleName { get; set; }
    }
}
