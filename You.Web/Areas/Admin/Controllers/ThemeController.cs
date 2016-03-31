using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;

namespace You.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ThemeController : Controller
    {
        ThemeService themeService = null;

        public ThemeController()
        {
            themeService = ThemeService.Current;
        }
        //
        // GET: /Admin/Theme/
        public ActionResult Index()
        {
            var ThemeList = themeService.GetThemes();
            TempData["Theme"] = ThemeList;
            ViewBag.ThemeList = ThemeList;
            Theme _theme = themeService.FindbyUser(Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            if (_theme == null) _theme = new Theme { Name = "Default" };
            return View(_theme);
        }

        public ActionResult Update(int id)
        {
            int uid = Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).Value);
            var _theme = themeService.FindbyUser(uid);
            var ThemeList = TempData["Theme"] as ICollection<Theme>;
            if (ThemeList == null) return Fail();
            var theme = ThemeList.FirstOrDefault(t => t.Id == id);

            if (_theme != null)
            {
                if (_theme.Id != id)
                {
                    _theme.Name = theme.Name;
                    _theme.Directory = theme.Directory;
                    _theme.SetTime = DateTime.Now;
                    if (themeService.Update(theme)) return Success();
                }
                else return Success("没有修改");
            }

            theme.Id = 0;
            theme.SetTime = DateTime.Now;
            theme.UserID = uid;
            theme = themeService.Add(theme);
            if (theme.Id > 0) return Success();

            return Fail();
        }
    }
}