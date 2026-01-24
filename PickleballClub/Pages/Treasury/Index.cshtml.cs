using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Treasury
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Transaction> Transactions { get;set; } = default!;
        public decimal TotalBalance { get; set; }

        public async Task OnGetAsync()
        {
            Transactions = await _context.Transactions
                .Include(t => t.TransactionCategory)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            var income = _context.Transactions
                .Include(t => t.TransactionCategory)
                .Where(t => t.TransactionCategory.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var expense = _context.Transactions
                .Include(t => t.TransactionCategory)
                .Where(t => t.TransactionCategory.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            TotalBalance = income - expense;
        }
    }
}
