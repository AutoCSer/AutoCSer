using System;
using System.Text;

namespace AutoCSer
{
    /// <summary>
    /// 其它编码缓存
    /// </summary>
    internal static class EncodingCacheOther
    {
        /// <summary>
        /// GB2312 编码
        /// </summary>
        internal static readonly EncodingCache GB2312 = new EncodingCache("GB2312", EncodingType.CompatibleAscii);
        /// <summary>
        /// GB18030 编码
        /// </summary>
        internal static readonly EncodingCache GB18030 = new EncodingCache("GB18030", EncodingType.CompatibleAscii);
        /// <summary>
        /// GBK 编码
        /// </summary>
        internal static readonly EncodingCache GBK = new EncodingCache("GBK", EncodingType.CompatibleAscii);
        /// <summary>
        /// BIG5 编码
        /// </summary>
        internal static readonly EncodingCache BIG5 = new EncodingCache("BIG5", EncodingType.CompatibleAscii);

        /// <summary>
        /// 获取字符编码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static Encoding GetEncoding(string name)
        {
            try
            {
                return Encoding.GetEncoding(name);
            }
            catch (Exception error)
            {
                AutoCSer.LogHelper.Exception(error, "编码解析错误 " + name, LogLevel.Exception | LogLevel.AutoCSer);
            }
            return null;
        }

#if DotNetStandard
        static EncodingCacheOther()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
#endif
    }
}
