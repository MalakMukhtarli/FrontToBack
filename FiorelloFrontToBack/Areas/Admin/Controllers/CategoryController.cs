using FiorelloFrontToBack.DAL;
using FiorelloFrontToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories.Where(c=>c.IsDeleted==false).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return NotFound();
            bool isExist = _context.Categories.Where(c=>c.IsDeleted==false).Any(c => c.Name.ToLower() == category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Name", "Eyni adda bashqa categoriya movcuddur..");
                return View();
            }
            category.IsDeleted = false;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Category category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c=>c.IsDeleted==false).Include(c=>c.Products).FirstOrDefault(c=>c.Id==id);
            if (category == null) return NotFound();
            category.IsDeleted = true;
            category.DeletedTime = DateTime.Now;
            foreach (Product product in category.Products)
            {
                product.IsDeleted = true;
                product.DeletedTime = DateTime.Now;
            }

            //_context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id == null) return NotFound();
            Category category = _context.Categories.Where(c => c.IsDeleted == false).FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if (id == null) return NotFound();
            if (!ModelState.IsValid) return NotFound();
            Category categorySelected = await _context.Categories.FindAsync(id);
            Category isExist = _context.Categories.Where(c => c.IsDeleted == false)
                .FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
            if (isExist != null)
            {
                if(isExist.Id != categorySelected.Id)
                {
                    ModelState.AddModelError("Name", "Eyni adda bashqa categoriya movcuddur..");
                    return View();
                }
            }

            categorySelected.Name = category.Name;
            categorySelected.Description = category.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
