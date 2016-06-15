using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace You.Web
{
    public class Fail : System.Web.Mvc.JsonResult
    {
        public Fail(object data=null)
        {
            Data = data;
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
        ///<summary>
        ///对操作结果进行处理
        ///</summary>
        ///<paramname="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            httpContext.Response.ContentType = "application/json; charset=utf-8";
            // 返回客户端定义的回调函数
            httpContext.Response.Write(JsonConvert.SerializeObject(new { result = false, error = Data }));//Data 是服务器返回的数据     
        }
    }
    public class Success : System.Web.Mvc.JsonResult
    {
        public Success(object data = null)
        {
            Data = data;
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
        ///<summary>
        ///对操作结果进行处理
        ///</summary>
        ///<paramname="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/json; charset=utf-8";
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            string jsonpCallback = context.HttpContext.Request["callback"], json = JsonConvert.SerializeObject(new { result = true, data = Data }, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Converters = new List<JsonConverter> { timeConverter } });
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(json);
            }
            else
            {
                Response.AddHeader("Content-Type", "application/javascript");
                Response.Write(String.Format("{0}({1});", jsonpCallback, json));
            }
            Response.End();
        }
    }
    public class Json : System.Web.Mvc.JsonResult
    {
        public Json(object data=null)
        {
            Data = data;
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
        ///<summary>
        ///对操作结果进行处理
        ///</summary>
        ///<paramname="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/json; charset=utf-8";
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            string jsonpCallback =context.HttpContext.Request["callback"],json = JsonConvert.SerializeObject(Data, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, Converters = new List<JsonConverter> { timeConverter } });

            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(json);
            }
            else
            {
                Response.AddHeader("Content-Type", "application/javascript");
                Response.Write(String.Format("{0}({1});", jsonpCallback, json));
            }
            Response.End();
        }
    }
}