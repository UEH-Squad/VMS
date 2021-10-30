using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Common;
using VMS.Domain.Models;

namespace VMS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IMailService _mailService;

        public LoginModel(SignInManager<User> signInManager,
            ILogger<LoginModel> logger,
            UserManager<User> userManager,
            IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mailService = mailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền tên đăng nhập")]
            [EmailAddress(ErrorMessage = "Tên đăng nhập không chính xác")]
            public string Email { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Vui lòng điền mật khẩu")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Nhớ tài khoản")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                User user = await _userManager.FindByEmailAsync(Input.Email);
                bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, Input.Password);
                if (isCorrectPassword)
                {
                    if (result.IsNotAllowed)
                    {
                        if (!user.EmailConfirmed)
                        {
                            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            string callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = user.Id, code, returnUrl = await _userManager.IsInRoleAsync(user, "User") ? Routes.EditUserProfile : Routes.EditOrgProfile },
                                protocol: Request.Scheme);

                            await _mailService.SendConfirmEmail(user.Email, callbackUrl);

                            return RedirectToPage("./ForgotPasswordConfirmation", new { userEmail = user.Email });
                        }
                    }
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");

                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}