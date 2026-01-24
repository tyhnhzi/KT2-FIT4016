using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Matches
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Members"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
            ViewData["Challenges"] = new SelectList(await _context.Challenges.Where(c => c.Status == ChallengeStatus.Ongoing).ToListAsync(), "Id", "Title");
            return Page();
        }

        [BindProperty]
        public Match Match { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            // Simple validation: unique players
            var players = new List<int?> { Match.Team1_Player1Id, Match.Team1_Player2Id, Match.Team2_Player1Id, Match.Team2_Player2Id };
            var uniquePlayers = players.Where(p => p.HasValue).Distinct().Count();
            var totalPlayers = players.Count(p => p.HasValue);

            if (uniquePlayers != totalPlayers)
            {
                ModelState.AddModelError("", "Một người không thể xuất hiện 2 lần trong cùng 1 trận!");
                ViewData["Members"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
                ViewData["Challenges"] = new SelectList(await _context.Challenges.Where(c => c.Status == ChallengeStatus.Ongoing).ToListAsync(), "Id", "Title");
                return Page( );
            }

            Match.MatchDate = DateTime.Now;
            // Admin created matches are auto-approved, others (Referee) are not
            Match.IsApproved = User.IsInRole("Admin");
            
            _context.Matches.Add(Match);

            // Only update Rank and Challenge score if match IS APPROVED (Admin created)
            if (Match.IsApproved)
            {
                await UpdateBusinessLogicAfterSelection(Match);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task UpdateBusinessLogicAfterSelection(Match match)
        {
            // Update Rank if Ranked
            if (match.IsRanked && match.WinningSide != WinningSide.None)
            {
                var team1Players = new List<int?> { match.Team1_Player1Id, match.Team1_Player2Id }.Where(id => id.HasValue).ToList();
                var team2Players = new List<int?> { match.Team2_Player1Id, match.Team2_Player2Id }.Where(id => id.HasValue).ToList();

                foreach (var id in team1Players)
                {
                    var m = await _context.Members.FindAsync(id);
                    if (m != null)
                    {
                        m.RankLevel += match.WinningSide == WinningSide.Team1 ? 0.1 : -0.1;
                        m.TotalMatches++;
                        if (match.WinningSide == WinningSide.Team1) m.WinMatches++;
                    }
                }

                foreach (var id in team2Players)
                {
                    var m = await _context.Members.FindAsync(id);
                    if (m != null)
                    {
                        m.RankLevel += match.WinningSide == WinningSide.Team2 ? 0.1 : -0.1;
                        m.TotalMatches++;
                        if (match.WinningSide == WinningSide.Team2) m.WinMatches++;
                    }
                }
            }

            // If it belongs to a Challenge
            if (match.ChallengeId.HasValue)
            {
                var challenge = await _context.Challenges.FindAsync(match.ChallengeId.Value);
                if (challenge != null && challenge.GameMode == ChallengeGameMode.TeamBattle)
                {
                    if (match.WinningSide == WinningSide.Team1) challenge.CurrentScore_TeamA++;
                    else if (match.WinningSide == WinningSide.Team2) challenge.CurrentScore_TeamB++;

                    if (challenge.Config_TargetWins.HasValue && 
                        (challenge.CurrentScore_TeamA >= challenge.Config_TargetWins || challenge.CurrentScore_TeamB >= challenge.Config_TargetWins))
                    {
                        challenge.Status = ChallengeStatus.Finished;
                    }
                }
            }
        }
    }
}
