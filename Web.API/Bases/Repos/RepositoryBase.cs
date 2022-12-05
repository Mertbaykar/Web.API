using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.API.DbContexts;

namespace Web.API.Bases.Repos
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        protected readonly BusinessContext _dbContext;
        protected readonly IMapper _mapper;
        public RepositoryBase(BusinessContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await Save();
            return entity;
        }
        //public async Task<TEntity> Create<TDTO>(TDTO entityDto) where TDTO : class
        //{
        //    try
        //    {
        //        TEntity entity = _mapper.Map<TDTO, TEntity>(entityDto);
        //        return await Create(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
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
        //public async Task<TEntity> Update<TDTO>(TDTO entityDto) where TDTO : class
        //{
        //    try
        //    {
        //        TEntity entity = _mapper.Map<TDTO, TEntity>(entityDto);
        //        return await Update(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
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
