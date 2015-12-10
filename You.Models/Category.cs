using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace You.Models
{
    /// <summary>
    /// 分类模型
    /// </summary>
    public class Category
    {
        [Key]
        [Display(Name = "分类")]
        public int CategoryID { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Display(Name = "分类名称", Description = "2-20个字符")]
        [Required(ErrorMessage = "必填")]
        [StringLength(50, ErrorMessage = "必须少于{0}个字符")]
        public string Name { get; set; }

         [Display(Name = "英文别名", Description = "用于url显示")]
        public string SubTitle { get; set; }

        [Display(Name = "类型")]
        public CategoryType Type { get; set; }

        public string TypeToString { get { return CategoryTypeString.TypeList[Type]; } }
        ///<summary>
        ///父栏目编号
        ///</summary>
        [Display(Name = "上级分类")]
        [Required(ErrorMessage = "必填")]
        public int ParentId { get; set; }

        [Display(Name = "上级路径")]
        public string ParentPath { get; set; }

        [Display(Name = "说明")]
        public string Description { get; set; }

        /// <summary>
        /// 内容排序
        /// </summary>
        [Display(Name = "内容排序", Description = "栏目所属内容的排序方式。")]
        public Nullable<int> ContentOrder { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        [Display(Name = "每页记录数", Description = "栏目所属内容的排序方式。")]
        public Nullable<int> PageSize { get; set; }

        /// <summary>
        /// 记录单位
        /// </summary>
        [Display(Name = "记录单位", Description = "记录的数量单位。如文章为“篇”；新闻为“条”。")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string RecordUnit { get; set; }

        [NotMapped]
        public List<Category> Children { get; set; }

        /// <summary>
        /// 记录名称
        /// </summary>
        [Display(Name = "记录名称", Description = "记录的名称。如“文章”、“新闻”、“教程”等。")]
        [StringLength(255, ErrorMessage = "必须少于{0}个字符")]
        public string RecordName { get; set; }

        public ItemState State { get; set; }
        public string StateToString { get { return StateString.StatusList[State]; } }

        public DateTime CreateTime { get; set; }

        [Display(Name = "排序")]
        public int Order { get; set; }
        public Category()
        {
            ContentOrder = 1;
            PageSize = 20;
            RecordUnit = "条";
            RecordName = "篇";
            ParentId = 0;
            State = ItemState.Nomal;
            Type = CategoryType.Article;
            CreateTime = DateTime.Now;
            Description = "";
            Order = 0;
            ParentPath = "0";
        }
    }
}
