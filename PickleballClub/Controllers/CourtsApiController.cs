using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Controllers;

[Authorize]
[ApiController]
[Route("api/courts")]
public class CourtsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CourtsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Court>>> GetCourts()
    {
        var courts = await _context.Courts.AsNoTracking().ToListAsync();
        return Ok(courts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Court>> GetCourt(int id)
    {
        var court = await _context.Courts.FindAsync(id);
        if (court == null)
        {
            return NotFound();
        }

        return Ok(court);
    }

    [HttpPost]
    public async Task<ActionResult<Court>> CreateCourt(Court court)
    {
        _context.Courts.Add(court);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCourt), new { id = court.Id }, court);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCourt(int id, Court court)
    {
        if (id != court.Id)
        {
            return BadRequest();
        }

        _context.Entry(court).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _context.Courts.AnyAsync(c => c.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourt(int id)
    {
        var court = await _context.Courts.FindAsync(id);
        if (court == null)
        {
            return NotFound();
        }

        _context.Courts.Remove(court);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
