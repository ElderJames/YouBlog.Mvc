using You.Models;
using System;

namespace You.Web.Areas.Admin.Models
{
    public class AttachmentViewModel
    {

        public int AttachmentID { get; set; }

      
        /// <summary>
        /// 文章使用数
        /// </summary>
        public int ArticleCount { get; set; }

        /// <summary>
        /// 原文件名
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// 上传者
        /// 对应User
        /// </summary>
        public string Inputer { get; set; }

        /// <summary>
        /// 附件类型 image,flash,media,file 四种类型
        /// </summary>
        public AttachmentType Type { get; set; }

        public string TypeToString { get { return AttachmentTypeString.TypeList[Type]; } }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileParth { get; set; }

        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime UploadDate { get; set; }

        public ItemState State { get; set; }

        public string StateToString { get { return StateString.StatusList[State]; } }
    }
}