using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using vehicle_management_backend.Core.Models;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

namespace vehicle_management_backend.Infrastructure.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly string _connectionString;

        public VehicleRepository(IConfiguration configuration)
        {
            // We get the connection string from appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // 1. GET ALL VEHICLES
        public async Task<List<VehicleMaster>> GetAllAsync()
        {
            var vehicleList = new List<VehicleMaster>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllVehicles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            vehicleList.Add(MapReaderToVehicle(reader));
                        }
                    }
                }
            }
            return vehicleList;
        }

        // 2. GET VEHICLE BY ID
        public async Task<VehicleMaster?> GetByIdAsync(Guid id)
        {
            VehicleMaster? vehicle = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetVehicleById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleId", id);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            vehicle = MapReaderToVehicle(reader);
                        }
                    }
                }
            }
            return vehicle;
        }

        // 3. ADD (INSERT) VEHICLE
        public async Task AddAsync(VehicleMaster vehicle)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_AddVehicle", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass parameters to the Stored Procedure
                    cmd.Parameters.AddWithValue("@VehicleId", vehicle.VehicleId);
                    cmd.Parameters.AddWithValue("@BrandId", vehicle.BrandId);
                    cmd.Parameters.AddWithValue("@ModelId", vehicle.ModelId);
                    cmd.Parameters.AddWithValue("@VehicleName", vehicle.VehicleName);
                    cmd.Parameters.AddWithValue("@RegNo", vehicle.RegNo);
                    // Handle NULL for ModelYear (int?)
                    cmd.Parameters.AddWithValue("@ModelYear", vehicle.ModelYear ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", vehicle.IsActive);
                    cmd.Parameters.AddWithValue("@CurrentStatus", vehicle.CurrentStatus);
                    cmd.Parameters.AddWithValue("@CreatedAt", vehicle.CreatedAt);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // 4. UPDATE VEHICLE
        public async Task UpdateAsync(VehicleMaster vehicle)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateVehicle", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VehicleId", vehicle.VehicleId);
                    cmd.Parameters.AddWithValue("@BrandId", vehicle.BrandId);
                    cmd.Parameters.AddWithValue("@ModelId", vehicle.ModelId);
                    cmd.Parameters.AddWithValue("@VehicleName", vehicle.VehicleName);
                    cmd.Parameters.AddWithValue("@RegNo", vehicle.RegNo);
                    cmd.Parameters.AddWithValue("@ModelYear", vehicle.ModelYear ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", vehicle.IsActive);
                    cmd.Parameters.AddWithValue("@CurrentStatus", vehicle.CurrentStatus);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // 5. DELETE VEHICLE
        public async Task DeleteAsync(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteVehicle", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleId", id);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // --- HELPER: MAPS SQL ROW TO C# OBJECT ---
        private VehicleMaster MapReaderToVehicle(SqlDataReader reader)
        {
            return new VehicleMaster
            {
                VehicleId = (Guid)reader["VehicleId"],
                BrandId = (Guid)reader["BrandId"],
                ModelId = (Guid)reader["ModelId"],
                VehicleName = reader["VehicleName"].ToString() ?? "",
                RegNo = reader["RegNo"].ToString() ?? "",
                // Check for DBNull before reading int
                ModelYear = reader["ModelYear"] != DBNull.Value ? (int?)reader["ModelYear"] : null,
                IsActive = (bool)reader["IsActive"],
                CurrentStatus = (int)reader["CurrentStatus"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }
    }
}