using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using You.Service;

namespace You.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeController : Controller
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


        #region 属性
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }
        #endregion
    }
}