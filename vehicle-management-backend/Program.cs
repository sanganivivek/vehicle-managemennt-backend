// ... your existing using statements ...

using Microsoft.EntityFrameworkCore;
using vehicle_management_backend.Application.Services.Implementations;
using vehicle_management_backend.Application.Services.Interfaces;
using vehicle_management_backend.Infrastructure.Data;
using vehicle_management_backend.Infrastructure.Repositories.Implementations;
using vehicle_management_backend.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Existing DB context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Existing Scoped Services
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IModelRepository, ModelRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

// 3. ADD THIS LINE (Required for Swagger to find your API)
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. WRAP IN DEVELOPMENT CHECK (Best practice)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vehicle Management API v1");
    });
}

app.UseHttpsRedirection(); // Add this for security
app.UseAuthorization();   // Add this if you plan to use security
app.MapControllers();
app.Run();