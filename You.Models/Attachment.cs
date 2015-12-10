using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace You.Models
{
    /// <summary>
    /// 附件
    /// <remarks>
    /// 创建：2014.02.27
    /// </remarks>
    /// </summary>
    public class Attachment
    {
        
        public int AttachmentID { get; set; }

        /// <summary>
        /// 模型ID
        /// </summary>
        //public Nullable<int> ModelID { get; set; }

        [JsonIgnore]
        public virtual ICollection<CommonModel> CommonModels { get; set; }

        /// <summary>
        /// 原文件名
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// 上传者
        /// 对应User
        /// </summary>
        public string Inputer  { get; set; }

        /// <summary>
        /// 附件类型 image,flash,media,file 四种类型
        /// </summary>
        public AttachmentType Type { get; set; }

        public AttachmentUsage Usage { get; set; }
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

        public string UsageToString { get { return UsageString.UsageList[Usage]; } }

        public Attachment()
        {
            CommonModels = new List<CommonModel>();
            UploadDate = DateTime.Now;
        }
    }
}
