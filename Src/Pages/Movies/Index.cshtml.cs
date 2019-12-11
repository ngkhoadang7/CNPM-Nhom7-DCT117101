using System;
using System.Globalization;
using System.Collections.Generic;
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
    public class IndexModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public IndexModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        public string getDate(Movie m){
            return m.ReleaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }


        public async Task OnGetAsync()
        {
            var movie = from m in _context.Movies
                         where m.Showing == true
                         select m;
            Movie = await movie.ToListAsync();
        }
    }
}
