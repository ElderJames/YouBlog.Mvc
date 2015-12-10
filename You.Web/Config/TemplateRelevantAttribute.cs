using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Threading;

namespace RX.Web
{
    /// <summary>
    /// 模板相关。
    /// </summary>
    public sealed class TemplateRelevantAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
            {
                var currentUserTemplate = this.GetCurrentUserTemplate();
                var template = string.IsNullOrEmpty(currentUserTemplate) ? TemplateService.DefaultTemplateName : currentUserTemplate;
                var controller = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
                var action = filterContext.RequestContext.RouteData.Values["Action"].ToString();

                if (string.IsNullOrWhiteSpace(viewResult.ViewName))
                {
                    viewResult.ViewName = string.Format(
                        "~/Views/{0}/{1}/{2}/{3}.{4}",
                        TemplateService.TemplateDirectoryName,
                        template,
                        controller,
                        action,
                        TemplateService.TemplateFileExtension);

                    return;
                }
            }

            base.OnResultExecuting(filterContext);
        }
    }

}