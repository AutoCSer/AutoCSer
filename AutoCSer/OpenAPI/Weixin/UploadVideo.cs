using System;
using System.Runtime.InteropServices;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 新增永久视频素材
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct UploadVideo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 描述
        /// </summary>
        public string introduction;
    }
}
