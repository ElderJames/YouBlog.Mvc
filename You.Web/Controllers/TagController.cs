using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Data.Types;
using You.Models;
using You.Service;
using PagedList;

namespace You.Web.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        public ActionResult Index(int id,int? page)
        {
            TagService tagService = GetService<Tag>() as TagService;
            var _tag=tagService.Find(id);
            ViewBag.Tag = _tag;
            if (_tag == null) return RedirectToAction("", "");
            CommonModelService commonModelService = GetService<CommonModel>() as  CommonModelService;
            int pageNumber = page ?? 1;
            var articles = commonModelService.FindList(0, cm => cm.State == CommonModelState.Normal && !cm.isPage && cm.Tag.FirstOrDefault(c => c.Name == _tag.Name) != null, OrderType.Desc, cm => cm.ReleaseDate);
            IPagedList<CommonModel> pageList = articles.ToPagedList(pageNumber, 1);
            return View(pageList);
        }
    }
}