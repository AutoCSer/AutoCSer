using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 值类型序列化包装处理
    /// </summary>
    public class BoxSerializeAttribute : Attribute
    {
        ///// <summary>
        ///// 二进制数据序列化
        ///// </summary>
        //public bool IsData = true;
        /// <summary>
        /// JSON序列化
        /// </summary>
        public bool IsJson = true;
        /// <summary>
        /// XML序列化
        /// </summary>
        public bool IsXml = true;
        /// <summary>
        /// 值类型序列化包装处理
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        public BoxSerializeAttribute() { }
    }
}
