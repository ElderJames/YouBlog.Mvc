using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;

namespace You.Web.Filter
{
    public class TemplateRelevantAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
            {
                var currentUserTheme = this.GetCurrentUserTheme();
                var template = string.IsNullOrEmpty(currentUserTheme) ? ThemeService.DefaultTemplateName : currentUserTheme;
                var controller = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
                var action = filterContext.RequestContext.RouteData.Values["Action"].ToString();

                if (string.IsNullOrWhiteSpace(viewResult.ViewName))
                {
                    viewResult.ViewName = string.Format(
                         "~/Content/Themes/{0}/{1}/{2}.{3}",
                         template,
                         controller,
                         action,
                         "cshtml"
                         );

                    return;
                }
            }

            base.OnResultExecuting(filterContext);
        }

        private string GetCurrentUserTheme()
        {
            var _theme= ThemeService.Current.FindbyUser(Convert.ToInt32(HttpContext.Current.GetOwinContext().Authentication.User.FindFirst(ClaimTypes.Sid).Value));
            return _theme.Directory;
        }
    }
}