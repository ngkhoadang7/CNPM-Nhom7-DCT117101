using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Tickets
{
    [Authorize(Roles = "Admin")]
    public class ManageModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public ManageModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public string getStartTimeFormatted(Ticket t)
        {
            return t.Showtime.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public PaginatedList<Ticket> Ticket { get;set; }

        //Filter
        public SelectList MovieList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string UserSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StaffSearch { get; set; }

        public async Task OnGetAsync(int? pageIndex, string currentMovieSearch, string currentUserSearch, string currentStaffSearch )
        {
            if (MovieSearch != null || StaffSearch != null || UserSearch != null )
            {
                pageIndex = 1;
            }
            else
            {
                MovieSearch = currentMovieSearch;
                UserSearch = currentUserSearch;
                StaffSearch = currentStaffSearch;
            }

            var movieList = from m in _context.Movies
                            orderby m.Title
                            select m.Title;

            MovieList = new SelectList(movieList.Distinct().ToList());


            IQueryable<Ticket> ticket = from t in _context.Tickets
                                            .Include(t => t.Showtime)
                                                .ThenInclude(t => t.Movie)
                                            .Include(t => t.Showtime)
                                                .ThenInclude(t => t.Theater)
                                        orderby t.Showtime.StartTime, t.Seat.Length
                                        select t;

            if (!string.IsNullOrEmpty(MovieSearch))
            {
                ticket = ticket.Where(x => x.Showtime.Movie.Title == MovieSearch);
            }
            if (!string.IsNullOrEmpty(UserSearch))
            {
                ticket = ticket.Where(x => x.UserID.Contains(UserSearch));
            }
            if (!string.IsNullOrEmpty(StaffSearch))
            {
                ticket = ticket.Where(x => x.UserID.Contains(StaffSearch));
            }
        
            Ticket = await PaginatedList<Ticket>.CreateAsync(ticket.AsNoTracking(), pageIndex ?? 1, 10);
        }
    }
}

