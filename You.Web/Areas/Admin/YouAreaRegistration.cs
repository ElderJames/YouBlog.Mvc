using System.Web.Mvc;

namespace You.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_Article",
                "Admin/Article/{id}",
                new { controller = "Article", action = "Index", id = UrlParameter.Optional },
                 new string[] { "You.Web.Areas.Admin.Controllers" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller="Home", action = "Index", id = UrlParameter.Optional },
                 new string[] { "You.Web.Areas.Admin.Controllers" }
            );
        }
    }
}