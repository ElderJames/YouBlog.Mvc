using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using You.Service;

namespace You.Web.Areas.Admin.UEditor_Code
{
    /// <summary>
    /// Crawler 的摘要说明
    /// </summary>
    public class CrawlerHandler : Handler
    {
        private string[] Sources;
        private Crawler[] Crawlers;
        public CrawlerHandler(HttpContextBase context) : base(context) { }

        public override JsonResult Process()
        {
            Sources = Request.Form.GetValues("source[]");
            if (Sources == null || Sources.Length == 0)
            {
                return new Json(new
                {
                    state = "参数错误：没有指定抓取源"
                });
            }
            Crawlers = Sources.Select(x => new Crawler(x, Server).Fetch()).ToArray();
            return new Json(new
            {
                state = "SUCCESS",
                list = Crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = Config.GetString("domain") + x.ServerUrl
                })
            });
        }
    }

    public class Crawler
    {
        public string SourceUrl { get; set; }
        public string ServerUrl { get; set; }
        public string State { get; set; }

        private HttpServerUtilityBase Server { get; set; }


        public Crawler(string sourceUrl, HttpServerUtilityBase server)
        {
            this.SourceUrl = sourceUrl;
            this.Server = server;
        }

        public Crawler Fetch()
        {

            var request = HttpWebRequest.Create(this.SourceUrl) as HttpWebRequest;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    State = "Url returns " + response.StatusCode + ", " + response.StatusDescription;
                    return this;
                }
                if (response.ContentType.IndexOf("image") == -1)
                {
                    State = "Url is not an image";
                    return this;
                }
                ServerUrl = PathFormatter.Format(Path.GetFileName(this.SourceUrl), Config.GetString("catcherPathFormat"));
                var savePath = Server.MapPath(ServerUrl);

                string bucket = Config.GetString("bucket");
                string Domain = Config.GetString("domain");
                //OssService ossService = new OssService(bucket, Domain);
                //if (ossService.Fetch(SourceUrl, ServerUrl))
                //{
                    if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                    }
                    try
                    {
                        var stream = response.GetResponseStream();
                        var reader = new BinaryReader(stream);
                        byte[] bytes;
                        using (var ms = new MemoryStream())
                        {
                            byte[] buffer = new byte[4096];
                            int count;
                            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, count);
                            }
                            bytes = ms.ToArray();
                        }
                        File.WriteAllBytes(savePath, bytes);
                        State = "SUCCESS";
                    }
                    catch (Exception e)
                    {
                        State = "抓取错误：" + e.Message;
                    }
                //}
                return this;
            }
        }
    }
}