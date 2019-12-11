using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Models.TheaterViewModels;
using MovieTheater.Models;

namespace MovieTheater.Pages.Staff
{
    [Authorize(Roles="Staff")]
    public class ListOfTicketsModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;
        private readonly UserManager<User> _userManager;

        public ListOfTicketsModel(MovieTheater.Data.MovieTheaterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public PaginatedList<Ticket>  Ticket { get;set; }

        [BindProperty]
        public List<GetTicketsViewModel> model { get; set; }


        public string getStartTimeFormatted(Ticket t)
        {
            return t.Showtime.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public async Task OnGetAsync(int? pageIndex, string currentSearchString)
        {
            if (SearchString != null )
            {
                pageIndex = 1;
            }
            else
            {
                SearchString = currentSearchString;
            }

            IQueryable<Ticket> ticket = from t in _context.Tickets
                                            .Include(t => t.Showtime)
                                                .ThenInclude(t => t.Movie)
                                            .Include(t => t.Showtime)
                                                .ThenInclude(t => t.Theater)
                                        where t.Got == false
                                        orderby t.Showtime.StartTime , t.Seat.Length
                                        select t;

            if (!string.IsNullOrEmpty(SearchString))
            {
                ticket = ticket.Where(t => t.CodeToGet.Equals(SearchString));
            }
            model = new List<GetTicketsViewModel>();
            foreach (var tic in ticket)
            {
                var temp = new GetTicketsViewModel();
                temp.TicketID = tic.ID;
                temp.IsSelected = false;
                model.Add(temp);
            }
            Ticket = await PaginatedList<Ticket>.CreateAsync(ticket.AsNoTracking(), pageIndex ?? 1, 10);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var staff = await _userManager.GetUserAsync(User);
            for (int i = 0; i < model.Count; i++)
            {
                var ticket = await _context.Tickets.SingleOrDefaultAsync(t => t.ID == model[i].TicketID);

                if(ticket == null)
                {
                    return NotFound();
                }

                if (model[i].IsSelected)
                {
                    ticket.Got = true;
                    ticket.StaffID = staff.Id;
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("./ListOfTickets");
        }
    }
}
