using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using VMS.Domain.Models;

namespace VMS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {

            [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng nhập mật khẩu mới")]
            [StringLength(30, MinimumLength = 4, ErrorMessage = "Mật khẩu phải có ít nhất 4 ký tự")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng xác nhận mật khẩu")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không chính xác")]
            public string ConfirmPassword { get; set; }
            
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public string Code { get; set; }
        }

        public IActionResult OnGet(string code, string email)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = code,
                    Email = email
                };
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _userManager.FindByNameAsync(Input.Email);
            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Input.Code)), Input.Password);
            if (result.Succeeded)
            {
                return Page();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
            }
    }
}
