using API.Core.DTOs;
using API.Core.DTOs.Category;
using API.Core.DTOs.Company;
using API.Core.DTOs.Product;
using AutoMapper;
using Web.API.Models;

namespace Web.API.MapperConfigurations
{
    public class MapperConfigurations : Profile
    {
        public MapperConfigurations()
        {
            CreateMap<RoleGroup, UserRoleGroupDTO>();

            CreateMap<Role, UserRoleDTO>();

            CreateMap<Employee, UserContextDTO>()
               .ForMember(x => x.RoleGroups, y => y.MapFrom(z => z.RoleGroups))
               .ForMember(x => x.Roles, y => y.MapFrom(z => z.RoleGroups.SelectMany(i => i.Roles).Distinct()));

            #region NameValuePair
            CreateMap<Category, GetCategoryDTO>();
            CreateMap<Company, GetCompanyDTO>();

            #endregion

            #region Product
            CreateMap<CreateProductDTO, Product>()
              .ForMember(x => x.Categories, y => y.Ignore())
              .ForMember(x => x.Company, y => y.Ignore());

            CreateMap<Product, GetProductDTO>(); 
            #endregion

        }
    }
}
