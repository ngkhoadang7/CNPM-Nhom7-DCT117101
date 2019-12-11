using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Movies
{
    [Authorize(Roles="Admin")]
    public class EditModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;
        private IWebHostEnvironment _environment;

        public EditModel(MovieTheater.Data.MovieTheaterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile poster)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (poster != null)
            {
                var path = Path.Combine( _environment.WebRootPath, "Picture", poster.FileName);
                var stream = new FileStream(path, FileMode.Create);
                await poster.CopyToAsync(stream);
                Movie.Poster = poster.FileName;
            }

            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Manage");
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.ID == id);
        }
    }
}
