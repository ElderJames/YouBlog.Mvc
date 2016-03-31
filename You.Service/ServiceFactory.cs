using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using You.Data;

namespace You.Service
{
    public class ServiceFactory
    {
        private readonly IDictionary<String, Object> factory = new Dictionary<String, Object>();
        private static ServiceFactory instance;

        public static ServiceFactory Current
        {
            get
            {
                if (instance == null) instance = new ServiceFactory();
                return instance;
            }
        }

        /// <summary>
        /// 动态映射创建实现了IService<T>接口的实例
        /// </summary>
        /// <typeparam name="T">Model实体</typeparam>
        /// <param name="repo">仓储实例</param>
        /// <returns>实现IService<T>接口的实例对象</returns>
        public IService<T> GetService<T>(object repo) where T : class
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            Type[] ts = assembly.GetTypes();
            Type t = typeof(T);//用于获取类型的 System.Type 对象

            if (factory.ContainsKey("iGdou.Service" + t.FullName))
                return factory["iGdou.Service" + t.FullName] as IService<T>;//强行转换成泛型接口
            IService<T> service;
            if (repo.GetType().Equals(typeof(IRepositorySync<T>)))
            {
                Type _class = ts.FirstOrDefault(o => o.Name.Equals(t.Name+"Service"));
              
            }
            if (repo.GetType().Equals(typeof(IRepositoryAsync<T>)))
            {
                Type _class = ts.FirstOrDefault(o => o.Name.Equals(t.Name+"ServiceAsync"));
        
            }

           
           
            if (_class != null)
            {
              
            }
            else
            {
                string className = "iGdou.Service.BaseService`1";
                className += "[[" + t.FullName + ", " + t.Assembly.FullName + "]]";
                Type type = Type.GetType(className);//获取该类名称的类型
                service = Activator.CreateInstance(type) as IService<T>;
                if (service == null) return null;
            }

            service.SetRespository((IRepository<T>)repo);

            factory.Add("iGdou.Service" + t.FullName, service);
            return service;
        }

        protected IService<T> GetServiceAsync<T>(IRepositoryAsync<T> repo) where T:class
        {
            IRepository<T> rep = repo;

            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            Type[] ts = assembly.GetTypes();
            Type t = typeof(T);//用于获取类型的 System.Type 对象
            if (factory.ContainsKey("iGdou.ServiceAsync" + t.FullName))
                return factory["iGdou.ServiceAsync" + t.FullName] as IService<T>;//强行转换成泛型接口
            IService<T> service;
            Type _class = ts.FirstOrDefault(o => o.Name.Equals(t.Name + "ServiceAsync"));
            if (_class != null)
            {
                service = Activator.CreateInstance(_class) as IService<T>;
                if (service != null)
                {
                    service.SetRespository((IRepository<T>)repo);
                    factory.Add("iGdou.ServiceAsync" + t.FullName, service);
                    return service;
                }
            }
            else
            {
                string className = "iGdou.Service.BaseServiceAsync`1";
                className += "[[" + t.FullName + ", " + t.Assembly.FullName + "]]";
                Type type = Type.GetType(className);//获取该类名称的类型
                service = Activator.CreateInstance(type) as IService<T>;
                if (service != null)
                {
                    service.SetRespository((IRepository<T>)repo);
                    factory.Add("iGdou.ServiceAsync" + t.FullName, service);
                    return service;
                }
            }
            return null;
        }
    }
}