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
    public class TagUrlProvider : RouteBase
    {
        TagService tagService = new TagService();
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径

            virtualPath = virtualPath.Substring(2).Replace(".html", "").Trim('/');

            string tagName, page = "";
            if (virtualPath.IndexOf(".") > 0)
            {
                tagName = virtualPath.Split('.')[0];
                page = virtualPath.Split('.')[1];
            }
            else
                tagName = virtualPath;

            //尝试根据分类名称获取相应分类，忽略大小写
            var tag = tagService.Find(c =>c.State==ItemState.Nomal&& c.SubTitle.Equals(tagName, StringComparison.OrdinalIgnoreCase));

            if (tag == null)//如果分类是null，可能不是我们要处理的URL，返回null，让匹配继续进行
                return null;

            //至此可以肯定是我们要处理的URL了
            var data = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            data.Values.Add("controller", "Tag");
            data.Values.Add("action", "Index");
            data.Values.Add("id", tag.Id);
            if (!string.IsNullOrEmpty(page)) data.Values.Add("page", page);
            data.DataTokens.Add("namespaces", new string[] { "You.Web.Controllers" });

            return data;//返回这个路由值将调用TagController.ShowTag(tag.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //判断请求是否来源于TagController.Showtag(string id),不是则返回null,让匹配继续
            object tagId, page;
            values.TryGetValue("id", out tagId); //values["id"] as string;
            values.TryGetValue("page", out page);


            if (tagId == null)//路由信息中缺少参数id，不是我们要处理的请求，返回null
                return null;

            //请求不是TagController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("controller") || !values["controller"].ToString().Equals("tag", StringComparison.OrdinalIgnoreCase))
                return null;
            //请求不是TagController.Showtag(string id)发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("action") || !values["action"].ToString().Equals("showtag", StringComparison.OrdinalIgnoreCase))
                return null;

            //至此，我们可以确定请求是TagController.Showtag(string id)发起的，生成相应的URL并返回
            var tag = tagService.Find(c => c.Id==  (int)tagId);

            if (tag == null)
                throw new ArgumentNullException("tag");//找不到分类抛出异常

            string path;//生成URL
            if (string.IsNullOrEmpty(tag.SubTitle)) path = "tag/" + tag.Id + (page == null ? "" : ("." + (int)page)) + ".html";
            else path = tag.SubTitle.Trim() + (page == null ? "" : ("." + (int)page)) + ".html";

            return new VirtualPathData(this, path.ToLowerInvariant());
        }
    }
}