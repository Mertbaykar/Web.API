using API.Core.DTOs;
using API.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.API.Bases
{
    public class ApiControllerBase : ControllerBase
    {
        private UserInfoDTO _currentUser = null;

        public UserInfoDTO CurrentUser
        {
            get { return GetCurrentUser(); }
            set { _currentUser = SetCurrentUser(); }
        }

        private UserInfoDTO GetCurrentUser()
        {
            if (_currentUser == null)
                _currentUser = SetCurrentUser();
            return _currentUser;
        }

        private UserInfoDTO SetCurrentUser()
        {
            if (HttpContext == null || HttpContext.Request == null)
                return null;

            #region Get Claims via Authorization Header
            var authorizationHeaderValues = HttpContext.Request.GetTypedHeaders().Headers.Authorization;
            if (string.IsNullOrWhiteSpace(authorizationHeaderValues))
                return null;
            string token = authorizationHeaderValues.FirstOrDefault()!.Split(" ").LastOrDefault()!;
            List<Claim> claims = JWTokenHelper.ReadToken(token).Claims.ToList();
            #endregion

            #region Fill CurrentUser
            string fullName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;
            Guid id = Guid.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value);
            List<string> roleGroups = claims.Where(x => x.Type == "RoleGroup").Select(x => x.Value).ToList();
            List<string> roles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            string email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            string phone = claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)!.Value;
            Guid companyId = Guid.Parse(claims.FirstOrDefault(x => x.Type == "CompanyId")!.Value); 

            UserInfoDTO currentUser = new UserInfoDTO
            {
                FullName= fullName,
                Id= id,
                RoleGroups= roleGroups,
                Roles = roles,
                Email = email,
                PhoneNumber= phone,
                CompanyId= companyId,
            };
            #endregion

            return currentUser;
        }

    }
}
