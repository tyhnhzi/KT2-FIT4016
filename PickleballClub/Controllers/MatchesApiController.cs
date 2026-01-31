using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Controllers;

[Authorize]
[ApiController]
[Route("api/matches")]
public class MatchesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MatchesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Match>>> GetMatches()
    {
        var matches = await _context.Matches.AsNoTracking().ToListAsync();
        return Ok(matches);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Match>> GetMatch(int id)
    {
        var match = await _context.Matches.FindAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        return Ok(match);
    }

    [HttpPost]
    public async Task<ActionResult<Match>> CreateMatch(Match match)
    {
        _context.Matches.Add(match);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, match);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMatch(int id, Match match)
    {
        if (id != match.Id)
        {
            return BadRequest();
        }

        _context.Entry(match).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _context.Matches.AnyAsync(m => m.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        var match = await _context.Matches.FindAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        _context.Matches.Remove(match);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
