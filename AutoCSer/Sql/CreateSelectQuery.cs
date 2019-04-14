using System;
using System.Linq.Expressions;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 创建查询信息
    /// </summary>
    /// <typeparam name="modelType">数据模型类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CreateSelectQuery<modelType>
        where modelType : class
    {
        /// <summary>
        /// 查询条件表达式
        /// </summary>
        internal Expression<Func<modelType, bool>> Where;
        /// <summary>
        /// 排序表达式集合,false为升序,true为降序
        /// </summary>
        internal KeyValue<LambdaExpression, bool>[] Orders;
        /// <summary>
        /// 排序字符串集合,false为升序,true为降序
        /// </summary>
        internal KeyValue<Field, bool>[] SqlFieldOrders;
        /// <summary>
        ///  是否需要排序
        /// </summary>
        internal bool IsOrder
        {
            get { return Orders != null || SqlFieldOrders != null; }
        }
        /// <summary>
        /// 获取记录数量,0表示不限
        /// </summary>
        internal int GetCount;
        /// <summary>
        /// 创建查询信息
        /// </summary>
        /// <param name="where"></param>
        internal CreateSelectQuery(Expression<Func<modelType, bool>> where)
        {
            Where = where;
            Orders = null;
            SqlFieldOrders = null;
            GetCount = 0;
        }
        /// <summary>
        /// 创建查询信息
        /// </summary>
        /// <param name="getCount"></param>
        /// <param name="order"></param>
        internal CreateSelectQuery(int getCount, KeyValue<Field, bool> order)
        {
            Where = null;
            Orders = null;
            SqlFieldOrders = null;
            GetCount = getCount;
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="sqlTable">数据库表格操作工具</param>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="query"></param>
        internal unsafe void WriteWhere(Table sqlTable, CharStream sqlStream, ref SelectQuery<modelType> query)
        {
            if (Where != null)
            {
                sqlStream.PrepLength(6);
                sqlStream.ByteSize += 6 * sizeof(char);
                int length = sqlStream.Length;
                sqlTable.Client.GetSql(Where, sqlStream, ref query);
                if (length == sqlStream.Length) sqlStream.ByteSize -= 6 * sizeof(char);
                else
                {
                    byte* where = (byte*)(sqlStream.Char + length);
                    *(uint*)(where - sizeof(uint)) = 'e' + (' ' << 16);
                    *(uint*)(where - sizeof(uint) * 2) = 'e' + ('r' << 16);
                    *(uint*)(where - sizeof(uint) * 3) = 'w' + ('h' << 16);
                }
            }
        }
        /// <summary>
        /// 排序字符串
        /// </summary>
        /// <param name="sqlTable">数据库表格操作工具</param>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="constantConverter"></param>
        /// <param name="query"></param>
        internal void WriteOrder(Table sqlTable, CharStream sqlStream, ConstantConverter constantConverter, ref SelectQuery<modelType> query)
        {
            if (Orders != null)
            {
                int isNext = 0;
                sqlStream.SimpleWriteNotNull(" order by ");
                foreach (KeyValue<LambdaExpression, bool> order in Orders)
                {
                    if (isNext == 0) isNext = 1;
                    else sqlStream.Write(',');
                    if (order.Key == null) throw new ArgumentNullException();
                    sqlTable.Client.GetSql(order.Key, sqlStream, ref query);
                    if (order.Value) sqlStream.SimpleWriteNotNull(" desc");
                }
            }
            else if (SqlFieldOrders != null)
            {
                int isNext = 0;
                sqlStream.SimpleWriteNotNull(" order by ");
                foreach (KeyValue<Field, bool> order in SqlFieldOrders)
                {
                    if (isNext == 0)
                    {
                        if (query.IndexFieldName == null) query.IndexFieldSqlName = constantConverter.ConvertName(query.IndexFieldName = order.Key.FieldInfo.Name);
                        isNext = 1;
                    }
                    else sqlStream.Write(',');
                    constantConverter.ConvertNameToSqlStream(sqlStream, order.Key.FieldInfo.Name);
                    if (order.Value) sqlStream.SimpleWriteNotNull(" desc");
                }
            }
        }
    }
}
