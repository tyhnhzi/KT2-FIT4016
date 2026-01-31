using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Controllers;

[Authorize]
[ApiController]
[Route("api/challenges")]
public class ChallengesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ChallengesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Challenge>>> GetChallenges()
    {
        var challenges = await _context.Challenges.AsNoTracking().ToListAsync();
        return Ok(challenges);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Challenge>> GetChallenge(int id)
    {
        var challenge = await _context.Challenges.FindAsync(id);
        if (challenge == null)
        {
            return NotFound();
        }

        return Ok(challenge);
    }

    [HttpPost]
    public async Task<ActionResult<Challenge>> CreateChallenge(Challenge challenge)
    {
        _context.Challenges.Add(challenge);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetChallenge), new { id = challenge.Id }, challenge);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateChallenge(int id, Challenge challenge)
    {
        if (id != challenge.Id)
        {
            return BadRequest();
        }

        _context.Entry(challenge).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _context.Challenges.AnyAsync(c => c.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteChallenge(int id)
    {
        var challenge = await _context.Challenges.FindAsync(id);
        if (challenge == null)
        {
            return NotFound();
        }

        _context.Challenges.Remove(challenge);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
