using System.ComponentModel.DataAnnotations;

namespace You.Models
{
    /// <summary>
    /// 评论模型
    /// </summary>
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        /// <summary>
        /// 模型ID
        /// </summary>
        public int ModelID { get; set; }

        /// <summary>
        /// 评论标题
        /// </summary>
        [Display(Name = "评论标题")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字")]
        public string Title { get; set; }
    }
}
