using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class DetailsModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public DetailsModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public Showtime Showtime { get; set; }

        public string getStartTimeFormatted(Showtime s){
            return s.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public string getNoSeatLeft(Showtime s)
        {
            return (s.Theater.NoSeat - s.NoSeatBooked).ToString();
        }

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
    }
}
