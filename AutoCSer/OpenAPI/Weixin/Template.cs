using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 模板消息
    /// </summary>
    /// <typeparam name="valueType">模板类型</typeparam>
    public sealed class Template<valueType>
    {
        /// <summary>
        /// 公众号微信号
        /// </summary>
        public string touser;
        /// <summary>
        /// 模板ID
        /// </summary>
        public string template_id;
        /// <summary>
        /// 
        /// </summary>
        public string url;
        /// <summary>
        /// 
        /// </summary>
        public string topcolor;
#if __IOS__
#else
        /// <summary>
        /// 
        /// </summary>
        public unsafe System.Drawing.Color TopColor
        {
            set
            {
                topcolor = AutoCSer.Extension.StringExtension.FastAllocateString(7);
                fixed (char* valueFixed = topcolor)
                {
                    *valueFixed = '#';
                    *(valueFixed + 1) = (char)AutoCSer.Extension.Number.ToHex((uint)value.R >> 4);
                    *(valueFixed + 2) = (char)AutoCSer.Extension.Number.ToHex((uint)value.R & 0xf);
                    *(valueFixed + 3) = (char)AutoCSer.Extension.Number.ToHex((uint)value.G >> 4);
                    *(valueFixed + 4) = (char)AutoCSer.Extension.Number.ToHex((uint)value.G & 0xf);
                    *(valueFixed + 5) = (char)AutoCSer.Extension.Number.ToHex((uint)value.B >> 4);
                    *(valueFixed + 6) = (char)AutoCSer.Extension.Number.ToHex((uint)value.B & 0xf);
                }
            }
        }
#endif
        /// <summary>
        /// 模版数据
        /// </summary>
        public valueType data;
    }
}
