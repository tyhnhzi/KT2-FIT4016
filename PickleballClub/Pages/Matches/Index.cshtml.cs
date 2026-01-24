using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Matches
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Match> Matches { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Matches = await _context.Matches
                .Include(m => m.Team1_Player1)
                .Include(m => m.Team1_Player2)
                .Include(m => m.Team2_Player1)
                .Include(m => m.Team2_Player2)
                .Include(m => m.Challenge)
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Team1_Player1)
                .Include(m => m.Team1_Player2)
                .Include(m => m.Team2_Player1)
                .Include(m => m.Team2_Player2)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null || match.IsApproved) return NotFound();

            match.IsApproved = true;
            await UpdateBusinessLogicAfterSelection(match);
            
            await _context.SaveChangesAsync();
            return RedirectToPage();
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
