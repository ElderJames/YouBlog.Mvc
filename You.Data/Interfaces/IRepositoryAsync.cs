using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using You.Data.Types;

namespace You.Data
{
    /// <summary>
    /// 仓储接口（异步方法）
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepositoryAsync<T>:IRepository<T> where T:class
    {
        int pageCount { get; set; }

        Task<T> AddAsync(T entity, bool isSave = true);

        Task<int> AddRangeAsync(IEnumerable<T> entities, bool isSave = true);

        Task<bool> UpdateAsync(T entity, bool isSave = true);

        Task<int> UpdateRangeAsync(IEnumerable<T> entities, bool isSave);

        Task<bool> DeleteAsync(T entity, bool isSave = true);

        Task<int> DeleteRangeAsync(IEnumerable<T> entities, bool isSave = true);

        Task<int> SaveAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> countLamdba = null);

        Task<bool> ExistAsync(Expression<Func<T, bool>> anyLambda);

        Task<T> FindAsync(int ID);

        Task<T> FindAsync(Expression<Func<T, bool>> findLambda);
        
        Task<IEnumerable<T>> FindAllAsync();

        Task<IEnumerable<T>> FindListAsync<TKey>(int number, Expression<Func<T, bool>> whereLandba, OrderType orderType, Expression<Func<T, TKey>> orderLandba);

        Task<IEnumerable<T>> FindPageListAsync<TKey>(int pageIndex, int pageNumber, Expression<Func<T, bool>> whereLandba, OrderType orderType, Expression<Func<T, TKey>> orderLandba);
    }
}
