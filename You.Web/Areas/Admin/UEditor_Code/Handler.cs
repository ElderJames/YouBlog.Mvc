using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace You.Web.Areas.Admin.UEditor_Code
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public abstract class Handler
    {
        public Handler(HttpContextBase context)
        {
            this.Request = context.Request;
            this.Server = context.Server;
            this.Response = context.Response;
        }

        public abstract JsonResult Process();
        public HttpRequestBase Request { get; private set; }
        public HttpResponseBase Response { get; private set; }
        public HttpServerUtilityBase Server { get; private set; }
    }
}