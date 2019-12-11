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

namespace MovieTheater.Pages.Theaters
{
    [Authorize(Roles="Admin")]
    public class DetailsModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public DetailsModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public Theater Theater { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Theater = await _context.Theaters.FirstOrDefaultAsync(m => m.ID == id);

            if (Theater == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
