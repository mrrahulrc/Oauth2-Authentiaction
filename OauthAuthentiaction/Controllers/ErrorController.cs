using Microsoft.AspNetCore.Mvc;

namespace OauthAuthentiaction.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult error()
        {
            return View();
        }
    }
}
