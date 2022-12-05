using API.Core.DTOs.Product;
using API.Core.Models;
using Web.API.Bases.Repos;
using Web.API.Models;

namespace Web.API.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        //Task<Product> CreateProduct(CreateProductDTO productDTO);
    }
}
