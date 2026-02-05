using System.ComponentModel.DataAnnotations;

namespace vehicle_management_backend.Core.Models
{
    public class Dealer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string MobileNo { get; set; }

        public string EmailId { get; set; }
    }
}