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
                //var template = string.IsNullOrEmpty(currentUserTemplate) ? TemplateService.DefaultTemplateName : currentUserTemplate;
                var controller = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
                var action = filterContext.RequestContext.RouteData.Values["Action"].ToString();

                if (string.IsNullOrWhiteSpace(viewResult.ViewName))
                {
                    viewResult.ViewName = string.Format(
                         "~/Views/{0}/{1}/{2}/{3}.{4}",
                        //TemplateService.TemplateDirectoryName,
                        // template,
                         controller,
                         action,
                         "cshtml"
                         //TemplateService.TemplateFileExtension
                         );

                    return;
                }
            }

            base.OnResultExecuting(filterContext);
        }

        private Theme GetCurrentUserTheme()
        {
            return ThemeService.Current.FindbyUser(Convert.ToInt32(HttpContext.Current.GetOwinContext().Authentication.User.FindFirst(ClaimTypes.Sid).Value));
        
        }
    }
}