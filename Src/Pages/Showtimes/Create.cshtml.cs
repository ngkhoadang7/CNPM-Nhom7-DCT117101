using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MovieTheater.Data;
using MovieTheater.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieTheater.Pages.Showtimes
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public CreateModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Showtime Showtime { get; set; }

        public SelectList MovieID { get; set; }

        public SelectList TheaterID { get; set; }

        public IActionResult OnGet(int? Error)
        {   
            var movie = from m in _context.Movies
                        where m.Showing == true
                        select m;

            MovieID = new SelectList(movie, "ID", "Title");
            TheaterID = new SelectList(_context.Theaters, "ID", "Name");

            if(Error == 1)
            {
                ModelState.AddModelError(string.Empty, "Giờ của suất chiếu không hợp lệ");
            }
            else if (Error == 2){
                ModelState.AddModelError(string.Empty, "Định dạng của suất chiếu không hợp lệ");
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var showtimes = (from s in _context.Showtimes
                                .Include(s => s.Movie)
                                .Include(s => s.Theater)
                            where s.StartTime.Date == Showtime.StartTime.Date &&
                                s.TheaterID == Showtime.TheaterID
                            select s).ToList();

            Showtime.Movie = (from m in _context.Movies
                                where m.ID == Showtime.MovieID
                              select m).First();

            foreach(Showtime st in showtimes)
            {
                if(DateTime.Compare(st.StartTime, Showtime.StartTime) < 0)
                {
                    if ( DateTime.Compare(st.StartTime.AddMinutes(st.Movie.Duration).AddMinutes(15),Showtime.StartTime) >= 0 ) 
                    {
                        return RedirectToPage("./Create", new { Error = 1 });
                    }
                } 
                else if(DateTime.Compare(st.StartTime, Showtime.StartTime) == 0)
                {
                    return RedirectToPage("./Create", new { Error = 1 });
                    
                } 
                else if(DateTime.Compare(st.StartTime, Showtime.StartTime) > 0) 
                {
                    if ( DateTime.Compare(Showtime.StartTime.AddMinutes(Showtime.Movie.Duration).AddMinutes(15),st.StartTime) >= 0 )
                    {
                        return RedirectToPage("./Create", new { Error = 1 });
                    }
                }
            }

            var theater = (from t in _context.Theaters
                           where t.ID == Showtime.TheaterID
                           select t).First();

            if( (theater.Format == "4DX" && Showtime.Format != "4DX") || (theater.Format == "2D" && Showtime.Format != "2D") || (theater.Format == "3D" && Showtime.Format == "4DX") )
            {
                return RedirectToPage("./Create", new { Error = 2 });
            }

            _context.Showtimes.Add(Showtime);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Manage");
        }
    }
}
