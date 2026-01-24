using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.Members
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Member> Members { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Members = await _context.Members
                .Include(m => m.User)
                .ToListAsync();
        }
    }
}
