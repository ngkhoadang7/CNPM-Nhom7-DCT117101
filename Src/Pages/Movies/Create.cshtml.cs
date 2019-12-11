using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Pages.Movies
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        private readonly MovieTheater.Data.MovieTheaterContext _context;
        private IWebHostEnvironment _environment;


        public CreateModel(MovieTheater.Data.MovieTheaterContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        //[BindProperty]
        // public IFormFile poster { get; set; }
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile poster)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var path = Path.Combine( _environment.WebRootPath, "Picture", poster.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await poster.CopyToAsync(stream);
            Movie.Poster = poster.FileName;

            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Manage");
        }
    }
}
