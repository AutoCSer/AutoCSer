using System;
using System.Data.Common;
using System.Data;
using AutoCSer.Extension;
using System.Collections.Generic;
using System.Threading;
using AutoCSer.Metadata;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace AutoCSer.Sql.Oracle
{
    /// <summary>
    /// Oracle 客户端
    /// </summary>
    public sealed unsafe class Client : Sql.Client
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
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                if ((createQuery.GetCount | query.SkipCount) != 0) sqlStream.WriteNotNull("select * from(");
                sqlStream.WriteNotNull("select ");
                if (query.MemberMap != null) DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                else sqlStream.Write('*');
                sqlStream.WriteNotNull(" from ");
                sqlStream.WriteNotNull(sqlTool.TableName);
                sqlStream.Write(' ');
                createQuery.WriteWhere(sqlTool, sqlStream, ref query);
                createQuery.WriteOrder(sqlTool, sqlStream, constantConverter, ref query);
                if ((createQuery.GetCount | query.SkipCount) != 0)
                {
                    sqlStream.WriteNotNull(")where rownum between ");
                    AutoCSer.Extension.Number.ToString(query.SkipCount, sqlStream);
                    sqlStream.WriteNotNull(" and ");
                    AutoCSer.Extension.Number.ToString(createQuery.GetCount, sqlStream);
                    sqlStream.WriteNotNull(" order by rownum asc");
                }
                query.Sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                sqlStream.SimpleWriteNotNull("select ");
                DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                sqlStream.SimpleWriteNotNull(" from ");
                sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                sqlStream.WriteNotNull(" where ");
                constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                sqlStream.Write('=');
                AutoCSer.Extension.Number.ToString(DataModel.Model<modelType>.GetIdentity(value), sqlStream);
                query.Sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                sqlStream.SimpleWriteNotNull("select ");
                DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                sqlStream.SimpleWriteNotNull(" from ");
                sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                sqlStream.WriteNotNull(" where ");
                DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                query.Sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
        internal override bool Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
        {
            if (query.MemberMap == null) query.MemberMap = sqlTool.GetSelectMemberMap(memberMap);
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                if (DataModel.Model<modelType>.Identity != null)
                {
                    long identity = DataModel.Model<modelType>.GetIdentity(value);
                    if (query.NotQuery)
                    {
                        sqlStream.WriteNotNull(@"update ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.SimpleWriteNotNull(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.WriteNotNull(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extension.Number.ToString(identity, sqlStream);
                    }
                    else
                    {
                        sqlStream.SimpleWriteNotNull("select ");
                        DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap, constantConverter);
                        sqlStream.SimpleWriteNotNull(" from ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extension.Number.ToString(identity, sqlStream);
                        int size = sqlStream.ByteSize >> 1;
                        sqlStream.WriteNotNull(@";
if sql%rowcount<>0 begin
 update ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.SimpleWriteNotNull(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.WriteNotNull(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extension.Number.ToString(identity, sqlStream);
                        sqlStream.SimpleWriteNotNull(@"
 ");
                        sqlStream.WriteNotNull(sqlStream.Char, size);
                        sqlStream.SimpleWriteNotNull(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return true;
                }
                if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    if (query.NotQuery)
                    {
                        sqlStream.WriteNotNull(@"update ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.SimpleWriteNotNull(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.WriteNotNull(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                    }
                    else
                    {
                        sqlStream.SimpleWriteNotNull("select ");
                        DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap, constantConverter);
                        sqlStream.SimpleWriteNotNull(" from ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        int size = sqlStream.ByteSize >> 1;
                        sqlStream.WriteNotNull(@";
if sql%rowcount<>0 begin
 update ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.SimpleWriteNotNull(" set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.WriteNotNull(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWriteNotNull(@"
 ");
                        sqlStream.WriteNotNull(sqlStream.Char, size);
                        sqlStream.SimpleWriteNotNull(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return true;
                }
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
                sqlStream.Dispose();
                Interlocked.Exchange(ref this.sqlStream, sqlStream);
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
        internal override bool Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                sqlTool.Log.Add(AutoCSer.Log.LogType.Error, query.Sql);
                if (query.NotQuery)
                {
                    if (executeNonQuery(connection, query.Sql) > 0)
                    {
                        sqlTool.CallOnUpdated(value, null, memberMap);
                        return true;
                    }
                }
                else
                {
                    bool isFinally = false;
                    try
                    {
                        using (DbCommand command = getCommand(connection, query.Sql, CommandType.Text))
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            if (reader.Read())
                            {
                                valueType oldValue = AutoCSer.Emit.Constructor<valueType>.New();
                                DataModel.Model<modelType>.Setter.Set(reader, oldValue, query.MemberMap);
                                if (reader.NextResult() && reader.Read())
                                {
                                    DataModel.Model<modelType>.Setter.Set(reader, value, query.MemberMap);
                                    sqlTool.CallOnUpdated(value, oldValue, memberMap);
                                    return isFinally = true;
                                }
                            }
                        }
                        isFinally = true;
                    }
                    finally
                    {
                        if (!isFinally) sqlTool.Log.Add(AutoCSer.Log.LogType.Error, query.Sql);
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
        /// <param name="transaction">事务操作</param>
        /// <param name="value">匹配成员值</param>
        /// <param name="memberMap">成员位图</param>
        /// <param name="query">查询信息</param>
        /// <returns>更新是否成功</returns>
        internal override bool Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
        {
            if (sqlTool.CallOnUpdate(value, memberMap) && executeNonQuery(transaction, query.Sql) > 0)
            {
                sqlTool.CallOnUpdated(transaction, value, memberMap);
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
        internal override void insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref InsertQuery query)
        {
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                if (DataModel.Model<modelType>.Identity != null)
                {
                    long identity;
                    if (sqlTool.Attribute.IsSetIdentity) DataModel.Model<modelType>.SetIdentity(value, identity = sqlTool.NextIdentity);
                    else sqlTool.Identity64 = identity = DataModel.Model<modelType>.GetIdentity(value);
                    sqlStream.SimpleWriteNotNull("insert into ");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.Write('(');
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap, constantConverter);
                    sqlStream.SimpleWriteNotNull(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, constantConverter, sqlTool);
                    if (!query.NotQuery)
                    {
                        sqlStream.WriteNotNull(@");
if sql%rowcount<>0 begin
 select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap, constantConverter);
                        sqlStream.SimpleWriteNotNull(" from ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extension.Number.ToString(identity, sqlStream);
                        sqlStream.SimpleWriteNotNull(@";
end");
                    }
                    else sqlStream.Write(')');
                }
                else
                {
                    sqlStream.SimpleWriteNotNull("insert into ");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.Write('(');
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap, constantConverter);
                    sqlStream.SimpleWriteNotNull(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, constantConverter, sqlTool);
                    if (!query.NotQuery && DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                    {
                        sqlStream.WriteNotNull(@");
if sql%rowcount<>0 begin
 select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap, constantConverter);
                        sqlStream.SimpleWriteNotNull(" from ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWriteNotNull(@";
end");
                    }
                    else sqlStream.Write(')');
                }
                query.Sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
        internal override bool Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
        {
            sqlTool.Log.Add(AutoCSer.Log.LogType.Error, query.Sql);
            if ((DataModel.Model<modelType>.Identity != null || DataModel.Model<modelType>.PrimaryKeys.Length != 0) && !query.NotQuery)
            {
                GetQuery<modelType> getQuery = new GetQuery<modelType> { MemberMap = DataModel.Model<modelType>.MemberMap, Sql = query.Sql };
                if (!Get(sqlTool, ref connection, value, ref getQuery)) return false;
            }
            else if (executeNonQuery(ref connection, query.Sql) <= 0) return false;
            sqlTool.CallOnInserted(value);
            return true;
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
        internal override bool Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
        {
            if (sqlTool.CallOnInsert(value) && executeNonQuery(transaction, query.Sql) > 0)
            {
                sqlTool.CallOnInserted(transaction, value);
                return true;
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
        /// <param name="array">数据数组</param>
        /// <returns></returns>
        internal override SubArray<valueType> Insert<valueType, modelType>(Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SubArray<valueType> array)
        {
            MemberMap<modelType> memberMap = MemberMap<modelType>.Default;
            InsertQuery query = new InsertQuery { NotQuery = true };
            LeftArray<valueType> newArray = new LeftArray<valueType>(array.Length);
            foreach (valueType value in array)
            {
                insert(sqlTool, value, memberMap, ref query);
                if (Insert(sqlTool, ref connection, value, ref query)) newArray.UnsafeAdd(value);
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
        internal override SubArray<valueType> Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, ref SubArray<valueType> array)
        {
            if (sqlTool.CallOnInsert(ref array))
            {
                MemberMap<modelType> memberMap = MemberMap<modelType>.Default;
                InsertQuery query = new InsertQuery();
                foreach (valueType value in array)
                {
                    insert(sqlTool, value, memberMap, ref query);
                    if (executeNonQuery(transaction, query.InsertSql) <= 0) return default(SubArray<valueType>);
                }
                sqlTool.CallOnInserted(transaction, array);
                return array;
            }
            return default(SubArray<valueType>);
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
        internal override bool Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, valueType value, ref InsertQuery query)
        {
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                if (DataModel.Model<modelType>.Identity != null)
                {
                    long identity = DataModel.Model<modelType>.GetIdentity(value);
                    if (query.NotQuery)
                    {
                        sqlStream.WriteNotNull(@"delete ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extension.Number.ToString(identity, sqlStream);
                    }
                    else
                    {
                        sqlStream.SimpleWriteNotNull("select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap, constantConverter);
                        sqlStream.SimpleWriteNotNull(" from ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extension.Number.ToString(identity, sqlStream);
                        sqlStream.WriteNotNull(@";
if sql%rowcount<>0 begin
 delete ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extension.Number.ToString(identity, sqlStream);
                        sqlStream.SimpleWriteNotNull(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return true;
                }
                else if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    if (query.NotQuery)
                    {
                        sqlStream.WriteNotNull(@"delete ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                    }
                    else
                    {
                        sqlStream.SimpleWriteNotNull("select ");
                        DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap, constantConverter);
                        sqlStream.SimpleWriteNotNull(" from ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.WriteNotNull(@";
if sql%rowcount<>0 begin
 delete ");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull(" where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWriteNotNull(@";
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return true;
                }
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
                sqlStream.Dispose();
                Interlocked.Exchange(ref this.sqlStream, sqlStream);
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
        internal override bool Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, valueType value, ref InsertQuery query)
        {
            sqlTool.Log.Add(AutoCSer.Log.LogType.Error, query.Sql);
            if (query.NotQuery)
            {
                if (executeNonQuery(ref connection, query.Sql) > 0)
                {
                    sqlTool.CallOnDeleted(value);
                    return true;
                }
            }
            else
            {
                GetQuery<modelType> getQuery = new GetQuery<modelType> { MemberMap = sqlTool.SelectMemberMap, Sql = query.Sql };
                if (Get(sqlTool, ref connection, value, ref getQuery))
                {
                    sqlTool.CallOnDeleted(value);
                    return true;
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
        /// <param name="transaction">事务操作</param>
        /// <param name="value">添加数据</param>
        /// <param name="query">添加数据查询信息</param>
        /// <returns></returns>
        internal override bool Delete<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, Transaction transaction, valueType value, ref InsertQuery query)
        {
            if (sqlTool.CallOnDelete(value) && executeNonQuery(transaction, query.Sql) > 0)
            {
                sqlTool.CallOnDeleted(transaction, value);
                return true;
            }
            return false;
        }
    }
}
