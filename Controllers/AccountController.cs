using Microsoft.AspNetCore.Authentication;
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
        private readonly UserContext _userContext;
        public AccountController(UserContext context)
        {
            _userContext = context;
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
                User user = await _userContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    _userContext.Users.Add(new User { Email = model.Email, Password = model.Password });
                    await _userContext.SaveChangesAsync();

                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login and(or) password");
                }
            }
            return View(model);
        }

        public async Task Authenticate(string userEmail)
        {
            //create one claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userEmail)
            };

            // create claim Identiti object
            ClaimsIdentity Id = new ClaimsIdentity(claims, "ApplicationCookies"
                ,ClaimsIdentity.DefaultNameClaimType
                ,ClaimsIdentity.DefaultRoleClaimType);

            //setting authenticational cookies
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(Id));
        }
    }
}
