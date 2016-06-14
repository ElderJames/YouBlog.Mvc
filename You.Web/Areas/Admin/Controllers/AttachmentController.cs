using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.IO;
using You.Service;
using You.Models;
using You.Web.Areas.Admin.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using You.Data.Types;
using System.Text;
using You.Web.Areas.Admin.UEditor_Code;

namespace You.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 附件控制器
    /// <remarks>
    /// 创建：2014.03.05
    /// </remarks>
    /// </summary>
    [Authorize]
    public class AttachmentController : Controller
    {
        private AttachmentService attachmentService;
        public UploadConfig UploadConfig { get; private set; }
        public UploadResult Result { get; private set; }

        public AttachmentController()
        {
            attachmentService = new AttachmentService();

            UploadConfig = new UploadConfig()
            {
                AllowExtensions = Config.GetStringList("imageAllowFiles"),
                PathFormat = Config.GetString("imagePathFormat"),
                SizeLimit = Config.GetInt("imageMaxSize"),
                UploadFieldName = Config.GetString("imageFieldName")
            };
            Result = new UploadResult() { State = UploadState.Unknown };
        }

        public ActionResult Index()
        {
            ////附件处理
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
                    _attachmentService.Delete(_att);
                }
            }
            return View();
        }

        //public ActionResult Token(string file="",string type="")
        //{
        //    string bucket = Config.GetString("bucket");
        //    string Domain = Config.GetString("domain");
        //    string savePath = PathFormatter.Format(file, Config.GetString("filePathFormat"));
        //    switch (type)
        //    {
        //        case "img": savePath = PathFormatter.Format(file, Config.GetString("imagePathFormat")); break;
        //        case "file": savePath = PathFormatter.Format(file, Config.GetString("filePathFormat")); break;
        //        case "scrawl": savePath = PathFormatter.Format(file, Config.GetString("scrawlPathFormat")); break;
        //        case "video":savePath= savePath = PathFormatter.Format(file, Config.GetString("videoPathFormat")); break;
        //        default:  break;
        //    }
          
           
        //    OssService ossService = new OssService(bucket, Domain);
        //    string token;
        //    token = ossService.GetToken(savePath);
        //    return Content(token);
        //}

        public ActionResult QiniuUploadCallBack()
        {
            string ret = Request.QueryString["upload_ret"];
            byte[] bytes = Convert.FromBase64String(ret);
            dynamic data = System.Web.Helpers.Json.Decode(Encoding.UTF8.GetString(bytes));
            //var localPath = Server.MapPath(savePath);
            return Json(new { original = data["name"], w = data["w"], h = data["h"], url = data["key"], state = "SUCCESS" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="originalPicture">原图地址</param>
        /// <returns>缩略图地址。生成失败返回null</returns>
        public ActionResult CreateThumbnail(string originalPicture)
        {
            //原图为缩略图直接返回其地址
            if (originalPicture.IndexOf("_s") > 0) return Json(originalPicture);
            //缩略图地址
            string _thumbnail = originalPicture.Insert(originalPicture.LastIndexOf('.'), "_s");
            //创建缩略图
            if (Core.Picture.CreateThumbnail(Server.MapPath(originalPicture), Server.MapPath(_thumbnail), 160, 120))
            {
                //记录保存在数据库中
                attachmentService.Add(new Attachment() { Extension = _thumbnail.Substring(_thumbnail.LastIndexOf('.') + 1), FileParth = "~" + _thumbnail, Inputer = ControllerContext.HttpContext.GetOwinContext().Authentication.User.Identity.Name, Type = AttachmentType.Image, UploadDate = DateTime.Now });
                return Json(_thumbnail);
            }
            return Json(null);
        }
        /// <summary>
        /// 上传附件(使用百度UEditor插件的上传功能，所以这里会调用UEditor_Coder目录下的Handler)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FileUpload()
        {
            Handler Action = null;
            var context = ControllerContext.HttpContext;
            string action = context.Request["action"].ToString();
            switch (action)
            {
                case "config":
                    Action = new ConfigHandler(context);
                    break;
                case "uploadimage":
                    Action = new UploadHandler(context, AttachmentType.Image, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("imageAllowFiles"),
                        PathFormat = Config.GetString("imagePathFormat"),
                        SizeLimit = Config.GetInt("imageMaxSize"),
                        UploadFieldName = Config.GetString("imageFieldName")
                    });
                    break;
                case "uploadscrawl":
                    Action = new UploadHandler(context, AttachmentType.Scrawl, new UploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = Config.GetString("scrawlPathFormat"),
                        SizeLimit = Config.GetInt("scrawlMaxSize"),
                        UploadFieldName = Config.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    });
                    break;
                case "uploadvideo":
                    Action = new UploadHandler(context, AttachmentType.Media, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("videoAllowFiles"),
                        PathFormat = Config.GetString("videoPathFormat"),
                        SizeLimit = Config.GetInt("videoMaxSize"),
                        UploadFieldName = Config.GetString("videoFieldName")
                    });
                    break;
                case "uploadfile":
                    Action = new UploadHandler(context, AttachmentType.File, new UploadConfig()
                    {
                        AllowExtensions = Config.GetStringList("fileAllowFiles"),
                        PathFormat = Config.GetString("filePathFormat"),
                        SizeLimit = Config.GetInt("fileMaxSize"),
                        UploadFieldName = Config.GetString("fileFieldName")
                    });
                    break;
                case "listimage":
                    Action = new ListFileManager(context, Config.GetString("imageManagerListPath"), Config.GetStringList("imageManagerAllowFiles"));
                    break;
                case "listfile":
                    Action = new ListFileManager(context, Config.GetString("fileManagerListPath"), Config.GetStringList("fileManagerAllowFiles"));
                    break;
                case "catchimage":
                    Action = new CrawlerHandler(context);
                    break;
                default:
                    Action = new NotSupportedHandler(context);
                    break;
            }
            return Action.Process();
        }

        public ActionResult Delete(int[] Id)
        {
            foreach (int id in Id)
            {
                var _att =attachmentService.Find(id);
                if (_att != null)
                {
                   var _filePath = Server.MapPath(_att.FileParth);
                    System.IO.File.Delete(_filePath);
                    attachmentService.Delete(_att, false);
                }
            }
            if (attachmentService.Save() > 0) return Success();
            else return Fail();
        }

        //public ActionResult Upload()
        //{
        //    //保存路径
        //    string _savePath;
        //    //文件路径
        //    string _fileParth = "~/Content/Upload/";

        //    //上传的文件
        //    HttpPostedFileBase _postFile = Request.Files["imgFile"];
        //    _fileParth+=  DateTime.Now.ToString("yyyy-MM") + "/";
        //}


        //public ActionResult Upload()
        //{
        //    var _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as You.Models.Config.UploadConfig;
        //    //文件最大限制
        //    int _maxSize = _uploadConfig.MaxSize;
        //    //保存路径
        //    string _savePath;
        //    //文件路径
        //    string _fileParth = "~/" + _uploadConfig.Path + "/";
        //    //文件名
        //    string _fileName;
        //    //扩展名
        //    string _fileExt;
        //    //文件类型
        //    string _dirName;
        //    //允许上传的类型
        //    Hashtable extTable = new Hashtable();
        //    extTable.Add("image", _uploadConfig.ImageExt);
        //    extTable.Add("flash", _uploadConfig.FileExt);
        //    extTable.Add("media", _uploadConfig.MediaExt);
        //    extTable.Add("file", _uploadConfig.FileExt);
        //    //上传的文件
        //    HttpPostedFileBase _postFile = Request.Files["imgFile"];
        //    if (_postFile == null) return Json(new { error = '1', message = "请选择文件" });
        //    _fileName = _postFile.FileName;
        //    _fileExt = Path.GetExtension(_fileName).ToLower();
        //    //文件类型
        //    _dirName = Request.QueryString["dir"];
        //    if (string.IsNullOrEmpty(_dirName))
        //    {
        //        _dirName = "image";
        //    }
        //    if (!extTable.ContainsKey(_dirName)) return Json(new { error = 1, message = "目录类型不存在" });
        //    //文件大小
        //    if (_postFile.InputStream == null || _postFile.InputStream.Length > _maxSize) return Json(new { error = 1, message = "文件大小超过限制" });
        //    //检查扩展名
        //    if (string.IsNullOrEmpty(_fileExt) || Array.IndexOf(((string)extTable[_dirName]).Split(','), _fileExt.Substring(1).ToLower()) == -1) return Json(new { error = 1, message = "不允许上传此类型的文件。 \n只允许" + ((String)extTable[_dirName]) + "格式。" });
        //    _fileParth += _dirName + "/" + DateTime.Now.ToString("yyyy-MM") + "/";
        //    _savePath = Server.MapPath(_fileParth);
        //    //检查上传目录
        //    if (!Directory.Exists(_savePath)) Directory.CreateDirectory(_savePath);
        //    string _newFileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + _fileExt;
        //     _savePath += _newFileName;
        //     _fileParth += _newFileName;
        //    //保存文件
        //    _postFile.SaveAs(_savePath);
        //    //保存数据库记录
        //    attachmentService.Add(new Attachment() { Extension = _fileExt.Substring(1), FileParth = _fileParth, Owner = User.Identity.Name, UploadDate = DateTime.Now, Type = _dirName });
        //    return Json(new { error = 0, url = Url.Content(_fileParth) });
        //}

        //public ActionResult Upload()
        //{
        //    byte[] uploadFileBytes = null;
        //    string uploadFileName = null;

        //    if (UploadConfig.Base64)
        //    {
        //        uploadFileName = UploadConfig.Base64Filename;
        //        uploadFileBytes = Convert.FromBase64String(Request[UploadConfig.UploadFieldName]);
        //    }
        //    else
        //    {
        //        var file = Request.Files[UploadConfig.UploadFieldName];
        //        uploadFileName = file.FileName;

        //        if (!CheckFileType(uploadFileName))
        //        {
        //            Result.State = UploadState.TypeNotAllow;
        //            return WriteResult();
        //        }
        //        if (!CheckFileSize(file.ContentLength))
        //        {
        //            Result.State = UploadState.SizeLimitExceed;
        //            return WriteResult();
        //        }

        //        uploadFileBytes = new byte[file.ContentLength];
        //        try
        //        {
        //            file.InputStream.Read(uploadFileBytes, 0, file.ContentLength);
        //        }
        //        catch (Exception)
        //        {
        //            Result.State = UploadState.NetworkError;
        //            return WriteResult();
        //        }
        //    }

        //    Result.OriginFileName = uploadFileName;

        //    var savePath = PathFormatter.Format(uploadFileName, UploadConfig.PathFormat);
        //    var localPath = Server.MapPath(savePath);
        //    try
        //    {
        //        if (!Directory.Exists(Path.GetDirectoryName(localPath)))
        //        {
        //            Directory.CreateDirectory(Path.GetDirectoryName(localPath));
        //        }
        //        System.IO.File.WriteAllBytes(localPath, uploadFileBytes);
        //        Result.Url = savePath;
        //        Result.State = UploadState.Success;
        //    }
        //    catch (Exception e)
        //    {
        //        Result.State = UploadState.FileAccessError;
        //        Result.ErrorMessage = e.Message;
        //    }
        //    return WriteResult();
        //}




        /// <summary>
        /// 附件管理列表
        /// </summary>
        /// <param name="id">公共模型ID</param>
        /// <param name="dir">目录（类型）</param>
        /// <returns></returns>
        //public ActionResult FileManagerJson(int? id ,string dir)
        //{
        //    AttachmentManagerViewModel _attachmentViewModel;
        //    IQueryable<Attachment> _attachments;
        //    //id为null，表示是公共模型id为null，此时查询数据库中没有跟模型对应起来的附件列表（以上传，但上传的文章……还未保存）
        //    if (id == null) _attachments = attachmentService.FindList(null, User.Identity.Name, dir);
        //    //id不为null，返回指定模型id和id为null（新上传的）附件了列表
        //    else _attachments = attachmentService.FindList((int)id, User.Identity.Name, dir, true);
        //    //循环构造AttachmentManagerViewModel
        //    var _attachmentList = new List<AttachmentManagerViewModel>(_attachments.Count());
        //    foreach(var _attachment in _attachments)
        //    {
        //        _attachmentViewModel = new AttachmentManagerViewModel() { datetime = _attachment.UploadDate.ToString("yyyy-MM-dd HH:mm:ss"), filetype = _attachment.Extension, has_file = false, is_dir = false, is_photo = _attachment.Type.ToLower() == "image" ? true : false, filename = Url.Content(_attachment.FileParth) };
        //        FileInfo _fileInfo = new FileInfo(Server.MapPath(_attachment.FileParth));
        //        _attachmentViewModel.filesize = (int)_fileInfo.Length;
        //        _attachmentList.Add(_attachmentViewModel);
        //    }
        //    return Json(new { moveup_dir_path = "", current_dir_path = "", current_url = "", total_count = _attachmentList.Count, file_list = _attachmentList },JsonRequestBehavior.AllowGet);
        //}

        private JsonResult WriteResult()
        {
            return Json(new
            {
                state = GetStateMessage(Result.State),
                url = Result.Url,
                title = Result.OriginFileName,
                original = Result.OriginFileName,
                error = Result.ErrorMessage
            });
        }
        private string GetStateMessage(UploadState state)
        {
            switch (state)
            {
                case UploadState.Success:
                    return "SUCCESS";
                case UploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }

        private bool CheckFileType(string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            return UploadConfig.AllowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
        }

        private bool CheckFileSize(int size)
        {
            return size < UploadConfig.SizeLimit;
        }

        public ActionResult List(int limit, int offset, ItemState state, AttachmentType type)
        {
            int total = 0;
            var _att = attachmentService.FindPageList(offset / limit + 1, limit, out total, att => att.State == state && att.Type == type, OrderType.Desc, att => att.UploadDate)
              .Select(att => new AttachmentViewModel
              {
                  AttachmentID = att.AttachmentID,
                  Extension = att.Extension,
                  FileParth = att.FileParth,
                  Inputer = att.Inputer,
                  OriginName = att.OriginName,
                  State = att.State,
                  Type = att.Type,
                  UploadDate = att.UploadDate,
                  ArticleCount = att.CommonModels.Count
              }).ToList();
            return Json(new { total = total, rows = _att });
        }
    }
}