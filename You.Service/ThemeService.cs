using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using You.Models;
using Newtonsoft.Json.Linq;
using System.Runtime.Remoting.Messaging;

namespace You.Service
{
    public class ThemeService : BaseService<Theme>,IService
    {
        //string[] m_subKeleyiFolder = Directory.GetDirectories(System.Web.Server.MapPath("/hvtimg\\"));

        //  private static readonly EmptyTemplateService _DefaultCommandService = new EmptyTemplateService();

        // private static TemplateServiceProvider currentProvider = () => _DefaultCommandService;
        public ICollection<Theme> Themes { get { return GetThemes(); } }
        

        public ThemeService()
        {
            TemplateDirectoryName = "~/Themes/";
            DefaultTemplateName = "Default";
            TemplateFileExtension = "cshtml";
        }

        public ICollection<Theme> GetThemes()
        {
            ICollection<Theme> themes = null;
            string themeBasePath = System.Web.HttpContext.Current.Server.MapPath(TemplateDirectoryName);
            string[] themePaths = Directory.GetDirectories(themeBasePath);
            if (themePaths.Length <= 0) return null;
            themes = new List<Theme>(); 
            for (int i = 0; i < themePaths.Length; i++)
            {
                string temp = themePaths[i];
                themePaths[i] = temp.Substring(temp.LastIndexOf('\\') + 1, temp.Length - temp.LastIndexOf('\\') - 1);
                string themeConfigJson=System.IO.File.ReadAllText(themeBasePath + themePaths[i]+"\\info.json");
                if (!string.IsNullOrEmpty(themeConfigJson))
                {
                    JObject config = JObject.Parse(themeConfigJson);
                    
                    themes.Add(new Theme
                    {
                        Id = i,
                        Name = config["Name"].ToString(),
                        Directory = themePaths[i]
                    });
                }
            }
            return themes;
        }

        public Theme FindbyUser(int uid)
        {
            var _theme = Find(t => t.UserID == uid);

            return _theme;
        }

        /// <summary>
        /// 获取当前应用程序正在使用的模板服务。
        /// </summary>
        //public static ITemplateService Current
        //{
        //    get { return currentProvider(); }
        //}

        /// <summary>
        /// 设置当前应用程序正在使用的模板服务提供者。
        /// </summary>
        // public static void SetProvider(TemplateServiceProvider provider)
        //{
        //    provider.MustNotNull("provider");

        //    currentProvider = provider;
        //}

        /// <summary>
        /// 模板路径。
        /// </summary>
        public static string TemplateDirectoryName { get; set; }

        /// <summary>
        /// 默认模板。
        /// </summary>
        public static string DefaultTemplateName { get; set; }

        /// <summary>
        /// 默认模板。
        /// </summary>
        public static string TemplateFileExtension { get; set; }

        public static ThemeService Current
        {
            get
            {
                var themeService = CallContext.GetData("Theme") as ThemeService;

                if (themeService == null)
                {
                    themeService = new ThemeService();
                    CallContext.SetData("Theme", themeService);
                }
                return themeService;
            }
        }
    }
}
