using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 获取素材列表查询
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct MediaQuery
    {
        /// <summary>
        /// 素材的类型
        /// </summary>
        public MediaQueryType type;
        /// <summary>
        /// 从全部素材的该偏移位置开始返回，0表示从第一个素材 返回
        /// </summary>
        public int offset;
        /// <summary>
        /// 返回素材的数量，取值在1到20之间
        /// </summary>
        public int count;
    }
}
