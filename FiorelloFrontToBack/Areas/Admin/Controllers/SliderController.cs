using FiorelloFrontToBack.DAL;
using FiorelloFrontToBack.Extensions;
using FiorelloFrontToBack.Helpers;
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
            ViewBag.SliderCount = _context.Sliders.Count();
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
        public async Task<IActionResult> Create(Slider sliders)
        {
            #region File
            //if (slider.Photo == null)
            //{
            //    return View(slider);
            //}
            //bool isExist = slider.Photo.IsImage();
            //if (!isExist)
            //{
            //    ModelState.AddModelError("Photo", "Zehmet olmasa shekil tipinde file sechin");
            //    return View(slider);
            //}
            //bool photoLength = slider.Photo.PhotoLength(200);
            //if (!photoLength)
            //{
            //    ModelState.AddModelError("Photo", "Zehmet olmasa sheklin olchusu 200kb kechmesin");
            //    return View(slider);
            //}
            //int sliderCount = _context.Sliders.Count();
            //if (sliderCount >= 5)
            //{
            //    ModelState.AddModelError("", "5den artiq shekil yukleye bilmersiniz");
            //    return View(slider);
            //}

            //slider.Image = await slider.Photo.AddImageAsync(_env.WebRootPath, "img");
            //await _context.Sliders.AddAsync(slider);
            #endregion

            #region Multiple
            if (sliders.Photos == null)
            {
                return View();
            }
            int sliderCount = _context.Sliders.Count();
            if (sliderCount + sliders.Photos.Length > 5)
            {
                ModelState.AddModelError("Photos", "Putun fayllarin sayi 5i keche bilmez!");
                return View();
            }
            foreach (IFormFile slider in sliders.Photos)
            {
                if (!slider.IsImage())
                {
                    ModelState.AddModelError("Photos", "Zehmet olmasa shekil tipinde file sechin");
                    return View(slider);
                }
                bool photoLength = slider.PhotoLength(200);
                if (!photoLength)
                {
                    ModelState.AddModelError("Photos", "Zehmet olmasa sheklin olchusu 200kb kechmesin");
                    return View(slider);
                }
                Slider newSlider = new Slider();
                newSlider.Image = await slider.AddImageAsync(_env.WebRootPath, "img");
                await _context.Sliders.AddAsync(newSlider);
            }
          
            
            #endregion

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
            bool isExist = slider.Photo.IsImage();
            if (!isExist)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa shekil tipinde file sechin");
                return View();
            }
            bool photoLength = slider.Photo.PhotoLength(200);
            if (!photoLength)
            {
                ModelState.AddModelError("Photo", "Zehmet olmasa sheklin olchusu 200kb kechmesin");
                return View();
            }

            Slider SliderSelected = await _context.Sliders.FindAsync(id);
            if (SliderSelected == null) return NotFound();

            bool isDeleted = Helper.DeletedPhoto(_env.WebRootPath, "img", SliderSelected);
            if (!isDeleted)
            {
                ModelState.AddModelError("Photos", "Sistemde bash veren bir problemle bagli file siline bilmedi");
                return View();
            }

            SliderSelected.Image = await slider.Photo.AddImageAsync(_env.WebRootPath, "img");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: SliderController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            int sliderCount = _context.Sliders.Count();
            if (sliderCount <= 1)
            {
                ModelState.AddModelError("", "Sonuncu slideri sile bilmersiniz!");
                return View(slider);
            }
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
            int sliderCount = _context.Sliders.Count();
            if (sliderCount <= 1)
            {
                ModelState.AddModelError("", "Sonuncu slideri sile bilmersiniz!");
                return View(slider);
            }
            _context.Sliders.Remove(slider);

            bool isDeleted = Helper.DeletedPhoto(_env.WebRootPath, "img", slider);
            if (!isDeleted)
            {
                ModelState.AddModelError("", "Sistemde bash veren bir problemle bagli file siline bilmedi");
                return View();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
