using System.Collections.Generic;
using System.Web.Mvc;
using You.Service;
using You.Models;
using You.Data.Types;
using System.Linq;
using PagedList;

namespace You.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            CommonModelService commonModelService = GetService<CommonModel>() as CommonModelService;
            int pageNumber = page ?? 1;

            var article = commonModelService.FindList(0, cm => cm.State == CommonModelState.Normal&&!cm.isPage, OrderType.Desc, cm => cm.ReleaseDate);
            IPagedList<CommonModel> pageList = article.ToPagedList(pageNumber, 1);
            return View(pageList);
        }

        public ActionResult Sidebar()
        {
            CategoryService categoryService = GetService<Category>() as CategoryService;
            var list = categoryService.FindList(10, cat => cat.State == ItemState.Nomal, OrderType.Desc, cat => cat.Order).ToList();
            return PartialView("Sidebar", list);
        }

        public ActionResult Menu()
        {
            MenuService menuService = GetService<Menu>() as MenuService;
            var _menu = menuService.FindAll().OrderBy(m => m.Order).ToList();
            return PartialView("Menu", _menu);
        }
    }
}