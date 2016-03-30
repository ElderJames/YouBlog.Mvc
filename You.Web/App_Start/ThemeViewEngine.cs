using System.Web.Mvc;

namespace You.Web
{
    /// <summary>
    /// 自定义主题引擎
    /// </summary>
    public class ThemeViewEngine : RazorViewEngine
    {
        public ThemeViewEngine()
        {
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new RazorView(controllerContext, this.GetVirtualPath(controllerContext, partialPath), null, false, base.FileExtensions, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(controllerContext, this.GetVirtualPath(controllerContext, viewPath), this.GetVirtualPath(controllerContext, masterPath), true, base.FileExtensions, base.ViewPageActivator);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            System.Diagnostics.Debug.WriteLine(virtualPath);

            if (virtualPath.StartsWith("~") && !string.IsNullOrEmpty(ThemeManager.Theme) && ThemeManager.Theme != "default")
            {
                var themePath = GetThemePath(virtualPath);
                if (base.FileExists(controllerContext, themePath))
                {
                    return true;
                }
                else
                {
                    return base.FileExists(controllerContext, virtualPath);
                }
            }
            return base.FileExists(controllerContext, virtualPath);
        }

        private static string GetThemePath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                return virtualPath;
            }
            return virtualPath.Insert(1, ThemeManager.ThemePath + ThemeManager.Theme);
        }

        private string GetVirtualPath(ControllerContext controllerContext, string virtualPath)
        {
            if (virtualPath.StartsWith("~") && !string.IsNullOrEmpty(ThemeManager.Theme) && ThemeManager.Theme != "default")
            {
                var themePath = GetThemePath(virtualPath);
                if (base.FileExists(controllerContext, themePath))
                {
                    return themePath;
                }
            }
            return virtualPath;
        }
    }
}