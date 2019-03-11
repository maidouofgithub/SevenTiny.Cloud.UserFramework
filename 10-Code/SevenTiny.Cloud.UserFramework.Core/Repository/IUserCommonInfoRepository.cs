using SevenTiny.Cloud.UserFramework.Core.Entity;
using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.UserFramework.Core.Repository
{
    public interface IUserCommonInfoRepository<TEntity> : IRepository<TEntity> where TEntity : UserCommonInfo
    {
        new Result Update(TEntity entity);
        Result Delete(int userId);
        Result LogicDelete(int userId);
        Result Recover(int userId);
        TEntity GetByUserId(int userId, Expression<Func<TEntity, object>> columns = null);
        List<TEntity> GetEntitiesDeleted();
        List<TEntity> GetEntitiesUnDeleted();
    }
}
