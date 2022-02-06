using System;

namespace AutoCSer
{
    /// <summary>
    /// 0 长度空数组
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public static class EmptyArray<T>
    {
        /// <summary>
        /// 0元素数组（严重警告，禁止对该对象进行 Array.Resize 操作，在无法保证的场景禁止使用）
        /// </summary>
        public static readonly T[] Array = new T[0];
        ///// <summary>
        ///// 默认数组元素
        ///// </summary>
        //public static readonly T EmptyValue = (new T[1])[0];
    }
}
