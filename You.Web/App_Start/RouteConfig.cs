using System.Web.Mvc;
using System.Web.Routing;

namespace You.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Admin/css/{*pathInfo}");

            routes.Add("Article", new ArticleUrlProvider());//文章规则
            routes.Add("Category", new CategoryUrlProvider());//栏目规则
            routes.Add("Tag", new TagUrlProvider());

            routes.MapRoute(
               name: "page",
               url: "p/{page}.html",
               defaults: new { controller = "Home", action = "Index", pageNum = UrlParameter.Optional },
               namespaces: new string[] { "You.Web.Controllers" }
           );

            routes.MapRoute(
              name: "post",
              url: "post/{id}.html",
              defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "You.Web.Controllers" }
          );

            routes.MapRoute(
        name: "cat2",
        url: "cat/{id}.{page}.html",
        defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional, page = UrlParameter.Optional },
        namespaces: new string[] { "You.Web.Controllers" }
        );

            routes.MapRoute(
                name: "cat",
                url: "cat/{id}.html",
                defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "You.Web.Controllers" }
            );
            

            routes.MapRoute(
         name: "t2",
         url: "tag/{id}.{page}.html",
         defaults: new { controller = "Tag", action = "Index", id = UrlParameter.Optional, page = UrlParameter.Optional },
         namespaces: new string[] { "You.Web.Controllers" }
     );

            routes.MapRoute(
               name: "t",
               url: "tag/{id}.html",
               defaults: new { controller = "Tag", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "You.Web.Controllers" }
           );

         

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "You.Web.Controllers" }
          );
            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}/{id}.html",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "You.Web.Controllers" }
            );
        }
    }
}
