using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Bookings
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
        public Booking Booking { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null) return NotFound();

            Booking = booking;
            
            ViewData["CourtList"] = new SelectList(await _context.Courts.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
            ViewData["MemberList"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["CourtList"] = new SelectList(await _context.Courts.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
                ViewData["MemberList"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
                return Page();
            }

            // Overlap Validation (Excluding current booking)
            var overlaps = await _context.Bookings.AnyAsync(b => 
                b.Id != Booking.Id &&
                b.CourtId == Booking.CourtId && 
                b.Status != BookingStatus.Cancelled &&
                ((Booking.StartTime >= b.StartTime && Booking.StartTime < b.EndTime) || 
                 (Booking.EndTime > b.StartTime && Booking.EndTime <= b.EndTime) ||
                 (Booking.StartTime <= b.StartTime && Booking.EndTime >= b.EndTime)));
            
            if (overlaps)
            {
                ModelState.AddModelError("", "Sân này đã có người đặt trong khung giờ bạn chọn.");
                ViewData["CourtList"] = new SelectList(await _context.Courts.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
                ViewData["MemberList"] = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
                return Page();
            }

            var bookingToUpdate = await _context.Bookings.FirstOrDefaultAsync(m => m.Id == Booking.Id);
            if (bookingToUpdate == null) return NotFound();

            bookingToUpdate.CourtId = Booking.CourtId;
            bookingToUpdate.MemberId = Booking.MemberId;
            bookingToUpdate.StartTime = Booking.StartTime;
            bookingToUpdate.EndTime = Booking.EndTime;
            bookingToUpdate.Notes = Booking.Notes;
            bookingToUpdate.Status = Booking.Status; // Admin can change status freely

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(Booking.Id)) return NotFound();
                else throw;
            }

            return RedirectToPage("./Index");
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
