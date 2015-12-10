using System.ComponentModel.DataAnnotations;

namespace You.Models
{
    /// <summary>
    /// 文章模型
    /// <remarks>创建：2014.02.06</remarks>
    /// </summary>
    public class Article
    {
        [Key]
        public int ArticleID { get; set; }

        /// <summary>
        /// 公共模型ID
        /// </summary>
        //[Display(Name = "公共模型ID")]
        //public int ModelID { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [StringLength(50, ErrorMessage = "必填少于{0}个字")]
        public string Author { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Display(Name = "来源")]
        [StringLength(50, ErrorMessage = "必填少于{0}个字")]
        public string Source { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Display(Name = "摘要")]
        [StringLength(255, ErrorMessage = "必填少于{0}个字")]
        public string Intro { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Required(ErrorMessage = "必填")]
        [DataType(DataType.Html)]
        public string Content { get; set; }
    }
}
