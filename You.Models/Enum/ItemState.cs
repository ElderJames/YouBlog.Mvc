using System.Collections.Generic;

namespace You.Models
{
    /// <summary>
    /// 条目状态
    /// </summary>
    public enum ItemState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Nomal,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted,
        /// <summary>
        /// 禁止
        /// </summary>
        Disable,
        /// <summary>
        /// 审核中
        /// </summary>
        In_Review,
        /// <summary>
        /// 已审核
        /// </summary>
        Reviewed,
        /// <summary>
        /// 不可删除
        /// </summary>
        Undeletable,
        /// <summary>
        /// 已过期
        /// </summary>
        Expired
    }

    public static class StateString
    {
        public static Dictionary<ItemState, string> StatusList
        {
            get
            {
                return new Dictionary<ItemState, string>() { 
                { ItemState.Deleted, "删除" },
                { ItemState.Nomal, "正常" },
                { ItemState.Disable, "禁止" },
                { ItemState.Expired,"已过期"},
                { ItemState.In_Review,"审核中"},
                { ItemState.Reviewed,"已审核"},
                { ItemState.Undeletable,"固定"}
                };
            }
        }
    }
  
}
