using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Challenges
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
        public Challenge Challenge { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var challenge = await _context.Challenges.FirstOrDefaultAsync(m => m.Id == id);
            if (challenge == null) return NotFound();
            Challenge = challenge;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var challengeToUpdate = await _context.Challenges.FirstOrDefaultAsync(m => m.Id == Challenge.Id);
            if (challengeToUpdate == null) return NotFound();

            challengeToUpdate.Title = Challenge.Title;
            challengeToUpdate.Description = Challenge.Description;
            challengeToUpdate.GameMode = Challenge.GameMode;
            challengeToUpdate.Type = Challenge.Type;
            challengeToUpdate.Status = Challenge.Status;
            challengeToUpdate.EntryFee = Challenge.EntryFee;
            challengeToUpdate.PrizePool = Challenge.PrizePool;
            challengeToUpdate.RewardDescription = Challenge.RewardDescription;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeExists(Challenge.Id)) return NotFound();
                else throw;
            }

            return RedirectToPage("./Index");
        }

        private bool ChallengeExists(int id)
        {
            return _context.Challenges.Any(e => e.Id == id);
        }
    }
}
