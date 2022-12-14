using API.Core.DTOs.Product;
using API.Core.RoleDefinitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.API.Bases;
using Web.API.Repositories.Interfaces;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApiControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Authorize(Roles = RoleDefinitions.ProductAdmin)]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _productRepository.Create(productDTO);
                    return Ok(product);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            else
                return BadRequest(ModelState);
        }
        [HttpGet]
        [Authorize(Roles = RoleDefinitions.ProductAdmin + "," + RoleDefinitions.ProductReadOnly + "," + RoleDefinitions.ProductEdit)]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _productRepository.GetRelatedProducts(CurrentUser);
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = RoleDefinitions.ProductAdmin + "," + RoleDefinitions.ProductReadOnly + "," + RoleDefinitions.ProductEdit)]
        public async Task<ActionResult> GetProduct([FromRoute] Guid id)
        {
            var product = await _productRepository.GetProduct(id);
            return Ok(product);
        }
    }
}
