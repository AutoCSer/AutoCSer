using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace AutoCSer.IO
{
    /// <summary>
    /// 文件编码BOM
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FileBom
    {
        /// <summary>
        /// BOM
        /// </summary>
        internal uint Bom;
        /// <summary>
        /// BOM长度
        /// </summary>
        internal int Length;

        /// <summary>
        /// 文件编码BOM唯一哈希
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct EncodingBom : IEquatable<EncodingBom>
        {
            /// <summary>
            /// 文件编码
            /// </summary>
            public Encoding Encoding;
            /// <summary>
            /// 隐式转换
            /// </summary>
            /// <param name="encoding">文件编码</param>
            /// <returns>文件编码BOM唯一哈希</returns>
            public static implicit operator EncodingBom(Encoding encoding) { return new EncodingBom { Encoding = encoding }; }
            /// <summary>
            /// 获取哈希值
            /// </summary>
            /// <returns>哈希值</returns>
            public unsafe override int GetHashCode()
            {
                int codePage = Encoding.CodePage;
                return ((codePage >> 3) | codePage) & 3;
            }
            /// <summary>
            /// 判断是否相等
            /// </summary>
            /// <param name="other">待匹配数据</param>
            /// <returns>是否相等</returns>
            public unsafe bool Equals(EncodingBom other)
            {
                return Encoding == other.Encoding;
            }
            /// <summary>
            /// 判断是否相等
            /// </summary>
            /// <param name="obj">待匹配数据</param>
            /// <returns>是否相等</returns>
            public override bool Equals(object obj)
            {
                return Equals((EncodingBom)obj);
            }
        }
        /// <summary>
        /// 文件编码BOM集合
        /// </summary>
        private static readonly UniqueDictionary<EncodingBom, FileBom> boms;
        /// <summary>
        /// 根据文件编码获取BOM
        /// </summary>
        /// <param name="encoding">文件编码</param>
        /// <param name="bom">文件编码 BOM</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void Get(Encoding encoding, ref FileBom bom)
        {
            boms.Get(encoding, ref bom);
        }
        /// <summary>
        /// 根据文件编码获取BOM
        /// </summary>
        /// <param name="encoding">文件编码</param>
        /// <returns>文件编码 BOM</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static FileBom Get(Encoding encoding)
        {
            FileBom bom = default(FileBom);
            boms.Get(encoding, ref bom);
            return bom;
        }
        static FileBom()
        {
            KeyValue<EncodingBom, FileBom>[] bomArray = new KeyValue<EncodingBom, FileBom>[4];
            int count = 0;
            bomArray[count++].Set(Encoding.Unicode, new FileBom { Bom = 0xfeffU, Length = 2 });
            bomArray[count++].Set(Encoding.BigEndianUnicode, new FileBom { Bom = 0xfffeU, Length = 2 });
            bomArray[count++].Set(Encoding.UTF8, new FileBom { Bom = 0xbfbbefU, Length = 3 });
            bomArray[count++].Set(Encoding.UTF32, new FileBom { Bom = 0xfeffU, Length = 4 });
            boms = new UniqueDictionary<EncodingBom, FileBom>(bomArray, 4);
        }
    }
}
