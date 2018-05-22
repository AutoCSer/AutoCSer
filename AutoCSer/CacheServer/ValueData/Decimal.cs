using System;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// decimal 参数
    /// </summary>
    internal sealed class Decimal
    {
        /// <summary>
        /// decimal
        /// </summary>
        internal decimal Value;
        /// <summary>
        /// decimal 参数
        /// </summary>
        /// <param name="value">decimal</param>
        internal Decimal(decimal value)
        {
            Value = value;
        }
        /// <summary>
        /// 默认值
        /// </summary>
        internal static readonly Decimal Default = new Decimal(0);
    }
}
