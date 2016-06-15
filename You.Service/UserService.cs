using Microsoft.AspNet.Identity;
using You.Models;
using System.Security.Claims;
using System.Web;

namespace You.Service
{
    public class UserService:BaseService<User>,IService<User>
    {
        public bool Exist(string name,string email) { return Exist(u =>u.UserName==name|| u.Email == email); }

        private static  User _currentUser;

        public User Find(string acount)
        {
            if (acount.IndexOf("@") > 0) return CurrentUser = Find(u => u.Email == acount);
            else return CurrentUser = Find(u => u.UserName == acount);
        }

        /// <summary>
        /// 创建令牌
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ClaimsIdentity CreateIdentity(User user)
        {
            ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            _identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            _identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserID.ToString()));
      
            _identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()));
            _identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            //_identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "ASP.NET Identity"));
            _identity.AddClaim(new Claim("RealName", user.RealName));
            return _identity;
        }

        /// <summary>
        /// 获取当前线程的用户
        /// </summary>
        /// <returns></returns>
        public static User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = HttpContext.Current.GetOwinContext().Get<User>("CurrentUser");
                if (_currentUser==null)
                {
                    var userService = ServiceFactory.GetService<User>();
                    _currentUser = userService.Find(int.Parse(HttpContext.Current.GetOwinContext().Authentication.User.Identity.GetUserId()));
                }
                return _currentUser;
            }
            private set
            {
                _currentUser = value;
                HttpContext.Current.GetOwinContext().Set("CurrentUser", value);
            }
        }
    }
}
