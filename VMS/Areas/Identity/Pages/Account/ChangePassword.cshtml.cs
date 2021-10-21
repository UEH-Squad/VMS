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
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu hiện tại")]
            public string CurrentPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu mới")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu mới")]
            [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không chính xác.")]
            public string ConfirmPassword { get; set; }
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Hãy đăng nhập trước khi đổi mật khẩu!");
                return Page();
            }
            if (ModelState.IsValid)
            {
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
