﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;
using TaskAuthenticationAuthorization.Models.ViewModels;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserContext _userDbContext;
        public AccountController(UserContext context)
        {
            _userDbContext = context;
        }

        [HttpGet]        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    user.Role = await _userDbContext.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
                    await Authenticate(user); // authentication

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userDbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {                  
                    Role userRole = await _userDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "buyer");
                    if (userRole != null)
                        user.Role = userRole;

                    // adding user to DB
                    _userDbContext.Users.Add(user);
                    await _userDbContext.SaveChangesAsync();

                    await Authenticate(user); // authentication

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(model);
        }

        public async Task Authenticate(User user)
        {
            //create one claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            // create claim Identiti object
            ClaimsIdentity Id = new ClaimsIdentity(claims, "ApplicationCookies"
                , ClaimsIdentity.DefaultNameClaimType
                , ClaimsIdentity.DefaultRoleClaimType);

            //setting authenticational cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(Id));
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
