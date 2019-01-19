using System;

namespace AutoCSer
{
    /// <summary>
    /// 空值相关参数
    /// </summary>
    /// <typeparam name="valueType">值类型</typeparam>
    public static class NullValue<valueType>
    {
        ///// <summary>
        ///// 默认空值
        ///// </summary>
        //public static readonly valueType Value = default(valueType);
        /// <summary>
        /// 0元素数组
        /// </summary>
        public static readonly valueType[] Array = new valueType[0];
    }
}
