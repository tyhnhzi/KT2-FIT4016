using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Bookings
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
            if (User.IsInRole("Guest")) return Forbid();

            // Setup select list for members (for Admin selection)
            if (User.IsInRole("Admin"))
            {
                ViewData["MemberList"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
            }
            
            ViewData["CourtList"] = new SelectList(await _context.Courts.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                    ViewData["MemberList"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
                return Page();
            }

            // Get current member if not Admin or if Admin didn't specify
            if (User.IsInRole("Admin") && Booking.MemberId == 0)
            {
                 ModelState.AddModelError("Booking.MemberId", "Vui lòng chọn hội viên.");
                 ViewData["MemberList"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
                 return Page();
            }
            
            if (!User.IsInRole("Admin"))
            {
                var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
                var member = await _context.Members.FirstOrDefaultAsync(m => m.IdentityUserId == userId);
                if (member == null) return BadRequest("Member profile not found.");
                
                Booking.MemberId = member.Id;
                Booking.Status = BookingStatus.Pending;
            }
            else
            {
                Booking.Status = BookingStatus.Confirmed;
            }

            // Overlap Validation
            var overlaps = await _context.Bookings.AnyAsync(b => 
                b.CourtId == Booking.CourtId && 
                b.Status != BookingStatus.Cancelled &&
                ((Booking.StartTime >= b.StartTime && Booking.StartTime < b.EndTime) || 
                 (Booking.EndTime > b.StartTime && Booking.EndTime <= b.EndTime) ||
                 (Booking.StartTime <= b.StartTime && Booking.EndTime >= b.EndTime)));
            
            if (overlaps)
            {
                ModelState.AddModelError("", "Sân này đã có người đặt trong khung giờ bạn chọn.");
                ViewData["CourtList"] = new SelectList(await _context.Courts.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
                if (User.IsInRole("Admin"))
                    ViewData["MemberList"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
                return Page();
            }

            Booking.CreatedDate = DateTime.Now;
            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
