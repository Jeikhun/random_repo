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
    public class ToyController : Controller
    {
        private readonly DolphinDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ToyController(DolphinDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Toy> toys = await _context.Toys.
                Include(x => x.ToyCategories).
                   ThenInclude(x => x.Category).
                Include(x => x.toyImages).
                Where(c => !c.IsDeleted).ToListAsync();
            return View(toys);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.
                Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Sellers = await _context.Sellers
                .Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.
                Where(p => !p.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Toy? toy)
        {
            int i = 0;
            ViewBag.Categories = await _context.Categories.
               Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Sellers = await _context.Sellers
                .Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.
                Where(p => !p.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (toy.FormFiles.Count == 0)
            {
                ModelState.AddModelError("", "Image must be added!");
                return View(toy);
            }
            foreach (var item in toy.FormFiles)
            {

                if (!Helper.isImage(item))
                {
                    ModelState.AddModelError("", "The format of file is image!!");
                    return View();
                }
                if (!Helper.isSizeOk(item, 1))
                {
                    ModelState.AddModelError("", "The size of image must less than 1mb!");
                    return View();
                }
                ToyImage toyImage = new ToyImage
                {

                    CreatedAt = DateTime.Now,
                    Toy = toy,
                    Image = item.CreateImage(_environment.WebRootPath, "admin/img/images"),
                    isMain = i == 0 ? true : false,
                };
                await _context.ToyImages.AddAsync(toyImage);
                i++;
            }
            toy.SellerId = toy.SellerId == 0 ? null : toy.SellerId;
            toy.PublisherId = toy.PublisherId == 0 ? null : toy.PublisherId;


            foreach (var item in toy.CategoryIds)
            {
                if (!await _context.Categories.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Invalid Category Id");
                    return View(toy);
                }
                ToyCategory toyCategory = new ToyCategory
                {
                    CreatedAt = DateTime.Now,
                    Toy = toy,
                    CategoryId = item
                };
                await _context.ToyCategories.AddAsync(toyCategory);
            }
            await _context.AddAsync(toy);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
           
            ViewBag.Categories = await _context.Categories.
               Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Sellers = await _context.Sellers
                .Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.
                Where(p => !p.IsDeleted).ToListAsync();

            Toy? toy = await _context.Toys.
                  Where(c => !c.IsDeleted && c.Id == id).
                Include(x => x.ToyCategories.Where(x => !x.IsDeleted)).
                   ThenInclude(x => x.Category).
                Include(x => x.toyImages.Where(x => !x.IsDeleted))
              .FirstOrDefaultAsync();
            if (toy == null)
            {
                return NotFound();
            }
            return View(toy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Toy toy)
        {
            ViewBag.Categories = await _context.Categories.
               Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Sellers = await _context.Sellers
                .Where(p => !p.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.
                Where(p => !p.IsDeleted).ToListAsync();


            Toy? updatedToy = await _context.Toys.
                AsNoTracking().
                  Where(c => !c.IsDeleted && c.Id == id).
                Include(x => x.ToyCategories.Where(x => !x.IsDeleted)).
                   ThenInclude(x => x.Category).
                Include(x => x.toyImages.Where(x => !x.IsDeleted))
              .FirstOrDefaultAsync();
            if (updatedToy is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updatedToy);
            }
            List<ToyCategory> RemoveableCategory = await _context.ToyCategories.
                Where(x => !toy.CategoryIds.Contains(x.CategoryId)).ToListAsync();

            _context.ToyCategories.RemoveRange(RemoveableCategory);
            foreach (var item in toy.CategoryIds)
            {
                if (_context.ToyCategories.Where(x => x.ToyId == id && x.CategoryId == item).Count() > 0)
                    continue;

                await _context.ToyCategories.AddAsync(new ToyCategory
                {
                    ToyId = id,
                    CategoryId = item
                });
            }
            
            toy.SellerId = toy.SellerId == 0 ? null : toy.SellerId;
            toy.PublisherId = toy.PublisherId == 0 ? null : toy.PublisherId;
            if (toy.FormFiles is not null && toy.FormFiles.Count > 0)
            {
                foreach (var item in toy.FormFiles)
                {
                    if (!Helper.isImage(item))
                    {
                        ModelState.AddModelError("", "The format of file is image!!");
                        return View(updatedToy);
                    }
                    if (!Helper.isSizeOk(item, 1))
                    {
                        ModelState.AddModelError("", "The size of image must less than 1mb!");
                        return View(updatedToy);
                    }
                    ToyImage toyImage = new ToyImage
                    {
                        CreatedAt = DateTime.Now,
                        Toy = toy,
                        Image = item.CreateImage(_environment.WebRootPath, "admin/img/images"),
                    };
                    await _context.ToyImages.AddAsync(toyImage);
                }
            }
            _context.Toys.Update(toy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SetAsMainImage(int id)
        {
            ToyImage? productImage = await _context.ToyImages.FindAsync(id);

            if (productImage is null)
            {
                return Json(new
                {
                    status = 404
                });
            }
            productImage.isMain = true;

            ToyImage? productImage1 = await _context.ToyImages.Where(x => x.isMain && x.ToyId == productImage.ToyId).FirstOrDefaultAsync();
            productImage1.isMain = false;

            await _context.SaveChangesAsync();
            return Json(new
            {
                status = 200
            });
        }
        public async Task<IActionResult> RemoveImage(int id)
        {
            ToyImage? productImage = await _context.ToyImages.
                Where(x => x.Id == id && !x.IsDeleted).
                FirstOrDefaultAsync();
            if (productImage is null)
            {
                return Json(new
                {
                    status = 404,
                    desc = "Image not found"
                });
            }
            if (productImage.isMain)
            {
                return Json(new
                {
                    status = 400,
                    desc = "Main image is not deleted"
                });
            }
            productImage.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Json(new
            {
                status = 200
            });
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Toy? Toy = await _context.Toys.
               Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();

            if (Toy == null)
            {
                return NotFound();
            }

            Toy.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
