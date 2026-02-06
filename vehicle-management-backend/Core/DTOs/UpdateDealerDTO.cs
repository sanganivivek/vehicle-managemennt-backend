using System.Text.Json.Serialization;

namespace vehicle_management_backend.Core.DTOs
{
    public class UpdateDealerDTO : CreateDealerDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}