using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericTypeBase
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal abstract Type CurrentType { get; }

        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected static HashType getCurrentType(GenericTypeBase type)
        {
            return type.CurrentType;
        }
    }
}
