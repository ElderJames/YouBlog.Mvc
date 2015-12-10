using System;
using System.Web.Mvc;

namespace You.Web.Areas.Admin
{

    /// <summary>
    /// 預設排除ModelState的 ID、CreateUser、CreateTime欄位驗證
    /// 並且使用ViewData[Exclude]回傳
    /// </summary>
    public class ModelStateExcludeAttribute : ActionFilterAttribute
    {
        public string[] Exclude { get; set; }

        /// <summary>
        /// 預設排除ModelState的 ID、CreateUser、CreateTime欄位驗證
        /// 並且使用ViewData[Exclude]回傳
        /// </summary>
        public ModelStateExcludeAttribute()
        {
            Exclude = new string[]
            {
                "ID",
                "CreateTime",
                "CreateUser"
            };
        }

        /// <summary>
        /// 可以自訂排除欄位，使用 , 分隔
        /// 並且使用ViewData[Exclude]回傳
        /// </summary>
        public ModelStateExcludeAttribute(string exclude)
        {
            if (exclude != null)
            {
                if (exclude.Contains(","))
                    Exclude = exclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                else
                    Exclude = new string[] { exclude };
            }

        }
        /// <summary>
        /// 進入 Action 的時候執行
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var item in Exclude)
            {
                //將排除欄位一一設定
                filterContext.Controller.ViewData.ModelState.Remove(item);
            }
            //設定完畢後將他利用 ViewData 傳到 Action 內
            filterContext.Controller.ViewData["Exclude"] = Exclude;
        }
    }

}