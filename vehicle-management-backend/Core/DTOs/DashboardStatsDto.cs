namespace vehicle_management_backend.Core.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int Rented { get; set; }
        public int Inmaintance { get; set; }
    }
}