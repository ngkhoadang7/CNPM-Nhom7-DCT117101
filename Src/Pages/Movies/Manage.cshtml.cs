using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;


namespace MovieTheater.Pages.Movies
{
    [Authorize(Roles="Admin")]
    public class ManageModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public ManageModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public PaginatedList<Movie> Movie { get;set; }

        //Filter
        [BindProperty(SupportsGet = true)]
        public string MovieSearch { get; set; }

        public string getDate(Movie m){
            return m.ReleaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            if (MovieSearch != null)
            {
                pageIndex = 1;
            }
            IQueryable<Movie> movie = from m in _context.Movies
                                        select m;

            if (!String.IsNullOrEmpty(MovieSearch))
            {
                movie = movie.Where(m => m.Title.ToLower().Contains(MovieSearch.ToLower()));
            }

            Movie = await PaginatedList<Movie>.CreateAsync(
                movie.AsNoTracking(), pageIndex ?? 1, 10);
        }
    }
}
