using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;


namespace MovieTheater.Areas.Identity.Pages.Admin.Roles
{
    [Authorize(Roles="Admin")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Role Name")]
            public string RoleName { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityrole = new IdentityRole 
                {
                    Name = Input.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityrole);

                if(result.Succeeded)
                {
                    return RedirectToPage("./Index");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }
            return Page();

        }
    }
}