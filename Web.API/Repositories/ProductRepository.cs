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

        public async Task<Product> Create(CreateProductDTO productDTO)
        {
            Product product = _mapper.Map<CreateProductDTO, Product>(productDTO);
            List<Guid> categoriesToAdd = productDTO.Categories;
            if (categoriesToAdd != null && categoriesToAdd.Count() > 0)
            {
                List<Category> categories = _dbContext.Categories.Where(x => categoriesToAdd.Contains(x.Id)).ToList();
                categories.ForEach(cat => product.Categories.Add(cat));
            }
            return await base.Create(product);
        }
    }
}
