using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Tickets
{
    [Authorize(Roles="User")]
    public class IndexModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;
         private readonly UserManager<User> _userManager;

        public IndexModel(MovieTheater.Data.MovieTheaterContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public string getStartTimeFormatted(Ticket t){
            return t.Showtime.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public PaginatedList<Ticket> Ticket { get;set; }

        //Filter
        public SelectList MovieList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieSearch { get; set; }

        public async Task OnGetAsync(int? pageIndex, string currentMovieSearch)
        {
            if (MovieSearch != null )
            {
                pageIndex = 1;
            }
            else
            {
                MovieSearch = currentMovieSearch;
            }

            var movieList = from m in _context.Movies
                            where m.Showing == true
                            orderby m.Title
                            select m.Title;

            MovieList = new SelectList(movieList.Distinct().ToList());

            var user = await _userManager.GetUserAsync(User);

            IQueryable<Ticket> ticket = from t in _context.Tickets
                                            .Include(t => t.Showtime)
                                                .ThenInclude(t => t.Movie)
                                            .Include(t => t.Showtime)
                                                .ThenInclude(t => t.Theater)
                                        where t.UserID == user.Id && t.Got == false
                                        orderby t.Showtime.StartTime, t.Seat.Length
                                        select t;

            if (!string.IsNullOrEmpty(MovieSearch))
            {
                ticket = ticket.Where(x => x.Showtime.Movie.Title == MovieSearch);
            }

            Ticket = await PaginatedList<Ticket>.CreateAsync(ticket.AsNoTracking(), pageIndex ?? 1, 10);
        }
    }
}
