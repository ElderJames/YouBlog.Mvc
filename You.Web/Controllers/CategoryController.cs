using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Data.Types;
using You.Models;
using You.Service;

namespace You.Web.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(int? id,int? page)
        {
            if (id == null) return RedirectToAction("", "");
            CategoryService categoryService = new CategoryService();
            var _category = categoryService.Find((int)id);
            if (_category==null) return RedirectToAction("", "");

            ViewBag.Category = _category;

            CommonModelService commonModelService = new CommonModelService();
            int pageNumber = page ?? 1;
            var article = commonModelService.FindList(0, cm => cm.State == CommonModelState.Normal && !cm.isPage && cm.CategoryID == id, OrderType.Desc, cm => cm.ReleaseDate);
            IPagedList<CommonModel> pageList = article.ToPagedList(pageNumber, 1);
            return View(pageList);
        }
    }
}