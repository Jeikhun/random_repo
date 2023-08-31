using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dolphin_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class LanguageController : Controller
    {
        private readonly DolphinDbContext _context;

        public LanguageController(DolphinDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Language> languges = await _context.Languages
                .Where(x=>!x.IsDeleted).ToListAsync();
            return View(languges);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Language language)
        {
            if(language == null || !ModelState.IsValid)
            {
                return View();
            }
            language.CreatedAt = DateTime.Now;
            await _context.Languages.AddAsync(language);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			Language? language = await _context.Languages.FirstOrDefaultAsync(x => x.Id == id);
			if (language == null)
			{
				return NotFound();
			}
			return View(language);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(Language newlanguage, int id)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			Language? language = await _context.Languages.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (language == null)
			{
				return NotFound();
			}
			language.Name = newlanguage.Name;
			language.UpdatedAt = DateTime.Now;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			Language? language = await _context.Languages.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (language == null)
			{
				return NotFound();
			}

			language.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

	}
}
