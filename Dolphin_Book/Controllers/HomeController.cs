using Dolphin_Book.Data.Contexts;
using Dolphin_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_Book.Controllers
{
    public class HomeController : Controller
    {
        private readonly DolphinDbContext _context;

        public HomeController(DolphinDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM();
            vm.books = await _context.Books.Where(x=>!x.IsDeleted)
                .Include(x=>x.BookCategories)
                 .ThenInclude(x=>x.Category)
                .Include(x=>x.Author)
                .Include(x=>x.Seller)
                .Include(x=>x.Language)              
                    .ToListAsync();
            vm.slides = await _context.Sliders.Where(x=>!x.IsDeleted).ToListAsync();
            return View(vm);
        }
    }
}
