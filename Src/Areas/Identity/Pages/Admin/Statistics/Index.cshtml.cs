using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace MovieTheater.Areas.Identity.Pages.Admin.Statistics
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public IndexModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public class QueryModel
        {
            public string Title { get; set; }
            public string Format { get; set; }
            public int TotalTickets { get; set; }
            public float TotalMoney { get; set; }
        }

        public List<QueryModel> Statistic2D {get; set;}
        public List<QueryModel> Statistic3D {get; set;}
        public List<QueryModel> Statistic4DX {get; set;}

        //Filter
        public SelectList MonthList { get; set; }
        public SelectList YearList { get; set; }
        public SelectList MovieList { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Movie { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Month { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Year { get; set; }

        public string getTotalTickets(List<QueryModel> List)
        {  
            int Total = 0;
            foreach (var item in List)
            {
                Total += item.TotalTickets;
            }
            return Total.ToString();
        }

        public string getTotalMoney(List<QueryModel> List)
        {  
            float Total = 0;
            foreach (var item in List)
            {
                Total += item.TotalMoney;
            }
            return Total.ToString();
        }

        public IActionResult OnGet()
        {
            var month = from m in _context.Showtimes
                            orderby m.StartTime.Month
                            select m.StartTime.Month;

            var year = from m in _context.Showtimes
                            orderby m.StartTime.Year
                            select m.StartTime.Year;

            var movie = from m in _context.Movies
                            orderby m.Title
                            select m.Title;
            
            MonthList = new SelectList(month.Distinct().ToList());
            YearList = new SelectList(year.Distinct().ToList());
            MovieList = new SelectList(movie.Distinct().ToList());

            Statistic2D = new List<QueryModel>();
            Statistic3D = new List<QueryModel>();
            Statistic4DX = new List<QueryModel>();

            var query = from s in _context.Showtimes
                            join m in _context.Movies on s.MovieID equals m.ID
                            join t in _context.Tickets on s.ID equals t.ShowtimeID
                            group s by new 
                            { 
                                m.Title,
                                s.Format,
                                TotalTickets = (from t in _context.Tickets
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Movie)
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Theater)
                                                where t.Showtime.Format == s.Format &&
                                                    t.Showtime.Movie.Title == m.Title
                                                select t).Count(),
                                TotalMoney = (from t in _context.Tickets
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Movie)
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Theater)
                                                where t.Showtime.Format == s.Format &&
                                                    t.Showtime.Movie.Title == m.Title
                                                select t).Sum(t => t.Money),
                            }
                            into g
                            select new QueryModel()
                            {
                                Title = g.Key.Title,
                                Format = g.Key.Format,
                                TotalTickets= g.Key.TotalTickets,
                                TotalMoney = g.Key.TotalMoney
                            };
            if( !string.IsNullOrEmpty(Month) && !string.IsNullOrEmpty(Year))
            {
                query = from s in _context.Showtimes
                        join m in _context.Movies on s.MovieID equals m.ID
                        join t in _context.Tickets on s.ID equals t.ShowtimeID
                        where s.StartTime.Year == Int32.Parse(this.Year) && s.StartTime.Month == Int32.Parse(this.Month) 
                        group s by new 
                        { 
                            m.Title,
                            s.Format,
                            TotalTickets = (from t in _context.Tickets
                                                .Include(t => t.Showtime)
                                                    .ThenInclude(t => t.Movie)
                                                .Include(t => t.Showtime)
                                                    .ThenInclude(t => t.Theater)
                                            where t.Showtime.Format == s.Format &&
                                                t.Showtime.Movie.Title == m.Title
                                            select t).Count(),
                            TotalMoney = (from t in _context.Tickets
                                                .Include(t => t.Showtime)
                                                    .ThenInclude(t => t.Movie)
                                                .Include(t => t.Showtime)
                                                    .ThenInclude(t => t.Theater)
                                            where t.Showtime.Format == s.Format &&
                                                t.Showtime.Movie.Title == m.Title
                                            select t).Sum(t => t.Money),
                        }
                        into g
                        select new QueryModel()
                        {
                            Title = g.Key.Title,
                            Format = g.Key.Format,
                            TotalTickets= g.Key.TotalTickets,
                            TotalMoney = g.Key.TotalMoney
                        };
            }
            else if (string.IsNullOrEmpty(Month) && !string.IsNullOrEmpty(Year))
            {
                query = from s in _context.Showtimes
                            join m in _context.Movies on s.MovieID equals m.ID
                            join t in _context.Tickets on s.ID equals t.ShowtimeID
                            where s.StartTime.Year == Int32.Parse(this.Year)
                            group s by new 
                            { 
                                m.Title,
                                s.Format,
                                TotalTickets = (from t in _context.Tickets
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Movie)
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Theater)
                                                where t.Showtime.Format == s.Format &&
                                                    t.Showtime.Movie.Title == m.Title
                                                select t).Count(),
                                TotalMoney = (from t in _context.Tickets
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Movie)
                                                    .Include(t => t.Showtime)
                                                        .ThenInclude(t => t.Theater)
                                                where t.Showtime.Format == s.Format &&
                                                    t.Showtime.Movie.Title == m.Title
                                                select t).Sum(t => t.Money),
                            }
                            into g
                            select new QueryModel()
                            {
                                Title = g.Key.Title,
                                Format = g.Key.Format,
                                TotalTickets= g.Key.TotalTickets,
                                TotalMoney = g.Key.TotalMoney
                            };
            } 
            else if ( !string.IsNullOrEmpty(Month) && string.IsNullOrEmpty(Year))
            {
                return Page();
            }
            if (!string.IsNullOrEmpty(Movie))
            {
                query = query.Where(x => x.Title == Movie);
            }

            foreach (var item in query)
            {
                if( item.Format == "2D")
                    Statistic2D.Add(item);
                else if( item.Format == "3D")
                    Statistic3D.Add(item);
                else if( item.Format =="4DX")
                    Statistic4DX.Add(item);
            }
            return Page();
        }
    }
}