using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weibo
{
    /// <summary>
    /// 可见性
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct Visible
    {
        /// <summary>
        /// 0：普通微博，1：私密微博，3：指定分组微博，4：密友微博
        /// </summary>
        public int type;
        /// <summary>
        /// 分组的组号
        /// </summary>
        public int list_id;
    }
}
