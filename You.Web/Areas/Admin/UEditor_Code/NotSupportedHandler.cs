using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace You.Web.Areas.Admin.UEditor_Code
{
    /// <summary>
    /// NotSupportedHandler 的摘要说明
    /// </summary>
    public class NotSupportedHandler : Handler
    {
        public NotSupportedHandler(HttpContextBase context)
            : base(context)
        {
        }

        public override JsonResult Process()
        {
            return new Json(new
            {
                state = "action 参数为空或者 action 不被支持。"
            });
        }
    }
}