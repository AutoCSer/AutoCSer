using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.MemberCopy
{
    /// <summary>
    /// 成员复制公共配置
    /// </summary>
    internal sealed class Common
    {
        /// <summary>
        /// 对象浅复制
        /// </summary>
        internal static readonly Func<object, object> CallMemberwiseClone = (Func<object, object>)Delegate.CreateDelegate(typeof(Func<object, object>), ((Func<object>)new Common().MemberwiseClone).Method);
    }
}
