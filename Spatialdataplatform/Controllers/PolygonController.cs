using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spatialdataplatform.Data;
using Spatialdataplatform.Models;

namespace Spatialdataplatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolygonController : Controller
    {
        private readonly SpatialDbContext _context;

        public PolygonController(SpatialDbContext context)
        {
            _context = context;
        }

        // Create a new polygon
        [HttpPost]
        public async Task<ActionResult<PolygonModel>> CreatePolygon(PolygonModel polygon)
        {
            _context.Polygons.Add(polygon);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPolygon), new { id = polygon.Id }, polygon);
        }

        // Get all polygons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PolygonModel>>> GetPolygons()
        {
            return await _context.Polygons.ToListAsync();
        }

        // Get a polygon by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PolygonModel>> GetPolygon(int id)
        {
            var polygon = await _context.Polygons.FindAsync(id);

            if (polygon == null)
            {
                return NotFound();
            }

            return polygon;
        }

        // Update a polygon
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolygon(int id, PolygonModel polygon)
        {
            if (id != polygon.Id)
            {
                return BadRequest();
            }

            _context.Entry(polygon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Polygons.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Delete a polygon
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolygon(int id)
        {
            var polygon = await _context.Polygons.FindAsync(id);
            if (polygon == null)
            {
                return NotFound();
            }

            _context.Polygons.Remove(polygon);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
