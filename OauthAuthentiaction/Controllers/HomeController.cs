using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace OauthAuthentiaction.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
