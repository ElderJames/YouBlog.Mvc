using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You.Models
{
    /// <summary>
    /// 主题模版
    /// </summary>
    public class Theme
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Directory { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public DateTime SetTime { get; set; }
    }
}
