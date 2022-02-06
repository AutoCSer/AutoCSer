using System;
using AutoCSer.Metadata;

namespace AutoCSer
{
    /// <summary>
    /// JSON 解析成员配置
    /// </summary>
    public class JsonDeSerializeMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 是否默认解析成员
        /// </summary>
        public bool IsDefault;
        ///// <summary>
        ///// JSON 解析成员配置
        ///// </summary>
        //public DeSerializeMemberAttribute() { }
        ///// <summary>
        ///// JSON 解析成员配置
        ///// </summary>
        ///// <param name="isIgnoreCurrent">是否禁止当前安装</param>
        //internal DeSerializeMemberAttribute(bool isIgnoreCurrent)
        //{
        //    IsIgnoreCurrent = isIgnoreCurrent;
        //}
    }
}
