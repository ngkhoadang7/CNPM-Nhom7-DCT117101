using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Theaters
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public IndexModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public PaginatedList<Theater> Theater { get;set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            IQueryable<Theater> theater = from t in _context.Theaters
                                            select t;
            Theater = await PaginatedList<Theater>.CreateAsync(theater.AsNoTracking(), pageIndex ?? 1, 10);
        }
    }
}
