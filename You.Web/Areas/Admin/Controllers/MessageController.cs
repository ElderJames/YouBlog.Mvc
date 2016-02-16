using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using You.Models;
using You.Service;
using You.Data.Types;
using System.IO;
using System.Text;
using You.Core;

namespace You.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class MessageController : Common.Controller
    {
        MessageService messageService = new MessageService();

        // GET: Admin/Message
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(MessageState? state, int offset, int limit = 20)
        {
            int total = 0;
            var list = messageService.FindPageList(offset / limit + 1, limit, out total, msg => msg.State == state, OrderType.Desc, msg => msg.CreateTime);
            return new Json(new { total = total, rows = list });
        }

      
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(int id,string ReplyContent)
        {
            var msg=messageService.Find(id);
            if (SendEmail(msg.Name, "回复："+msg.Content.Substring(0,msg.Content.Length> 20?20:msg.Content.Length), ReplyContent, msg.Email))
            {
                msg.ReplyContent = ReplyContent;
                msg.ReplyTime = DateTime.Now;
                msg.State = MessageState.Replyed;
                messageService.Update(msg);
                return Success();
            }
            return Fail();
        }

        public ActionResult Delete(int[] Id)
        {
            foreach (int id in Id)
            {
                var _msg =messageService.Find(id);
                if (_msg != null)
                {
                    _msg.State =MessageState.Deleted;
                    messageService.Update(_msg, false);
                }
            }
            if (messageService.Save() > 0) return Success();
            else return Fail();
        }

        public ActionResult Recovery(int[] Id)
        {
            foreach (int id in Id)
            {
                var _msg = messageService.Find(id);
                if (_msg != null)
                {
                    _msg.State = string.IsNullOrEmpty(_msg.ReplyContent)? MessageState.noReply:MessageState.Replyed;
                    messageService.Update(_msg, false);
                }
            }
            if (messageService.Save() > 0) return Success();
            else return Fail();
        }

        [NonAction]
        public bool SendEmail(string Name,string Title, string Content, string Address)
        {
            FileStream fs = new FileStream(Server.MapPath("~/App_Data/email.html"), FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            string html_doc = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            html_doc=html_doc.Replace("{{Name}}",Name ).Replace("{{Content}}",Content.Replace("\n\r","<br/>"));
     
            
            Email email = new Email();
            email.displayName = "广东海洋大学海洋遥感教育科普基地";
            email.mailFrom = "yaogan@yangshunjie.com";
            email.mailPwd = "ABCabc123";
            email.mailSubject = Title;
            email.mailBody = html_doc;
            email.isbodyHtml = true;    //是否是HTML
            email.host = "smtp.yangshunjie.com";//如果是QQ邮箱则：smtp:qq.com,依次类推
            email.mailToArray = new string[] { Address };//接收者邮件集合
            email.mailCcArray = new string[] { Address };//抄送者邮件集合
            //ResultMsg result = new ResultMsg();
            if (email.Send()) return true;
            else return false;
            //{
            //    result.result = "success";
            //    //result.Message = "结果已通过邮件形式发送给申请人！";
            //    //result.Message = html_doc;
            //}
            //else
            //{
            //    result.result = "fail";
            //    result.Message = "发送失败，请检查网络连接。";
            //}
            //return JsonConvert.SerializeObject(result);

        }
    }
}