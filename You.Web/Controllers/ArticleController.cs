using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;

namespace You.Web.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(int id)
        {
            CommonModelService commonModelService = GetService<CommonModel>() as CommonModelService;
            var model = commonModelService.Find(id);
            if (model == null) return RedirectToAction("", "");
            if (model.isPage) return View("Page",model);
            else return View("Article",model);
        }
    }
}