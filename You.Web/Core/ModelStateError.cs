using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace You.Web
{
    public static class Error
    {
        /// <summary>
        /// 获取表单验证错误信息集合
        /// </summary>
        /// <param name="ModelState"></param>
        /// <returns></returns>
        public static object ModelState(ModelStateDictionary ModelState)
        {
            Dictionary<string, string> sb = new Dictionary<string, string>();
            //获取所有错误的Key
            List<string> Keys = ModelState.Keys.ToList();
            //获取每一个key对应的ModelStateDictionary
            foreach (var key in Keys)
            {
                var errors = ModelState[key].Errors.ToList();
                //将错误描述添加到sb中
                foreach (var error in errors)
                {
                    sb.Add(key, error.ErrorMessage);
                }
            }
            return sb;
        }
    }
}
