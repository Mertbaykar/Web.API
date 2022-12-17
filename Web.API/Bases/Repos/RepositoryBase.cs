using API.Core.DTOs;
using API.Core.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.API.DbContexts;

namespace Web.API.Bases.Repos
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        protected readonly BusinessContext _dbContext;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpcontextAccessor;


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
            if (_httpcontextAccessor.HttpContext == null || _httpcontextAccessor.HttpContext.Request == null)
                return null;

            #region Get Claims via Authorization Header
            var authorizationHeaderValues = _httpcontextAccessor.HttpContext.Request.GetTypedHeaders().Headers.Authorization;
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
        public RepositoryBase(BusinessContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpcontextAccessor = httpContextAccessor;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await Save();
            return entity;
        }

        public async Task<TEntity> Get(Guid id)
        {
            var entity = await _dbContext.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }
        public async Task<bool> Delete(Guid id)
        {
            var entity = await Get(id);
            if (entity == null)
                return false;
            entity.DeActivate();
            await Save();
            return true;
        }
        public async Task<TEntity> Update(TEntity entity)
        {
            var realEntity = await Get(entity.Id);
            if (realEntity == null)
                return null;
            _mapper.Map(entity, realEntity);
            await Save();
            return realEntity;
        }

        public async Task<bool> Save()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
