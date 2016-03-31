using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using You.Service;
using You.Data;
using You.Data.Repository;

namespace You.Web
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected static IService<T> GetService<T>() where T:class
        {
            IDbContext db = ContextFactory.GetCurrentContext<EFDbContext>();
            IRepository<T> repo =new EFRepositorySync<T>(db);
            return ServiceFactory.Current.GetService<T>(repo);
    
        }

        protected static IService<T> GetServiceAsync<T>() where T :class
        {
            IDbContext db = ContextFactory.GetCurrentContext<EFDbContext>();
            IRepository<T> repo = new EFRepositoryAsync<T>(db);
            return ServiceFactory.Current.GetService<T>(repo);
        }

        protected System.Web.Mvc.JsonResult Json(object data = null)
        {
            return new You.Web.Json(data);
        }

        public System.Web.Mvc.JsonResult Success(object data = null)
        {
            return new You.Web.Success(data);
        }

        public System.Web.Mvc.JsonResult Fail(object data = null)
        {
            return new You.Web.Fail(data);
        }

        #region 属性

        public ClaimsPrincipal User { get { return HttpContext.GetOwinContext().Authentication.User; } }
        public IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        #endregion
    }
}