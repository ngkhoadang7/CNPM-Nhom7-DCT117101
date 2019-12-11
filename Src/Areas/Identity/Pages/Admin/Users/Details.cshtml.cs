using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Areas.Identity.Pages.Admin.Users
{
    [Authorize(Roles="Admin")]
    public class DetailsModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public DetailsModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public User MyUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MyUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (MyUser == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
