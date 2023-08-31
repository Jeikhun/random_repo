using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Extentions;
using Dolphin_Book.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dolphin_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AuthorController : Controller
    {
        private readonly DolphinDbContext _context;
		private readonly IWebHostEnvironment _env;

		public AuthorController(DolphinDbContext context, IWebHostEnvironment env)
        {
            _context = context;
			_env = env;
		}

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Author> authors = await _context.Authors
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(authors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
			if (author.FormFile == null)
			{
				ModelState.AddModelError("FormFile", "Wrong!");
				return View();
			}
            if (!Helper.isImage(author.FormFile))
            {
				ModelState.AddModelError("FormFile", "Is not Image!");
				return View();
			}
            if (!Helper.isSizeOk(author.FormFile, 1))
            {
				ModelState.AddModelError("FormFile", "wrong Size");
				return View();
			}

			author.Image = author.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");

			author.CreatedAt = DateTime.Now;
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			Author? author = await _context.Authors.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (author == null)
			{
				return NotFound();
			}
			return View(author);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, Author newAuthor)
		{
			Author? author = await _context.Authors.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

			if (author == null)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				return View(author);
			}


			if (newAuthor.FormFile != null)
			{
				if (!Helper.isImage(newAuthor.FormFile))
				{
					ModelState.AddModelError("FormFile", "Is not Image!");
					return View();
				}
				if (!Helper.isSizeOk(newAuthor.FormFile, 1))
				{
					ModelState.AddModelError("FormFile", "Wrong Size!");
					return View();
				}
				if (author.Image == null)
				{
					ModelState.AddModelError("Image", "Image is null!");
					return View();
				}
				Helper.RemoveImage(_env.WebRootPath, "admin/img/images", author.Image);
				author.Image = newAuthor.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");
			}
			author.FullName = newAuthor.FullName;
			author.Age= newAuthor.Age;
			author.UpdatedAt = DateTime.Now;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));

		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			Author? author = await _context.Authors.Where(x => !x.IsDeleted).FirstOrDefaultAsync();
			if (author == null)
			{
				return NotFound();
			}

			author.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
