using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You.Models
{
    public class WebSiteConfig
    {
        public int id { get; set; }
        public int UserID { get; set; }
        public Theme DefaultTheme { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
