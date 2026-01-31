using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Controllers;

[Authorize]
[ApiController]
[Route("api/participants")]
public class ParticipantsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ParticipantsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants()
    {
        var participants = await _context.Participants.AsNoTracking().ToListAsync();
        return Ok(participants);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Participant>> GetParticipant(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null)
        {
            return NotFound();
        }

        return Ok(participant);
    }

    [HttpPost]
    public async Task<ActionResult<Participant>> CreateParticipant(Participant participant)
    {
        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParticipant), new { id = participant.Id }, participant);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateParticipant(int id, Participant participant)
    {
        if (id != participant.Id)
        {
            return BadRequest();
        }

        _context.Entry(participant).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _context.Participants.AnyAsync(p => p.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteParticipant(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null)
        {
            return NotFound();
        }

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
