using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// 字节数组
        /// </summary>
        [FieldOffset(0)]
        public byte[] ByteArray;
        /// <summary>
        /// 图像块
        /// </summary>
        [FieldOffset(0)]
        public Image Image;
        /// <summary>
        /// 图形控制扩展
        /// </summary>
        [FieldOffset(0)]
        public PraphicControl PraphicControl;
        /// <summary>
        /// 图形文本扩展
        /// </summary>
        [FieldOffset(0)]
        public PlainText PlainText;
        /// <summary>
        /// 应用程序扩展
        /// </summary>
        [FieldOffset(0)]
        public Application Application;
    }
}
