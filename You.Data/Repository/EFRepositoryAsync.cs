using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using You.Data.Types;

namespace You.Data.Repository
{
    /// <summary>
    /// EF异步仓储
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public class EFRepositoryAsync<T>  where T : class
    {
        private DbContext _baseDbContext;
        public int pageCount { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext">数据上下文</param>
        public EFRepositoryAsync(IDbContext db)
        {
            _baseDbContext = db as DbContext;
        }

    
    }
}
