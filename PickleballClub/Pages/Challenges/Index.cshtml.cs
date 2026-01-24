using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Challenges
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Challenge> Challenges { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
                if (userId != null)
                {
                    var member = await _context.Members.FirstOrDefaultAsync(m => m.IdentityUserId == userId);
                    if (member == null)
                    {
                        member = new Member
                        {
                            IdentityUserId = userId,
                            FullName = User.Identity.Name ?? "New Member",
                            JoinDate = DateTime.Now,
                            RankLevel = 1.0,
                            Status = "Active",
                            DOB = DateTime.Now.AddYears(-20),
                            PhoneNumber = "0000000000"
                        };
                        _context.Members.Add(member);
                        await _context.SaveChangesAsync();
                        
                        var user = await _userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles.Count == 0) await _userManager.AddToRoleAsync(user, "Member");
                        }
                    }
                }
            }

            var query = _context.Challenges
                .Include(c => c.Participants)
                .AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                // Non-admins see Open, Ongoing, Finished. 
                query = query.Where(c => c.Status != ChallengeStatus.Pending);
            }

            // Custom Sorting: Open -> Ongoing -> Finished
            // Assuming Enum Values: Open=?, Ongoing=?, Finished=?
            // We can use a custom sorting logic in-memory if dataset is small, or conditional order in SQL
            
            var allChallenges = await query.ToListAsync();
            
            Challenges = allChallenges.OrderBy(c => 
            {
                // Custom Priority: Open(1) -> Ongoing(2) -> Finished(3) -> Others(4)
                if (c.Status == ChallengeStatus.Open) return 1;
                if (c.Status == ChallengeStatus.Ongoing) return 2;
                if (c.Status == ChallengeStatus.Finished) return 3;
                return 4; // Pending or Cancelled
            }).ToList();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            if (!User.IsInRole("Admin")) return Forbid();

            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge != null && challenge.Status == ChallengeStatus.Pending)
            {
                challenge.Status = ChallengeStatus.Open;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostStartAsync(int id)
        {
            if (!User.IsInRole("Admin")) return Forbid();

            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge != null && challenge.Status == ChallengeStatus.Open)
            {
                challenge.Status = ChallengeStatus.Ongoing;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostJoinAsync(int id)
        {
            if (!User.Identity.IsAuthenticated) return Challenge();

            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null || challenge.Status != ChallengeStatus.Open) return NotFound();

            // Get current member
            var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            var member = await _context.Members.FirstOrDefaultAsync(m => m.IdentityUserId == userId);

            if (member == null) return BadRequest("Member not found.");

            // Check if already joined
            var exists = await _context.Participants.AnyAsync(p => p.ChallengeId == id && p.MemberId == member.Id);
            if (!exists)
            {
                _context.Participants.Add(new Participant 
                { 
                    ChallengeId = id, 
                    MemberId = member.Id,
                    Status = ParticipantStatus.Confirmed,
                    EntryFeePaid = false,
                    EntryFeeAmount = challenge.EntryFee
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
