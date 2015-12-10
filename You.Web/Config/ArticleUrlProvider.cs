using System;
using System.Web.Mvc;
using System.Web.Routing;
using You.Service;
using You.Models;
namespace You.Web
{
    public class ArticleUrlProvider : RouteBase
    {
        CommonModelService commonModelService = new CommonModelService();
        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径

            virtualPath = virtualPath.Substring(2).Replace(".html","").Trim('/');

            var article = commonModelService.Find(cm => cm.State == CommonModelState.Normal && cm.SubTitle.Equals(virtualPath, StringComparison.OrdinalIgnoreCase));
            //尝试根据分类名称获取相应分类，忽略大小写
            

            if (article == null)//如果分类是null，可能不是我们要处理的URL，返回null，让匹配继续进行
                return null;

            //至此可以肯定是我们要处理的URL了
            var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            data.Values.Add("controller", "Article");
            data.Values.Add("action", "Index");
            data.Values.Add("id", article.ModelID);
            data.DataTokens.Add("namespaces", new string[] { "You.Web.Controllers" });

            return data;//返回这个路由值将调用CategoryController.ShowCategory(category.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //判断请求是否来源于CategoryController.Showcategory(string id),不是则返回null,让匹配继续
           object id;
            values.TryGetValue("id", out id); //values["id"] as string;

            if (id == null)//路由信息中缺少参数id，不是我们要处理的请求，返回null
                return null;



            //请求不是CategoryController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("article", StringComparison.OrdinalIgnoreCase))
                return null;
            //请求不是CategoryController.Showcategory(string id)发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("action") || !values["action"].ToString().Equals("index", StringComparison.OrdinalIgnoreCase))
                return null;

            //至此，我们可以确定请求是CategoryController.Showcategory(string id)发起的，生成相应的URL并返回
            var article = commonModelService.Find(cm => cm.State == CommonModelState.Normal && cm.ModelID ==(int)id);

            if (article == null)
                throw new ArgumentNullException("article");//找不到分类抛出异常

            string path;
            if (string.IsNullOrEmpty(article.SubTitle)) path = "post/" + article.ModelID+".html";
            else path= article.SubTitle.Trim() + ".html";//生成URL

            return new VirtualPathData(this, path.ToLowerInvariant());
        }
    }
}