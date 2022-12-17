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
        public ProductRepository(BusinessContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(dbContext, mapper, httpContextAccessor)
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

        public async Task<Product> CreateAsAdmin(CreateProductDTO productDTO)
        {
            Product product = _mapper.Map<CreateProductDTO, Product>(productDTO);
            List<Guid> categories2Add = productDTO.Categories;
            Guid companyId = productDTO.Company;
            if (companyId == Guid.Empty)
                throw new Exception("Firma alanı boş bırakılamaz.");

            AddCategories(product, categories2Add);
            product.ChangeCompany(companyId);
            return await base.Create(product);
        }

        public async Task<Product> CreateAsOrdinary(CreateProductDTO productDTO)
        {
            Product product = _mapper.Map<CreateProductDTO, Product>(productDTO);
            List<Guid> categories2Add = productDTO.Categories;
            AddCategories(product, categories2Add);
            product.CompanyId = CurrentUser.CompanyId;
            return await base.Create(product);
        }

        public async Task<Product> Create(CreateProductDTO productDTO)
        {
            try
            {
                if (CurrentUser.IsInRole(RoleDefinitions.ProductAdmin))
                    return await CreateAsAdmin(productDTO);
                return await CreateAsOrdinary(productDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<List<GetProductDTO>> GetRelatedProducts()
        {
            List<Product> products = null;

            if (CurrentUser.IsInRole(RoleDefinitions.ProductAdmin))
                products = await _dbContext.Products.Include(x => x.Company).Include(x => x.Categories).ToListAsync();
            else
            {
                var currentUserId = CurrentUser.Id;
                products = _dbContext.Products.Where(x => x.Company.Employees.Any(y => y.Id == currentUserId))
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
