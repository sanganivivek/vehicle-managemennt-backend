using System.ComponentModel.DataAnnotations;

namespace vehicle_management_backend.Core.Models
{
    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = "info"; // success, warning, info
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}