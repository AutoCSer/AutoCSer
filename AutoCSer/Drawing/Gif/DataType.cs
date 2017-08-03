using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType : byte
    {
        /// <summary>
        /// 图像块
        /// </summary>
        Image,
        /// <summary>
        /// 图形控制扩展(需要89a版本)
        /// </summary>
        GraphicControl,
        /// <summary>
        /// 图形文本扩展(需要89a版本)
        /// </summary>
        PlainText,
        /// <summary>
        /// 注释扩展(需要89a版本)
        /// </summary>
        Comment,
        /// <summary>
        /// 应用程序扩展(需要89a版本)
        /// </summary>
        Application,
    }
}
