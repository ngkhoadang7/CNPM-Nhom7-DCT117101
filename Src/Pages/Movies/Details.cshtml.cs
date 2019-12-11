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

namespace MovieTheater.Pages.Movies
{
    [AllowAnonymous]
    public class DetailsModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public DetailsModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }
        

        public Movie Movie { get; set; }

        public string getDate(Movie m){
            return m.ReleaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
