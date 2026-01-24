using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Treasury
{
    [Authorize(Roles = "Admin")]
    public class CreateCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateCategoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TransactionCategory Category { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.TransactionCategories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
