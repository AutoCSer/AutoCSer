﻿using AutoCSer.Memory;
using System;
using System.Threading;

namespace AutoCSer.Sql.MsSql
{
    /// <summary>
    /// SQL Server 2005 客户端
    /// </summary>
    internal sealed unsafe class Sql2005 : Sql2000
    {
        /// <summary>
        /// 排序名称
        /// </summary>
        private const string orderOverName = "_ROW_";
        /// <summary>
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal Sql2005(Connection connection) : base(connection) { }
        /// <summary>
        /// 获取表格名称的SQL语句
        /// </summary>
        protected override string getTableNameSql
        {
            get
            {
                return "select name from sysobjects where objectproperty(id,'IsUserTable')=1";
            }
        }
        /// <summary>
        /// 查询对象集合
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="createQuery"></param>
        /// <param name="query">查询信息</param>
        /// <param name="keyName">关键之名称</param>
        /// <param name="sqlStream"></param>
        private unsafe void selectKeys<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query, string keyName, CharStream sqlStream)
            where valueType : class, modelType
            where modelType : class
        {
            sqlStream.SimpleWrite("select ");
            if (query.MemberMap != null) DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
            else sqlStream.Write('*');
            sqlStream.SimpleWrite(" from[");
            sqlStream.SimpleWrite(sqlTool.TableName);
            sqlStream.Write("]with(nolock)where ");
            sqlStream.SimpleWrite(keyName);
            sqlStream.SimpleWrite(" in(select ");
            sqlStream.SimpleWrite(keyName);
            sqlStream.SimpleWrite(" from(select ");
            sqlStream.SimpleWrite(keyName);
            sqlStream.Write(",row_number()over(");
            int startIndex = sqlStream.Length;
            createQuery.WriteOrder(sqlTool, sqlStream, constantConverter, ref query);
            string fieldName, fieldSqlName;
            query.GetIndex(out fieldName, out fieldSqlName);
            int count = sqlStream.Length - startIndex;
            sqlStream.SimpleWrite(")as ");
            sqlStream.SimpleWrite(orderOverName);
            sqlStream.SimpleWrite(" from[");
            sqlStream.SimpleWrite(sqlTool.TableName);
            sqlStream.SimpleWrite("]with(nolock)");
            sqlTool.Client.GetSql(createQuery.Where.Expression, sqlStream, ref query);
            if (query.IndexFieldName == null) query.SetIndex(fieldName, fieldSqlName);
            sqlStream.SimpleWrite(")as T where ");
            sqlStream.SimpleWrite(orderOverName);
            sqlStream.SimpleWrite(" between ");
            AutoCSer.Extensions.NumberExtension.ToString(query.SkipCount, sqlStream);
            sqlStream.SimpleWrite(" and ");
            AutoCSer.Extensions.NumberExtension.ToString(query.SkipCount + createQuery.GetCount - 1, sqlStream);
            sqlStream.Write(')');
            if (count != 0) sqlStream.Write(sqlStream.Char + startIndex, count);
        }
        /// <summary>
        /// 查询对象集合
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="createQuery"></param>
        /// <param name="query">查询信息</param>
        /// <param name="sqlStream"></param>
        private unsafe void selectRows<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query, CharStream sqlStream)
            where valueType : class, modelType
            where modelType : class
        {
            sqlStream.Write("select * from(select ");
            if (query.MemberMap != null) DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
            else sqlStream.Write('*');
            sqlStream.Write(",row_number()over(");
            createQuery.WriteOrder(sqlTool, sqlStream, constantConverter, ref query);
            string fieldName, fieldSqlName;
            query.GetIndex(out fieldName, out fieldSqlName);
            sqlStream.SimpleWrite(")as ");
            sqlStream.SimpleWrite(orderOverName);
            sqlStream.SimpleWrite(" from[");
            sqlStream.SimpleWrite(sqlTool.TableName);
            sqlStream.SimpleWrite("]with(nolock)");
            sqlTool.Client.GetSql(createQuery.Where.Expression, sqlStream, ref query);
            if (query.IndexFieldName == null) query.SetIndex(fieldName, fieldSqlName);
            sqlStream.SimpleWrite(")as T where ");
            sqlStream.SimpleWrite(orderOverName);
            sqlStream.SimpleWrite(" between ");
            AutoCSer.Extensions.NumberExtension.ToString(query.SkipCount, sqlStream);
            sqlStream.SimpleWrite(" and ");
            AutoCSer.Extensions.NumberExtension.ToString(query.SkipCount + createQuery.GetCount - 1, sqlStream);
        }
        /// <summary>
        /// 查询对象集合
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="createQuery"></param>
        /// <param name="query">查询信息</param>
        /// <param name="sqlStream"></param>
        private unsafe void selectNoOrder<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query, CharStream sqlStream)
            where valueType : class, modelType
            where modelType : class
        {
            sqlStream.SimpleWrite("select ");
            int count = query.SkipCount + createQuery.GetCount;
            if (count != 0)
            {
                sqlStream.SimpleWrite("top ");
                AutoCSer.Extensions.NumberExtension.ToString(count, sqlStream);
                sqlStream.Write(' ');
            }
            if (query.MemberMap != null) DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
            else sqlStream.Write('*');
            sqlStream.SimpleWrite(" from [");
            sqlStream.SimpleWrite(sqlTool.TableName);
            sqlStream.SimpleWrite("]with(nolock)");
            createQuery.WriteWhere(sqlTool, sqlStream, ref query);
            createQuery.WriteOrder(sqlTool, sqlStream, constantConverter, ref query);
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="createQuery"></param>
        /// <param name="query">查询信息</param>
        /// <returns>对象集合</returns>
        internal override void GetSelectQuery<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query)
        {
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                if (query.SkipCount != 0 && createQuery.IsOrder)
                {
                    if (DataModel.Model<modelType>.PrimaryKeys.Length == 1) selectKeys(sqlTool, ref createQuery, ref query, constantConverter.ConvertName(DataModel.Model<modelType>.PrimaryKeys[0].FieldInfo.Name), sqlStream);
                    else if (DataModel.Model<modelType>.Identity != null) selectKeys(sqlTool, ref createQuery, ref query, constantConverter.ConvertName(DataModel.Model<modelType>.Identity.FieldInfo.Name), sqlStream);
                    else selectRows(sqlTool, ref createQuery, ref query, sqlStream);
                }
                else selectNoOrder(sqlTool, ref createQuery, ref query, sqlStream);
                query.Sql = sqlStream.ToString();
            }
            finally
            {
                UnmanagedPool.Default.Push(ref buffer);
                sqlStream.Dispose();
                Interlocked.Exchange(ref this.sqlStream, sqlStream);
            }
        }
        /// <summary>
        /// 根据表格名称获取表格信息的SQL语句
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息的SQL语句</returns>
        protected override string getTableSql(string tableName)
        {
            return @"declare @id int
set @id=object_id(N'[dbo].[" + tableName + @"]')
if(select top 1 id from sysobjects where id=@id and objectproperty(id,N'IsUserTable')=1)is not null begin
 select columnproperty(id,name,'IsIdentity')as isidentity,id,xusertype,name,length,xprec,xscale,isnullable,colid,isnull((select top 1 text from syscomments where id=syscolumns.cdefault and colid=1),'')as defaultValue
  ,isnull((select value from ::fn_listextendedproperty(null,'user','dbo','table','" + tableName + @"','column',syscolumns.name)as property where property.name='MS_Description'),'')as remark
  from syscolumns where id=@id order by colid
 if @@rowcount<>0 begin
  select a.indid,a.colid,b.name,(case when b.status=2 then 'UQ' else(select top 1 xtype from sysobjects where name=b.name)end)as type from sysindexkeys a left join sysindexes b on a.id=b.id and a.indid=b.indid where a.id=@id order by a.indid,a.keyno
 end
end";
            //备注
            //"select top 1 value from ::fn_listextendedproperty(null,'user','dbo','table','" + tableName + "','column','" + reader["name"].ToString() + "')as property where property.name='MS_Description'"
        }
    }
}
