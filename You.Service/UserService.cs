using Microsoft.AspNet.Identity;
using System.Security.Claims;
using You.Models;

namespace You.Service
{
    public class UserService:BaseService<User>
    {
        public bool Exist(string name,string email) { return Exist(u =>u.UserName==name|| u.Email == email); }

        public User Find(string acount)
        {
            if (acount.IndexOf("@") > 0) return Find(u => u.Email == acount);
            else return Find(u => u.UserName == acount);
        }

        public ClaimsIdentity CreateIdentity(User user)
        {
            ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            _identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            _identity.AddClaim(new Claim(ClaimTypes.Sid, user.UserID.ToString()));
            _identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            _identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "ASP.NET Identity"));
            _identity.AddClaim(new Claim("RealName", user.RealName));
            return _identity;
        }
    }

}
