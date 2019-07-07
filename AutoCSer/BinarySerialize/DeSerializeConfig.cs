using System;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 反序列化配置参数
    /// </summary>
    public sealed class DeSerializeConfig
    {
        /// <summary>
        /// 数据是否完整
        /// </summary>
        public bool IsFullData = true;
        /// <summary>
        /// 最大数组长度
        /// </summary>
        public int MaxArraySize = int.MaxValue;
        /// <summary>
        /// 是否自动释放成员位图
        /// </summary>
        internal bool IsDisposeMemberMap;
    }
}
