using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VMS.Domain.Models;

namespace VMS.Areas.Identity.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ChangePasswordModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]
            [DataType(DataType.Password)]
            public string CurrentPassword { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng nhập mật khẩu mới")]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không chính xác.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("./Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, Input.CurrentPassword, Input.NewPassword);
                if(result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return RedirectToPage("./ChangePasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu hiện tại không chính xác");
                    return Page();
                }
            }
            return Page();
        }
    }
}
