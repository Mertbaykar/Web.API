using API.Core.DTOs.Product;
using API.Core.Models;
using Web.API.Bases.Repos;
using Web.API.Models;

namespace Web.API.Repositories.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product> Create(CreateProductDTO productDTO);
        void AddCategories(Product product, List<Guid> categories);
        void AddCompanies(Product product, List<Guid> companies);
    }
}
