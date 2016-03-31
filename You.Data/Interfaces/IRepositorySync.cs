using You.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace You.Data
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepositorySync<T>:IRepository<T> where T : class
    {
        int pageCount { get; set; }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>添加后的数据实体</returns>
        T Add(T entity, bool isSave = true);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        int AddRange(IEnumerable<T> entities, bool isSave = true);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        bool Update(T entity, bool isSave = true);

        int UpdateRange(IEnumerable<T> entities, bool isSave);
   

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        bool Delete(T entity, bool isSave = true);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>删除的记录数</returns>
        int DeleteRange(IEnumerable<T> entities, bool isSave = true);

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>受影响的记录数</returns>
        int Save();

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="countLamdba">查询表达式</param>
        /// <returns>记录数</returns>
        int Count(Expression<Func<T, bool>> countLamdba = null);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        bool Exist(Expression<Func<T, bool>> anyLambda);
     

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="ID">实体ID</param>
        /// <returns></returns>
        T Find(int ID);
       

        /// <summary>
        /// 查找实体 
        /// </summary>
        /// <param name="findLambda">Lambda表达式</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> findLambda);


        /// <summary>
        /// 查找所有列表
        /// </summary>
        /// <returns></returns>
        IQueryable<T> FindAll();

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <param name="number">返回的记录数【0-返回所有】</param>
        /// <param name="whereLandba">查询条件</param>
        /// <param name="orderType">排序方式</param>
        /// <param name="orderLandba">排序条件</param>
        /// <returns></returns>
        IQueryable<T> FindList<TKey>(int number, Expression<Func<T, bool>> whereLandba, OrderType orderType, Expression<Func<T, TKey>> orderLandba);

        IQueryable<T> FindPageList<TKey>(int pageIndex, int pageNumber, Expression<Func<T, bool>> whereLandba, OrderType orderType, Expression<Func<T, TKey>> orderLandba);
    }
}