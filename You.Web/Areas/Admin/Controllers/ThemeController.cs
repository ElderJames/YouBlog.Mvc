using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using You.Service;

namespace You.Web.Areas.Admin.Controllers
{
    public class ThemeController : Common.Controller
    {
        ThemeService themeService = null;

        public ThemeController()
        {
            themeService = new ThemeService(Request);
        }
        //
        // GET: /Admin/Theme/
        public ActionResult Index()
        {
            ViewBag.ThemeList = themeService.GetThemes();
            var _theme = themeService.FindbyUser(Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).ToString()));
            if (_theme == null) _theme.Name = "Default";
            return View(_theme);
        }

        public ActionResult Update(string name)
        {
            var _theme = themeService.FindbyUser(Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).ToString()));
            if (_theme != null)
            {

            }
            return Success();
        }
    }
}