using API.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.API.Bases.Repos;
using Web.API.DbContexts;
using Web.API.Models;
using Web.API.Repositories.Interfaces;

namespace Web.API.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(BusinessContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
        public async Task<Employee> GetEmployeeWithRoles(UserLogin userLogin)
        {
            try
            {
                var employee = await _dbContext.Employees.Include(x => x.RoleGroups).ThenInclude(x => x.Roles).FirstOrDefaultAsync(x => x.Email == userLogin.Username && x.Password == userLogin.Password);
                return employee;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
