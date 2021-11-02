using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VMS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        [TempData]
        public string UserEmail { get; set; }

        public void OnGet(string userEmail)
        {
            this.UserEmail = userEmail;
        }
    }
}