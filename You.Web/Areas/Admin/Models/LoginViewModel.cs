using System.ComponentModel.DataAnnotations;

namespace You.Web.Areas.Admin.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "要求输入{2}到{1}个字符")]
        [Display(Name = "邮箱")]
        public string Acount { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "要求输入{2}到{1}个字符")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 记住我
        /// </summary>
        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    
        
    }
}