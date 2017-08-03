using System;
using System.Drawing;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 位图信息
    /// </summary>
    internal sealed class BitmapInfo : AutoCSer.Threading.Link<BitmapInfo>
    {
        /// <summary>
        /// 位图
        /// </summary>
        internal Bitmap Bitmap;
        /// <summary>
        /// 跳图数量
        /// </summary>
        internal int KeepCount;
        /// <summary>
        /// 获取下一个位图信息
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="keepCount"></param>
        /// <returns></returns>
        internal BitmapInfo Next(out Bitmap bitmap, out int keepCount)
        {
            bitmap = Bitmap;
            keepCount = KeepCount;
            return LinkNext;
        }
    }
}
