namespace Web.API.Bases.Repos
{

    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<List<TEntity>> GetAll();
        Task<TEntity> Get(Guid id);
        Task<TEntity> Create(TEntity entity);
        //Task<TEntity> Create<TDTO>(TDTO entityDto) where TDTO : class;
        Task<TEntity> Update(TEntity entity);
        //Task<TEntity> Update<TDTO>(TDTO entityDto) where TDTO : class;
        Task<bool> Delete(Guid id);
        Task<bool> Save();
    }
}

