using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You.Models
{
    public enum MessageState
    {
        noReply,Replyed,Deleted
    }
    public static class MessageStateString
    {
        public static Dictionary<MessageState, string> StatusList
        {
            get
            {
                return new Dictionary<MessageState, string>() {
                { MessageState.Deleted, "已删除" },
                 { MessageState.noReply, "未回复" },
                    { MessageState.Replyed, "已回复" },
                };
            }
        }
    }
}
