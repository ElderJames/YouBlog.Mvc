using System.Collections.Generic;

namespace You.Models
{
    public enum CommonModelState
    {
        Normal = 9, UnDelete = -9,Unchecked = 0, NoReplied = 20, Replied = 29, Deleted = -3, Rejection = -2, Draft = -1
    }
    public static class CommonModelStateString
    {
        public static Dictionary<CommonModelState, string> StateList
        {
            get
            {
                return new Dictionary<CommonModelState, string>() {
                { CommonModelState.UnDelete, "固定" },
                { CommonModelState.Deleted, "已删除" },
                { CommonModelState.Rejection, "退稿" },
                { CommonModelState.Draft, "草稿" },
                { CommonModelState.Unchecked,"未审核"},
                { CommonModelState.Normal,"正常"},
                { CommonModelState.NoReplied,"未回复"},
                { CommonModelState.Replied,"已回复"}
                };
            }
        }
    }
}
