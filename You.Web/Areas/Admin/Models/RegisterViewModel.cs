using System.ComponentModel.DataAnnotations;

namespace You.Web.Areas.Admin.Models
{
    public class RegisterViewModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [MaxLength(20, ErrorMessage = "用户名不可超出20个字符")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "邮箱")]
        [MaxLength(250, ErrorMessage = "Email地址长度无法超过250个字符")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email格式不正确")]
        public string Email { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [MaxLength(20, ErrorMessage = "用户名不可超出20个字符")]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度需要{2}到{1}个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
       // [Required(ErrorMessage = "请输入验证码")]
        //[StringLength(4, MinimumLength = 4, ErrorMessage = "验证码不正确")]
        //[Display(Name = "验证码")]
        //public string VerificationCode { get; set; }
    }
}