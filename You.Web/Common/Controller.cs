using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace You.Web.Common
{
    public class Controller : System.Web.Mvc.Controller
    {
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