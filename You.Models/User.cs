using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace You.Models
{
    [DisplayName("用户信息")]
    [DisplayColumn("UserName")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [DisplayName("用户名")]
        [Required(ErrorMessage = "请输入用户名")]
        [MaxLength(20, ErrorMessage = "用户名不可超出20个字符")]
        public string UserName { get; set; }

        [DisplayName("用户邮箱")]
        [Required(ErrorMessage = "请输入Email地址")]
        [Description("使用邮箱地址作为登录帐号")]
        [MaxLength(250, ErrorMessage = "Email地址长度无法超过250个字符")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的Email格式")]
        public string Email { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string Password { get; set; }

        [DisplayName("真实姓名")]
        [MaxLength(20, ErrorMessage = "用户名不可超出20个字符")]
        public string RealName { get; set; }

        [DisplayName("状态")]
        public UserState State { get; set; }

        [DisplayName("注册时间")]
        public DateTime RegisterOn { get; set; }

        [DisplayName("最后登录时间")]
        public DateTime LoginTime { get; set; }

        [DisplayName("最后登录IP")]
        public string LoginIP { get; set; }

        public string StateToString { get { return UserStateString.StateList[State]; } }
    }
}