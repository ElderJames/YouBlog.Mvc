using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using You.Service;
using You.Data;
using You.Data.Repository;
using You.Models;
using Microsoft.AspNet.Identity;

namespace You.Web
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected static IService<T> GetService<T>() where T : class
        {
            IDbContext db = ContextFactory.GetCurrentContext<EFDbContext>();
            IRepository<T> repo = new EFRepository<T>(db);
            return ServiceFactory.GetService(repo);
        }

        protected System.Web.Mvc.JsonResult Json(object data = null)
        {
            return new Json(data);
        }

        public System.Web.Mvc.JsonResult Success(object data = null)
        {
            return new Success(data);
        }

        public System.Web.Mvc.JsonResult Fail(object data = null)
        {
            return new Fail(data);
        }

        #region 属性

        public new User User
        {
            get
            {
                if (UserService.CurrentUser == null)
                {
                    
                }
                return UserService.CurrentUser;
            }
        }

        public IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        #endregion
    }
}