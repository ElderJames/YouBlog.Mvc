using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using You.Data;
using You.Data.Types;

namespace You.Service
{
    /// <summary>
    /// 业务逻辑基类(异步)
    /// </summary>
    public partial class BaseServiceAsync<T> where T:class
    {
        private IRepositoryAsync<T> _repo;

        public int pageCount { get; set; }
        private BaseServiceAsync(IRepositoryAsync<T> repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>添加后的数据实体</returns>
        public async Task<T> Add(T entity, bool isSave = true)
        {
            return await _repo.AddAsync(entity, isSave);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public async Task<int> Add(IEnumerable<T> entities, bool isSave = true)
        {
            return await _repo.AddRangeAsync(entities, isSave);
        }     

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public async Task<bool> Update(T entity, bool isSave = true)
        {
            return await _repo.UpdateAsync(entity, isSave);
        }    

        public async Task<int> UpdateRange(IEnumerable<T> entities,bool isSave=true)
        {
            return await _repo.UpdateRangeAsync(entities,isSave);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns></returns>
        public async Task<bool> Delete(T entity, bool isSave = true)
        {
            return await _repo.DeleteAsync(entity, isSave);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">数据列表</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>删除的记录数</returns>
        public async Task<int> DeleteRange(IEnumerable<T> entities, bool isSave = true)
        {
            return await _repo.DeleteRangeAsync(entities, isSave);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>受影响的记录数</returns>
        public async Task<int> Save()
        {
            return await  _repo.SaveAsync();
        }

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="countLamdba">查询表达式</param>
        /// <returns>记录数</returns>
        public async Task<int> Count(Expression<Func<T, bool>> countLamdba = null)
        {
            return await _repo.CountAsync(countLamdba);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查询表达式</param>
        /// <returns>布尔值</returns>
        public async Task<bool> Exist(Expression<Func<T, bool>> anyLambda)
        {
            return await  _repo.ExistAsync(anyLambda);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="ID">实体ID</param>
        /// <returns></returns>
        public async Task<T> Find(int ID)
        {
            return await _repo.FindAsync(ID);
        }  

        /// <summary>
        /// 查找实体 
        /// </summary>
        /// <param name="findLambda">Lambda表达式</param>
        /// <returns></returns>
        public async Task<T> Find(Expression<Func<T, bool>> findLambda)
        {
            return await _repo.FindAsync(findLambda);
        }   

        /// <summary>
        /// 查找所有列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindAll()
        {
            return await _repo.FindAllAsync();
        }

        /// <summary>
        /// 查找数据列表
        /// </summary>
        /// <param name="number">返回的记录数【0-返回所有】</param>
        /// <param name="whereLandba">查询条件</param>
        /// <param name="orderType">排序方式</param>
        /// <param name="orderLandba">排序条件</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindList<TKey>(int number, Expression<Func<T, bool>> whereLandba, OrderType orderType, Expression<Func<T, TKey>> orderLandba)
        {
            return await _repo.FindListAsync<TKey>(number, whereLandba, orderType, orderLandba);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="pageIndex">页码【从1开始】</param>
        /// <param name="pageNumber">每页记录数</param>
        /// <param name="totalNumber">总记录数</param>
        /// <param name="whereLandba">查询表达式</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="orderLandba">排序表达式</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> FindPageList<TKey>(int pageIndex, int pageNumber, Expression<Func<T, bool>> whereLandba, OrderType orderType, Expression<Func<T, TKey>> orderLandba)
        {
            var data = await _repo.FindPageListAsync<TKey>(pageIndex, pageNumber, whereLandba, orderType, orderLandba) ;
            pageCount=_repo.pageCount;
            return data;
        }
    }
}
