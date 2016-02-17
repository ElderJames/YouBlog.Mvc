using System.Web.Mvc;
using You.Service;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using You.Data.Security;
using You.Web.Areas.Admin.Models;

namespace You.Web.Areas.Admin.Controllers
{

    public class AccountController : Common.Controller
    {
        UserService userService = new UserService();
        // GET: Admin/Acount
        public ActionResult Index(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(returnUrl)) return Redirect("~/Admin/Home");
                else return Redirect(returnUrl);
            }
                TempData["returnUrl"] = returnUrl;
            return View();
        }

       
        [HttpPost]
        public ActionResult Index(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var _user = userService.Find(loginViewModel.Acount);
                if (_user == null) ModelState.AddModelError("","密码不正确或用户名不存在");
                else if (_user.Password == Encryption.Sha256(loginViewModel.Password))
                {
                    _user.LoginTime = System.DateTime.Now;
                    _user.LoginIP = Request.UserHostAddress;
                    userService.Update(_user);
                    
                    var _identity = userService.CreateIdentity(_user);
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = loginViewModel.RememberMe }, _identity);
                    if (TempData["returnUrl"] != null) return Redirect(TempData["returnUrl"].ToString());
                    else return Redirect("~/Admin/Home");
                }
                else ModelState.AddModelError("", "密码不正确或用户名不存在");
            }
            return View(loginViewModel);
        }

        //public ActionResult Register(string returnUrl)
        //{
        //    TempData["returnUrl"] = returnUrl;
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Register(RegisterViewModel register)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (userService.Exist(register.UserName,register.Email)) ModelState.AddModelError("", "该用户名或邮箱已注册");
        //        else
        //        {
        //            User _user = new User
        //            {
        //                UserName=register.UserName,
        //                Email = register.Email,
        //                RealName = register.RealName,
        //                Password = Encryption.Sha256(register.Password),
        //                //用户状态问题
        //                State = UserState.NoAdminValidation,
        //                RegisterOn = System.DateTime.Now,
        //                LoginTime = System.DateTime.Now,
        //                LoginIP = Request.UserHostAddress
        //            };
        //            _user = userService.Add(_user);
        //            if (_user.UserID > 0)
        //            {
        //                var _identity = userService.CreateIdentity(_user);
        //                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //                AuthenticationManager.SignIn(_identity);
        //                //Session.Add("UserId", _user.Id);
        //                //Session.Add("Account", register.Email);
        //                //Session.Add("UserName", string.IsNullOrEmpty(_user.RealName)?_user.UserName:_user.RealName);
        //                //Session.Add("Roles", _user.Role.Menus);
        //                if (TempData["returnUrl"] != null) return Redirect(TempData["returnUrl"].ToString());
        //                else return RedirectToAction("Index", "Home");
        //                   // return Content("注册成功！");
        //            }
        //            else { ModelState.AddModelError("", "注册失败！"); }
        //        }
        //    }
        //    return View(register);
        //}

    }
}