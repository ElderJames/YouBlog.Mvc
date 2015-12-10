using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using You.Web.Models;
using System.Web.Mvc;

namespace You.Web.Areas.Admin.UEditor_Code
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContextBase context) : base(context) { }

        public override JsonResult Process()
        {
            return new Json(Config.Items);
        }
    }
}