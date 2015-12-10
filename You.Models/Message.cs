using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "必填")]
        public string Name { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        [Display(Name = "QQ号码")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "{1}-{0}个数字")]
        public string Phone { get; set; }

        /// <summary>
        /// Email地址
        /// </summary>
        [Display(Name = "Email地址")]
        [DataType(DataType.EmailAddress, ErrorMessage = "必须输入正确的Email地址")]
        public string Email { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "必填")]
        [StringLength(1000, ErrorMessage = "必须少于{0}个字符")]
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [Display(Name = "回复内容")]
        public string ReplyContent { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        [Display(Name = "回复时间")]
        public Nullable<DateTime> ReplyTime { get; set; }

        public MessageState State { get; set; }

        public string StateToString { get { return MessageStateString.StatusList[State]; } }
        public Message() 
        {
            State = MessageState.noReply;
            CreateTime = DateTime.Now;
        }
    }
}
