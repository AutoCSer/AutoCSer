using System;
using System.Threading;
using System.Collections.Generic;
using AutoCSer.Metadata;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL列转换
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ColumnBuilder
    {
        /// <summary>
        /// SQL 客户端操作
        /// </summary>
        public Client Client;
        /// <summary>
        /// 数据列集合
        /// </summary>
        public LeftArray<Column> Columns;
        /// <summary>
        /// SQL列转换
        /// </summary>
        /// <param name="column">数据列</param>
        public void Append(Column column)
        {
            if (column.SqlColumnType == null) Columns.Add(column);
            else
            {
                foreach (Column sqlColumn in get(column.SqlColumnType))
                {
                    Column copyColumn = AutoCSer.MemberCopy.Copyer<Column>.MemberwiseClone(sqlColumn);
                    copyColumn.Name = column.Name + "_" + copyColumn.Name;
                    Columns.Add(copyColumn);
                }
            }
        }
        /// <summary>
        /// 获取SQL列转换集合
        /// </summary>
        /// <param name="type">SQL列类型</param>
        /// <returns>SQL列转换集合</returns>
        private Column[] get(Type type)
        {
            Column[] columns;
            Monitor.Enter(sqlColumnLock);
            try
            {
                columns = getNoLock(type);
            }
            finally { Monitor.Exit(sqlColumnLock); }
            return columns;
        }
        /// <summary>
        /// 获取SQL列转换集合
        /// </summary>
        /// <param name="type">SQL列类型</param>
        /// <returns>SQL列转换集合</returns>
        private Column[] getNoLock(Type type)
        {
            Column[] columns;
            if (!sqlColumns.TryGetValue(type, out columns))
            {
                int index = Columns.Length;
                append(type);
                sqlColumns.Add(type, columns = new SubArray<Column> { Array = Columns.Array, Start = index, Length = Columns.Length - index }.GetArray());
                Columns.Length = index;
            }
            return columns;
        }
        /// <summary>
        /// 添加SQL列类型
        /// </summary>
        /// <param name="type">SQL列类型</param>
        private void append(Type type)
        {
            foreach (KeyValue<MemberIndexInfo, MemberAttribute> member in Field.GetMemberIndexs(type, TypeAttribute.GetAttribute<ColumnAttribute>(type, false)))
            {
                Column column = Client.GetColumn(member.Key.Member.Name, member.Key.MemberSystemType, member.Value);
                if (column.SqlColumnType == null) Columns.Add(column);
                else
                {
                    foreach (Column sqlColumn in getNoLock(column.SqlColumnType))
                    {
                        Column copyColumn = AutoCSer.MemberCopy.Copyer<Column>.MemberwiseClone(sqlColumn);
                        copyColumn.Name = column.Name + "_" + copyColumn.Name;
                        Columns.Add(copyColumn);
                    }
                }
            }
        }
        /// <summary>
        /// SQL列转换类型集合
        /// </summary>
        private static Dictionary<Type, Column[]> sqlColumns = DictionaryCreator.CreateOnly<Type, Column[]>();
        /// <summary>
        /// SQL列转换类型集合访问锁
        /// </summary>
        private static readonly object sqlColumnLock = new object();
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            Monitor.Enter(sqlColumnLock);
            try
            {
                if (sqlColumns.Count != 0) sqlColumns = DictionaryCreator.CreateOnly<Type, Column[]>();
            }
            finally { Monitor.Exit(sqlColumnLock); }
        }
        static ColumnBuilder()
        {
            Pub.ClearCaches += clearCache;
        }
    }
}
