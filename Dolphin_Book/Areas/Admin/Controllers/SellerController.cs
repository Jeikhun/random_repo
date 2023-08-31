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
    public class SellerController : Controller
    {
		private readonly DolphinDbContext _context;
		private readonly IWebHostEnvironment _env;

		public SellerController(DolphinDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<Seller> sellers = await _context.Sellers
				.Where(x => !x.IsDeleted).ToListAsync();
			return View(sellers);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Seller seller)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (seller.FormFile == null)
			{
				ModelState.AddModelError("FormFile", "Wrong!");
				return View();
			}
			if (!Helper.isImage(seller.FormFile))
			{
				ModelState.AddModelError("FormFile", "Is not Image!");
				return View();
			}
			if (!Helper.isSizeOk(seller.FormFile, 1))
			{
				ModelState.AddModelError("FormFile", "wrong Size");
				return View();
			}

			seller.Image = seller.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");

			seller.CreatedAt = DateTime.Now;
			await _context.Sellers.AddAsync(seller);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			Seller? seller = await _context.Sellers.Where(x=>!x.IsDeleted&&x.Id==id).FirstOrDefaultAsync();

			if(seller == null)
			{
				return NotFound();
			}
			return View(seller);

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, Seller newSeller)
		{
			Seller? seller = await _context.Sellers.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
			if(!ModelState.IsValid)
			{
				return View(seller);
			}

			if(seller == null)
			{
				return NotFound();
			}



			if (newSeller.FormFile != null)
			{
				if (!Helper.isImage(newSeller.FormFile))
				{
					ModelState.AddModelError("FormFile", "Is not Image!");
					return View();
				}
				if (!Helper.isSizeOk(newSeller.FormFile, 1))
				{
					ModelState.AddModelError("FormFile", "Wrong Size!");
					return View();
				}
				if(seller.Image == null)
				{
					ModelState.AddModelError("Image", "Image is null!");
					return View();
				}
				Helper.RemoveImage(_env.WebRootPath, "admin/img/images", seller.Image);
				seller.Image = newSeller.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");
			}
				seller.Name = newSeller.Name;
				seller.UpdatedAt = DateTime.Now;
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));

		}
		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			Seller? seller = await _context.Sellers.Where(x => !x.IsDeleted).FirstOrDefaultAsync();
			if(seller == null)
			{
				return NotFound();
			}

			seller.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
