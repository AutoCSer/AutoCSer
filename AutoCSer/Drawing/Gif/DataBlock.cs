using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 数据块
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DataBlock
    {
        /// <summary>
        /// 数据块对象
        /// </summary>
        private object value;
        /// <summary>
        /// 图像块
        /// </summary>
        public Image Image
        {
            get
            {
                if (Type == DataType.Image) return new UnionType { Value = value }.Image;
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 图形控制扩展
        /// </summary>
        public PraphicControl GraphicControl
        {
            get
            {
                if (Type == DataType.Image) return new UnionType { Value = value }.PraphicControl;
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 图形文本扩展
        /// </summary>
        public PlainText PlainText
        {
            get
            {
                if (Type == DataType.Image) return new UnionType { Value = value }.PlainText;
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 应用程序扩展
        /// </summary>
        public Application Application
        {
            get
            {
                if (Type == DataType.Image) return new UnionType { Value = value }.Application;
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 注释扩展索引范围
        /// </summary>
        private RangeLength commentRangeLength;
        /// <summary>
        /// 注释扩展
        /// </summary>
        public SubArray<byte> Comment
        {
            get
            {
                if (Type == DataType.Comment) return new SubArray<byte> { Array = new UnionType { Value = value }.ByteArray, Start = commentRangeLength.Start, Length = commentRangeLength.Length };
                throw new InvalidCastException(Type.ToString());
            }
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType Type { get; private set; }
        /// <summary>
        /// 设置图像块
        /// </summary>
        /// <param name="image">图像块</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(Image image)
        {
            Type = DataType.Image;
            value = image;
        }
        /// <summary>
        /// 设置图形控制扩展
        /// </summary>
        /// <param name="graphicControl">图形控制扩展</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(PraphicControl graphicControl)
        {
            Type = DataType.GraphicControl;
            value = graphicControl;
        }
        /// <summary>
        /// 设置注释扩展
        /// </summary>
        /// <param name="comment">注释扩展</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(ref SubArray<byte> comment)
        {
            Type = DataType.Comment;
            value = comment.Array;
            commentRangeLength.Set(comment.Start, comment.Length);
        }
        /// <summary>
        /// 设置图形文本扩展
        /// </summary>
        /// <param name="plainText">图形文本扩展</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(PlainText plainText)
        {
            Type = DataType.PlainText;
            value = plainText;
        }
        /// <summary>
        /// 设置应用程序扩展
        /// </summary>
        /// <param name="application">应用程序扩展</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(Application application)
        {
            Type = DataType.Application;
            value = application;
        }
    }
}
