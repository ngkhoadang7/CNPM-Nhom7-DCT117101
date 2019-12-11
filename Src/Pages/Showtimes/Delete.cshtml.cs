using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Showtimes
{
    [Authorize(Roles="Admin")]
    public class DeleteModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public DeleteModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Showtime Showtime { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater).FirstOrDefaultAsync(s => s.ID == id);

            if (Showtime == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Showtime = await _context.Showtimes.FindAsync(id);

            if (Showtime != null)
            {
                _context.Showtimes.Remove(Showtime);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
