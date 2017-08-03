using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 位图填充
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal unsafe struct ImageFillBitmap
    {
        /// <summary>
        /// 当前颜色索引
        /// </summary>
        internal byte* CurrentIndex;
        /// <summary>
        /// 颜色列表
        /// </summary>
        internal Color* Colors;
        /// <summary>
        /// 图像宽度
        /// </summary>
        internal int Width;
        /// <summary>
        /// 填充颜色列表
        /// </summary>
        /// <param name="height">填充行数</param>
        /// <param name="bitmap">位图当前填充位置</param>
        /// <param name="bitMapSpace">位图填充留空</param>
        internal void FillColor(int height, byte* bitmap, int bitMapSpace)
        {
            byte* row = CurrentIndex;
            for (byte* rowEnd = CurrentIndex + Width * height; row != rowEnd; bitmap += bitMapSpace)
            {
                byte* col = row;
                for (row += Width; col != row; ++col)
                {
                    Color color = Colors[*col];
                    *bitmap++ = color.Blue;
                    *bitmap++ = color.Green;
                    *bitmap++ = color.Red;
                }
            }
            CurrentIndex = row;
        }
        /// <summary>
        /// 填充颜色索引
        /// </summary>
        /// <param name="height">填充行数</param>
        /// <param name="bitmap">位图当前填充位置</param>
        /// <param name="bitMapSpace">位图填充留空</param>
        internal void FillIndex(int height, byte* bitmap, int bitMapSpace)
        {
            byte* row = CurrentIndex;
            for (byte* rowEnd = CurrentIndex + Width * height; row != rowEnd; bitmap += bitMapSpace)
            {
                byte* col = row;
                for (row += Width; col != row; ++col)
                {
                    byte color = *col;
                    *bitmap++ = color;
                    *bitmap++ = color;
                    *bitmap++ = color;
                }
            }
            CurrentIndex = row;
        }
    }
}
