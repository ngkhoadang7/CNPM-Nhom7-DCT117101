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
using MovieTheater.Pages;

namespace MovieTheater.Areas.Identity.Pages.Admin.Users
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public IndexModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public PaginatedList<User> MyUser { get;set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            IQueryable<User> user = from t in _context.Users
                                            select t;  
            MyUser = await PaginatedList<User>.CreateAsync(user.AsNoTracking(), pageIndex ?? 1, 10);
        }
    }
}
