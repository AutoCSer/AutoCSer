using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 图形处置方法
    /// </summary>
    public enum PraphicControlMethodType : byte
    {
        /// <summary>
        /// 不使用处置方法
        /// </summary>
        None = 0,
        /// <summary>
        /// 不处置图形，保留当前的图像，再绘制一帧图像在上面
        /// </summary>
        KeepCurrent = 1,
        /// <summary>
        /// 回复到背景色
        /// </summary>
        BackgroundColor = 2,
        /// <summary>
        /// 回复到先前状态
        /// </summary>
        PreviousState = 3,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom4 = 4,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom5 = 5,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom6 = 6,
        /// <summary>
        /// 自定义
        /// </summary>
        Custom7 = 7
    }
}
