using System;
using System.Linq;
using System.Web.Mvc;
using You.Data.Security;
using You.Models;
using You.Service;

namespace You.Web.Areas.Admin.Controllers
{
    public class InstallController : Common.Controller
    {
        UserService userService = new UserService();
        CommonModelService commonModelService = new CommonModelService();
        CategoryService categoryService = new CategoryService();

        // GET: Admin/Default
        public ActionResult Index()
        {
            var _users = userService.FindAll();
            if (_users.Count() > 0) return RedirectToAction("", "",new { Area="Admin"});

            var _user = userService.Add(new User { UserID = 1, UserName = "admin", Email = "admin@admin.com", Password = Encryption.Sha256("admin123"), RealName = "", State = UserState.Super, LoginIP = Request.UserHostAddress, LoginTime = DateTime.Now, RegisterOn = DateTime.Now });

            categoryService.Add(new Category { CategoryID = 1, Name = "生活",SubTitle="live",ParentId = 0, CreateTime = DateTime.Now, Type = CategoryType.Article, State = ItemState.Nomal });
       
            commonModelService.Add(new CommonModel { Title = "世界，你好", SubTitle = "hello-worda", CategoryID = 1, Hits = 0, UserID = 1, Article = new Article { ArticleID = 1, Author = "杨舜杰", Content = "欢迎使用我做的网站，开发语言为C#，数据库是MSSQL", Intro = "欢迎使用我做的网站" }, Model = "Article", ReleaseDate = DateTime.Now, State = CommonModelState.UnDelete, Tags = "欢迎,使用" });
          


            return Success(_user);
        }
    }
}