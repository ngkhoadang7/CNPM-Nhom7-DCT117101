using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;
using MovieTheater.Models.TheaterViewModels;

namespace MovieTheater.Pages.Tickets
{
    [Authorize(Roles="User")]
    public class BuyticketModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;
        private readonly UserManager<User> _userManager;

        public BuyticketModel(MovieTheater.Data.MovieTheaterContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string getStartTimeFormatted(Showtime s)
        {
            return s.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public string getStartTimeToValueInput(Showtime s)
        {
            return s.StartTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).Replace(' ', 'T');;
        }

        public bool SeatIsBooked(int seat)
        {
            foreach (Ticket t in Tickets)
            {
                if (Int32.Parse(t.Seat) == seat)
                    return true;
            }
            return false;
        }

        public string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IList<Showtime> Showtime { get; set; }

        [BindProperty]
        public IList<Ticket> Tickets { get; set; }

        public async Task<IActionResult> OnGetAsync(int ShowtimeID)
        {
            if (String.IsNullOrEmpty(ShowtimeID.ToString()))
            {
                return RedirectToPage("../Movies/Index");
            }

            var showtime = from s in _context.Showtimes
                                .Include(s => s.Theater)
                                .Include(s=> s.Movie)
                           where s.ID == ShowtimeID
                           orderby s.StartTime
                           select s;

            var ticket = from t in _context.Tickets
                         where t.ShowtimeID == ShowtimeID
                         select t;

            Showtime = await showtime.ToListAsync();
            Tickets = await ticket.ToListAsync();

            return Page();
        }

        [BindProperty]
        public Ticket Ticket {get;set;}

        public async Task<IActionResult> OnPostAsync( string[] selectedSeats)
        {
            foreach(var i in Tickets){
                foreach(string seat in selectedSeats){
                    if(Tickets.Where(t => t.Seat == seat) != null){
                        selectedSeats = selectedSeats.Where(s => s != seat).ToArray();
                        break;
                    }
                }
            }

            var money = 95000;
            if(Ticket.Showtime.Format == "3D")
            {
                money = 125000;
            }
            else if (Ticket.Showtime.Format == "4DX")
            {
                money = 150000;
            }

            var user = await _userManager.GetUserAsync(User);
            var Code = RandomString();
            foreach(string seat in selectedSeats)
            {
                Ticket emptyTicket = new Ticket(){
                    ShowtimeID = Ticket.ShowtimeID,
                    Seat = seat,
                    UserID = user.Id,
                    CodeToGet = Code,
                    Got = false,
                    Money = money
                };
                _context.Tickets.Add(emptyTicket);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("../Movies/Index");
        }
    }
}
