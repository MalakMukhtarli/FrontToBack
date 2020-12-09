using FiorelloFrontToBack.DAL;
using FiorelloFrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: SliderController
        public ActionResult Index()
        {
            return View(_context.Sliders.ToList());
        }

        // GET: SliderController/Details/5
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        // GET: SliderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (slider.Photo == null)
            {
                return View();
            }

            bool isExist = slider.Photo.ContentType.Contains("image/");
            if (!isExist)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa shekil tipinde file sechin");
                return View();
            }

            if ((slider.Photo.Length / 1024)>200)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sheklin olchusu 200kb kechmesin");
                return View();
            }

            string PhotoName = Guid.NewGuid().ToString() + slider.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", PhotoName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(fileStream);
            }

            slider.Image = PhotoName;
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: SliderController/Update/5
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        // POST: SliderController/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (slider.Photo == null)
            {
                return View();
            }
            if (id == null) return NotFound();
            bool isExist = slider.Photo.ContentType.Contains("image/");
            if (!isExist)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa shekil tipinde file sechin");
                return View();
            }

            if ((slider.Photo.Length / 1024) > 200)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sheklin olchusu 200kb kechmesin");
                return View();
            }

            Slider SliderSelected = await _context.Sliders.FindAsync(id);
            if (SliderSelected == null) return NotFound();

            string pathOldImage = Path.Combine(_env.WebRootPath, "img", SliderSelected.Image);
            if (System.IO.File.Exists(pathOldImage))
            {
                System.IO.File.Delete(pathOldImage);
            }

            string PhotoName = Guid.NewGuid().ToString() + slider.Photo.FileName;
            SliderSelected.Image = PhotoName;
           
            string pathNewImage = Path.Combine(_env.WebRootPath, "img", PhotoName);
            using (FileStream fileStream = new FileStream(pathNewImage, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(fileStream);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: SliderController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        // POST: SliderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            _context.Sliders.Remove(slider);

            string path = Path.Combine(_env.WebRootPath, "img", slider.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
