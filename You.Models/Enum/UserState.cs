using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace You.Models
{
    public enum UserState
    {
        Normal, NoEmailValidation, NoAdminValidation, Deleted, Disable,Super,NoUsed
    }

    public static class UserStateString
    {
        public static Dictionary<UserState, string> StateList
        {
            get
            {
                return new Dictionary<UserState, string>() { 
                    {UserState.Normal, "正常" },
                    {UserState.NoEmailValidation, "未验证邮箱" },
                    {UserState.NoAdminValidation, "未手动验证" },
                    {UserState.Deleted,"已删除"},
                    {UserState.Disable,"已禁用"},
                    {UserState.NoUsed,"未使用"},
                    {UserState.Super,"超级用户"}
                };
            }
        }
    }
}
