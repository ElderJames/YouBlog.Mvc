using System.Collections.Generic;

namespace You.Models
{
    public enum NavType
    {
        Link,Page,Category
    }

    public static class NavTypeString
    {
        public static Dictionary<NavType, string> TypeList
        {
            get
            {
                return new Dictionary<NavType, string>() { 
                    {NavType.Link, "链接" },
                    {NavType.Category, "分类" },
                    {NavType.Page, "页面" },
                };
            }
        }
    }
}
