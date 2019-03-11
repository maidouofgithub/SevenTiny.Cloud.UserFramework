using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Core.Enum;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace SevenTiny.Cloud.UserFramework.Core.Repository
{
    public class UserCommonInfoRepository<TEntity> : Repository<TEntity>, IUserCommonInfoRepository<TEntity> where TEntity : UserCommonInfo
    {
        public UserCommonInfoRepository(UserFrameworkDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        UserFrameworkDbContext dbContext;

        public new Result Update(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;

            base.Update(entity);
            return Result.Success();
        }

        public Result Delete(int userId)
        {
            dbContext.Delete<TEntity>(t => t.UserId.Equals(userId));
            return Result.Success();
        }

        public Result LogicDelete(int userId)
        {
            var entity = GetByUserId(userId);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                dbContext.Update(entity);
            }
            return Result.Success();
        }

        public Result Recover(int userId)
        {
            var entity = GetByUserId(userId);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.UnDeleted;
                dbContext.Update(entity);
            }
            return Result.Success();
        }

        public TEntity GetByUserId(int userId, Expression<Func<TEntity, object>> columns = null)
        {
            return columns != null
                ? dbContext.Queryable<TEntity>().Where(t => t.UserId == userId).Select(columns).Limit(1).ToEntity()
                : dbContext.QueryOne<TEntity>(t => t.UserId.Equals(userId));
        }

        public List<TEntity> GetEntitiesDeleted()
            => dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.Deleted);

        public List<TEntity> GetEntitiesUnDeleted()
            => dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.UnDeleted);
    }
}
