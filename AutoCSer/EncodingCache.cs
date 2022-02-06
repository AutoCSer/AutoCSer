using System;
using System.Runtime.CompilerServices;
using System.Text;
using AutoCSer.Extension;

namespace AutoCSer
{
    /// <summary>
    /// 编码类型
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    //internal unsafe struct EncodingCache
    {
        /// <summary>
        /// 写字节数据
        /// </summary>
        /// <param name="text"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal int WriteBytesNotEmpty(string text, byte[] buffer)
        {
            int length = text.Length;
            if ((Type & (1 | 4)) == 0) return Encoding.GetBytes(text, 0, length, buffer, 0);
            fixed (char* textFixed = text)
            fixed (byte* bufferFixed = buffer)
            {
                if ((Type & 4) != 0) Memory.CopyNotNull(textFixed, bufferFixed, length <<= 1);
                else AutoCSer.Extension.StringExtension.WriteBytes(textFixed, length, bufferFixed);
            }
            return length;
        }
    }
    /// <summary>
    /// 编码类型
    /// </summary>
    //public static class EncodingCacheOther
    {
#if DotNetStandard
        static EncodingCacheOther()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
#endif
    }
}
