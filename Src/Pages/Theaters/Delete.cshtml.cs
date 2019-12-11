using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Theaters
{
    [Authorize(Roles="Admin")]
    public class DeleteModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public DeleteModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Theater = await _context.Theaters.FindAsync(id);

            if (Theater != null)
            {
                _context.Theaters.Remove(Theater);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
