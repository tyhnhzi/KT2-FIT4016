using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Bookings
{
    [Authorize(Roles = "Admin,Member")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Booking> Bookings { get;set; } = default!;

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

            var query = _context.Bookings
                .Include(b => b.Member)
                .Include(b => b.Court)
                .AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                // Only show confirmed bookings or your own
                var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
                var member = await _context.Members.FirstOrDefaultAsync(m => m.IdentityUserId == userId);
                
                query = query.Where(b => b.Status == BookingStatus.Confirmed || (member != null && b.MemberId == member.Id));
            }

            Bookings = await query.OrderByDescending(b => b.StartTime).ToListAsync();
        }

        public async Task<IActionResult> OnPostConfirmAsync(int id)
        {
            if (!User.IsInRole("Admin")) return Forbid();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Confirmed;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
