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

namespace MovieTheater.Pages.Theaters
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;

        public CreateModel(MovieTheater.Data.MovieTheaterContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Theater Theater { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Theaters.Add(Theater);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
