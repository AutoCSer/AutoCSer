using System;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化类型配置
    /// </summary>
    public class SerializeAttribute : MemberFilterAttribute.InstanceField
    {
        /// <summary>
        /// 是否作用于未知派生类型，默认为 true
        /// </summary>
        public bool IsBaseType = true;
        /// <summary>
        /// 当没有 JSON 序列化成员时是否预留 JSON 序列化标记，默认为 false
        /// </summary>
        public bool IsJson;
        /// <summary>
        /// 当没有 JSON 序列化成员时是否预留 JSON 序列化标记，默认为 false
        /// </summary>
        internal bool GetIsJson
        {
            get { return IsJson & GetIsMemberMap; }
        }
        /// <summary>
        /// 是否检测相同的引用成员(作为根节点时有效)，默认为 true
        /// </summary>
        public bool IsReferenceMember = true;
        /// <summary>
        /// 是否序列化成员位图，默认为 true 在 IsAnonymousFields 为 false 时生效
        /// </summary>
        public bool IsMemberMap = true;
        /// <summary>
        /// 是否选择匿名字段，默认为 false
        /// </summary>
        public bool IsAnonymousFields;
        /// <summary>
        /// 是否序列化成员位图
        /// </summary>
        internal bool GetIsMemberMap
        {
            get { return IsMemberMap & !IsAnonymousFields; }
        }
        /// <summary>
        /// 二进制数据序列化类型配置
        /// </summary>
        public SerializeAttribute() { }
        /// <summary>
        /// 二进制数据序列化类型配置
        /// </summary>
        /// <param name="isReferenceMember">是否检测相同的引用成员</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal SerializeAttribute(bool isReferenceMember) 
        {
            IsMemberMap = false;
            IsReferenceMember = isReferenceMember;
        }
    }
}
