using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You.Models
{
    public enum AttachmentType
    {
        Image,Scrawl, Flash, Media, File
    }
    public static class AttachmentTypeString
    {
        public static Dictionary<AttachmentType, string> TypeList
        {
            get
            {
                return new Dictionary<AttachmentType, string>() { 
                    { AttachmentType.Image, "图片" },
                    { AttachmentType.Scrawl, "涂鸦" },
                    { AttachmentType.Flash, "Flash动画" },
                    { AttachmentType.Media, "视频" },
                    { AttachmentType.File, "文件" },
                };
            }
        }
    }
}
