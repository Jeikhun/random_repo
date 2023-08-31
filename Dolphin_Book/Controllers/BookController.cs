using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_Book.Controllers
{
    public class BookController : Controller
    {
        private readonly DolphinDbContext _context;

        public BookController(DolphinDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> BookDetails(int id)
        {
            BookVM vm = new BookVM();
            vm.book = await _context.Books.Where(x => !x.IsDeleted && x.Id==id)
                .Include(x => x.BookCategories)
                 .ThenInclude(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Seller)
            .Include(x => x.Language)
                    .FirstOrDefaultAsync();
            if (vm.book == null)
            {
                return NotFound();
            }
            return View(vm);
        }
    }
}
