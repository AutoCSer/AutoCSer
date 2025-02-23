﻿using System;
using AutoCSer.Metadata;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Common;
using AutoCSer.Extensions;
using System.Data;
using System.Runtime.CompilerServices;
using AutoCSer.Log;
using System.Data.SqlClient;
using AutoCSer.Memory;
using System.Threading;

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
        /// 最小时间
        /// </summary>
        public virtual DateTime MinDateTime { get { return DateTime.MinValue; } }
        /// <summary>
        /// 常量转换
        /// </summary>
        protected ConstantConverter constantConverter;
        /// <summary>
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        /// <param name="constantConverter">常量转换</param>
        protected Client(Connection connection, ConstantConverter constantConverter = null)
        {
            this.constantConverter = constantConverter ?? ConstantConverter.Default;
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
                bool isOpen = false;
                try
                {
                    createConnection(ref connection);
                    isOpen = true;
                }
                finally
                {
                    if (!isOpen && connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
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
        /// 关闭错误连接
        /// </summary>
        /// <param name="connection"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CloseErrorConnection(ref DbConnection connection)
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }
        /// <summary>
        /// 关闭错误连接并重新获取连接
        /// </summary>
        /// <param name="connection"></param>
        internal void ResetErrorConnection(ref DbConnection connection)
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
                if (ConnectionPool.IsPool) connection = GetConnection();
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
        /// <param name="commandType"></param>
        /// <returns>SQL命令</returns>
        protected abstract DbCommand getCommand(DbConnection connection, string sql, CommandType commandType);
        /// <summary>
        /// 获取SQL命令
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="commandType"></param>
        /// <returns>SQL命令</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected DbCommand getCommand(DbConnection connection, string sql, CommandType commandType, int timeoutSeconds)
        {
            DbCommand command = getCommand(connection, sql, commandType);
            if (timeoutSeconds > 0) command.CommandTimeout = timeoutSeconds;
            return command;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeoutSeconds"></param>
        /// <returns>受影响的行数</returns>
        protected ReturnValue<int> executeNonQuery(ref DbConnection connection, string sql, int timeoutSeconds = 0)
        {
            if (connection == null)
            {
                connection = GetConnection();
                if (connection == null) return ReturnType.ConnectionFailed;
            }
            using (DbCommand command = getCommand(connection, sql, CommandType.Text, timeoutSeconds)) return command.ExecuteNonQuery();
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
            using (DbCommand command = getCommand(connection, sql, CommandType.Text)) return command.ExecuteNonQuery();
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="transaction">事务操作</param>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        protected int executeNonQuery(Transaction transaction, string sql)
        {
            using (DbCommand command = getCommand(transaction.Connection, sql, CommandType.Text))
            {
                command.Transaction = transaction.DbTransaction;
                return command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="timeoutSeconds">命令超时时间秒数，0 表示默认不设置</param>
        /// <returns>是否成功</returns>
        public ReturnValue<int> ExecuteNonQuery(string sql, int timeoutSeconds = 0)
        {
            DbConnection connection = null;
            bool isFreeConnection = false;
            try
            {
                ReturnValue<int> returnValue = executeNonQuery(ref connection, sql, timeoutSeconds);
                FreeConnection(ref connection);
                isFreeConnection = true;
                return returnValue;
            }
            finally
            {
                if (!isFreeConnection) CloseErrorConnection(ref connection);
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
        /// 获取表格名称集合
        /// </summary>
        /// <returns></returns>
        public abstract LeftArray<string> GetTableNames();
        /// <summary>
        /// 获取指定表格名称，如果表格不存在返回第一个表格名称（仅用于 Excel）
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual string GetFirstTableName(string tableName)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 判断表格是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool IsTable(string tableName)
        {
            foreach (string name in GetTableNames())
            {
                if (name == tableName) return true;
            }
            return false;
        }
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
                ColumnBuilder sqlColumn = new ColumnBuilder { Client = this, Columns = new LeftArray<Column>(0) };
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
            using (DbCommand command = getCommand(connection, sql, CommandType.Text))
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
                sqlStream.SimpleWrite("ix_");
                sqlStream.SimpleWrite(tableName);
                foreach (Column column in columnCollection.Columns)
                {
                    sqlStream.Write('_');
                    sqlStream.SimpleWrite(column.Name);
                }
            }
            else sqlStream.SimpleWrite(columnCollection.Name);
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
        internal virtual void GetSql<modelType>(Expression expression, CharStream sqlStream, ref SelectQuery<modelType> query)
            where modelType : class
        {
            AutoCSer.Sql.MsSql.ExpressionConverter expressionConverter = new AutoCSer.Sql.MsSql.ExpressionConverter { SqlStream = sqlStream, ConstantConverter = constantConverter };
            expressionConverter.Convert(expression);
            if (query.IndexFieldName == null) query.SetIndex(expressionConverter.FirstMemberName, expressionConverter.FirstMemberSqlName);
        }
        /// <summary>
        /// 条件表达式转换为 SQL 字符串
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal string GetWhere(Expression expression)
        {
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                AutoCSer.Sql.MsSql.ExpressionConverter expressionConverter = new AutoCSer.Sql.MsSql.ExpressionConverter { SqlStream = sqlStream, ConstantConverter = constantConverter };
                expressionConverter.Convert(expression);
                return sqlStream.ToString();
            }
            finally
            {
                UnmanagedPool.Default.Push(ref buffer);
                sqlStream.Dispose();
                Interlocked.Exchange(ref this.sqlStream, sqlStream);
            }
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
        /// <returns>返回值类型</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnType GetSelectQuery<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, MemberMap<modelType> memberMap, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class
        {
            query.MemberMap = DataModel.Model<modelType>.CopyMemberMap;
            if (SetRealMemberMap(sqlTool, query.MemberMap))
            {
                if (memberMap != null && !memberMap.IsDefault) query.MemberMap.And(memberMap);
                GetSelectQuery(sqlTool, ref createQuery, ref query);
                return ReturnType.Success;
            }
            return ReturnType.EmptyMemberMap;
        }
        /// <summary>
        /// 设置真实成员位图（用于 Excel）
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="modelType"></typeparam>
        /// <param name="sqlTool"></param>
        /// <param name="memberMap"></param>
        /// <returns>是否设置成功</returns>
        internal virtual bool SetRealMemberMap<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, MemberMap<modelType> memberMap)
            where valueType : class, modelType
            where modelType : class
        {
            return true;
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
        internal virtual ReturnValue<LeftArray<valueType>> Select<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SelectQuery<modelType> query)
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
                        ReturnType returnType = ReturnType.Unknown;
                        try
                        {
                            using (DbCommand command = getCommand(connection, query.Sql, CommandType.Text))
                            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                            {
                                int skipCount = query.SkipCount;
                                while (skipCount != 0 && reader.Read()) --skipCount;
                                if (skipCount == 0)
                                {
                                    LeftArray<valueType> array = new LeftArray<valueType>(0);
                                    while (reader.Read())
                                    {
                                        valueType value = AutoCSer.Metadata.DefaultConstructor<valueType>.Constructor();
                                        DataModel.Model<modelType>.Setter.Set(reader, value, query.MemberMap, Connection.Type);
                                        array.Add(value);
                                    }
                                    returnType = ReturnType.Success;
                                    return array;
                                }
                            }
                            returnType = ReturnType.Success;
                            return new LeftArray<valueType>(0);
                        }
                        finally
                        {
                            if (returnType == ReturnType.Unknown) sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
                        }
                    }
                    return ReturnType.ConnectionFailed;
                }
            }
            finally { query.Free(); }
            return ReturnType.NotFoundSql;
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
        internal virtual ReturnValue<LeftArray<valueType>> Select<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SelectQuery<modelType> query, Func<DbDataReader, valueType> readValue)
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
                        ReturnType returnType = ReturnType.Unknown;
                        try
                        {
                            using (DbCommand command = getCommand(connection, query.Sql, CommandType.Text))
                            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                            {
                                int skipCount = query.SkipCount;
                                while (skipCount != 0 && reader.Read()) --skipCount;
                                if (skipCount == 0)
                                {
                                    LeftArray<valueType> array = new LeftArray<valueType>(0);
                                    while (reader.Read())
                                    {
                                        valueType value = readValue(reader);
                                        if (value != null) array.Add(value);
                                    }
                                    returnType = ReturnType.Success;
                                    return array;
                                }
                            }
                            returnType = ReturnType.Success;
                            return new LeftArray<valueType>(0);
                        }
                        finally
                        {
                            if (returnType == ReturnType.Unknown) sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
                        }
                    }
                    return ReturnType.ConnectionFailed;
                }
            }
            finally { query.Free(); }
            return ReturnType.NotFoundSql;
        }

        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="sql">SQL 语句</param>
        /// <param name="readValue">读取数据委托</param>
        /// <param name="log">日志处理</param>
        /// <returns>数据库记录集合</returns>
        public ReturnValue<LeftArray<valueType>> Select<valueType>(string sql, Func<DbDataReader, valueType> readValue, ILog log = null)
            where valueType : class
        {
            DbConnection connection = null;
            try
            {
                ReturnValue<LeftArray<valueType>> array = Select(ref connection, sql, readValue, log ?? Connection.Log);
                FreeConnection(ref connection);
                return array;
            }
            catch (Exception error)
            {
                CloseErrorConnection(ref connection);
                return error;
            }
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="readValue"></param>
        /// <param name="log"></param>
        /// <returns>对象集合</returns>
        internal virtual ReturnValue<LeftArray<valueType>> Select<valueType>(ref DbConnection connection, string sql, Func<DbDataReader, valueType> readValue, ILog log)
            where valueType : class
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                ReturnType returnType = ReturnType.Unknown;
                try
                {
                    using (DbCommand command = getCommand(connection, sql, CommandType.Text))
                    using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        LeftArray<valueType> array = new LeftArray<valueType>(0);
                        while (reader.Read())
                        {
                            valueType value = readValue(reader);
                            if (value != null) array.Add(value);
                        }
                        returnType = ReturnType.Success;
                        return array;
                    }
                }
                finally
                {
                    if (returnType == ReturnType.Success) (log ?? AutoCSer.LogHelper.Default).Error(sql, LogLevel.Error | LogLevel.AutoCSer);
                }
            }
            return ReturnType.ConnectionFailed;
        }

        /// <summary>
        /// 获取数据表格
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="log"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, ILog log = null, int timeoutSeconds = 0)
        {
            DbConnection connection = null;
            bool IsFreeConnection = false;
            try
            {
                DataTable table = GetDataTable(ref connection, sql, log ?? Connection.Log, timeoutSeconds);
                FreeConnection(ref connection);
                IsFreeConnection = true;
                return table;
            }
            finally
            {
                if (!IsFreeConnection) CloseErrorConnection(ref connection);
            }
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="log"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        internal DataTable GetDataTable(ref DbConnection connection, string sql, ILog log, int timeoutSeconds)
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                bool isFinally = false;
                try
                {
                    DataTable table = new DataTable();
                    using (DbCommand command = getCommand(connection, sql, CommandType.Text, timeoutSeconds))
                    using (DbDataAdapter adapter = getAdapter(command)) adapter.Fill(table);
                    return table;
                }
                finally
                {
                    if (!isFinally) (log ?? AutoCSer.LogHelper.Default).Error(sql, LogLevel.Error | LogLevel.AutoCSer);
                }
            }
            return null;
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="reader">读取数据委托</param>
        /// <param name="log">日志处理</param>
        /// <param name="timeoutSeconds">命令超时时间秒数，0 表示默认不设置</param>
        /// <returns>是否成功</returns>
        public ReturnType CustomReader(string sql, Action<DbDataReader> reader, ILog log = null, int timeoutSeconds = 0)
        {
            DbConnection connection = null;
            bool isFreeConnection = false;
            try
            {
                ReturnType returnType = CustomReader(ref connection, sql, reader, log ?? Connection.Log, timeoutSeconds);
                FreeConnection(ref connection);
                isFreeConnection = true;
                return returnType;
            }
            finally
            {
                if (!isFreeConnection) CloseErrorConnection(ref connection);
            }
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="readValue"></param>
        /// <param name="log"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns>是否成功</returns>
        internal ReturnType CustomReader(ref DbConnection connection, string sql, Action<DbDataReader> readValue, ILog log, int timeoutSeconds)
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                ReturnType returnType = ReturnType.Unknown;
                try
                {
                    using (DbCommand command = getCommand(connection, sql, CommandType.Text, timeoutSeconds))
                    using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        while (reader.Read()) readValue(reader);
                        return returnType = ReturnType.Success;
                    }
                }
                finally
                {
                    if (returnType == ReturnType.Unknown) (log ?? AutoCSer.LogHelper.Default).Error(sql, LogLevel.Error | LogLevel.AutoCSer);
                }
            }
            return ReturnType.ConnectionFailed;
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public virtual DbParameter CreateParameter(string name, object value, System.Data.DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            SqlParameter parameter = new SqlParameter(name, value);
            parameter.DbType = dbType;
            parameter.Direction = direction;
            return parameter;
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public virtual DbParameter CreateParameter(string name, object value, System.Data.DbType dbType, int size, ParameterDirection direction = ParameterDirection.Output)
        {
            SqlParameter parameter = new SqlParameter(name, value);
            parameter.DbType = dbType;
            parameter.Direction = direction;
            parameter.Size = size;
            return parameter;
        }
        /// <summary>
        /// 执行储存过程
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="log"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string storedProcedureName, DbParameter[] parameters, ILog log = null, int timeoutSeconds = 0)
        {
            DbConnection connection = null;
            bool isFreeConnection = false;
            int isExecute;
            try
            {
                isExecute = ExecuteStoredProcedure(ref connection, storedProcedureName, parameters, log ?? Connection.Log, timeoutSeconds);
                FreeConnection(ref connection);
                isFreeConnection = true;
            }
            finally
            {
                if (!isFreeConnection) CloseErrorConnection(ref connection);
            }
            return isExecute;
        }
        /// <summary>
        /// 执行储存过程
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="log"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        internal int ExecuteStoredProcedure(ref DbConnection connection, string storedProcedureName, DbParameter[] parameters, ILog log, int timeoutSeconds)
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                using (DbCommand command = getCommand(connection, storedProcedureName, CommandType.StoredProcedure, timeoutSeconds))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
            return int.MinValue;
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
        internal ReturnType Get<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref GetQuery<modelType> query)
            where valueType : class, modelType
            where modelType : class
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                ReturnType returnType = ReturnType.Unknown;
                try
                {
                    using (DbCommand command = getCommand(connection, query.Sql, CommandType.Text))
                    using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        if (reader.Read())
                        {
                            DataModel.Model<modelType>.Setter.Set(reader, value, query.MemberMap, Connection.Type);
                            return returnType = ReturnType.Success;
                        }
                    }
                    return returnType = ReturnType.NotFoundData;
                }
                finally
                {
                    if (returnType == ReturnType.Unknown) sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
                }
            }
            return ReturnType.ConnectionFailed;
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
        internal abstract ReturnType Update<valueType, modelType>
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
        internal ReturnType Update<valueType, modelType>
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
                return ReturnType.TransactionCancel;
            }
            return ReturnType.EventCancel;
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
        internal abstract ReturnType Update<valueType, modelType>
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
        internal abstract ReturnType Update<valueType, modelType>
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
        internal ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class
        {
            if (memberMap == null) memberMap = MemberMap<modelType>.Default;
            if (DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, sqlTool))
            {
                insert(sqlTool, value, memberMap, ref query);
                return ReturnType.Success;
            }
            return ReturnType.VerifyError;
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
        internal ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query, bool isIgnoreTransaction)
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
                return ReturnType.TransactionCancel;
            }
            return ReturnType.EventCancel;
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
        internal abstract ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
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
        internal abstract ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
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
        internal ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref SubArray<valueType> array)
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
                        return ReturnType.VerifyError;
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
            return ReturnType.Success;
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
        internal ReturnValue<SubArray<valueType>> Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SubArray<valueType> array, bool isIgnoreTransaction)
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
                return ReturnType.TransactionCancel;
            }
            return ReturnType.EventCancel;
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
        internal abstract ReturnValue<SubArray<valueType>> Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SubArray<valueType> array)
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
        internal abstract ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, ref SubArray<valueType> array)
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
        internal abstract ReturnType Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, ref InsertQuery query)
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
        internal ReturnType Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query, bool isIgnoreTransaction)
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
                return ReturnType.TransactionCancel;
            }
            return ReturnType.EventCancel;
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
        internal abstract ReturnType Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
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
        internal abstract ReturnType Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
            where valueType : class, modelType
            where modelType : class;
    }
}
