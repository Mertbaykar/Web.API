using API.Core.DTOs;
using API.Core.DTOs.Product;
using API.Core.Models;
using Web.API.Bases.Repos;
using Web.API.Models;

namespace Web.API.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product> Create(CreateProductDTO productDTO);
        Task<Product> CreateAsAdmin(CreateProductDTO productDTO);
        Task<Product> CreateAsOrdinary(CreateProductDTO productDTO);
        Task<List<GetProductDTO>> GetRelatedProducts();
        Task<GetProductDTO> GetProduct(Guid id);
        void AddCategories(Product product, List<Guid> categories);
    }
}
