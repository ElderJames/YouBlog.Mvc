using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Service;
using You.Models;
using You.Data.Types;

namespace You.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        protected CategoryService categoryService;
        protected CommonModelService commonModelService;

        public CategoryController()
        {
            categoryService = GetService<Category>() as CategoryService;
            commonModelService = GetService<CommonModel>() as CommonModelService;
        }
        // GET: You/Category
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tree(CategoryType? type)
        {
            var _tree = categoryService.FindTree(type);
            return Json(_tree);
        }

        public ActionResult List(CategoryType? type)
        {
            try
            {
                List<Category> list = categoryService.FindList(type);
                return Json(list);
            }
            catch
            {
                return Json(new { });
            }
        }

        public ActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                category=categoryService.Add(category);
                if(category.CategoryID>0)
                    return Json(new { result = true, cat = category });
            }
            return Fail();
        }

        public ActionResult Edit(int id, FormCollection collection)
        {
            if (id > 0)
            {
                Category _category = categoryService.Find(id);
                TryUpdateModel(_category, "", collection.AllKeys, new string[] { "CategoryID", "ParentPath", "CreateTime", "State", "Type" });
                if (categoryService.Update(_category)) return Success();
            }
            return Fail();
        }

        //public ActionResult Delete(int id,int removeTo=0)
        //{
        //    var _commonModel = commonModelService.FindByCategory(id);
        //    foreach (var com in _commonModel)
        //    {
        //        com.CategoryID = removeTo;
        //        commonModelService.Update(com,false);
        //    }
        //    commonModelService.Save();
        //    if (categoryService.Delete(id)) return Json(true);
        //    else return Json(false);
        //}

        [HttpPost]
        public ActionResult Delete(int[] CategoryID)
        {
            foreach (var mid in CategoryID)
            {
                var _category = categoryService.Find(mid);
                if (_category != null)
                {
                    _category.State = ItemState.Deleted;
                    categoryService.Update(_category, false);
                }
                else return Fail("未找到此菜单项");
            }
            if (categoryService.Save() > 0) return Success();
            else return Fail();
        }
        [HttpPost]
        public ActionResult Recovery(int[] CategoryID)
        {
            foreach (var mid in CategoryID)
            {
                var _category = categoryService.Find(mid);
                if (_category != null)
                {
                    _category.State = ItemState.Nomal;
                    categoryService.Update(_category, false);
                }
            }
            if (categoryService.Save() == CategoryID.Count()) return Success();
            return Fail("未找到此菜单项");
        }
    }
}