using System.ComponentModel.DataAnnotations;

namespace vehicle_management_backend.Core.Models
{
    public class VehicleMaster
    {
        [Key]
        public Guid VehicleId { get; set; } // Renamed from Id to VehicleId

        public Guid BrandId { get; set; }
        public Guid ModelId { get; set; }

        public string VehicleName { get; set; } // Used to be RegNo in some versions
        public bool IsActive { get; set; }      // Added this because Controller uses it
    }
}