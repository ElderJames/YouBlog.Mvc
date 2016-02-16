using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;

namespace You.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class TagController : Common.Controller
    {
        // GET: Admin/Tag
        TagService tagService;

        public TagController() { tagService = new TagService(); }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var _tag = tagService.FindAll().Select(t => t.Name );
            return Json(_tag);
        }

        public ActionResult All(ItemState state=0,int limit=10,int offset=0,string search="")
        {
            var total=0;
            var _tag = tagService.FindPageList(offset / limit + 1, limit, out total, t => t.Name.Contains(search) && t.State == state, Data.Types.OrderType.Desc, t => t.CreateTime);
            return Json(new { total = total, rows = _tag });
        }

        public ActionResult Delete(int id)
        {
            var _tag = tagService.Find(id);
            _tag.State = ItemState.Deleted;
            if (tagService.Update(_tag)) return Success();
            else return Fail();
        }

        public ActionResult Recovery(int id)
        {
            var _tag = tagService.Find(id);
            _tag.State = ItemState.Nomal;
            if (tagService.Update(_tag)) return Success();
            else return Fail();
        }

        public ActionResult search(string key)
        {
            var _tags = tagService.Find(key);
            return Json(_tags);
        }
    }
}