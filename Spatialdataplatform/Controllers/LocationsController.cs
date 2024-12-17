using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spatialdataplatform.Data;
using Spatialdataplatform.Models;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Location = Spatialdataplatform.Models.Location;
using Npgsql;

namespace Spatialdataplatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly SpatialDbContext _context;

        public LocationsController(SpatialDbContext context)
        {
            _context = context;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            var locations = await _context.Locations.ToListAsync();

            if (!locations.Any())
            {
                return NotFound("No locations found.");
            }

            return Ok(locations);
        }

        // POST: api/Locations
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            if (location.Coordinate == null)
            {
                return BadRequest("The location must have valid coordinates.");
            }

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location);
        }

        // GET: api/Locations/within
        [HttpGet("within")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsWithin(double latitude, double longitude, double distance)
        {
            if (distance <= 0)
            {
                return BadRequest("Distance must be greater than zero.");
            }

            try
            {
                var sql = @"
            SELECT * 
            FROM Locations
            WHERE ST_DWithin(Coordinate, ST_SetSRID(ST_MakePoint(@longitude, @latitude), 4326), @distance)";

                var locationsWithin = await _context.Locations
                    .FromSqlRaw(sql,
                        new NpgsqlParameter("@longitude", longitude),
                        new NpgsqlParameter("@latitude", latitude),
                        new NpgsqlParameter("@distance", distance))
                    .ToListAsync();

                if (!locationsWithin.Any())
                {
                    return NotFound("No locations found within the specified distance.");
                }

                return Ok(locationsWithin);
            }
            catch (Exception ex)
            {
                // Log the error (optional) and return a server error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // GET: api/Locations/intersect
        // Example: Get Locations that intersect with a polygon
        // POST: api/Locations/intersect
        [HttpPost("intersect")]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationsIntersecting([FromBody] Polygon polygon)
        {
            if (polygon == null || polygon.IsEmpty)
            {
                return BadRequest("A valid polygon must be provided.");
            }

            try
            {
                var wkt = polygon.AsText(); // Convert the polygon to WKT (Well-Known Text) format

                var sql = @"
            SELECT * 
            FROM Locations
            WHERE ST_Intersects(Coordinate, ST_GeomFromText(@wkt, 4326))";

                var locationsIntersecting = await _context.Locations
                    .FromSqlRaw(sql, new NpgsqlParameter("@wkt", wkt))
                    .ToListAsync();

                if (!locationsIntersecting.Any())
                {
                    return NotFound("No locations found that intersect with the provided polygon.");
                }

                return Ok(locationsIntersecting);
            }
            catch (Exception ex)
            {
                // Log the error (optional) and return a server error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
