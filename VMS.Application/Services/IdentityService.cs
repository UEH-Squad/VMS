using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Models;

namespace VMS.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public IdentityService(UserManager<User> userManager,
                           IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _httpContext = httpContext;
        }

        /*
         * As UserManager requires a scoped DbContext, there're cases that two or more threads trying to access one DbContext at a time.
         * To avoid this, we have to queue a Task related to UserManager to run on the thread pool and then return the result using Task.Run(() => ...).Result;
         */

        public User FindUserById(string userId)
        {
            return Task.Run(() => _userManager.FindByIdAsync(userId)).Result;
        }

        public User GetCurrentUser()
        {
            return Task.Run(() => _userManager.GetUserAsync(_httpContext.HttpContext?.User)).Result;
        }
    }
}