using API.Core.DTOs.Category;
using API.Core.DTOs.Company;
using API.Core.DTOs.Product;
using API.Core.HTTPClients;
using API.Core.RoleDefinitions;
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
        private readonly CompanyClient _companyClient;

        public HomeController(ILogger<HomeController> logger, ProductClient productClient, CategoryClient categoryClient, CompanyClient companyClient)
        {
            _logger = logger;
            _productClient = productClient;
            _categoryClient = categoryClient;
            _companyClient = companyClient;
        }

        public IActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [Authorize(Roles = RoleDefinitions.ProductAdmin)]
        public IActionResult Welcome()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        #region CreateProduct
        [Authorize(Roles = RoleDefinitions.ProductAdmin)]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _categoryClient.GetAll<GetCategoryDTO>();
            var companies = await _companyClient.GetAll<GetCompanyDTO>();
            ViewBag.Categories = categories;
            ViewBag.Companies = companies;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = RoleDefinitions.ProductAdmin)]
        public async Task<IActionResult> CreateProduct(CreateProductDTO createProductDTO)
        {
            try
            {
                var response = await _productClient.PostAsJsonAsync(createProductDTO);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Json(new
                    {
                        Message = "Ürün eklendi"
                    });
                }
                return Json(new
                {
                    Message = "Ürün eklenemedi"
                });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Message = "Ürün eklenemedi. " + ex.Message
                });
            }

        }
        #endregion

        [HttpGet]
        [Authorize(Roles = RoleDefinitions.ProductAdmin + "," + RoleDefinitions.ProductReadOnly + "," + RoleDefinitions.ProductEdit)]
        public async Task<IActionResult> GetProducts()
        {
            List<GetProductDTO> products = await _productClient.GetProducts();
            return View(products);
        }

        [HttpGet]
        [Authorize(Roles = RoleDefinitions.ProductAdmin + "," + RoleDefinitions.ProductReadOnly + "," + RoleDefinitions.ProductEdit)]
        public async Task<IActionResult> ProductEdit(Guid id)
        {
            GetProductDTO product = await _productClient.GetProduct(id);
            var categories = await _categoryClient.GetAll<GetCategoryDTO>();
            var companies = await _companyClient.GetAll<GetCompanyDTO>();
            ViewBag.Categories = categories;
            ViewBag.Companies = companies;
            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = RoleDefinitions.ProductAdmin)]
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