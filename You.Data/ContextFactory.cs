using System.Runtime.Remoting.Messaging;

namespace You.Data
{
    /// <summary>
    /// 上下文简单工厂
    /// <remarks>
    /// 创建：2014.02.05
    /// </remarks>
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// 获取当前请求内的数据上下文
        /// </summary>
        /// <returns></returns>
        public static YouDbContext GetCurrentContext()
        {
            YouDbContext _nContext = CallContext.GetData("YouContext") as YouDbContext;
            if (_nContext == null)
            {
                _nContext = new YouDbContext();
                CallContext.SetData("YouContext", _nContext);
            }
            return _nContext;
        }

        //public static MyDbContextInitializer GetCurrentContext()
        //{
        //    MyDbContextInitializer _nContext = CallContext.GetData("YouContext") as MyDbContextInitializer;
        //    if (_nContext == null)
        //    {
        //        _nContext = new MyDbContextInitializer();
        //        CallContext.SetData("YouContext", _nContext);
        //    }
        //    return _nContext;
        //}
    }
}
