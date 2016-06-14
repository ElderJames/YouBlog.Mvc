using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using You.Data;
using System.Runtime.Remoting.Messaging;
using You.Data.Repository;

namespace You.Service
{
    public class ServiceFactory
    {
        //获取当前程序集中的所有类的类型
        static Type[] ts = Assembly.GetExecutingAssembly().GetTypes();

        /// <summary>
        /// 动态映射创建实现了IService<T>接口的实例
        /// 如果字符集中已有继承了BaseService<T>的类，则返回该类的实例
        /// 否则返回BaseService<T> 的实例
        /// </summary>
        /// <typeparam name="T">Model实体</typeparam>
        /// <param name="repo">仓储实例</param>
        /// <returns>实现IService<T>接口的实例对象</returns>
        public static IService<T> GetService<T>(IRepository<T> repo) where T : class
        {
            //获取传入类型的 System.Type 对象
            Type t = typeof(T);
            //取当前线程内存块中可能已存储的Service对象
            var _service = CallContext.GetData($"Server.BaseService<{t.Name}>") as IService<T>;
            if (_service != null) return _service;
            //查询扩展业务类是否存在于程序集中
            Type _class = ts.FirstOrDefault(o => o.Name.Equals(t.Name + "Service"));
            if (_class != null)
            {
                _service = Activator.CreateInstance(_class) as IService<T>;
            }
            else
            {
                //拼接类名，其中包含传入类型
                string className = "Server.Service.BaseService`1";
                className += "[[" + t.FullName + ", " + t.Assembly.FullName + "]]";
                //获取该类名对应的类型
                Type type = Type.GetType(className);
                //利用反射实例化对象
                _service = Activator.CreateInstance(type) as IService<T>;
            }
            if (_service == null) return null;
            //注入仓储对象
            _service.SetRespository(repo);

            //将对象保存到当前线程的内存块中
            CallContext.SetData($"Server.BaseService<{t.Name}>", _service);
            return _service;
        }

        public static IService<T> GetService<T>() where T : class
        {
            IDbContext db = ContextFactory.GetCurrentContext<EFDbContext>();
            IRepository<T> repo = new EFRepository<T>(db);
            return GetService(repo);
        }
    }
}