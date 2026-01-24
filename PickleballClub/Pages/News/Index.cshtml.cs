using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PickleballClub.Data;
using PickleballClub.Models;

namespace PickleballClub.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.News> News { get;set; } = default!;

        public async Task OnGetAsync()
        {
            News = await _context.News
                .OrderByDescending(n => n.PostedDate)
                .ToListAsync();
        }
    }
}
