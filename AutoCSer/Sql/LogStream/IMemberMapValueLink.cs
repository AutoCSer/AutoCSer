using System;

namespace AutoCSer.Sql.LogStream
{
    /// <summary>
    /// 成员位图绑定对象链表
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public interface IMemberMapValueLink<valueType>
        where valueType : class
    {
        /// <summary>
        /// 下一个节点
        /// </summary>
        valueType MemberMapValueLink { get; set; }
    }
}
