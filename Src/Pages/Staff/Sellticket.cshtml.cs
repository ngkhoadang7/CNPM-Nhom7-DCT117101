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

namespace MovieTheater.Pages.Staff
{
    [Authorize(Roles="Staff")]
    public class SellticketModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;
        private readonly UserManager<User> _userManager;

        public SellticketModel(MovieTheater.Data.MovieTheaterContext context,UserManager<User> userManager)
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
        public IList<Showtime> Showtime { get; set; }

        [BindProperty]
        public IList<Ticket> Tickets { get; set; }
        
        [BindProperty]
        public Ticket Ticket {get;set;}

        [BindProperty]
        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int ShowtimeID, bool Error)
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
            if(Error)
            {
                ViewData["Error"] = "UserID is not valid";
            }
            else 
            {
                ViewData["Error"] = "";
            }
            return Page();
        }

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

            if(!String.IsNullOrEmpty(UserId))
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if(user == null)
                {
                    return RedirectToPage("./Buyticket", new { ShowtimeID = Ticket.ShowtimeID, Error = true });
                }
                else 
                {
                    UserId = user.Id;
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
            
            var staff = await _userManager.GetUserAsync(User);

            foreach(string seat in selectedSeats)
            {
                Ticket TempTicket = new Ticket(){
                    ShowtimeID = Ticket.ShowtimeID,
                    Seat = seat,
                    UserID = UserId,
                    StaffID = staff.Id,
                    Got = true,
                    Money = money
                };
                _context.Tickets.Add(TempTicket);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("../Index");
        }
    }
}
