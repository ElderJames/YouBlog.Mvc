using You.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace You.Web.Areas.Admin.Models
{
    public class NavigationViewModel
    {
        [Display(Name = "栏目名称")]
        [Required(ErrorMessage = "请输入名称")]
        public string Name { get; set; }

        [Display(Name = "上级栏目")]
        [Required(ErrorMessage = "请输入上级栏目")]
        public int ParentID { get; set; }

        public DateTime CreateTime { get; set; }

        public ItemState State { get; set; }

        /// <summary>
        /// 栏目关键字
        /// </summary>
        [Display(Name = "栏目关键字")]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// 栏目描述
        /// </summary>
        [Display(Name = "栏目描述")]
        public string MetaDescription { get; set; }

        /// <summary>
        /// 栏目类型【0-常规栏目；1-单页栏目；2-外部链接】
        /// </summary>
        [Display(Name = "栏目类型")]
        [Required(ErrorMessage = "必填")]
        public NavType Type { get; set; }

        /// <summary>
        /// 内容模型【仅在栏目为普通栏目时有效】
        /// </summary>
        [Display(Name = "内容模型")]
        [StringLength(50, ErrorMessage = "必须少于{0}个字符")]
        public string Model { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [Display(Name = "分类")]
        public int CategoryID { get; set; }

        /// <summary>
        /// 栏目视图
        /// </summary>
        [Display(Name = "栏目视图", Description = "栏目页的视图，最多255个字符。。")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string CategoryView { get; set; }

        /// <summary>
        /// 内容页视图
        /// </summary>
        [Display(Name = "内容视图", Description = "内容页视图，最多255个字符。。")]
        [StringLength(255, ErrorMessage = "×")]
        public string ContentView { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [Display(Name = "链接地址", Description = "点击栏目时跳转到的链接地址,最多255个字符。")]
        [StringLength(255, ErrorMessage = "×")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 栏目排序
        /// </summary>
        [Display(Name = "栏目排序", Description = "针对同级栏目,数字越小顺序越靠前。")]
        public int Order { get; set; }
    }
}