using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace You.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "别名")]
        public string SubTitle { get; set; }

        /// <summary>
        /// 文章
        /// </summary>
        [JsonIgnore]
        [Display(Name = "文章")]
        public virtual ICollection<CommonModel> Articles{ get; set; }

       // [NotMapped]
        //public int ArticleCount { get { return Articles == null ? 0 : Articles.Count; } }

        /// <summary>
        /// 查询量
        /// </summary>
        [Display(Name = "查询量")]
        public int SearchNum { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        [Display(Name = "点击量")]
        public int Hit { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        public ItemState State { get; set; }

        public string StateToString { get { return StateString.StatusList[State]; } }

        public Tag()
        {
            Hit = 0;
            SearchNum = 0;
            CreateTime = DateTime.Now;
            State = ItemState.Nomal;
        }
    }
}
