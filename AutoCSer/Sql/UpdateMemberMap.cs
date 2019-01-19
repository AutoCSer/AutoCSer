using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据更新成员位图
    /// </summary>
    /// <typeparam name="tableType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    public abstract class UpdateMemberMap<tableType, modelType>
        where tableType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        protected tableType value;
        /// <summary>
        /// 成员位图
        /// </summary>
        protected AutoCSer.Metadata.MemberMap<modelType> memberMap;
        /// <summary>
        /// 数据更新成员位图
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">成员位图</param>
        protected UpdateMemberMap(tableType value, AutoCSer.Metadata.MemberMap<modelType> memberMap)
        {
            this.value = value ?? AutoCSer.Emit.Constructor<tableType>.New();
            this.memberMap = memberMap;
        }
        /// <summary>
        /// 设置成员索引
        /// </summary>
        /// <param name="memberIndex">成员索引</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void setMember(AutoCSer.Metadata.MemberMap<modelType>.MemberIndex memberIndex)
        {
            if (memberMap == null) memberMap = AutoCSer.Metadata.MemberMap<modelType>.NewEmpty();
            memberIndex.SetMember(memberMap);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="table">数据表格</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>更新是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]

        protected bool update(Table<tableType, modelType> table, bool isIgnoreTransaction)
        {
            return memberMap != null && table.UpdateQueue(value, memberMap, isIgnoreTransaction);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="table">数据表格</param>
        /// <param name="onUpdated">更新数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void update(Table<tableType, modelType> table, Action<tableType> onUpdated, bool isIgnoreTransaction)
        {
            if (memberMap == null) onUpdated(null);
            table.UpdateQueue(value, memberMap, onUpdated, isIgnoreTransaction);
        }
    }
}
