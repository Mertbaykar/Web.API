using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using API.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Core.Helpers;
using Web.API.Repositories.Interfaces;
using Web.API.Models;
using AutoMapper;
using API.Core.DTOs;
using Web.API.Bases;

namespace Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public LoginController(IConfiguration config, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _config = config;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
        {
            //var currentuser = CurrentUser;
            var user = await _employeeRepository.GetEmployeeWithRoles(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        // To generate token
        private string GenerateToken(Employee user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.FullName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }.ToList();

            if (user.RoleGroups != null && user.RoleGroups.Count() > 0)
            {
                List<RoleGroup> roleGroups = user.RoleGroups.ToList();
                List<Role> roles = user.RoleGroups.SelectMany(x => x.Roles).Distinct().ToList();
                roleGroups.ForEach(roleGroup => claims.Add(new Claim("RoleGroup", roleGroup.Name)));
                roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.Name)));
            }
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim("CompanyId", user.CompanyId.ToString()));
            claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
