using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using You.Models;

namespace You.Data
{
    public class ThemeFactory
    {
        public static Theme GetCurrentTheme()
        {
            Theme _theme = CallContext.GetData("Theme") as Theme;
            if (_theme == null)
            {
                _theme = new Theme();
                CallContext.SetData("Theme", _theme);
            }
            return _theme;
        }

        public static void SetCurrentTheme(Theme theme)
        {
            CallContext.SetData("Theme", theme);
        }
    }
}
