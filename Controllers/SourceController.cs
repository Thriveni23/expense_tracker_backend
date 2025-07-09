using ExpenseTrackerCrudWebAPI.Database;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SourceController : ControllerBase
    {
        private readonly ExpenseTrackerDBContext _context;

        public SourceController(ExpenseTrackerDBContext context)
        {
            _context = context;
        }

        // GET: api/Source
        [HttpGet]
        public async Task<IActionResult> GetSources()
        {
            var sources = await _context.Sources.ToListAsync();
            return Ok(sources);
        }

        // POST: api/Source
        [HttpPost("add")]
        public async Task<IActionResult> AddSource([FromBody] Source source)
        {
            _context.Sources.Add(source);
            await _context.SaveChangesAsync();
            return Ok(source);
        }

        // DELETE: api/Source/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSource(int id)
        {
            var source = await _context.Sources.FindAsync(id);
            if (source == null)
                return NotFound();

            _context.Sources.Remove(source);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
