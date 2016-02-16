using You.Data.Types;
using You.Models;
using You.Service;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using You.Data.Security;

namespace You.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class UserController : Common.Controller
    {
        UserService userService = new UserService();
   
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(UserState? state, int limit = 10, int offset = 0)
        {
            int total = 0;
            var _users = userService.FindPageList(offset + 1, limit, out total, u => state == null || (state != null && u.State == state), OrderType.Asc, u => u.UserID);
            //.Select(u => new { Id=u.Id,UserName = u.UserName, RealName = u.RealName, Email = u.Email, RoleID = u.RoleID, Role = u.Role.Name, DepartmentID = u.DepartmentID, Department = u.Department.Name, JobID = u.JobID, Job = u.Job.Name, RegisterOn = u.RegisterOn, State = u.StateToString(), LoginTime = u.LoginTime });
            return Json(new { total = total, rows = _users });
        }

        [HttpPost]
        public ActionResult Add(User user)
        {
            if (!ModelState.IsValid) return Fail(Error.ModelState(ModelState));
            if (userService.Exist(user.UserName, user.Email)) return Fail("用户已存在");
            user.State = UserState.NoUsed;
            user.RegisterOn = DateTime.Now;
            user.LoginTime = DateTime.Now;
            user.LoginIP = Request.UserHostName;
            user.Password = Encryption.Sha256(user.Password);
            user = userService.Add(user);
            if (user.UserID > 0)
            {
                return Success();
            }
            else return Fail("数据插入失败");
        }

        public ActionResult Delete(int[] Id)
        {
            foreach (int id in Id)
            {
                if (HttpContext.GetOwinContext().Authentication.User.FindFirst(ClaimTypes.Sid).ToString() == id.ToString()) return Fail("不能删除自己");
                var _user = userService.Find(id);
                if (_user != null)
                {
                    if (_user.State == UserState.Super) return Fail("不能删除固定管理员!");
                    _user.State = UserState.Deleted;
                    userService.Delete(_user, false);
                }
            }
            if (userService.Save() > 0) return Success();
            else return  Fail();

        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var _user = userService.Find(id);
            if (!string.IsNullOrEmpty(collection["Password"]))
            {
                _user.Password = Encryption.Sha256(collection["Password"]);
                _user.State = UserState.NoUsed;
            }
        

            if (TryUpdateModel(_user, "", collection.AllKeys, new string[] { "Id", "Password", "RegisterOn", "LoginTime" }))
            {
                userService.Save();
                return Success();
            }
            else return Fail(Error.ModelState(ModelState));
        }

        [HttpPost]
        public ActionResult Recovery(int[] Id)
        {
            foreach (var uid in Id)
            {
                var _user = userService.Find(uid);
                if (_user != null)
                {
                    _user.State = UserState.Normal;
                    userService.Update(_user, false);
                }
            }
            if (userService.Save() > 0) return Success();
            return Fail("未找到此用户");
        }

    }
}