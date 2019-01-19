using System;
using AutoCSer.Metadata;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化成员配置
    /// </summary>
    public class SerializeMemberAttribute : AutoCSer.Metadata.IgnoreMemberAttribute
    {
        /// <summary>
        /// 全局版本编号（添加字段）
        /// </summary>
        public uint GlobalVersion;
        /// <summary>
        /// 全局版本编号（删除字段），大于添加字段全局版本编号时有效 ，静态字段不能用 public 修饰
        /// </summary>
        public uint RemoveGlobalVersion;
        /// <summary>
        /// 字段是否已经被删除
        /// </summary>
        internal bool IsRemove
        {
            get { return RemoveGlobalVersion > GlobalVersion; }
        }
        /// <summary>
        /// 默认为 true 表示字段删除前用 public 修饰
        /// </summary>
        public bool IsRemovePublic = true;
        /// <summary>
        /// 是否采用 JSON 混合序列化
        /// </summary>
        public bool IsJson;
        /// <summary>
        /// 是否采用 JSON 混合序列化
        /// </summary>
        internal virtual bool GetIsJson
        {
            get { return IsJson; }
        }

        /// <summary>
        /// 默认空配置
        /// </summary>
        internal static readonly SerializeMemberAttribute Null = new SerializeMemberAttribute();
    }
}
