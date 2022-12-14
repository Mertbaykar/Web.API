using API.Core.DTOs;
using API.Core.DTOs.Product;
using API.Core.Models;
using API.Core.RoleDefinitions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.API.Bases.Repos;
using Web.API.DbContexts;
using Web.API.Models;
using Web.API.Repositories.Interfaces;

namespace Web.API.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(BusinessContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }

        public void AddCategories(Product product, List<Guid> categories)
        {
            if (categories != null && categories.Count() > 0)
            {
                List<Category> categories2Add = _dbContext.Categories.Where(x => categories.Contains(x.Id)).ToList();
                categories2Add.ForEach(cat => product.Categories.Add(cat));
            }
        }

        public void ChangeCompany(Product product, Guid companyId)
        {
            if (companyId != null && companyId != Guid.Empty)
            {
                var company = _dbContext.Companies.FirstOrDefault(x => x.Id == companyId);
                product.CompanyId = companyId;
            }
        }

        public async Task<Product> Create(CreateProductDTO productDTO)
        {
            Product product = _mapper.Map<CreateProductDTO, Product>(productDTO);
            List<Guid> categories2Add = productDTO.Categories;
            Guid companyId = productDTO.Company;
            AddCategories(product, categories2Add);
            ChangeCompany(product, companyId);
            return await base.Create(product);
        }

        public async Task<List<GetProductDTO>> GetRelatedProducts(UserInfoDTO currentUser)
        {
            List<Product> products = null;

            if (currentUser.IsInRole(RoleDefinitions.ProductAdmin))
                products = await _dbContext.Products.Include(x => x.Company).Include(x => x.Categories).ToListAsync();
            else
            {
                var currentUserId = currentUser.Id;
                products = _dbContext.Products.Where(x => x.Company.Employees.Any(y=> y.Id == currentUserId))
                    .Include(x => x.Company)
                    .Include(x => x.Categories)
                    .ToList();
            }

            var result = _mapper.Map<List<Product>, List<GetProductDTO>>(products);
            return result;
        }

        public async Task<GetProductDTO> GetProduct(Guid id)
        {
            Product product = null;

            product = await _dbContext.Products.Include(x => x.Company).Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
            var result = _mapper.Map<Product, GetProductDTO>(product);
            return result;
        }
    }
}
