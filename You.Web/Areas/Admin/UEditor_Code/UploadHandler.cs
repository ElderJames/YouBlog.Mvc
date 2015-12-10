using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Service;
using You.Models;

namespace You.Web.Areas.Admin.UEditor_Code
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : Handler
    {
        public AttachmentService attachmentService = new AttachmentService();
        public UploadConfig UploadConfig { get; private set; }
        public UploadResult Result { get; private set; }
        public HttpContextBase Context { get; set; }

        public AttachmentType FileType;

        public UploadHandler(HttpContextBase context,AttachmentType FileType, UploadConfig config)
            : base(context)
        {
            this.UploadConfig = config;
            this.FileType = FileType;
            this.Context = context;
            this.Result = new UploadResult() { State = UploadState.Unknown };
        }

        public override JsonResult Process()
        {
            byte[] uploadFileBytes = null;
            string uploadFileName = null;

            if (UploadConfig.Base64)
            {
                uploadFileName = UploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(Request[UploadConfig.UploadFieldName]);
            }
            else
            {
                var file = Request.Files[UploadConfig.UploadFieldName];
                uploadFileName = file.FileName;

                if (!CheckFileType(uploadFileName))
                {
                    Result.State = UploadState.TypeNotAllow;
                    return WriteResult();
                }
                if (!CheckFileSize(file.ContentLength))
                {
                    Result.State = UploadState.SizeLimitExceed;
                    return WriteResult();
                }

                uploadFileBytes = new byte[file.ContentLength];
                try
                {
                    file.InputStream.Read(uploadFileBytes, 0, file.ContentLength);
                }
                catch (Exception)
                {
                    Result.State = UploadState.NetworkError;
                    return WriteResult();
                }
            }

            Result.OriginFileName = uploadFileName;

            var savePath = PathFormatter.Format(uploadFileName, UploadConfig.PathFormat);
            var localPath = Server.MapPath(savePath);
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                File.WriteAllBytes(localPath, uploadFileBytes);
                Result.Url = savePath;
                Result.State = UploadState.Success;
                attachmentService.Add(new Attachment() { OriginName = Result.OriginFileName, Extension = Path.GetExtension(uploadFileName).ToLower(), FileParth = Result.Url, Inputer = Context.GetOwinContext().Authentication.User.Identity.Name, UploadDate = DateTime.Now, Type = FileType });
                return WriteResult();
            }
            catch (Exception e)
            {
                Result.State = UploadState.FileAccessError;
                Result.ErrorMessage = e.Message;
                return WriteResult();
            }
            
        }

        private JsonResult WriteResult()
        {
            return new Json(new
            {
                state = GetStateMessage(Result.State),
                success = Result.State == UploadState.Success ? 1 : 0,
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
    }

    public class UploadConfig
    {
        /// <summary>
        /// 文件命名规则
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// 上传表单域名称
        /// </summary>
        public string UploadFieldName { get; set; }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        public int SizeLimit { get; set; }

        /// <summary>
        /// 上传允许的文件格式
        /// </summary>
        public string[] AllowExtensions { get; set; }

        /// <summary>
        /// 文件是否以 Base64 的形式上传
        /// </summary>
        public bool Base64 { get; set; }

        /// <summary>
        /// Base64 字符串所表示的文件名
        /// </summary>
        public string Base64Filename { get; set; }
    }

    public class UploadResult
    {
        public UploadState State { get; set; }
        public string Url { get; set; }
        public string OriginFileName { get; set; }

        public string ErrorMessage { get; set; }
    }

    public enum UploadState
    {
        Success = 0,
        SizeLimitExceed = -1,
        TypeNotAllow = -2,
        FileAccessError = -3,
        NetworkError = -4,
        Unknown = 1,
    }

}