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

namespace MovieTheater.Pages.Showtimes
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public IndexModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public string getStartTimeFormatted(Showtime s){
            return s.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public string getNoSeatLeft(Showtime s)
        {
            return (s.Theater.NoSeat - s.NoSeatBooked).ToString();
        }


        public PaginatedList<Showtime> Showtime { get;set; }

        //Filter
        public SelectList TimeList { get; set; }
        public SelectList MovieList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StartTimeSearch { get; set; }

        public async Task OnGetAsync(int? pageIndex, string currentMovieSearch, string currentStartTimeSearch, string movieSearch)
        {
            if(!string.IsNullOrEmpty(movieSearch))
            {
                MovieSearch = movieSearch;
            }
            if (MovieSearch != null || StartTimeSearch != null )
            {
                pageIndex = 1;
            }
            else
            {
                MovieSearch = currentMovieSearch;
                StartTimeSearch = currentStartTimeSearch;
            }
            

            var movieList = from m in _context.Movies
                            where m.Showing == true
                            orderby m.Title
                            select m.Title;

            var timeList = from s in _context.Showtimes
                            where s.StartTime >= DateTime.Now
                            orderby s.StartTime ascending 
                            select s.StartTime.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            TimeList = new SelectList(timeList.Distinct().ToList());
            MovieList = new SelectList(movieList.Distinct().ToList());

            IQueryable<Showtime> showtimes = from s in _context.Showtimes
                                                .Include(s => s.Movie)
                                                .Include(s => s.Theater)
                                            where s.StartTime >= DateTime.Now
                                            orderby s.StartTime 
                                            select s;

            if (!string.IsNullOrEmpty(StartTimeSearch))
            {   
                var myDate = DateTime.ParseExact(StartTimeSearch, "dd/MM/yyyy",CultureInfo.InvariantCulture);
                showtimes = showtimes.Where(x => x.StartTime.Date == myDate);
            }
            if (!string.IsNullOrEmpty(MovieSearch))
            {
                showtimes = showtimes.Where(x => x.Movie.Title == MovieSearch);
            }

            Showtime = await PaginatedList<Showtime>.CreateAsync(showtimes.AsNoTracking(), pageIndex ?? 1, 10);
        }
    }
}
