using Microsoft.AspNetCore.Mvc;

namespace WebApplication6.Properties
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
