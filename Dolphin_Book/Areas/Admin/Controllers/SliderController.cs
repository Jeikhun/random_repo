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
    public class SliderController : Controller
    {
        private readonly DolphinDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(DolphinDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Slide> slides = await _context.Sliders.Where(x=>!x.IsDeleted).ToListAsync();
            if (slides == null)
            {
                return NotFound();
            }
            return View(slides);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slide slide)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slide.FormFile == null)
            {
                ModelState.AddModelError("FormFile", "Wrong!");
                return View();
            }
            if (!Helper.isImage(slide.FormFile))
            {
                ModelState.AddModelError("FormFile", "Is not Image!");
                return View();
            }
            if (!Helper.isSizeOk(slide.FormFile, 1))
            {
                ModelState.AddModelError("FormFile", "wrong Size");
                return View();
            }

            slide.Image = slide.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");

            slide.CreatedAt = DateTime.Now;
            await _context.Sliders.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Slide? slide = await _context.Sliders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (slide == null)
            {
                return NotFound();
            }
            return View(slide);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Slide newSlide)
        {
            Slide? slide = await _context.Sliders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View(slide);
            }

            if (slide == null)
            {
                return NotFound();
            }



            if (newSlide.FormFile != null)
            {
                if (!Helper.isImage(newSlide.FormFile))
                {
                    ModelState.AddModelError("FormFile", "Is not Image!");
                    return View();
                }
                if (!Helper.isSizeOk(newSlide.FormFile, 1))
                {
                    ModelState.AddModelError("FormFile", "Wrong Size!");
                    return View();
                }
                if (slide.Image == null)
                {
                    ModelState.AddModelError("Image", "Image is null!");
                    return View();
                }
                Helper.RemoveImage(_env.WebRootPath, "admin/img/images", slide.Image);
                slide.Image = newSlide.FormFile.CreateImage(_env.WebRootPath, "admin/img/images");
            }

            slide.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Slide? slide = await _context.Sliders.Where(x => !x.IsDeleted).FirstOrDefaultAsync();
            if (slide == null)
            {
                return NotFound();
            }

            slide.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
