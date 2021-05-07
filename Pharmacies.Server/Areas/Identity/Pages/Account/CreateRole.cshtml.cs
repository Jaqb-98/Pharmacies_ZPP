using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;

namespace Pharmacies.Server.Areas.Identity.Pages.Account.Manage
{
    public class CreateRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJSRuntime JsRuntime;

        [BindProperty]
        public RoleViewModel Input { get; set; }

        public CreateRoleModel(RoleManager<IdentityRole> roleManager, IJSRuntime JSRuntime)
        {
            _roleManager = roleManager;
            JsRuntime = JSRuntime;
        }
        public class RoleViewModel
        {
            [Required]
            public string RoleName { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {

                var role = new IdentityRole
                {
                    Name = Input.RoleName
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Page();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return Page();
        }
        public void OnGet()
        {
        }

    }
}
