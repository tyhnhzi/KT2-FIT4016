using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Treasury
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var transaction = await _context.Transactions.FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null) return NotFound();

            Transaction = transaction;
            ViewData["Categories"] = new SelectList(await _context.TransactionCategories.ToListAsync(), "Id", "CategoryName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = new SelectList(await _context.TransactionCategories.ToListAsync(), "Id", "CategoryName");
                return Page();
            }

            var transactionToUpdate = await _context.Transactions.FirstOrDefaultAsync(m => m.Id == Transaction.Id);
            if (transactionToUpdate == null) return NotFound();

            transactionToUpdate.Amount = Transaction.Amount;
            transactionToUpdate.Description = Transaction.Description;
            transactionToUpdate.CategoryId = Transaction.CategoryId;
            // Optionally update date, or keep original. Let's allow updating date if needed, or just keep it simple.
            // Assuming we might want to fix the date if entered wrongly.
            // transactionToUpdate.TransactionDate = Transaction.TransactionDate; 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(Transaction.Id)) return NotFound();
                else throw;
            }

            return RedirectToPage("./Index");
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
