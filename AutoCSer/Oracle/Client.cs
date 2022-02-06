using System;
using System.Data.Common;
using System.Data;
using AutoCSer.Extensions;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Metadata;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using AutoCSer.Memory;

namespace AutoCSer.Sql.Oracle
{
    /// <summary>
    /// Oracle 客户端
    /// </summary>
    public sealed class Client : Sql.Client
    {
        /// <summary>
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal Client(Connection connection) : base(connection)
        {
            constantConverter = ConstantConverter.Default;
        }
        /// <summary>
        /// 创建 SQL 连接
        /// </summary>
        /// <param name="connection"></param>
        protected override void createConnection(ref DbConnection connection)
        {
            (connection = new OracleConnection(Connection.ConnectionString)).Open();
        }
        /// <summary>
        /// 获取SQL命令
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql">SQL语句</param>
        /// <param name="commandType"></param>
        /// <returns>SQL命令</returns>
        protected override DbCommand getCommand(DbConnection connection, string sql, CommandType commandType)
        {
            DbCommand command = new OracleCommand(sql, new UnionType { Value = connection }.OracleConnection);
            command.CommandType = commandType;
            return command;
        }
        /// <summary>
        /// 获取数据适配器
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据适配器</returns>
        protected override DbDataAdapter getAdapter(DbCommand command)
        {
            return new OracleDataAdapter(new UnionType { Value = command }.OracleCommand);
        }
        /// <summary>
        /// 获取表格名称集合
        /// </summary>
        /// <returns></returns>
        public override LeftArray<string> GetTableNames()
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 成员信息转换为数据列
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="memberAttribute">SQL成员信息</param>
        /// <returns>数据列</returns>
        internal override Column GetColumn(Type type, MemberAttribute memberAttribute)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        internal override void CreateIndex(DbConnection connection, string tableName, ColumnCollection columnCollection)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 判断表格是否存在
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格是否存在</returns>
        private bool isTable(DbConnection connection, string tableName)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 根据表格名称获取表格信息
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息</returns>
        internal override TableColumnCollection GetTable(DbConnection connection, string tableName)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="table">表格信息</param>
        internal override void CreateTable(DbConnection connection, TableColumnCollection table)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 删除列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">删除列集合</param>
        internal override void DeleteFields(DbConnection connection, ColumnCollection columnCollection)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 新增列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">新增列集合</param>
        internal override void AddFields(DbConnection connection, ColumnCollection columnCollection)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override DbParameter CreateParameter(string name, object value, System.Data.DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            OracleParameter parameter = new OracleParameter(name, value);
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
        public override DbParameter CreateParameter(string name, object value, System.Data.DbType dbType, int size, ParameterDirection direction = ParameterDirection.Output)
        {
            OracleParameter parameter = new OracleParameter(name, value);
            parameter.DbType = dbType;
            parameter.Direction = direction;
            parameter.Size = size;
            return parameter;
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
                if ((createQuery.GetCount | query.SkipCount) != 0) sqlStream.Write("select * from(");
                sqlStream.Write("select ");
                if (query.MemberMap != null) DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                else sqlStream.Write('*');
                sqlStream.Write(" from ");
                sqlStream.Write(sqlTool.TableName);
                sqlStream.Write(' ');
                createQuery.WriteWhere(sqlTool, sqlStream, ref query);
                createQuery.WriteOrder(sqlTool, sqlStream, constantConverter, ref query);
                if ((createQuery.GetCount | query.SkipCount) != 0)
                {
                    sqlStream.Write(")where rownum between ");
                    AutoCSer.Extensions.NumberExtension.ToString(query.SkipCount, sqlStream);
                    sqlStream.Write(" and ");
                    AutoCSer.Extensions.NumberExtension.ToString(createQuery.GetCount, sqlStream);
                    sqlStream.Write(" order by rownum asc");
                }
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
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        internal override void GetByIdentity<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref GetQuery<modelType> query)
        {
            query.MemberMap = DataModel.Model<modelType>.CopyMemberMap;
            if (memberMap != null && !memberMap.IsDefault) query.MemberMap.And(memberMap);
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                sqlStream.SimpleWrite("select ");
                DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                sqlStream.SimpleWrite(" from ");
                sqlStream.SimpleWrite(sqlTool.TableName);
                sqlStream.Write(" where ");
                constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                sqlStream.Write('=');
                AutoCSer.Extensions.NumberExtension.ToString(DataModel.Model<modelType>.GetIdentity(value), sqlStream);
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
        /// 查询对象
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        internal override void GetByPrimaryKey<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref GetQuery<modelType> query)
        {
            query.MemberMap = DataModel.Model<modelType>.CopyMemberMap;
            if (memberMap != null && !memberMap.IsDefault) query.MemberMap.And(memberMap);
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                sqlStream.SimpleWrite("select ");
                DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                sqlStream.SimpleWrite(" from ");
                sqlStream.SimpleWrite(sqlTool.TableName);
                sqlStream.Write(" where ");
                DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
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
        /// 更新数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        /// <returns></returns>
        internal override unsafe ReturnType Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
        {
            if (query.MemberMap == null) query.MemberMap = sqlTool.GetSelectMemberMap(memberMap);
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                if (DataModel.Model<modelType>.Identity != null)
                {
                    long identity = DataModel.Model<modelType>.GetIdentity(value);
                    if (query.NotQuery)
                    {
                        sqlStream.Write(@"update ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.Write(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select ");
                        DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        int size = sqlStream.Data.CurrentIndex >> 1;
                        sqlStream.Write(@";
if sql%rowcount<>0 begin
 update ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.Write(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.SimpleWrite(@"
 ");
                        sqlStream.Write(sqlStream.Char, size);
                        sqlStream.SimpleWrite(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return ReturnType.Success;
                }
                if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    if (query.NotQuery)
                    {
                        sqlStream.Write(@"update ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.Write(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select ");
                        DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        int size = sqlStream.Data.CurrentIndex >> 1;
                        sqlStream.Write(@";
if sql%rowcount<>0 begin
 update ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.Write(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWrite(@"
 ");
                        sqlStream.Write(sqlStream.Char, size);
                        sqlStream.SimpleWrite(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return ReturnType.Success;
                }
            }
            finally
            {
                UnmanagedPool.Default.Push(ref buffer);
                sqlStream.Dispose();
                Interlocked.Exchange(ref this.sqlStream, sqlStream);
            }
            return ReturnType.NotFoundPrimaryKey;
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
        internal override ReturnType Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
                if (query.NotQuery)
                {
                    if (executeNonQuery(connection, query.Sql) > 0)
                    {
                        sqlTool.CallOnUpdated(value, null, memberMap);
                        return ReturnType.Success;
                    }
                    return ReturnType.NotFoundData;
                }
                else
                {
                    ReturnType returnType = ReturnType.Unknown;
                    try
                    {
                        using (DbCommand command = getCommand(connection, query.Sql, CommandType.Text))
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            if (reader.Read())
                            {
                                valueType oldValue = AutoCSer.Metadata.DefaultConstructor<valueType>.Constructor();
                                DataModel.Model<modelType>.Setter.Set(reader, oldValue, query.MemberMap);
                                if (reader.NextResult() && reader.Read())
                                {
                                    DataModel.Model<modelType>.Setter.Set(reader, value, query.MemberMap);
                                    sqlTool.CallOnUpdated(value, oldValue, memberMap);
                                    return returnType = ReturnType.Success;
                                }
                            }
                        }
                        return returnType = ReturnType.NotFoundData;
                    }
                    finally
                    {
                        if (returnType == ReturnType.Unknown) sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
                    }
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
        /// <param name="transaction">事务操作</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        /// <returns>更新是否成功</returns>
        internal override ReturnType Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
        {
            if (sqlTool.CallOnUpdate(value, memberMap))
            {
                if (executeNonQuery(transaction, query.Sql) > 0)
                {
                    sqlTool.CallOnUpdated(transaction, value, memberMap);
                    return ReturnType.Success;
                }
                return ReturnType.NotFoundData;
            }
            return ReturnType.EventCancel;
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
        internal override void insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref InsertQuery query)
        {
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                if (DataModel.Model<modelType>.Identity != null)
                {
                    long identity;
                    if (sqlTool.Attribute.IsSetIdentity) DataModel.Model<modelType>.SetIdentity(value, identity = sqlTool.NextIdentity);
                    else sqlTool.Identity64 = identity = DataModel.Model<modelType>.GetIdentity(value);
                    sqlStream.SimpleWrite("insert into ");
                    sqlStream.SimpleWrite(sqlTool.TableName);
                    sqlStream.Write('(');
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap, constantConverter);
                    sqlStream.SimpleWrite(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, constantConverter, sqlTool);
                    if (!query.NotQuery)
                    {
                        sqlStream.Write(@");
if sql%rowcount<>0 begin
 select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.SimpleWrite(@";
end");
                    }
                    else sqlStream.Write(')');
                }
                else
                {
                    sqlStream.SimpleWrite("insert into ");
                    sqlStream.SimpleWrite(sqlTool.TableName);
                    sqlStream.Write('(');
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap, constantConverter);
                    sqlStream.SimpleWrite(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, constantConverter, sqlTool);
                    if (!query.NotQuery && DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                    {
                        sqlStream.Write(@");
if sql%rowcount<>0 begin
 select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWrite(@";
end");
                    }
                    else sqlStream.Write(')');
                }
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
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection">SQL连接</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <returns></returns>
        internal override ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
        {
            sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
            if ((DataModel.Model<modelType>.Identity != null || DataModel.Model<modelType>.PrimaryKeys.Length != 0) && !query.NotQuery)
            {
                GetQuery<modelType> getQuery = new GetQuery<modelType> { MemberMap = DataModel.Model<modelType>.MemberMap, Sql = query.Sql };
                ReturnType returnType = Get(sqlTool, ref connection, value, ref getQuery);
                if (returnType != ReturnType.Success) return returnType;
            }
            else
            {
                ReturnValue<int> returnValue = executeNonQuery(ref connection, query.Sql);
                if (returnValue.ReturnType != ReturnType.Success) return returnValue.ReturnType;
                if (returnValue.Value <= 0) return ReturnType.ExecuteFailed;
            }
            sqlTool.CallOnInserted(value);
            return ReturnType.Success;
        }
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
        internal override ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
        {
            if (sqlTool.CallOnInsert(value))
            {
                if (executeNonQuery(transaction, query.Sql) > 0)
                {
                    sqlTool.CallOnInserted(transaction, value);
                    return ReturnType.Success;
                }
                return ReturnType.ExecuteFailed;
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
        /// <returns></returns>
        internal override ReturnValue<SubArray<valueType>> Insert<valueType, modelType>(Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SubArray<valueType> array)
        {
            MemberMap<modelType> memberMap = MemberMap<modelType>.Default;
            InsertQuery query = new InsertQuery { NotQuery = true };
            LeftArray<valueType> newArray = new LeftArray<valueType>(array.Length);
            foreach (valueType value in array)
            {
                insert(sqlTool, value, memberMap, ref query);
                if (Insert(sqlTool, ref connection, value, ref query) == ReturnType.Success) newArray.UnsafeAdd(value);
            }
            return new SubArray<valueType>(ref newArray);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="transaction">事务操作</param>
        /// <param name="array">数据数组</param>
        /// <returns></returns>
        internal override ReturnType Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, ref SubArray<valueType> array)
        {
            if (sqlTool.CallOnInsert(ref array))
            {
                MemberMap<modelType> memberMap = MemberMap<modelType>.Default;
                InsertQuery query = new InsertQuery();
                foreach (valueType value in array)
                {
                    insert(sqlTool, value, memberMap, ref query);
                    if (executeNonQuery(transaction, query.InsertSql) <= 0) return ReturnType.ExecuteFailed;
                }
                sqlTool.CallOnInserted(transaction, array);
                return ReturnType.Success;
            }
            return ReturnType.EventCancel;
        }

        /// <summary>
        /// 获取删除数据 SQL 语句
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="value">删除数据</param>
        /// <param name="query">删除数据查询信息</param>
        /// <returns></returns>
        internal override ReturnType Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, ref InsertQuery query)
        {
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                if (DataModel.Model<modelType>.Identity != null)
                {
                    long identity = DataModel.Model<modelType>.GetIdentity(value);
                    if (query.NotQuery)
                    {
                        sqlStream.Write(@"delete ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.Write(@";
if sql%rowcount<>0 begin
 delete ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.SimpleWrite(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return ReturnType.Success;
                }
                else if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    if (query.NotQuery)
                    {
                        sqlStream.Write(@"delete ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.Write(@";
if sql%rowcount<>0 begin
 delete ");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWrite(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return ReturnType.Success;
                }
            }
            finally
            {
                UnmanagedPool.Default.Push(ref buffer);
                sqlStream.Dispose();
                Interlocked.Exchange(ref this.sqlStream, sqlStream);
            }
            return ReturnType.NotFoundPrimaryKey;
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
        internal override ReturnType Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
        {
            //sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
            if (query.NotQuery)
            {
                ReturnValue<int> returnValue = executeNonQuery(ref connection, query.Sql);
                if (returnValue.ReturnType == ReturnType.Success)
                {
                    if (returnValue.Value > 0)
                    {
                        sqlTool.CallOnDeleted(value);
                        return ReturnType.Success;
                    }
                    return ReturnType.NotFoundData;
                }
                return returnValue.ReturnType;
            }
            else
            {
                GetQuery<modelType> getQuery = new GetQuery<modelType> { MemberMap = sqlTool.SelectMemberMap, Sql = query.Sql };
                ReturnType returnType = Get(sqlTool, ref connection, value, ref getQuery);
                if (returnType == ReturnType.Success) sqlTool.CallOnDeleted(value);
                return returnType;
            }
        }
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
        internal override ReturnType Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
        {
            if (sqlTool.CallOnDelete(value))
            {
                if (executeNonQuery(transaction, query.Sql) > 0)
                {
                    sqlTool.CallOnDeleted(transaction, value);
                    return ReturnType.Success;
                }
                return ReturnType.NotFoundData;
            }
            return ReturnType.EventCancel;
        }
    }
}
