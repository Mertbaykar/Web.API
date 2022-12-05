using API.Core.Models;
using Web.API.Bases.Repos;
using Web.API.Models;

namespace Web.API.Repositories.Interfaces
{
    public interface IEmployeeRepository: IRepositoryBase<Employee>
    {
        Task<Employee> GetEmployeeWithRoles(UserLogin userLogin);
    }
}
