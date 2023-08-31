using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dolphin_Book.Controllers
{
    public class OrderController : Controller
    {
        [Authorize(Roles = "User, SuperAdmin, Admin")]
        public IActionResult Basket()
        {
            return View();
        }
        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}
