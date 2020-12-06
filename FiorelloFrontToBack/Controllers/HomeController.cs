using FiorelloFrontToBack.DAL;
using FiorelloFrontToBack.Models;
using FiorelloFrontToBack.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //HttpContext.Session.SetString("name", "Melek");
            //Response.Cookies.Append("surname", "Muxtarli", new CookieOptions { MaxAge=TimeSpan.FromMinutes(30) });
            HomeVM homeVM = new HomeVM
            {
                Sliders = _db.Sliders.ToList(),
                SliderContent=_db.SliderContents.FirstOrDefault(),
                Categories = _db.Categories.Where(c=>c.IsDeleted==false).ToList(),
                Products = _db.Products.Take(8).Include(p=>p.Category).Where(p => p.IsDeleted == false).ToList(),
                About = _db.Abouts.FirstOrDefault(),
                AboutSubtitleLists = _db.AboutSubtitleLists.ToList(),
                SectionHeaders = _db.SectionHeaders.ToList(),
                Experts = _db.Experts.ToList(),
                Blogs = _db.Blogs.ToList()
            };
            return View(homeVM);
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            Product product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            List<BasketVM> basket;
            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }
            BasketVM IsExist = basket.FirstOrDefault(p => p.Id == id);
            if (IsExist==null)
            {
                basket.Add(new BasketVM
                {
                    Id = id,
                    Count = 1
                });
            }
            else
            {
                IsExist.Count += 1;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Basket()
        {
            
            //string sesion = HttpContext.Session.GetString("name");
            //string cookies = Request.Cookies["surname"];
            //return Content(sesion + " " + cookies);
            List<BasketVM> dbBasket = new List<BasketVM>();
            if (Request.Cookies["basket"] != null)
            {
                List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

                foreach (BasketVM pro in basket)
                {
                    Product dbProduct = await _db.Products.FindAsync(pro.Id);
                    pro.Image = dbProduct.Image;
                    pro.Title = dbProduct.Title;
                    pro.Price = dbProduct.Price * pro.Count;
                    dbBasket.Add(pro);
                }

            }
            return View(dbBasket);
        }

        public IActionResult RemoveBasketData(int id)
        {
            List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            BasketVM IsExist = basket.FirstOrDefault(p => p.Id == id);
            basket.Remove(IsExist);
                
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            return RedirectToAction(nameof(Basket));
        }

    }
}
