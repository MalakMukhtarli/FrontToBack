using FiorelloFrontToBack.DAL;
using FiorelloFrontToBack.Models;
using FiorelloFrontToBack.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorelloFrontToBack.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public HeaderViewComponent(AppDbContext context,
                                   UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.BasketCount = 0;
            ViewBag.UserFullname = String.Empty;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.UserFullname = (await _userManager.FindByNameAsync(User.Identity.Name)).Name + " " +
                    (await _userManager.FindByNameAsync(User.Identity.Name)).Surname;
            }

            if (Request.Cookies["basket"] != null)
            {
                List<BasketVM> baskets = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

                // muxtelif mehsullarin sayi ucun
                ViewBag.BasketCount = baskets.Count();

                // butun mehsullarin sayi ucun
                ViewBag.BasketCount = baskets.Sum(p => p.Count);
            }
            Bio model = _context.Bios.FirstOrDefault();
            return View(await Task.FromResult(model));
        }
    }
}
