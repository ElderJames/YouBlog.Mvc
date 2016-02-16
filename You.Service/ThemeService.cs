using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using You.Models;

namespace You.Service
{
    public class ThemeService : BaseService<Theme>
    {

        //string[] m_subKeleyiFolder = Directory.GetDirectories(System.Web.Server.MapPath("/hvtimg\\"));

        //  private static readonly EmptyTemplateService _DefaultCommandService = new EmptyTemplateService();

        // private static TemplateServiceProvider currentProvider = () => _DefaultCommandService;

        public static ICollection<Theme> Themes = null;
        private HttpRequestBase request;

        public ThemeService(HttpRequestBase request)
        {
            this.request = request;
            TemplateDirectoryName = "Templates";
            DefaultTemplateName = "Default";
            TemplateFileExtension = "cshtml";
        }

        public ICollection<Theme> GetThemes()
        {
            if (Themes != null)
                return Themes;
            
            ICollection<Theme> themes = null;
            string themeBasePath = request.RequestContext.HttpContext.Server.MapPath("~/Content/Themes/");
            string[] themePaths = Directory.GetDirectories(themeBasePath);
            for (int i = 0; i < themePaths.Length; i++)
            {
                string temp = themePaths[i];
                themePaths[i]= temp.Substring(temp.LastIndexOf('\\') + 1, temp.Length - temp.LastIndexOf('\\') - 1),
                themes.Add(new Theme { 
                    Id=i,
                Name= themePaths[i],
                Directory=TemplateDirectoryName+"/"+themePaths[i]
                });
                 
            }
            return themes;
        }

        public Theme FindbyUser(int uid)
        {
            return Find(t => t.UserID == uid);
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
    }
}
