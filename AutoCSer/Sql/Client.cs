using System;
using AutoCSer.Metadata;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Common;
using AutoCSer.Extension;
using System.Data;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL 客户端操作
    /// </summary>
    public abstract unsafe class Client
    {
        /// <summary>
        /// Sql 字符流
        /// </summary>
        protected CharStream sqlStream;
        /// <summary>
        /// SQL连接信息
        /// </summary>
        internal Connection Connection;
        /// <summary>
        /// 连接池
        /// </summary>
        internal ConnectionPool ConnectionPool;
        /// <summary>
        /// 时间间隔毫秒数
        /// </summary>
        public virtual int NowTimeMilliseconds
        {
            get { return MsSql.Sql2000.DefaultNowTimeMilliseconds; }
        }
        /// <summary>
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        protected Client(Connection connection)
        {
            Connection = connection;
            ConnectionPool = ConnectionPool.Get(connection.Attribute.ClientType, connection.ConnectionString, connection.IsPool);
        }
        /// <summary>
        /// 获取 SQL 连接
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal DbConnection GetConnection()
        {
            DbConnection connection = ConnectionPool.Pop();
            if (connection == null)
            {
                Exception exception = null;
                try
                {
                    createConnection(ref connection);
                }
                catch (Exception error)
                {
                    exception = error;
                }
                if (exception != null)
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                    AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Error, exception);
                }
            }
            return connection;
        }
        /// <summary>
        /// 创建 SQL 连接
        /// </summary>
        /// <param name="connection"></param>
        protected abstract void createConnection(ref DbConnection connection);
        /// <summary>
        /// 释放 SQL 连接
        /// </summary>
        /// <param name="connection"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void FreeConnection(ref DbConnection connection)
        {
            ConnectionPool.Push(ref connection);
        }
        /// <summary>
        /// 关闭错误连接并重新获取连接
        /// </summary>
        /// <param name="connection"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CloseErrorConnection(ref DbConnection connection)
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = GetConnection();
            }
        }
        /// <summary>
        /// 创建一个事务处理对象
        /// </summary>
        /// <param name="isolationLevel">默认为 RepeatableRead</param>
        /// <returns></returns>
        public Transaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
        {
            return new Transaction(this, isolationLevel);
        }
        /// <summary>
        /// 获取SQL命令
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL语句</param>
        /// <returns>SQL命令</returns>
        protected abstract DbCommand getCommand(DbConnection connection, string sql);
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        protected int executeNonQuery(ref DbConnection connection, string sql)
        {
            if (connection == null)
            {
                connection = GetConnection();
                if (connection == null) return int.MinValue;
            }
            using (DbCommand command = getCommand(connection, sql)) return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected int executeNonQuery(DbConnection connection, string sql)
        {
            using (DbCommand command = getCommand(connection, sql)) return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="transaction">事务操作</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        protected int executeNonQuery(Transaction transaction, string sql)
        {
            using (DbCommand command = getCommand(transaction.Connection, sql))
            {
                command.Transaction = transaction.DbTransaction;
                return command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 获取数据集并关闭SQL命令
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据集</returns>
        protected DataSet getDataSet(DbCommand command)
        {
            DbDataAdapter adapter = getAdapter(command);
            if (adapter != null)
            {
                DataSet data = new DataSet();
                adapter.Fill(data);
                return data;
            }
            return null;
        }
        /// <summary>
        /// 获取数据适配器
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据适配器</returns>
        protected abstract DbDataAdapter getAdapter(DbCommand command);
        /// <summary>
        /// 成员信息转换为数据列
        /// </summary>
        /// <param name="name">成员名称</param>
        /// <param name="type">成员类型</param>
        /// <param name="memberAttribute">SQL成员信息</param>
        /// <returns>数据列</returns>
        internal Column GetColumn(string name, Type type, MemberAttribute memberAttribute)
        {
            Column column = TypeAttribute.GetAttribute<ColumnAttribute>(type, false) == null ? GetColumn(type, memberAttribute) : new Column { SqlColumnType = type };
            column.Name = name;
            return column;
        }
        /// <summary>
        /// 成员信息转换为数据列
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="memberAttribute">SQL成员信息</param>
        /// <returns>数据列</returns>
        internal abstract Column GetColumn(Type type, MemberAttribute memberAttribute);
        /// <summary>
        /// SQL列转换
        /// </summary>
        /// <param name="table">表格信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void ToSqlColumn(TableColumnCollection table)
        {
            table.Columns.Columns = ToSqlColumn(table.Columns.Columns);
        }
        /// <summary>
        /// SQL列转换
        /// </summary>
        /// <param name="columns">数据列集合</param>
        /// <returns></returns>
        internal Column[] ToSqlColumn(Column[] columns)
        {
            if (columns.any(column => column.SqlColumnType != null))
            {
                ColumnBuilder sqlColumn = new ColumnBuilder { Client = this };
                sqlColumn.Columns.PrepLength(columns.Length << 1);
                foreach (Column column in columns) sqlColumn.Append(column);
                return sqlColumn.Columns.ToArray();
            }
            return columns;
        }
        /// <summary>
        /// 查询单值数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal virtual object GetValue(DbConnection connection, string sql)
        {
            using (DbCommand command = getCommand(connection, sql))
            {
                object value = command.ExecuteScalar();
                if (value != null && value != DBNull.Value) return value;
            }
            return null;
        }
        /// <summary>
        /// 根据表格名称获取表格信息
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息</returns>
        internal abstract TableColumnCollection GetTable(DbConnection connection, string tableName);
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="table">表格信息</param>
        internal abstract void CreateTable(DbConnection connection, TableColumnCollection table);
        /// <summary>
        /// 是否支持索引
        /// </summary>
        internal virtual bool IsIndex
        {
            get { return true; }
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        internal abstract void CreateIndex(DbConnection connection, string tableName, ColumnCollection columnCollection);
        /// <summary>
        /// 写入索引名称
        /// </summary>
        /// <param name="sqlStream">SQL语句流</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        internal static void AppendIndexName(CharStream sqlStream, string tableName, ColumnCollection columnCollection)
        {
            if (string.IsNullOrEmpty(columnCollection.Name))
            {
                sqlStream.SimpleWriteNotNull("ix_");
                sqlStream.SimpleWriteNotNull(tableName);
                foreach (Column column in columnCollection.Columns)
                {
                    sqlStream.Write('_');
                    sqlStream.SimpleWriteNotNull(column.Name);
                }
            }
            else sqlStream.SimpleWriteNotNull(columnCollection.Name);
        }
        /// <summary>
        /// 删除列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">删除列集合</param>
        internal abstract void DeleteFields(DbConnection connection, ColumnCollection columnCollection);
        /// <summary>
        /// 是否支持新增列
        /// </summary>
        internal virtual bool IsAddField
        {
            get { return true; }
        }
        /// <summary>
        /// 新增列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">新增列集合</param>
        internal abstract void AddFields(DbConnection connection, ColumnCollection columnCollection);
        /// <summary>
        /// 委托关联表达式转SQL表达式
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="query">查询信息</param>
        internal virtual void GetSql<modelType>(LambdaExpression expression, CharStream sqlStream, ref SelectQuery<modelType> query)
            where modelType : class
        {
            AutoCSer.Sql.MsSql.ExpressionConverter expressionConverter = new AutoCSer.Sql.MsSql.ExpressionConverter { SqlStream = sqlStream };
            expressionConverter.Convert(expression.Body);
            if (query.IndexFieldName == null) query.SetIndex(expressionConverter.FirstMemberName, expressionConverter.FirstMemberSqlName);
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="createQuery"></param>
        /// <param name="query">查询信息</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetSelectQuery<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, MemberMap<modelType> memberMap, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class
        {
            query.MemberMap = DataModel.Model<modelType>.CopyMemberMap;
            if (memberMap != null && !memberMap.IsDefault) query.MemberMap.And(memberMap);
            GetSelectQuery(sqlTool, ref createQuery, ref query);
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
        internal virtual void GetSelectQuery<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        /// <returns>对象集合</returns>
        internal virtual LeftArray<valueType> Select<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SelectQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class
        {
            try
            {
                if (query.Sql != null)
                {
                    if (connection == null) connection = GetConnection();
                    if (connection != null)
                    {
                        if (query.IndexFieldName != null)
                        {
                            sqlTool.CreateIndex(connection, query.IndexFieldName, false);
                            query.IndexFieldName = null;
                        }
                        using (DbCommand command = getCommand(connection, query.Sql))
                        {
                            DbDataReader reader = null;
                            try
                            {
                                reader = command.ExecuteReader(CommandBehavior.SingleResult);
                            }
                            catch (Exception error)
                            {
                                sqlTool.Log.Add(AutoCSer.Log.LogType.Error, error, query.Sql);
                            }
                            if (reader != null)
                            {
                                using (reader)
                                {
                                    int skipCount = query.SkipCount;
                                    while (skipCount != 0 && reader.Read()) --skipCount;
                                    if (skipCount == 0)
                                    {
                                        LeftArray<valueType> array = new LeftArray<valueType>();
                                        while (reader.Read())
                                        {
                                            valueType value = AutoCSer.Emit.Constructor<valueType>.New();
                                            DataModel.Model<modelType>.Setter.Set(reader, value, query.MemberMap);
                                            array.Add(value);
                                        }
                                        return array;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally { query.Free(); }
            return default(LeftArray<valueType>);
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        /// <param name="readValue"></param>
        /// <returns>对象集合</returns>
        internal virtual LeftArray<valueType> Select<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SelectQuery<modelType> query, Func<DbDataReader, valueType> readValue)
            where valueType : class, modelType
            where modelType : class
        {
            try
            {
                if (query.Sql != null)
                {
                    if (connection == null) connection = GetConnection();
                    if (connection != null)
                    {
                        if (query.IndexFieldName != null)
                        {
                            sqlTool.CreateIndex(connection, query.IndexFieldName, false);
                            query.IndexFieldName = null;
                        }
                        using (DbCommand command = getCommand(connection, query.Sql))
                        {
                            DbDataReader reader = null;
                            try
                            {
                                reader = command.ExecuteReader(CommandBehavior.SingleResult);
                            }
                            catch (Exception error)
                            {
                                sqlTool.Log.Add(AutoCSer.Log.LogType.Error, error, query.Sql);
                            }
                            if (reader != null)
                            {
                                using (reader)
                                {
                                    int skipCount = query.SkipCount;
                                    while (skipCount != 0 && reader.Read()) --skipCount;
                                    if (skipCount == 0)
                                    {
                                        LeftArray<valueType> array = new LeftArray<valueType>();
                                        while (reader.Read())
                                        {
                                            valueType value = readValue(reader);
                                            if (value != null) array.Add(value);
                                        }
                                        return array;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally { query.Free(); }
            return default(LeftArray<valueType>);
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="readValue"></param>
        /// <returns>对象集合</returns>
        internal virtual LeftArray<valueType> Select<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, string sql, Func<DbDataReader, valueType> readValue)
            where valueType : class, modelType
            where modelType : class
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                using (DbCommand command = getCommand(connection, sql))
                {
                    DbDataReader reader = null;
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SingleResult);
                    }
                    catch (Exception error)
                    {
                        sqlTool.Log.Add(AutoCSer.Log.LogType.Error, error, sql);
                    }
                    if (reader != null)
                    {
                        using (reader)
                        {
                            LeftArray<valueType> array = new LeftArray<valueType>();
                            while (reader.Read())
                            {
                                valueType value = readValue(reader);
                                if (value != null) array.Add(value);
                            }
                            return array;
                        }
                    }
                }
            }
            return default(LeftArray<valueType>);
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="readValue"></param>
        /// <returns>对象集合</returns>
        internal virtual void CustomReader(Sql.Table sqlTool, ref DbConnection connection, string sql, Action<DbDataReader> readValue)
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                using (DbCommand command = getCommand(connection, sql))
                {
                    DbDataReader reader = null;
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SingleResult);
                    }
                    catch (Exception error)
                    {
                        sqlTool.Log.Add(AutoCSer.Log.LogType.Error, error, sql);
                    }
                    if (reader != null)
                    {
                        using (reader)
                        {
                            while (reader.Read()) readValue(reader);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        internal abstract void GetByIdentity<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref GetQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        internal abstract void GetByPrimaryKey<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref GetQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 执行SQL语句并更新成员
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">目标对象</param>
        /// <param name="query">查询信息</param>
        /// <returns>更新是否成功</returns>
        internal bool Get<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref GetQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                using (DbCommand command = getCommand(connection, query.Sql))
                {
                    try
                    {
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            if (reader.Read())
                            {
                                DataModel.Model<modelType>.Setter.Set(reader, value, query.MemberMap);
                                return true;
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        sqlTool.Log.Add(AutoCSer.Log.LogType.Error, error, query.Sql);
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        /// <returns></returns>
        internal abstract bool Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns>更新是否成功</returns>
        internal bool Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query, bool isIgnoreTransaction)
            where valueType : class, modelType
            where modelType : class
        {
            if (sqlTool.CallOnUpdate(value, memberMap))
            {
                if (isIgnoreTransaction) return Update(sqlTool, ref connection, value, memberMap, ref query);
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(false))
                {
                    try
                    {
                        return Update(sqlTool, ref connection, value, memberMap, ref query);
                    }
                    finally { AutoCSer.DomainUnload.Unloader.TransactionEnd(); }
                }
            }
            return false;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        /// <returns>更新是否成功</returns>
        internal abstract bool Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="transaction">事务操作</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        /// <returns>更新是否成功</returns>
        internal abstract bool Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class;

        /// <summary>
        /// 获取添加数据 SQL 语句
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">添加数据</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">添加数据查询信息</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class
        {
            if (memberMap == null) memberMap = MemberMap<modelType>.Default;
            if (DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, sqlTool))
            {
                insert(sqlTool, value, memberMap, ref query);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取添加数据 SQL 语句
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">添加数据</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">添加数据查询信息</param>
        internal abstract void insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns></returns>
        internal bool Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query, bool isIgnoreTransaction)
            where valueType : class, modelType
            where modelType : class
        {
            if (sqlTool.CallOnInsert(value))
            {
                if (isIgnoreTransaction) return Insert(sqlTool, ref connection, value, ref query);
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(false))
                {
                    try
                    {
                        return Insert(sqlTool, ref connection, value, ref query);
                    }
                    finally { AutoCSer.DomainUnload.Unloader.TransactionEnd(); }
                }
            }
            return false;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <returns></returns>
        internal abstract bool Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="transaction">事务操作</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <returns></returns>
        internal abstract bool Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class;

        /// <summary>
        /// 验证数据与初始化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="array">添加数据数组</param>
        /// <returns></returns>
        internal bool Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref SubArray<valueType> array)
            where valueType : class, modelType
            where modelType : class
        {
            if (DataModel.Model<modelType>.Verifyer.IsVerifyer)
            {
                MemberMap<modelType> memberMap = MemberMap<modelType>.Default;
                foreach(valueType value in array)
                {
                    if (!DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, sqlTool))
                    {
                        //Console.WriteLine(value.toJson());
                        return false;
                    }
                }
            }
            if (DataModel.Model<modelType>.Identity != null)
            {
                foreach (valueType value in array)
                {
                    if (sqlTool.Attribute.IsSetIdentity) DataModel.Model<modelType>.SetIdentity(value, sqlTool.NextIdentity);
                    else sqlTool.Identity64 = DataModel.Model<modelType>.GetIdentity(value);
                }
            }
            return true;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="array">数据数组</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns></returns>
        internal SubArray<valueType> Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SubArray<valueType> array, bool isIgnoreTransaction)
            where valueType : class, modelType
            where modelType : class
        {
            if (sqlTool.CallOnInsert(ref array))
            {
                if (isIgnoreTransaction) return Insert(sqlTool, ref connection, ref array);
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(false))
                {
                    try
                    {
                        return Insert(sqlTool, ref connection, ref array);
                    }
                    finally { AutoCSer.DomainUnload.Unloader.TransactionEnd(); }
                }
            }
            return default(SubArray<valueType>);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="array">数据数组</param>
        /// <returns>成功添加的数据</returns>
        internal abstract SubArray<valueType> Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SubArray<valueType> array)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="transaction">事务操作</param>
        /// <param name="array">数据数组</param>
        /// <returns>成功添加的数据</returns>
        internal abstract SubArray<valueType> Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, ref SubArray<valueType> array)
            where valueType : class, modelType
            where modelType : class;

        /// <summary>
        /// 获取删除数据 SQL 语句
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">删除数据</param>
        /// <param name="query">删除数据查询信息</param>
        /// <returns></returns>
        internal abstract bool Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
        /// <returns></returns>
        internal bool Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query, bool isIgnoreTransaction)
            where valueType : class, modelType
            where modelType : class
        {
            if (sqlTool.CallOnDelete(value))
            {
                if (isIgnoreTransaction) return Delete(sqlTool, ref connection, value, ref query);
                if (AutoCSer.DomainUnload.Unloader.TransactionStart(false))
                {
                    try
                    {
                        return Delete(sqlTool, ref connection, value, ref query);
                    }
                    finally { AutoCSer.DomainUnload.Unloader.TransactionEnd(); }
                }
            }
            return false;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <returns></returns>
        internal abstract bool Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class;
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="transaction">事务操作</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <returns></returns>
        internal abstract bool Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class;
    }
}
