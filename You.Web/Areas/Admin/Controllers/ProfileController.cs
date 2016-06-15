using System;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;
using Microsoft.Owin.Security;
using System.Security.Claims;
using You.Web.Areas.Admin.Models;
using You.Data.Security;

namespace You.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        UserService userService;
       
        public ProfileController() { userService = GetService<User>() as UserService; }
        // GET: Admin/Profile

        public ActionResult ChangePassword()
        {
            //User _user = userService.Find(1);
            //_user.Password = Core.Security.Encryption.Sha256("qwaszx");
            //userService.Update(_user);
            //ViewBag.Id = Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).Value);
            ViewBag.Id = User.UserID;
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(int id, Models.ChangePasswordViewModel change)
        {
            
            if (ModelState.IsValid)
                if (Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).Value) != id) ModelState.AddModelError("", "非法篡改");
                else
                {
                    User _user = userService.Find(id);
                    if (_user.Password !=Encryption.Sha256(change.OriginalPassword)) ModelState.AddModelError("", "原始密码错误");
                    else
                    {
                        _user.Password = Encryption.Sha256(change.Password);
                        if (userService.Update(_user)) return new Success();
                    }
                }
            return View(change);
        }

        public ActionResult Update()
        {
            User _user = userService.Find(Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).Value));
            ViewBag.Id = _user.UserID;
            UpdateProfileViewModel model = new UpdateProfileViewModel { RealName = _user.RealName };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(int id, UpdateProfileViewModel model)
        {
            if (ModelState.IsValid)
                if (Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).Value) != id) ModelState.AddModelError("", "非法篡改");
                else
                {
                    User _user = userService.Find(id);
                    _user.RealName = model.RealName;
                    if (userService.Update(_user)) return new Success();
                }
            return View(model);
        }
    }
}