
﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;
using TaskAuthenticationAuthorization.Models.ViewModels;


namespace TaskAuthenticationAuthorization.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserContext db;
        private readonly ShoppingContext context;
        public AccountController(UserContext db, ShoppingContext context)
        {
            this.db = db;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModels model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    Role userRole = await db.Role.FirstOrDefaultAsync(r => r.Id == user.RoleId);
                    user.Role = userRole;

                    await Authentication(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect login or password");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModels model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .FirstOrDefaultAsync(u => u.Email.Equals(model.Email));

                if (user == null)
                {
                    user = new User { Email = model.Email, Password = model.Password, BuyerType = Models.User.buyerType.regular };
                    Role userRole = await db.Role.FirstOrDefaultAsync(r => r.Name == "buyer");

                    if (userRole != null)
                    {
                        user.Role = userRole;
                        user.RoleId = userRole.Id;
                    }

                    db.Users.Add(user);

                    await db.SaveChangesAsync();

                    await Authentication(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect login or password");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        private async Task Authentication(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("UserId", user.Id.ToString()),
            };

            if (user.Role?.Name == "buyer")
            {
                claims.Add(new Claim("BuyerType", user.BuyerType.ToString()));
            }

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize(Policy = "OnlyVIP")]
        public IActionResult MyDiscount()
        {
            Customer customer = context.Customers.FirstOrDefault(c => c.FirstName == User.Identity.Name);
            return View(customer);
        }
    }
}
