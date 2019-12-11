using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Showtimes
{
    public class EditModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public EditModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Showtime Showtime { get; set; }

        public SelectList MovieID { get; set; }

        public SelectList TheaterID { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? Error)
        {
            if (id == null)
            {
                return NotFound();
            }

            Showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Theater).FirstOrDefaultAsync(m => m.ID == id);

            if (Showtime == null)
            {
                return NotFound();
            }

            MovieID = new SelectList(_context.Movies, "ID", "Title");
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
                if( st.ID != Showtime.ID)
                {
                    if(DateTime.Compare(st.StartTime, Showtime.StartTime) < 0)
                    {
                        if ( DateTime.Compare(st.StartTime.AddMinutes(st.Movie.Duration).AddMinutes(15),Showtime.StartTime) >= 0 ) 
                        {
                            return RedirectToPage("./Edit", new { id = Showtime.ID, Error = 1 });
                        }
                    } 
                    else if(DateTime.Compare(st.StartTime, Showtime.StartTime) == 0)
                    {
                        return RedirectToPage("./Edit", new { id = Showtime.ID, Error = 1 });
                        
                    } 
                    else if(DateTime.Compare(st.StartTime, Showtime.StartTime) > 0) 
                    {
                        if ( DateTime.Compare(Showtime.StartTime.AddMinutes(Showtime.Movie.Duration).AddMinutes(15),st.StartTime) >= 0 )
                        {
                            return RedirectToPage("./Edit", new { id = Showtime.ID, Error = 1 });
                        }
                    }
                }
            }

            var theater = (from t in _context.Theaters
                           where t.ID == Showtime.TheaterID
                           select t).First();

            if( (theater.Format == "4DX" && Showtime.Format != "4DX") || (theater.Format == "2D" && Showtime.Format != "2D") || (theater.Format == "3D" && Showtime.Format == "4DX") )
            {
                return RedirectToPage("./Edit", new { id = Showtime.ID, Error = 2 });
            }

            var EditShowtime = await _context.Showtimes.FirstOrDefaultAsync(m => m.ID == Showtime.ID);

            if (EditShowtime.MovieID != Showtime.MovieID)
            {
                EditShowtime.MovieID = Showtime.MovieID;
            }
            if (EditShowtime.TheaterID != Showtime.TheaterID)
            {
                EditShowtime.TheaterID = Showtime.TheaterID;
            }
            if (EditShowtime.StartTime != Showtime.StartTime)
            {
                EditShowtime.StartTime = Showtime.StartTime;
            }
            if (EditShowtime.Format != Showtime.Format)
            {
                EditShowtime.Format = Showtime.Format;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Manage");
        }

    }
}
