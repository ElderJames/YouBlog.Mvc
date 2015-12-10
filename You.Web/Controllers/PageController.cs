using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace You.Web.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult Index(int id)
        {

            return View();
        }
    }
}