using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Potato.DbContext.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _blogContext;
        private Dictionary<Type, object>? _repositories;

        public UnitOfWork(AppDbContext blogContext)
        {
            _blogContext = blogContext;
        }
        public void Dispose()
        {

        }
        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }
            if (hasCustomRepository)
            {
                var customRepository = _blogContext.GetService<IRepository<TEntity>>();
                if (customRepository != null)
                {
                    return customRepository;
                }
            }
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_blogContext);
            }
            return (IRepository<TEntity>)_repositories[type];
        }
        public int SaveChanges(bool ensureAutoHistory = false)
        {
            throw new NotImplementedException();
        }
    }
}
