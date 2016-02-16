using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace You.Web.Filter
{
    public class TemplateRelevantAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
            {
                //var currentUserTemplate = this.GetCurrentUserTemplate();
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

        //private string GetCurrentUserTemplate()
        //{
        //    return TemplateService.Current.GetTemplate(Thread.CurrentPrincipal.Identity.Name);
        //}
    }
}