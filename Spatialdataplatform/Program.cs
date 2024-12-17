using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using Spatialdataplatform.Data;
using Spatialdataplatform.Models;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext with PostgreSQL and NetTopologySuite
builder.Services.AddDbContext<SpatialDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
        npgsqlOptions => npgsqlOptions.UseNetTopologySuite())
            .EnableSensitiveDataLogging()  // Log sensitive data for troubleshooting (be careful in production)
            .LogTo(Console.WriteLine, LogLevel.Information));  // Log to console for more insights
//var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
//builder.Services.AddDbContext<DbContext>(options =>
//    options.UseNpgsql(connectionString));

//Console.WriteLine($"Connection String: {connectionString}");


// Add other necessary services (e.g., Swagger, authentication, etc.)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Swagger UI setup for API testing in development environment (optional)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
