using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages
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

        public decimal TotalBalance { get; set; }
        public int OpenChallengesCount { get; set; }
        public int UpcomingBookingsCount { get; set; }
        public List<Member> TopRankingMembers { get; set; } = new();

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
                        // Auto-create member record for newly registered users
                        member = new Member
                        {
                            IdentityUserId = userId,
                            FullName = User.Identity.Name ?? "New Member",
                            JoinDate = DateTime.Now,
                            RankLevel = 1.0,
                            Status = "Active",
                            DOB = DateTime.Now.AddYears(-20), // Default
                            PhoneNumber = "0000000000"
                        };
                        _context.Members.Add(member);
                        await _context.SaveChangesAsync();

                        // Also ensure they have the "Member" role if they don't have any
                        var user = await _userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles.Count == 0)
                            {
                                await _userManager.AddToRoleAsync(user, "Member");
                            }
                        }
                    }
                }
            }

            TotalBalance = await _context.Transactions.SumAsync(t => t.Amount);

            OpenChallengesCount = await _context.Challenges
                .CountAsync(c => c.Status == ChallengeStatus.Open);

            UpcomingBookingsCount = await _context.Bookings
                .CountAsync(b => b.StartTime > DateTime.Now && b.Status == BookingStatus.Confirmed);

            TopRankingMembers = await _context.Members
                .OrderByDescending(m => m.RankLevel)
                .Take(5)
                .ToListAsync();
        }
    }
}
