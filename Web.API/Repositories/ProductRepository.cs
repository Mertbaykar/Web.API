using API.Core.DTOs.Product;
using API.Core.Models;
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

        public void AddCompanies(Product product, List<Guid> companies)
        {
            if (companies != null && companies.Count() > 0)
            {
                List<Company> companies2Add = _dbContext.Companies.Where(x => companies.Contains(x.Id)).ToList();
                companies2Add.ForEach(cat => product.Companies.Add(cat));
            }
        }

        public async Task<Product> Create(CreateProductDTO productDTO)
        {
            Product product = _mapper.Map<CreateProductDTO, Product>(productDTO);
            List<Guid> categories2Add = productDTO.Categories;
            List<Guid> companies2Add = productDTO.Companies;
            AddCategories(product, categories2Add);
            AddCompanies(product, companies2Add);
            return await base.Create(product);
        }
    }
}
