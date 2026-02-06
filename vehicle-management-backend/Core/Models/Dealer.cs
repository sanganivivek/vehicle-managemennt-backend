using System.ComponentModel.DataAnnotations;

namespace vehicle_management_backend.Core.Models
{
    public class Dealer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string GSTNo { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        // Ensure this matches the case of the property in DealerService (I used lowercase 'status' to match your request)
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}