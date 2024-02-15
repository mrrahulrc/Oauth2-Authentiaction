using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace OauthAuthentiaction.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {            
            ViewBag.email = HttpContext.Items["email"];
            return View();
        }
    }
}
