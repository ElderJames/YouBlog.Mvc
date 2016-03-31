using System;
using System.Runtime.Remoting.Messaging;

namespace You.Data
{
    /// <summary>
    /// 上下文简单工厂
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前请求线程内的数据上下文
        /// </summary>
        /// <returns></returns>
        public static IDbContext GetCurrentContext<TContext>() where TContext:class
        {
            Type contextType = typeof(TContext);
            IDbContext _nContext = CallContext.GetData(contextType.FullName) as IDbContext;
            if (_nContext == null)
            {
                //反射创建一个数据上下文实例
                _nContext = Activator.CreateInstance(contextType) as IDbContext;
                CallContext.SetData(contextType.FullName, _nContext);
            }
            return _nContext;
        }
    }
}
