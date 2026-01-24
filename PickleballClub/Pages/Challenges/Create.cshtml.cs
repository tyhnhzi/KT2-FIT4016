using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PickleballClub.Data;
using PickleballClub.Models;
using System.Security.Claims;

namespace PickleballClub.Pages.Challenges
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Challenge Challenge { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Default status - Pending for members, Open for Admin
            Challenge.Status = User.IsInRole("Admin") ? ChallengeStatus.Open : ChallengeStatus.Pending;
            Challenge.CreatedDate = DateTime.Now;
            Challenge.ModifiedDate = DateTime.Now;
            Challenge.PrizePool = 0; // Calculated or seeded
            
            _context.Challenges.Add(Challenge);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
