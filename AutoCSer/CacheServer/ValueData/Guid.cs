using System;

namespace AutoCSer.CacheServer.ValueData
{
    /// <summary>
    /// Guid 参数
    /// </summary>
    internal sealed class Guid
    {
        /// <summary>
        /// Guid
        /// </summary>
        internal System.Guid Value;
        /// <summary>
        /// Guid 参数
        /// </summary>
        /// <param name="value">Guid</param>
        internal Guid(System.Guid value)
        {
            Value = value;
        }
        /// <summary>
        /// 默认值
        /// </summary>
        internal static readonly Guid Default = new Guid(default(System.Guid));
    }
}
