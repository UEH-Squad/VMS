using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public ConfirmEmailModel(UserManager<User> userManager, ILogger<LoginModel> logger, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code, string returnUrl = null)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                IdentityResult confirmEmailResult = await _userManager.ConfirmEmailAsync(user, code);

                if (confirmEmailResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);

                    _logger.LogInformation("User logged in.");

                    return LocalRedirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Email confirmation failed.", ex.Message);
            }
            finally
            {
                StatusMessage = "Không thể xác nhận email.";
            }

            return Page();
        }
    }
}