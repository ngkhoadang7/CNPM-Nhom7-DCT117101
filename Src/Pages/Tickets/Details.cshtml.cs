using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Tickets
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public DetailsModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public Ticket Ticket { get; set; }

        public string getStartTimeFormatted(Ticket t)
        {
            return t.Showtime.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket = await _context.Tickets
                .Include(t => t.Showtime)
                    .ThenInclude(t => t.Movie)
                .Include(t => t.Showtime)
                    .ThenInclude(t => t.Theater)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Ticket == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
