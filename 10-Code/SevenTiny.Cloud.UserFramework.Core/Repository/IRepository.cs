using SevenTiny.Cloud.UserFramework.Infrastructure.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.UserFramework.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Result Add(TEntity entity);
        Result Add(IList<TEntity> entities);
        Result Update(TEntity entity);
        Result Delete(TEntity entity);
    }
}
