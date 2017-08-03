using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 图形文本扩展
    /// </summary>
    public sealed class PlainText
    {
        /// <summary>
        /// 文本框离逻辑屏幕的左边界距离
        /// </summary>
        public short Left { get; private set; }
        /// <summary>
        /// 文本框离逻辑屏幕的上边界距离
        /// </summary>
        public short Top { get; private set; }
        /// <summary>
        /// 文本框像素宽度
        /// </summary>
        public short Width { get; private set; }
        /// <summary>
        /// 文本框像素高度
        /// </summary>
        public short Height { get; private set; }
        /// <summary>
        /// 字符宽度
        /// </summary>
        public short CharacterWidth { get; private set; }
        /// <summary>
        /// 字符高度
        /// </summary>
        public short CharacterHeight { get; private set; }
        /// <summary>
        /// 前景色在全局颜色列表中的索引
        /// </summary>
        public byte ColorIndex { get; private set; }
        /// <summary>
        /// 背景色在全局颜色列表中的索引
        /// </summary>
        public byte BlackgroundColorIndex { get; private set; }
        /// <summary>
        /// 文本数据块集合
        /// </summary>
        internal LeftArray<SubArray<byte>> TextData;
        /// <summary>
        /// 文本数据
        /// </summary>
        public byte[] Text
        {
            get { return Decoder.BlocksToByte(ref TextData); }
        }
        /// <summary>
        /// 图形文本扩展
        /// </summary>
        /// <param name="data">当前解析数据</param>
        internal unsafe PlainText(byte* data)
        {
            Left = *(short*)data;
            Top = *(short*)(data + 2);
            Width = *(short*)(data + 4);
            Height = *(short*)(data + 6);
            CharacterWidth = *(data + 8);
            CharacterHeight = *(data + 9);
            ColorIndex = *(data + 10);
            BlackgroundColorIndex = *(data + 11);
        }
    }
}
