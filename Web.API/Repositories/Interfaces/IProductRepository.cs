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
        Task<List<GetProductDTO>> GetRelatedProducts(UserInfoDTO currentUser);
        Task<GetProductDTO> GetProduct(Guid id);
        void AddCategories(Product product, List<Guid> categories);
        void ChangeCompany(Product product, Guid companyId);
    }
}
