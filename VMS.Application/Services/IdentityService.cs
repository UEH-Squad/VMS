using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly IHttpContextAccessor _httpContext;

        public IdentityService(UserManager<IdentityUser> userManager,
                           IHttpContextAccessor httpContext)
        {
            _usermanager = userManager;
            _httpContext = httpContext;
        }

        /*
         * As UserManager requires a scoped DbContext, there're cases that two or more threads trying to access one DbContext at a time.
         * To avoid this, we have to queue a Task related to UserManager to run on the thread pool and then return the result using Task.Run(() => ...).Result;
         */

        public IdentityUser FindUserById(string userId)
        {
            return Task.Run(() => _usermanager.FindByIdAsync(userId)).Result;
        }

        public IdentityUser GetCurrentUser()
        {
            return Task.Run(() => _usermanager.GetUserAsync(_httpContext.HttpContext.User)).Result;
        }
    }
}