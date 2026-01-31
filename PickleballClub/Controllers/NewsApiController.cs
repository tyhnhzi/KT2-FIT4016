using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Controllers;

[Authorize]
[ApiController]
[Route("api/news")]
public class NewsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NewsApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetNews()
    {
        var news = await _context.News.AsNoTracking().ToListAsync();
        return Ok(news);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<News>> GetNewsItem(int id)
    {
        var item = await _context.News.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<News>> CreateNewsItem(News item)
    {
        _context.News.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNewsItem), new { id = item.Id }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateNewsItem(int id, News item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        _context.Entry(item).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _context.News.AnyAsync(n => n.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNewsItem(int id)
    {
        var item = await _context.News.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _context.News.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
