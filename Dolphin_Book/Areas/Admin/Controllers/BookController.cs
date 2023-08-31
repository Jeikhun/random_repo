using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Extentions;
using Dolphin_Book.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Data;

namespace Dolphin_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class BookController : Controller
    {
        private readonly DolphinDbContext _context;
		private readonly IWebHostEnvironment _env;

		public BookController(DolphinDbContext dbContext, IWebHostEnvironment env)
        {
            _context = dbContext;
			_env = env;
		}


        [HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<Book> books = await _context.Books.
				Include(x => x.BookCategories).
				   ThenInclude(x => x.Category).
				Include(x => x.Author).
				Include(x => x.Seller).
				Include(x => x.Language).
				Where(c => !c.IsDeleted).ToListAsync();
			return View(books);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.Categories = await _context.Categories.
				Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Authors = await _context.Authors
				.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Sellers = await _context.Sellers.
				Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Languages = await _context.Languages.
				Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Publishers = await _context.Publishers
				.Where(x => !x.IsDeleted).ToListAsync();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Book book)
		{
			int i = 0;
			ViewBag.Categories = await _context.Categories.
				Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Authors = await _context.Authors
				.Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Sellers = await _context.Sellers.
				Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Languages = await _context.Languages.
				Where(p => !p.IsDeleted).ToListAsync();
			ViewBag.Publishers = await _context.Publishers.
				Where(p => !p.IsDeleted).ToListAsync();
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (book.FormFile == null)
			{
				ModelState.AddModelError("FormFile", "Wrong!");
				return View();
			}
			if (!Helper.isImage(book.FormFile))
			{
				ModelState.AddModelError("FormFile", "Is not Image!");
				return View();
			}
			if (!Helper.isSizeOk(book.FormFile, 1))
			{
				ModelState.AddModelError("FormFile", "wrong Size");
				return View();
			}
			book.Image = book.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");


			foreach (var item in book.CategoryIds)
			{
				if (!await _context.Categories.AnyAsync(x => x.Id == item))
				{
					ModelState.AddModelError("", "Invalid Category Id");
					return View(book);
				}
				BookCategory bookCategory = new BookCategory
				{
					CreatedAt = DateTime.Now,
					Book = book,
					CategoryId = item
				};
				await _context.BookCategories.AddAsync(bookCategory);
			}
			book.CreatedAt = DateTime.Now;
			await _context.AddAsync(book);

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			ViewBag.Categories = await _context.Categories.
				Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Authors = await _context.Authors
				.Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Sellers = await _context.Sellers.
				Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Languages = await _context.Languages.
				Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Publishers = await _context.Publishers.
				Where(p => !p.IsDeleted).ToListAsync();

			Book? book = await _context.Books
				.Where(b => !b.IsDeleted && b.Id==id).
				Include(x => x.BookCategories.Where(x =>!x.IsDeleted)).
				   ThenInclude(x => x.Category).
				Include(x => x.Author).Where(x=>!x.IsDeleted).
				Include(x => x.Seller).Where(x => !x.IsDeleted).
				Include(x => x.Language).Where(x => !x.IsDeleted).
				Include(x=>x.Publisher).Where(x=> !x.IsDeleted)
				.FirstOrDefaultAsync();

			if (book == null)
			{
				return NotFound();
			}
			return View(book);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, Book newBook)
		{
			ViewBag.Categories = await _context.Categories.AsNoTracking().
				Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Authors = await _context.Authors
				.Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Sellers = await _context.Sellers.
				Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Languages = await _context.Languages.
				Where(b => !b.IsDeleted).ToListAsync();
			ViewBag.Publishers = await _context.Publishers.
				Where(p => !p.IsDeleted).ToListAsync();

			Book? book = await _context.Books.AsNoTracking()
				.Where(b => !b.IsDeleted && b.Id == id).
				Include(x => x.BookCategories.Where(x => !x.IsDeleted)).
				   ThenInclude(x => x.Category).
				Include(x => x.Author).
				Include(x => x.Seller).
				Include(x => x.Language).
				Include(x => x.Publisher)
				.FirstOrDefaultAsync();

			if (book == null)
			{
				return NotFound();
			}
			if (!ModelState.IsValid)
			{
				return View(book);
			}

			List<BookCategory> removeCategory = await _context.BookCategories.
				Where(x => !newBook.CategoryIds.Contains(x.CategoryId)).ToListAsync();

			_context.BookCategories.RemoveRange(removeCategory);
			foreach (var item in newBook.CategoryIds)
			{
				if (_context.BookCategories.Where(x => x.BookId == id && x.CategoryId == item).Count() > 0)
					continue;

				await _context.BookCategories.AddAsync(new BookCategory
				{
					BookId = id,
					CategoryId = item
				});
			}
            newBook.SellerId = newBook.SellerId == 0 ? null : newBook.SellerId;
            newBook.AuthorId = newBook.AuthorId == 0 ? null : newBook.AuthorId;
            newBook.LanguageId = newBook.LanguageId == 0 ? null : newBook.LanguageId;
            newBook.PublisherId = newBook.PublisherId == 0 ? null : newBook.PublisherId;
			
            if (newBook.FormFile != null)
			{
				if (!Helper.isImage(newBook.FormFile))
				{
					ModelState.AddModelError("FormFile", "Is not Image!");
					return View();
				}
				if (!Helper.isSizeOk(newBook.FormFile, 1))
				{
					ModelState.AddModelError("FormFile", "Wrong Size!");
					return View();
				}
				if (book.Image != null)
				{
					Helper.RemoveImage(_env.WebRootPath, "admin/img/images", book.Image);

				}

				newBook.Image = newBook.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");
				if (newBook.Image == null)
				{
					ModelState.AddModelError("FormFile", "Image is null!");
					return View();
				}
			}
			newBook.CreatedAt = book.CreatedAt;
			newBook.UpdatedAt = DateTime.Now;
			_context.Books.Update(newBook);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));

		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			Book? book = await _context.Books.FindAsync(id);
			if(book == null)
			{
				return NotFound();
			}
			book.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

	}
}
