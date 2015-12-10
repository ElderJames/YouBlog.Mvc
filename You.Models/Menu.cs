using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace You.Models
{
    public class Menu
    {
        [Key]
        [Required(ErrorMessage ="请输入菜单名")]
        public int Id { get; set; }

        [Display(Name = "菜单名")]
        public string Name { get; set; }

        [Display(Name = "url")]
        public string Url { get; set; }

        [Display(Name ="排序")]
        public int Order { get; set; }

        public Menu() {
            Url = "";
        }
    }
}
