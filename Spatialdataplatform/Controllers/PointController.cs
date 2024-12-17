using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spatialdataplatform.Data;
using Spatialdataplatform.Models;

namespace Spatialdataplatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : Controller
    {
        private readonly SpatialDbContext _context;

        public PointController(SpatialDbContext context)
        {
            _context = context;
        }

        // Create a new point
        [HttpPost]
        public async Task<ActionResult<PointModel>> CreatePoint(PointModel point)
        {
            _context.Points.Add(point);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPoint), new { id = point.Id }, point);
        }

        // Get all points
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointModel>>> GetPoints()
        {
            return await _context.Points.ToListAsync();
        }

        // Get a point by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PointModel>> GetPoint(int id)
        {
            var point = await _context.Points.FindAsync(id);

            if (point == null)
            {
                return NotFound();
            }

            return point;
        }

        // Update a point
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePoint(int id, PointModel point)
        {
            if (id != point.Id)
            {
                return BadRequest();
            }

            _context.Entry(point).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Points.Any(e => e.Id == id))
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

        // Delete a point
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoint(int id)
        {
            var point = await _context.Points.FindAsync(id);
            if (point == null)
            {
                return NotFound();
            }

            _context.Points.Remove(point);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
