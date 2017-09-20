using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 远程函数配置
    /// </summary>
    public sealed class RemoteMethodAttribute : RemoteMemberAttribute
    {
        /// <summary>
        /// 是否生成函数
        /// </summary>
        internal override bool IsMethod
        {
            get { return true; }
        }
    }
}
