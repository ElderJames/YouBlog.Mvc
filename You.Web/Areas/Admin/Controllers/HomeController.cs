using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using You.Service;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace You.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController :Controller
    {
        //UserService userService = new UserService();
        //CommonModelService commonModelService = new CommonModelService();
        //CategoryService categoryService = new CategoryService();

        // GET: Admin/Home
        public ActionResult Index()
        {
            //var _user = userService.Find(1);
            //var _identity = userService.CreateIdentity(_user);
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //AuthenticationManager.SignIn(_identity);

            return View();
        }

        public ActionResult Overview()
        {
            var context = HttpContext.GetOwinContext();
            var Claim = context.Authentication.User.FindFirst("Menus");
            string MenuString = Claim.Value;
            return Content(MenuString);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("", "Account");
        }


        [HttpGet]
        public async Task<ActionResult> Translate(string query)
        {
            try
            {
                Regex regex = new Regex(@"^[A-Za-z0-9]+$");
                if (!string.IsNullOrEmpty(query) && !regex.IsMatch(query))
                {
                    var httpClient = new HttpClient();
                    string json = await httpClient.GetStringAsync("http://fanyi.youdao.com/openapi.do?type=data&doctype=json&version=1.1&relatedUrl=http%3A%2F%2Ffanyi.youdao.com%2F&keyfrom=fanyiweb&key=null&translate=on&q=" + Url.Encode(query));

                    JObject o = JObject.Parse(json);
                    JArray data = new JArray();
                    if ((int)o["errorCode"] == 0)
                    {
                        data = (JArray)o["translation"];
                        if (o["web"] != null)
                            foreach (var v in (JArray)o["web"])
                            {
                                if (v["key"].ToString() == query)
                                    foreach (var s in (JArray)v["value"])
                                    {
                                        data.Add(s);
                                    }
                            }
                        if (data.Count > 0) return Success(data);
                    }
                }
            }
            catch
            {
                return Fail();
            }
            return Fail();
        }
        #region 属性
        // private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        #endregion
    }
}