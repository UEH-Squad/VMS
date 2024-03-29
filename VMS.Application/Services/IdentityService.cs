﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
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

        public bool IsLoggedIn()
        {
            return _httpContext.HttpContext?.User.Identity != null && _httpContext.HttpContext.User.Identity.IsAuthenticated;
        }

        public string GetCurrentUserId()
        {
            return GetCurrentUser()?.Id;
        }

        public User GetCurrentUserWithAddresses()
        {
            string currentUserId = GetCurrentUserId();
            return Task.Run(() => _userManager.Users.Include(x => x.UserAddresses)
                                                    .ThenInclude(x => x.AddressPath)
                                                    .SingleOrDefaultAsync(x => x.Id == currentUserId)).Result;
        }

        public string GetCurrentUserAddress()
        {
            User user = Task.Run(() => _userManager.Users.Include(u => u.UserAddresses)
                                                         .ThenInclude(x => x.AddressPath)
                                                         .SingleOrDefaultAsync(u => u.Id == GetCurrentUserId())).Result;

            if (user is not null)
            {
                List<AddressPath> addressPaths = user.UserAddresses.OrderByDescending(u => u.AddressPath.Depth)
                                                                    .Select(a => a.AddressPath)
                                                                    .ToList();
                if (addressPaths.Count == 3)
                {
                    return $"{user.Address}, {addressPaths[0].Name}, {addressPaths[1].Name}, {addressPaths[2].Name}";
                }
            }

            return string.Empty;
        }

        public User GetUserWithFavoritesAndRecruitmentsById(string userId)
        {
            return Task.Run(() => _userManager.Users.Include(x => x.Favorites)
                                                    .Include(x => x.Recruitments).ThenInclude(x => x.Activity)
                                                    .SingleOrDefaultAsync(x => x.Id == userId)).Result;
        }

        public void UpdateUser(User user)
        {
            Task.Run(() => _userManager.UpdateAsync(user));
        }

        public bool IsCorrectCurrentUserPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            else
            {
                User currentUser = GetCurrentUser();
                return Task.Run(() => _userManager.CheckPasswordAsync(currentUser, password)).Result;
            }
        }

        public bool IsInRole(string userId, Role role)
        {
            return Task.Run(() => _userManager.IsInRoleAsync(FindUserById(userId), role.ToString())).Result;
        }
    }
}