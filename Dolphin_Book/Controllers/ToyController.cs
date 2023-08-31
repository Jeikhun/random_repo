using Microsoft.AspNetCore.Mvc;

namespace Dolphin_Book.Controllers
{
    public class ToyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ToyDetails()
        {
            return View();
        }
    }
}
