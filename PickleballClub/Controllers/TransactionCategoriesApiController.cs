using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Controllers;

[Authorize]
[ApiController]
[Route("api/transaction-categories")]
public class TransactionCategoriesApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TransactionCategoriesApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionCategory>>> GetCategories()
    {
        var categories = await _context.TransactionCategories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransactionCategory>> GetCategory(int id)
    {
        var category = await _context.TransactionCategories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionCategory>> CreateCategory(TransactionCategory category)
    {
        _context.TransactionCategories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, TransactionCategory category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await _context.TransactionCategories.AnyAsync(c => c.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.TransactionCategories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.TransactionCategories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
