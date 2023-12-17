using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models.ViewModels;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class AccountController : Controller
    {
        //private readonly 
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    return View();
        //}
    }
}
