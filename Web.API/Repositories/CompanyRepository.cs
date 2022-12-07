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
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(BusinessContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
    }
}
