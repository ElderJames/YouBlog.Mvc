using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace You.Models
{
    /// <summary>
    /// 内容公共模型
    /// <remarks>创建：2014.02.06</remarks>
    /// </summary>
    
    [DisplayColumn("Title")]
    public class CommonModel
    {
        [Key]
        public int ModelID { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Display(Name = "分类")]
        public int CategoryID { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string Model { get; set; }
 
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [Required(ErrorMessage = "必填")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字")]
        public string Title { get; set; }

        /// <summary>
        /// 英文别名
        /// </summary>
        [Display(Name = "别名")]
        [StringLength(100, ErrorMessage = "必须少于{0}个字")]
        public string SubTitle { get; set; }

        /// <summary>
        /// 录入者
        /// </summary>
        [Display(Name = "录入者")]
        [Required]
        [JsonIgnore]
        public User Inputer { get; set; }

        public int UserID { get; set; }

        [Display(Name = "标签")]
        public string Tags { get; set; }

        [Display(Name="设置为独立页面")]
        public bool isPage { get; set; }
        /// <summary>
        /// 点击
        /// </summary>
        [Display(Name = "点击")]
        public int Hits { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        [Display(Name = "发布日期")]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// 状态
        /// <remarks>
        /// 【-3删除，-2退稿，-1草稿，0-未审核，9-正常】<br />
        /// 【咨询：20未回复，29已回复】
        /// </remarks>
        /// </summary>
        [Display(Name = "状态")]
        public CommonModelState State { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string DefaultPicUrl { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        public virtual Article Article { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// 栏目
        /// </summary>
        [Required]
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tag> Tag { get; set; }
        /// <summary>
        /// 附件列表
        /// </summary>
        public virtual ICollection<Attachment> Attachment { get; set; }

        /// <summary>
        /// 状态列表，只读静态变量
        /// </summary>
        public static Dictionary<CommonModelState, string> StatusList
        {
            get
            {
                return new Dictionary<CommonModelState, string>() { 
                { CommonModelState.Deleted, "删除" },
                { CommonModelState.Rejection, "退稿" },
                { CommonModelState.Draft, "草稿" },
                { CommonModelState.Unchecked,"未审核"},
                { CommonModelState.Normal,"正常"},
                { CommonModelState.NoReplied,"未回复"},
                { CommonModelState.Replied,"已回复"}
                };
            }
        }

        public CommonModel()
        {
            isPage = false;
            ReleaseDate = DateTime.Now;
            State = CommonModelState.Normal;
        }
    }
}
