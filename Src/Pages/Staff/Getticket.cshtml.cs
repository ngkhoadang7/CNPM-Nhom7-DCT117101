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
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Staff
{
    [Authorize(Roles="Staff")]
    public class GetticketModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;
        private readonly UserManager<User> _userManager;

        public GetticketModel(MovieTheater.Data.MovieTheaterContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Ticket Ticket { get; set; }

        public string getStartTimeFormatted(Ticket t){
            return t.Showtime.StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket = await _context.Tickets
                .Include(t => t.Showtime)
                .ThenInclude(t => t.Movie)
                .Include(t => t.Showtime)
                .ThenInclude(t => t.Theater)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (Ticket == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FirstOrDefaultAsync(m => m.ID == id);
            
            if(ticket == null)
            {
                return NotFound();
            }

            var staff = await _userManager.GetUserAsync(User);

            ticket.Got = true;
            ticket.StaffID = staff.Id;

            await _context.SaveChangesAsync();            
            
            return RedirectToPage("./ListOfTickets");
        }

    }
}
