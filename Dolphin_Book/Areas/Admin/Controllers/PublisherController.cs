using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_Book.Areas.Admin.Controllers
{

	[Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class PublisherController : Controller
	{
		private readonly DolphinDbContext _context;

		public PublisherController(DolphinDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<Publisher> publishers = await _context.Publishers
				.Where(x => !x.IsDeleted).ToListAsync();
			return View(publishers);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Publisher publisher)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}


			publisher.CreatedAt = DateTime.Now;
			await _context.Publishers.AddAsync(publisher);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			Publisher? publisher = await _context.Publishers.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (publisher == null)
			{
				return NotFound();
			}
			return View(publisher);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, Publisher newPublisher)
		{
			Publisher? publisher = await _context.Publishers.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
			if (!ModelState.IsValid)
			{
				return View(publisher);
			}

			if (publisher == null)
			{
				return NotFound();
			}

			publisher.Name = newPublisher.Name;
			publisher.UpdatedAt = DateTime.Now;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));

		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			Publisher? publisher = await _context.Publishers.Where(x => !x.IsDeleted).FirstOrDefaultAsync();
			if (publisher == null)
			{
				return NotFound();
			}

			publisher.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}