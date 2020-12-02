using FiorelloFrontToBack.DAL;
using FiorelloFrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewBag.ProCount = _db.Products.Count();
            return View(_db.Products.Take(8).ToList());
        }

        public IActionResult LoadMore(int skip)
        {
            List<Product> model = _db.Products.Skip(skip).Take(8).ToList();
            return PartialView("_ProductPartial", model);
            // OLD VERSION
            //return Json(_db.Products.Skip(skip).Take(8).ToList());
        }
    }
}
