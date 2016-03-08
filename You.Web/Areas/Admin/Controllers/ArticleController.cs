using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;
using System.Security.Claims;
using You.Core;
using Newtonsoft.Json.Linq;
using You.Data.Types;
using System.Text.RegularExpressions;
using System.IO;
using You.Web.Models;

namespace You.Web.Areas.Admin.Controllers
{

    [Authorize]
    public class ArticleController : Common.Controller
    {
        private ArticleService articleService;
        private CommonModelService commonModelService;
        private TagService tagService;
        public ArticleController() { articleService = new ArticleService(); commonModelService = new CommonModelService(); tagService = new TagService(); }

        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.Title = "编辑文章";
                ViewBag.Method = "Edit";
                var model = commonModelService.Find((int)id);
                return View("Edit", model);
            }
            ViewBag.Title = "添加文章";
            ViewBag.Method = "Add";
            return View("Edit");
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <returns>视图页面</returns>
        //public ActionResult p(int? id)
        //{
            
        //}

       
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CommonModel commonModel, string addtags, string uploadimg)
        {
            if (ModelState.IsValid)
            {
                //设置固定值
                int userID = Convert.ToInt32(HttpContext.GetOwinContext().Authentication.User.FindFirst(ClaimTypes.Sid).Value);
                string userName = HttpContext.GetOwinContext().Authentication.User.Identity.Name;
                commonModel.Hits = 0;
                commonModel.UserID = userID;
                commonModel.Model = "Article";
                commonModel.ReleaseDate = System.DateTime.Now;
                commonModel.State = CommonModelState.Normal;
                if (string.IsNullOrEmpty(commonModel.Article.Author)) commonModel.Article.Author = userName;
                commonModel.Model = "Article";
                commonModel = commonModelService.Add(commonModel);
                tagService.Update(commonModel, add: addtags);

                if (commonModel.ModelID > 0)
                {
                    bool noPic = false;
                    if (string.IsNullOrEmpty(commonModel.DefaultPicUrl)) noPic = true;

                    if (!string.IsNullOrEmpty(uploadimg))
                    {
                        string[] upload = uploadimg.Split(',');

                        //附件处理
                        AttachmentService _attachmentService = new AttachmentService();
                        //查询相关附件
                        var _attachments = _attachmentService.FindList(0, att => upload.Contains(att.FileParth), OrderType.Desc, att => att.UploadDate).ToList();//_attachmentService.FindList(null, user, string.Empty).ToList();
                        //遍历附件
                        foreach (var _att in _attachments)
                        {
                            var _filePath = Url.Content(_att.FileParth);
                            ////文章首页图片或内容中使用了该附件则更改ModelID为文章保存后的ModelID
                            if ((commonModel.DefaultPicUrl != null && commonModel.DefaultPicUrl.IndexOf(_filePath) >= 0) || commonModel.Article.Content.IndexOf(_filePath) > 0)
                            {
                                if (noPic) commonModel.DefaultPicUrl = _filePath;
                                noPic = false;
                                //var _att = _attachmentService.Find(att => att.FileParth == _filePath);
                                _att.Usage = AttachmentUsage.Aritcle;
                                _att.CommonModels.Add(commonModel);
                                _attachmentService.Update(_att);
                            }
                            //未使用改附件则删除附件和数据库中的记录
                            else if (_att.CommonModels.Count == 0)
                            {
                                System.IO.File.Delete(Server.MapPath(_att.FileParth));
                                _attachmentService.Delete(_att);
                            }
                        }
                    }

                    //string thumbPath = commonModel.DefaultPicUrl.Insert(commonModel.DefaultPicUrl.LastIndexOf('.'), "_s").Replace("image", "thumb");
                    //if (Core.Picture.CreateThumbnail(Server.MapPath(commonModel.DefaultPicUrl), Server.MapPath(thumbPath), 160, 120))
                    //{
                    //    if (!noPic)
                    //    {
                    //        System.IO.File.Delete(Server.MapPath(commonModel.DefaultPicUrl));
                    //        _attachmentService.Delete(_attachments.SingleOrDefault(a => a.FileParth == commonModel.DefaultPicUrl));
                    //    }
                    //    commonModel.DefaultPicUrl = thumbPath;
                    //    commonModelService.Update(commonModel);
                    //}
                    return Json(true);
                }
            }
            return View(commonModel);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public JsonResult Delete(int[] ModelID)
        {
            foreach (int mid in ModelID)
            {
                var _commonModel = commonModelService.Find(mid);
                if (_commonModel == null) return Fail("查无此文章");
                if (_commonModel.State == CommonModelState.UnDelete) return  Fail("《"+_commonModel.Title+"》是固定文章，不能删除！");
                _commonModel.State = CommonModelState.Deleted;
            }
            if (commonModelService.Save() > 0) return Success();
            else return Fail();
        }
        
        public ActionResult RealDelete(int[] ModelID)
        {
            foreach (int mid in ModelID)
            {
                var _commonModel = commonModelService.Find(mid);
                if (_commonModel == null) return Fail("查无此文章");
                if (_commonModel.State == CommonModelState.UnDelete) return Fail("《" + _commonModel.Title + "》是固定文章，不能删除！");
                commonModelService.Delete(_commonModel, false);
            }
            //附件处理
            AttachmentService _attachmentService = new AttachmentService();
            //查询相关附件
            var _attachments = _attachmentService.FindAll().ToList();//_attachmentService.FindList(null, user, string.Empty).ToList();
            if (_attachments == null) return View();
            //遍历附件
            foreach (var _att in _attachments)
            {
                var _filePath = Server.MapPath(_att.FileParth);
                //未使用改附件则删除附件和数据库中的记录
                if (_att.CommonModels.Count == 0)
                {
                    System.IO.File.Delete(_filePath);
                    _attachmentService.Delete(_att,false);
                }
            }
            if (commonModelService.Save() > 0) return Success();
            else return Fail();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        public JsonResult Recovery(int[] ModelID)
        {
            foreach (int mid in ModelID)
            {
                var _commonModel = commonModelService.Find(mid);
                if (_commonModel == null) return Fail("查无此文章");
                _commonModel.State = CommonModelState.Normal;
            }
            if (commonModelService.Save() > 0) return Success();
            else return Fail();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var model = commonModelService.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,string addtags, string removetags, string uploadimg)
        {
            string user = HttpContext.GetOwinContext().Authentication.User.Identity.Name;
            int _id = id;
            var _commonModel = commonModelService.Find(_id);
            TryUpdateModel(_commonModel, new string[] { "CategoryID", "Title", "DefaultPicUrl", "Tags" });
            TryUpdateModel(_commonModel.Article, "Article", new string[] { "Author", "DefaultPicUrl", "Source", "Intro", "Content" });

            if (ModelState.IsValid)
            {
                if (commonModelService.Update(_commonModel))
                {
                    tagService.Update(_commonModel, addtags, removetags);
                    List<Attachment> _attachments = new List<Attachment>() ;
                    AttachmentService _attachmentService = new AttachmentService();

                    //如果有新的上传附件
                    if (!string.IsNullOrEmpty(uploadimg))
                    {
                        //获取每个附件的地址
                        string[] upload = uploadimg.Split(',');
                        //找出刚上传的附件（为绑定ModelID，所以需要用地址来查找）以及在文章中有的附件
                        _attachments = _attachmentService.FindList(0, att => upload.Contains(att.FileParth) || att.CommonModels.FirstOrDefault(c=>c.ModelID==_commonModel.ModelID)!=null, OrderType.Desc, att => att.UploadDate).ToList();
                    }
                        //没有新上传的附件，就知查找文章中原有的附件（可能现在被删除了）
                    else _attachments = _attachmentService.FindList(0, att => att.CommonModels.FirstOrDefault(c=>c.ModelID==_commonModel.ModelID)!=null, OrderType.Desc, att => att.UploadDate).ToList();
                   
                    foreach (var _att in _attachments)
                    {
                        var _filePath = Url.Content(_att.FileParth);
                        //找出这个附件在不在这篇文章的目前（未更新）状态中
                        if ((_commonModel.DefaultPicUrl != null && _commonModel.DefaultPicUrl.IndexOf(_filePath) >= 0) || _commonModel.Article.Content.IndexOf(_filePath)> 0)
                        {
                            //在的话就将该附件与这篇文章关联
                            _att.CommonModels.Add(_commonModel);
                            _attachmentService.Update(_att,false);
                        }
                        else
                        {
                            //不在的话就移除
                            _att.CommonModels.Remove(_commonModel);
                            //如果该文件无关联文章
                            if(_att.CommonModels.Count<=0)
                            {
                                var localPath = Server.MapPath(_filePath);
                                if (Directory.Exists(Path.GetDirectoryName(localPath)))
                                {
                                    System.IO.File.Delete(localPath);
                                    _attachmentService.Delete(_att,false);
                                }
                            }else _attachmentService.Update(_att,false);
                        }
                    }
                    _attachmentService.Save();
                    return Success();
                }
            }
            return View(_commonModel);
        }

        //菜单
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }
        /// <summary>
        /// 全部文章
        /// </summary>
        /// <returns></returns>


        /// <summary>
        /// 文章列表Json【注意权限问题，普通人员是否可以访问？】
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="input">录入</param>
        /// <param name="category">栏目</param>
        /// <param name="fromDate">日期起</param>
        /// <param name="toDate">日期止</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录</param>
        /// <returns></returns>
        public ActionResult List(string title, string input, int? category, DateTime? fromDate, DateTime? toDate, string search, int offset, int limit = 20, CommonModelState state = CommonModelState.Normal)
        {
            if (category == null) category = 0;
            int _total;
            var _rows = commonModelService.FindPageList(out _total, offset / limit + 1, limit, "Article", title, null, null, Server.UrlDecode(search), (int)category, input, fromDate, toDate, OrderType.Desc, state)
                .Select(cm => new CommonModelViewModel()
                {
                    CategoryID = cm.CategoryID,
                    CategoryName = cm.Category.Name,
                    Title = cm.Title,
                    Hits = cm.Hits,
                    Inputer = cm.Inputer.UserName,
                    Intro = cm.Article.Intro,
                    DefaultPicUrl = cm.DefaultPicUrl,
                   // CommentCount = cm.com.ToList().Count,
                    Model = cm.Model,
                    ModelID = cm.ModelID,
                    ReleaseDate = cm.ReleaseDate,
                    State = cm.State
                });
            return Json(new { total = _total, rows = _rows.ToList() });
        }

        public ActionResult MyList()
        {
            return View();
        }

        /// <summary>
        /// 我的文章列表
        /// </summary>
        /// <param name="title"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult MyJsonList(string title, Nullable<DateTime> fromDate, Nullable<DateTime> toDate, int pageIndex = 1, int pageSize = 20)
        {
            int _total;
            var _rows = commonModelService.FindPageList(out _total, pageIndex, pageSize, "Article", title, null, null, null, 0, string.Empty, fromDate, toDate, 0).Select(
                cm => new CommonModelViewModel()
                {
                    CategoryID = cm.CategoryID,
                    CategoryName = cm.Category.Name,
                    DefaultPicUrl = cm.DefaultPicUrl,
                    Hits = cm.Hits,
                    Inputer = cm.Inputer.UserName,
                    Model = cm.Model,
                    ModelID = cm.ModelID,
                    ReleaseDate = cm.ReleaseDate,

                    Title = cm.Title
                });
            return Json(new { total = _total, rows = _rows.ToList() });
        }

        public ActionResult Translate(string query)
        {
            try
            {
                Regex regex = new Regex(@"^[A-Za-z0-9]+$");
                if (!string.IsNullOrEmpty(query) && !regex.IsMatch(query))
                {
                    HttpHelper http = new HttpHelper();
                    HttpItem item = new HttpItem { URL = "http://fanyi.youdao.com/openapi.do?type=data&doctype=json&version=1.1&relatedUrl=http%3A%2F%2Ffanyi.youdao.com%2F&keyfrom=fanyiweb&key=null&translate=on&q=" + Url.Encode(query), ContentType = "application/json" };
                    string json = http.GetHtml(item).Html;
                    JObject o = JObject.Parse(json);
                    JArray data = new JArray();
                    if ((int)o["errorCode"] == 0)
                    {
                        data = (JArray)o["translation"];
                        if (o["web"] != null)
                            foreach (var v in (JArray)o["web"])
                            {
                                if (v["key"].ToString() == query)
                                    foreach (var s in (JArray)v["value"])
                                    {
                                        data.Add(s);
                                    }
                            }
                        if (data.Count > 0) return Success(data);
                    }
                }
            }
            catch
            {
                return Fail();
            }
            return Fail();
        }
    }
}