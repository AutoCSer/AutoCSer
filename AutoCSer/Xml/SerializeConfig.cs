using System;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML 序列化配置参数
    /// </summary>
    public sealed class SerializeConfig
    {
        /// <summary>
        /// 循环引用检测深度
        /// </summary>
        public const int DefaultCheckLoopDepth = 64;
        /// <summary>
        /// XML头部
        /// </summary>
        public string Header = @"<?xml version=""1.0"" encoding=""utf-8""?>";
        /// <summary>
        /// 根节点名称
        /// </summary>
        public string BootNodeName = "xml";
        /// <summary>
        /// 集合子节点名称
        /// </summary>
        public string ItemName = "item";
        /// <summary>
        /// 成员位图
        /// </summary>
        public MemberMap MemberMap;
        /// <summary>
        /// 循环引用检测深度，0 表示实时检测，默认为 64
        /// </summary>
        public int CheckLoopDepth;
        /// <summary>
        /// 是否输出空对象
        /// </summary>
        public bool IsOutputNull;
        /// <summary>
        /// 是否输出长度为 0 的字符串，默认为 true
        /// </summary>
        public bool IsOutputEmptyString = true;
        /// <summary>
        /// 成员位图类型不匹配时是否使用默认输出，默认为 true
        /// </summary>
        public bool IsMemberMapErrorToDefault = true;
        /// <summary>
        /// 获取并设置自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">序列化成员位图</param>
        /// <returns>序列化成员位图</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public MemberMap SetCustomMemberMap(MemberMap memberMap)
        {
            MemberMap oldMemberMap = MemberMap;
            MemberMap = memberMap;
            return oldMemberMap;
        }
    }
}
