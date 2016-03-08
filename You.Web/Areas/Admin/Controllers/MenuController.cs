using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;

namespace You.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class MenuController : Common.Controller
    {
        // GET: Admin/Menu
        MenuService menuService;

        public MenuController() { menuService = new MenuService(); }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(Menu menu) 
        {
            if (menuService.Exist(m => m.Name == menu.Name)) return Fail("此菜单名已存在");
            menu = menuService.Add(menu);
            if (menu.Id>0)
              return Success();
            return Fail();
        }

        public ActionResult List()
        {
            var _menu = menuService.FindAll().Select(t => t.Name);
            return new Json(_menu);
        }

        public ActionResult All(ItemState state = 0, int limit = 10, int offset = 0, string search = "")
        {
            var total = 0;
            var _menu = menuService.FindPageList(offset / limit + 1, limit, out total, t => t.Name.Contains(search), Data.Types.OrderType.Asc, t => t.Order);
            return new Json(new { total = total, rows = _menu });
        }

        public ActionResult Delete(int id)
        {
            var _menu = menuService.Find(id);
            if (menuService.Delete(_menu)) return Success();
            else return Fail();
        }

        public ActionResult Recovery(int id)
        {
            var _menu = menuService.Find(id);
            if (menuService.Delete(_menu)) return Success();
            else return Fail();
        }
    }
}