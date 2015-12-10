using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using You.Service;
using You.Models;

namespace You.Web
{
    public class CategoryUrlProvider : RouteBase
    {
        CategoryService categoryService = new CategoryService();
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径

            virtualPath = virtualPath.Substring(2).Replace(".html", "").Trim('/');

            string categoryName, page="";
            if (virtualPath.IndexOf(".") > 0)
            {
                categoryName = virtualPath.Split('.')[0];
                page = virtualPath.Split('.')[1];
            }
            else
                categoryName = virtualPath;

           //尝试根据分类名称获取相应分类，忽略大小写
           var category = categoryService.Find(c =>c.State==ItemState.Nomal&& c.SubTitle.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)//如果分类是null，可能不是我们要处理的URL，返回null，让匹配继续进行
                return null;

            //至此可以肯定是我们要处理的URL了
            var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            data.Values.Add("controller", "Category");
            data.Values.Add("action", "Index");
            data.Values.Add("id", category.CategoryID);
            if (!string.IsNullOrEmpty(page)) data.Values.Add("page", page);
            data.DataTokens.Add("namespaces", new string[] { "You.Web.Controllers" });

            return data;//返回这个路由值将调用CategoryController.ShowCategory(category.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //判断请求是否来源于CategoryController.Showcategory(string id),不是则返回null,让匹配继续
            object categoryId,page;
            values.TryGetValue("id", out categoryId); //values["id"] as string;
            values.TryGetValue("page", out page);

            if (categoryId == null)//路由信息中缺少参数id，不是我们要处理的请求，返回null
                return null;

            //请求不是CategoryController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("category", StringComparison.OrdinalIgnoreCase))
                return null;
            //请求不是CategoryController.Showcategory(string id)发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("action") || !values["action"].ToString().Equals("index", StringComparison.OrdinalIgnoreCase))
                return null;

            //至此，我们可以确定请求是CategoryController.Showcategory(string id)发起的，生成相应的URL并返回
            var category = categoryService.Find(c => c.CategoryID==  (int)categoryId);

            if (category == null)
                throw new ArgumentNullException("category");//找不到分类抛出异常

            string path;//生成URL
            if (string.IsNullOrEmpty(category.SubTitle)) path = "cat/" + category.CategoryID+(page==null?"":("."+(int)page))+".html";
            else path = category.SubTitle.Trim() + (page == null ? "" : ("." + (int)page)) + ".html";

            return new VirtualPathData(this, path.ToLowerInvariant());
        }
    }
}