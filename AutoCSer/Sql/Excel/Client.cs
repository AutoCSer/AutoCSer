using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.Common;
using System.Data;
using System.Threading;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Memory;

namespace AutoCSer.Sql.Excel
{
    /// <summary>
    /// Excel 客户端
    /// </summary>
    internal unsafe class Client : Sql.Client
    {
        /// <summary>
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal Client(Sql.Connection connection) : base(connection) { }
        /// <summary>
        /// 创建 SQL 连接
        /// </summary>
        /// <param name="connection"></param>
        protected override void createConnection(ref DbConnection connection)
        {
            (connection = new OleDbConnection(Connection.ConnectionString)).Open();
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
            DbCommand command = new OleDbCommand(sql, new UnionType.OleDbConnection { Object = connection }.Value);
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
            return new OleDbDataAdapter(new UnionType.OleDbCommand { Object = command }.Value);
        }
        /// <summary>
        /// 表格名称
        /// </summary>
        private const string schemaTableName = "Table_Name";
        /// <summary>
        /// 获取表格名称集合
        /// </summary>
        /// <returns></returns>
        public override LeftArray<string> GetTableNames()
        {
            using (DbConnection dbConnection = GetConnection())
            using (DataTable table = ((OleDbConnection)dbConnection).GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null))
            {
                DataRowCollection rows = table.Rows;
                LeftArray<string> names = new LeftArray<string>(rows.Count);
                foreach (DataRow row in rows) names.UnsafeAdd(row[schemaTableName].ToString());
                return names;
            }
        }
        /// <summary>
        /// 获取指定表格名称，如果表格不存在返回第一个表格名称
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public override string GetFirstTableName(string TableName)
        {
            string FirstTableName = null;
            using (DbConnection dbConnection = GetConnection())
            using (DataTable table = ((OleDbConnection)dbConnection).GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null))
            {
                DataRowCollection rows = table.Rows;
                foreach (DataRow row in rows)
                {
                    string NextTableName = row[schemaTableName].ToString();
                    if (NextTableName == TableName) return TableName;
                    if (FirstTableName == null) FirstTableName = NextTableName;
                }
            }
            return FirstTableName;
        }
        /// <summary>
        /// 成员信息转换为数据列
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="memberAttribute">SQL成员信息</param>
        /// <returns>数据列</returns>
        internal override Column GetColumn(Type type, MemberAttribute memberAttribute)
        {
            SqlDbType sqlType = SqlDbType.NVarChar;
            int size = memberAttribute.MaxStringLength;
            Type memberType = memberAttribute.DataType ?? type;
            if (memberType == typeof(string))
            {
                if (size > 0) sqlType = SqlDbType.NVarChar;
                else
                {
                    sqlType = SqlDbType.NText;
                    size = int.MaxValue;
                }
            }
            else
            {
                sqlType = memberType.formCSharpType(memberAttribute);
                size = sqlType.getSize(memberAttribute); 
            }
            return new Column
            {
                DbType = sqlType,
                Size = size,
                IsNull = memberAttribute.IsDefaultMember && memberType != typeof(string) ? type.isNull() : memberAttribute.IsNull,
                Remark = memberAttribute.Remark,
                DefaultValue = memberAttribute.DefaultValue,
                UpdateValue = memberAttribute.UpdateValue
            };
        }
        /// <summary>
        /// 查询单值数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal override object GetValue(DbConnection connection, string sql)
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
            using (DbCommand command = getCommand(connection, "select top 1 * from [" + tableName + "]", CommandType.Text))
            using (DataSet dataSet = getDataSet(command))
            {
                DataTable table = dataSet.Tables[0];
                Column identity = null;
                LeftArray<Column> columns = new LeftArray<Column>(table.Columns.Count);
                foreach (DataColumn dataColumn in table.Columns)
                {
                    Column column = new Column
                    {
                        Name = dataColumn.ColumnName,
                        DbType = Sql.DbType.formCSharpType(dataColumn.DataType),
                        Size = dataColumn.MaxLength,
                        DefaultValue = dataColumn.DefaultValue == null ? null : dataColumn.DefaultValue.ToString(),
                        IsNull = dataColumn.AllowDBNull,
                    };
                    if (dataColumn.AutoIncrement) identity = column;
                    columns.UnsafeAdd(column);
                }
                return new TableColumnCollection
                {
                    Columns = new ColumnCollection
                    {
                        Name = tableName,
                        Columns = columns.Array,
                        Type = ColumnCollectionType.None
                    },
                    Identity = identity
                };
            }
        }
        /// <summary>
        /// 创建表格
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="table">表格信息</param>
        internal override void CreateTable(DbConnection connection, TableColumnCollection table)
        {
            string tableName = table.Columns.Name, sql;
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                sqlStream.SimpleWrite("create table ");
                sqlStream.SimpleWrite(tableName);
                sqlStream.SimpleWrite(" (");
                bool isNext = false;
                foreach (Column column in table.Columns.Columns)
                {
                    if (isNext) sqlStream.Write(',');
                    constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
                    sqlStream.Write(' ');
                    sqlStream.Write(column.DbType.getSqlTypeName());
                    isNext = true;
                }
                sqlStream.Write(')');
                sql = sqlStream.ToString();
            }
            finally
            {
                UnmanagedPool.Default.Push(ref buffer);
                sqlStream.Dispose();
                Interlocked.Exchange(ref this.sqlStream, sqlStream);
            }
            executeNonQuery(connection, sql);
        }
        /// <summary>
        /// 是否支持索引
        /// </summary>
        internal override bool IsIndex
        {
            get { return false; }
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
        /// 删除列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">删除列集合</param>
        internal override void DeleteFields(DbConnection connection, ColumnCollection columnCollection)
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// 是否支持新增列
        /// </summary>
        internal override bool IsAddField
        {
            get { return false; }
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
        /// 设置真实成员位图
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <typeparam name="modelType"></typeparam>
        /// <param name="sqlTool"></param>
        /// <param name="memberMap"></param>
        internal override void SetRealMemberMap<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, MemberMap<modelType> memberMap)
        {
            if (sqlTool.Attribute.IsSetRealMemberMap)
            {
                MemberMap<modelType> realMemberMap = MemberMap<modelType>.NewEmpty();
                using (DbConnection connection = GetConnection())
                using (DbCommand command = getCommand(connection, "select top 1 * from [" + sqlTool.TableName + "]", CommandType.Text))
                using (DataSet dataSet = getDataSet(command))
                {
                    foreach (DataColumn dataColumn in dataSet.Tables[0].Columns) realMemberMap.SetMember(dataColumn.ColumnName);
                }
                memberMap.And(realMemberMap);
            }
        }
        /// <summary>
        /// 委托关联表达式转SQL表达式
        /// </summary>
        /// <param name="expression">委托关联表达式</param>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="query">查询信息</param>
        internal override void GetSql<modelType>(Expression expression, CharStream sqlStream, ref SelectQuery<modelType> query)
        {
            throw new InvalidOperationException();
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
                sqlStream.SimpleWrite("select ");
                int count = query.SkipCount + createQuery.GetCount;
                if (count != 0)
                {
                    sqlStream.SimpleWrite("top ");
                    AutoCSer.Extensions.NumberExtension.ToString(count, sqlStream);
                    sqlStream.Write(' ');
                }
                if (query.MemberMap != null) AutoCSer.Sql.DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                else sqlStream.Write('*');
                sqlStream.SimpleWrite(" from [");
                sqlStream.SimpleWrite(sqlTool.TableName);
                sqlStream.Write(']');
                createQuery.WriteWhere(sqlTool, sqlStream, ref query);
                createQuery.WriteOrder(sqlTool, sqlStream, constantConverter, ref query);
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
        /// 获取查询信息
        /// </summary>
        /// <typeparam name="valueType">对象类型</typeparam>
        /// <typeparam name="modelType">模型类型</typeparam>
        /// <param name="sqlTool">SQL操作工具</param>
        /// <param name="connection"></param>
        /// <param name="query">查询信息</param>
        /// <returns>对象集合</returns>
        internal override ReturnValue<LeftArray<valueType>> Select<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            try
            {
                if (query.Sql != null)
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
                                int skipCount = query.SkipCount;
                                while (skipCount != 0 && reader.Read()) --skipCount;
                                if (skipCount == 0)
                                {
                                    LeftArray<valueType> array = new LeftArray<valueType>(0);
                                    while (reader.Read())
                                    {
                                        valueType value = AutoCSer.Metadata.DefaultConstructor<valueType>.Constructor();
                                        DataModel.Model<modelType>.Setter.Set(reader, value, query.MemberMap);
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
        internal override ReturnValue<LeftArray<valueType>> Select<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SelectQuery<modelType> query, Func<DbDataReader, valueType> readValue)
        {
            try
            {
                if (query.Sql != null)
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
            throw new InvalidOperationException();
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
            throw new InvalidOperationException();
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
        internal override ReturnType Update<valueType, modelType>
            (Sql.Table<valueType, modelType> sqlTool, valueType value, MemberMap<modelType> memberMap, ref UpdateQuery<modelType> query)
        {
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
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
            throw new InvalidOperationException();
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
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
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
            return ReturnType.InvalidOperation;
        }
    }
}
