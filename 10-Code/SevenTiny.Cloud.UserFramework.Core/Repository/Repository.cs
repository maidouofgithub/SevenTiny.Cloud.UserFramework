using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.UserFramework.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(UserFrameworkDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        UserFrameworkDbContext dbContext;

        public Result Add(TEntity entity)
        {
            dbContext.Add(entity);
            return Result.Success();
        }

        public Result Add(IList<TEntity> entities)
        {
            dbContext.Add<TEntity>(entities);
            return Result.Success();
        }

        public Result Update(TEntity entity)
        {
            dbContext.Update(entity);
            return Result.Success();
        }

        public Result Delete(TEntity entity)
        {
            dbContext.Delete<TEntity>(entity);
            return Result.Success();
        }
    }
}
