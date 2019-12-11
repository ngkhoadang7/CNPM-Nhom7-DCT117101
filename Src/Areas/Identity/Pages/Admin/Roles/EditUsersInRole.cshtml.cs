using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MovieTheater.Models.TheaterViewModels;
using MovieTheater.Data;
using MovieTheater.Models;

namespace MovieTheater.Areas.Identity.Pages.Admin.Roles
{
    [Authorize(Roles="Admin")]
    public class EditUsersInRole : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public EditUsersInRole(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public string roleIdMoveBack { get; set;}
        
        [BindProperty]
        public List<UserRoleViewModel> model { get; set;}

        public async Task<IActionResult> OnGetAsync(string roleId)
        {
            if (roleId == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }
            roleIdMoveBack = role.Id;

            model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel();
                userRoleViewModel.UserId = user.Id;
                userRoleViewModel.UserName = user.UserName;

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else 
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync( string roleId)
        {
            if (roleId == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToPage("./Edit", new { Id = roleId });
                }
            }

            return RedirectToPage("./Edit", new { Id = roleId });
        }
    }
}