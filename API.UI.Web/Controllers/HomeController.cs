using API.Core.DTOs.Category;
using API.Core.DTOs.Product;
using API.Core.HTTPClients;
using API.UI.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductClient _productClient;
        private readonly CategoryClient _categoryClient;

        public HomeController(ILogger<HomeController> logger, ProductClient productClient, CategoryClient categoryClient)
        {
            _logger = logger;
            _productClient = productClient;
            _categoryClient = categoryClient;
        }

        public IActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Welcome()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _categoryClient.GetAll<GetCategoryDTO>();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(CreateProductDTO createProductDTO)
        {
            var response = await _productClient.PostAsJsonAsync(createProductDTO);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                ModelState.AddModelError("Price", "Ürün başarıyla eklendi");
            else
                ModelState.AddModelError("Price", "Ürün eklenemedi");

            return RedirectToAction("CreateProduct");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}