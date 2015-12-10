using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You.Models
{
    public enum CategoryType
    {
        Article,Circle,Picture,File
    }

    public static class CategoryTypeString
    {
        public static Dictionary<CategoryType, string> TypeList
        {
            get
            {
                return new Dictionary<CategoryType, string>() { 
                    {CategoryType.Article, "文章" },
                    {CategoryType.Circle, "圈子" },
                     {CategoryType.Picture, "图片" },
                     {CategoryType.File, "文件" }
                };
            }
        }
    }
}
