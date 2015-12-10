using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Service;

namespace You.Web.Areas.Admin.UEditor_Code
{
    /// <summary>
    /// FileManager 的摘要说明
    /// </summary>
    public class ListFileManager : Handler
    {
        enum ResultState
        {
            Success,
            InvalidParam,
            AuthorizError,
            IOError,
            PathNotFound
        }

        private int Start;
        private int Size;
        private int Total;
        private ResultState State;
        private String PathToList;
        private String[] FileList;
        private String[] SearchExtensions;

        public ListFileManager(HttpContextBase context, string pathToList, string[] searchExtensions)
            : base(context)
        {
            this.SearchExtensions = searchExtensions.Select(x => x.ToLower()).ToArray();
            this.PathToList = pathToList;
        }

        public override JsonResult Process()
        {
            try
            {
                Start = String.IsNullOrEmpty(Request["start"]) ? 0 : Convert.ToInt32(Request["start"]);
                Size = String.IsNullOrEmpty(Request["size"]) ? Config.GetInt("imageManagerListSize") : Convert.ToInt32(Request["size"]);
            }
            catch (FormatException)
            {
                State = ResultState.InvalidParam;
                return WriteResult();
            }

            string bucket = Config.GetString("bucket");
            string Domain = Config.GetString("domain");
            //OssService ossService = new OssService(bucket, Domain);
            var buildingList = new List<String>();
         
            try
            {
               // buildingList.AddRange(ossService.List(100, PathToList)
               //.Where(x => SearchExtensions.Contains(Path.GetExtension(x.Key).ToLower()))
               //.Select(x => Domain + x.Key));
                var localPath = Server.MapPath(PathToList);
                buildingList.AddRange(Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
                    .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                    .Select(x => PathToList + x.Substring(localPath.Length).Replace("\\", "/")));
                Total = buildingList.Count;
                FileList = buildingList.OrderBy(x => x).Skip(Start).Take(Size).ToArray();
                return WriteResult();
            }
            catch (UnauthorizedAccessException)
            {
                State = ResultState.AuthorizError;
                return WriteResult();
            }
            catch (DirectoryNotFoundException)
            {
                State = ResultState.PathNotFound;
                return WriteResult();
            }
            catch (IOException)
            {
                State = ResultState.IOError;
                return WriteResult();
            }
        }

        private JsonResult WriteResult()
        {
            return new Json(new
            {
                state = GetStateString(),
                list = FileList == null ? null : FileList.Select(x => new { url = x }),
                start = Start,
                size = Size,
                total = Total
            });
        }

        private string GetStateString()
        {
            switch (State)
            {
                case ResultState.Success:
                    return "SUCCESS";
                case ResultState.InvalidParam:
                    return "参数不正确";
                case ResultState.PathNotFound:
                    return "路径不存在";
                case ResultState.AuthorizError:
                    return "文件系统权限不足";
                case ResultState.IOError:
                    return "文件系统读取错误";
            }
            return "未知错误";
        }
    }
}