using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 禁止安装属性
    /// </summary>
    public abstract class IgnoreMemberAttribute : Attribute
    {
        /// <summary>
        /// 是否禁止当前安装
        /// </summary>
        public bool IsIgnoreCurrent;
        /// <summary>
        /// 是否禁止当前安装
        /// </summary>
        internal virtual bool GetIsIgnoreCurrent
        {
            get { return IsIgnoreCurrent; }
        }
        /// <summary>
        /// 是否安装[AutoCSer.code]
        /// </summary>
        public bool IsSetup
        {
            get
            {
                return !GetIsIgnoreCurrent;
            }
        }
    }
}
