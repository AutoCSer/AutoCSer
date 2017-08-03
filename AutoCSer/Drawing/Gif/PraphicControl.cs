using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 图形控制扩展
    /// </summary>
    public sealed class PraphicControl
    {
        /// <summary>
        /// 延迟时间，单位1/100秒
        /// </summary>
        public short DelayTime { get; private set; }
        /// <summary>
        /// 透明色索引值
        /// </summary>
        public byte TransparentColorIndex { get; private set; }
        /// <summary>
        /// 是否使用使用透明颜色
        /// </summary>
        public byte IsTransparentColor { get; private set; }
        /// <summary>
        /// 用户输入标志，指出是否期待用户有输入之后才继续进行下去，置位表示期待，值否表示不期待。
        /// </summary>
        public byte IsUseInput { get; private set; }
        /// <summary>
        /// 图形处置方法
        /// </summary>
        public PraphicControlMethodType MethodType { get; private set; }
        /// <summary>
        /// 图形控制扩展
        /// </summary>
        /// <param name="data">当前解析数据</param>
        internal unsafe PraphicControl(byte* data)
        {
            byte flag = *data;
            MethodType = (PraphicControlMethodType)((flag >> 2) & 7);
            IsUseInput = (byte)(flag & 2);
            IsTransparentColor = (byte)(flag & 1);
            DelayTime = *(short*)(data + 1);
            TransparentColorIndex = *(data + 3);
        }
    }
}
