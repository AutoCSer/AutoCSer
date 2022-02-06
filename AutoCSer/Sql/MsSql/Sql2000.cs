using System;
using System.Linq.Expressions;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using AutoCSer.Extensions;
using System.Collections.Generic;
using AutoCSer.Metadata;
using AutoCSer.Memory;

namespace AutoCSer.Sql.MsSql
{
    /// <summary>
    /// SQL Server 2000 客户端
    /// </summary>
    internal unsafe class Sql2000 : Client
    {
        /// <summary>
        /// 默认间隔毫秒数
        /// </summary>
        internal const int DefaultNowTimeMilliseconds = 4;
        /// <summary>
        /// 最小时间
        /// </summary>
        private static readonly DateTime minDateTime = new DateTime(1753, 1, 1);
        /// <summary>
        /// 最小时间
        /// </summary>
        public override DateTime MinDateTime { get { return minDateTime; } }
        /// <summary>
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal Sql2000(Connection connection) : base(connection, ConstantConverter.SqlServer) { }
        /// <summary>
        /// 创建 SQL 连接
        /// </summary>
        /// <param name="connection"></param>
        protected override void createConnection(ref DbConnection connection)
        {
            (connection = new SqlConnection(Connection.ConnectionString)).Open();
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
            DbCommand command = new SqlCommand(sql, new UnionType.SqlConnection { Object = connection }.Value);
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
            return new SqlDataAdapter(new UnionType.SqlCommand { Object = command }.Value);
        }
        /// <summary>
        /// 获取表格名称的SQL语句
        /// </summary>
        protected virtual string getTableNameSql
        {
            get
            {
                return "select name from sysobjects where(status&0xe0000000)=0x60000000 and objectproperty(id,'IsUserTable')=1";
            }
        }
        /// <summary>
        /// 获取表格名称集合
        /// </summary>
        /// <returns></returns>
        public override LeftArray<string> GetTableNames()
        {
            LeftArray<string> names = new LeftArray<string>(0);
            using (DbConnection dbConnection = GetConnection())
            using (DbCommand command = getCommand(dbConnection, getTableNameSql, CommandType.Text))
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read()) names.Add((string)reader[0]);
            }
            return names;
        }
        /// <summary>
        /// 最大字符串长度
        /// </summary>
        private const int maxStringSize = 4000;
        /// <summary>
        /// 成员信息转换为数据列
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="memberAttribute">SQL成员信息</param>
        /// <returns>数据列</returns>
        internal override Column GetColumn(Type type, MemberAttribute memberAttribute)
        {
            SqlDbType sqlType = SqlDbType.NVarChar;
            int size = maxStringSize;
            Type memberType = memberAttribute.DataType ?? type;
            if (memberType == typeof(string))
            {
                if (memberAttribute.MaxStringLength > 0 && memberAttribute.MaxStringLength <= maxStringSize)
                {
                    if (memberAttribute.IsFixedLength) sqlType = memberAttribute.IsAscii ? SqlDbType.Char : SqlDbType.NChar;
                    else sqlType = memberAttribute.IsAscii ? SqlDbType.VarChar : SqlDbType.NVarChar;
                    size = memberAttribute.MaxStringLength <= maxStringSize ? memberAttribute.MaxStringLength : maxStringSize;
                }
                else if (!memberAttribute.IsFixedLength && memberAttribute.MaxStringLength == 0)
                {
                    sqlType = memberAttribute.IsAscii ? SqlDbType.VarChar : SqlDbType.NVarChar;
                    size = 0;
                }
                else
                {
                    sqlType = memberAttribute.IsAscii ? SqlDbType.Text : SqlDbType.NText;
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
        /// 根据表格名称获取表格信息的SQL语句
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息的SQL语句</returns>
        protected virtual string getTableSql(string tableName)
        {
            return @"declare @id int
set @id=object_id(N'[dbo].[" + tableName + @"]')
if(select top 1 id from sysobjects where id=@id and objectproperty(id,N'IsUserTable')=1)is not null begin
 select columnproperty(id,name,'IsIdentity')as isidentity,id,xusertype,name,length,xprec,xscale,isnullable,colid,isnull((select top 1 text from syscomments where id=syscolumns.cdefault and colid=1),'')as defaultValue,isnull((select top 1 cast(value as varchar(256))from sysproperties where id=syscolumns.id and smallid=syscolumns.colid),'')as remark from syscolumns where id=@id order by colid
 if @@rowcount<>0 begin
  select a.indid,a.colid,b.name,(case when b.status=2 then 'UQ' else(select top 1 xtype from sysobjects where name=b.name)end)as type from sysindexkeys a left join sysindexes b on a.id=b.id and a.indid=b.indid where a.id=@id order by a.indid,a.keyno
 end
end";
        }
        /// <summary>
        /// 根据表格名称获取表格信息
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="tableName">表格名称</param>
        /// <returns>表格信息</returns>
        internal override TableColumnCollection GetTable(DbConnection connection, string tableName)
        {
            using (DbCommand command = getCommand(connection, getTableSql(tableName), CommandType.Text))
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
            {
                Column identity = null;
                Dictionary<short, Column> columns = DictionaryCreator.CreateShort<Column>();
                while (reader.Read())
                {
                    SqlDbType type = DbType.GetType((short)reader["xusertype"]);
                    int size;
                    switch (type)
                    {
                        case SqlDbType.NChar: case SqlDbType.NVarChar: size = (int)(short)reader["length"] >> 1; break;
                        case SqlDbType.Text: case SqlDbType.NText: size = int.MaxValue; break;
                        case SqlDbType.Decimal:
                        case SqlDbType.Money:
                        case SqlDbType.SmallMoney:
                            size = ((int)(byte)reader["xprec"] << 8) + (byte)reader["xscale"];
                            break;
                        default: size = (int)(short)reader["length"]; break;
                    }
                    Column column = new Column
                    {
                        Name = reader["name"].ToString(),
                        DbType = type,
                        Size = size,
                        DefaultValue = formatDefaultValue(reader["defaultValue"]),
                        Remark = reader["remark"].ToString(),
                        //GetColumnRemark(table, connection, name),
                        IsNull = (int)reader["isnullable"] == 1,
                    };
                    columns.Add((short)reader["colid"], column);
                    if ((int)reader["isidentity"] == 1) identity = column;
                }
                LeftArray<ColumnCollection> columnCollections = new LeftArray<ColumnCollection>(0);
                if (reader.NextResult())
                {
                    short indexId = -1;
                    string indexName = null;
                    ColumnCollectionType columnType = ColumnCollectionType.Index;
                    LeftArray<short> columnId = new LeftArray<short>(0);
                    while (reader.Read())
                    {
                        if (indexId != (short)reader["indid"])
                        {
                            if (indexId != -1)
                            {
                                Column[] indexs = columnId.GetArray(columnIndex => columns[columnIndex]);
                                columnCollections.Add(new ColumnCollection
                                {
                                    Type = columnType,
                                    Name = indexName,
                                    Columns = indexs
                                });
                            }
                            columnId.Length = 0;
                            indexId = (short)reader["indid"];
                            indexName = reader["name"].ToString();
                            string type = reader["type"].ToString();
                            if (type == "PK") columnType = ColumnCollectionType.PrimaryKey;
                            else if (type == "UQ") columnType = ColumnCollectionType.UniqueIndex;
                            else columnType = ColumnCollectionType.Index;
                        }
                        columnId.Add((short)reader["colid"]);
                    }
                    if (indexId != -1)
                    {
                        columnCollections.Add(new ColumnCollection
                        {
                            Type = columnType,
                            Name = indexName,
                            Columns = columnId.GetArray(columnIndex => columns[columnIndex])
                        });
                    }
                }
                if (columns.Count != 0)
                {
                    ColumnCollection primaryKey = columnCollections.FirstOrDefault(columnCollection => columnCollection.Type == ColumnCollectionType.PrimaryKey);
                    return new TableColumnCollection
                    {
                        Columns = new ColumnCollection
                        {
                            Name = tableName,
                            Columns = columns.Values.getArray(),
                            Type = ColumnCollectionType.None
                        },
                        Identity = identity,
                        PrimaryKey = primaryKey,
                        Indexs = columnCollections.GetFindArray(columnCollection => columnCollection.Type != ColumnCollectionType.PrimaryKey)
                    };
                }
                return null;
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
                sqlStream.SimpleWrite("create table[");
                sqlStream.SimpleWrite(this.Connection.Owner);
                sqlStream.SimpleWrite("].[");
                sqlStream.SimpleWrite(tableName);
                sqlStream.SimpleWrite("](");
                bool isTextImage = false, isNext = false;
                foreach (Column column in table.Columns.Columns)
                {
                    if (isNext) sqlStream.Write(',');
                    appendColumn(sqlStream, column);
                    if (!isTextImage) isTextImage = column.DbType.isTextImageType();
                    isNext = true;
                }
                ColumnCollection primaryKey = table.PrimaryKey;
                if (primaryKey != null && primaryKey.Columns.length() != 0)
                {
                    isNext = false;
                    sqlStream.SimpleWrite(",primary key(");
                    foreach (Column column in primaryKey.Columns)
                    {
                        if (isNext) sqlStream.Write(',');
                        constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
                        isNext = true;
                    }
                    sqlStream.Write(')');
                }
                sqlStream.SimpleWrite(")on[primary]");
                if (isTextImage) sqlStream.Write(" textimage_on[primary]");
                foreach (Column column in table.Columns.Columns)
                {
                    if (!string.IsNullOrEmpty(column.Remark))
                    {
                        sqlStream.Write(@"
exec dbo.sp_addextendedproperty @name=N'MS_Description',@value=N");
                        constantConverter.Convert(sqlStream, column.Remark);
                        sqlStream.Write(",@level0type=N'USER',@level0name=N'");
                        sqlStream.SimpleWrite(this.Connection.Owner);
                        sqlStream.Write("',@level1type=N'TABLE',@level1name=N'");
                        sqlStream.SimpleWrite(tableName);
                        sqlStream.Write("', @level2type=N'COLUMN',@level2name=N'");
                        constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
                        sqlStream.Write('\'');
                    }
                }
                if (table.Indexs != null)
                {
                    foreach (ColumnCollection columns in table.Indexs)
                    {
                        if (columns != null && columns.Columns.length() != 0)
                        {
                            sqlStream.SimpleWrite(@"
create");
                            if (columns.Type == ColumnCollectionType.UniqueIndex) sqlStream.SimpleWrite(" unique");
                            sqlStream.SimpleWrite(" index[");
                            AppendIndexName(sqlStream, tableName, columns);
                            sqlStream.SimpleWrite("]on[");
                            sqlStream.SimpleWrite(this.Connection.Owner);
                            sqlStream.SimpleWrite("].[");
                            sqlStream.SimpleWrite(tableName);
                            sqlStream.SimpleWrite("](");
                            isNext = false;
                            foreach (Column column in columns.Columns)
                            {
                                if (isNext) sqlStream.Write(',');
                                constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
                                isNext = true;
                            }
                            sqlStream.SimpleWrite(")on[primary]");
                        }
                    }
                }
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
        /// 写入列信息
        /// </summary>
        /// <param name="sqlStream">SQL语句流</param>
        /// <param name="column">列信息</param>
        private void appendColumn(CharStream sqlStream, Column column)
        {
            constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
            sqlStream.Write(' ');
            sqlStream.SimpleWrite(column.DbType.ToString());
            if (column.DbType.isDecimalType() && column.Size != 0)
            {
                sqlStream.Write('(');
                AutoCSer.Extensions.NumberExtension.ToString(column.Size >> 8, sqlStream);
                sqlStream.Write(',');
                AutoCSer.Extensions.NumberExtension.ToString((column.Size) & 0xff, sqlStream);
                sqlStream.Write(')');
            }
            //if (isIdentity) sqlStream.Write(" identity(1,1)not");
            //else
            //{
            if (column.DbType.isStringType() && column.Size != int.MaxValue)
            {
                sqlStream.Write('(');
                sqlStream.SimpleWrite(column.Size == 0 ? "max" : column.Size.toString());
                sqlStream.Write(')');
            }
            else if (column.DbType == SqlDbType.Decimal) sqlStream.Write("(18,2)");
            if (column.DefaultValue != null)
            {
                sqlStream.SimpleWrite(" default ");
                sqlStream.SimpleWrite(column.DefaultValue);
            }
            if (!column.IsNull) sqlStream.SimpleWrite(" not");
            //}
            sqlStream.SimpleWrite(" null");
        }
        /// <summary>
        /// 删除默认值左右括号()
        /// </summary>
        /// <param name="defaultValue">默认值</param>
        /// <returns>默认值</returns>
        protected static string formatDefaultValue(object defaultValue)
        {
            if (defaultValue != null)
            {
                string value = defaultValue.ToString();
                if (value.Length != 0)
                {
                    int valueIndex = 0, index = 0;
                    int[] valueIndexs = new int[value.Length];
                    for (int length = value.Length; index != length; ++index)
                    {
                        if (value[index] == '(') ++valueIndex;
                        else if (value[index] == ')') valueIndexs[--valueIndex] = index;
                    }
                    index = 0;
                    for (int length = value.Length - 1; valueIndexs[index] == length && value[index] == '('; --length) ++index;
                    value = value.Substring(index, value.Length - (index << 1));
                }
                return value;
            }
            return null;
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tableName">表格名称</param>
        /// <param name="columnCollection">索引列集合</param>
        internal override void CreateIndex(DbConnection connection, string tableName, ColumnCollection columnCollection)
        {
            CharStream sqlStream = System.Threading.Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            string sql;
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                sqlStream.Write(columnCollection.Type == ColumnCollectionType.UniqueIndex ? @"
create unique index[" : @"
create index[");
                AppendIndexName(sqlStream, tableName, columnCollection);
                sqlStream.SimpleWrite("]on[");
                sqlStream.SimpleWrite(this.Connection.Owner);
                sqlStream.SimpleWrite("].[");
                sqlStream.SimpleWrite(tableName);
                sqlStream.SimpleWrite("](");
                bool isNext = false;
                foreach (Column column in columnCollection.Columns)
                {
                    if (isNext) sqlStream.Write(',');
                    constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
                    isNext = true;
                }
                sqlStream.SimpleWrite(")on[primary]");
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
        /// 删除列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">删除列集合</param>
        internal override void DeleteFields(DbConnection connection, ColumnCollection columnCollection)
        {
            string tableName = columnCollection.Name, sql;
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                foreach (Column column in columnCollection.Columns)
                {
                    sqlStream.Write(@"
alter table [");
                    sqlStream.SimpleWrite(this.Connection.Owner);
                    sqlStream.SimpleWrite("].[");
                    sqlStream.SimpleWrite(tableName);
                    sqlStream.SimpleWrite(@"]drop column ");
                    constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
                }
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
        /// 新增列集合
        /// </summary>
        /// <param name="connection">SQL连接</param>
        /// <param name="columnCollection">新增列集合</param>
        internal override void AddFields(DbConnection connection, ColumnCollection columnCollection)
        {
            string tableName = columnCollection.Name, sql;
            CharStream sqlStream = Interlocked.Exchange(ref this.sqlStream, null);
            if (sqlStream == null) sqlStream = new CharStream(default(AutoCSer.Memory.Pointer));
            bool isUpdateValue = false;
            AutoCSer.Memory.Pointer buffer = default(AutoCSer.Memory.Pointer);
            try
            {
                buffer = UnmanagedPool.Default.GetPointer();
                sqlStream.Reset(ref buffer);
                foreach (Column column in columnCollection.Columns)
                {
                    sqlStream.Write(@"
alter table [");
                    sqlStream.SimpleWrite(this.Connection.Owner);
                    sqlStream.SimpleWrite("].[");
                    sqlStream.SimpleWrite(tableName);
                    sqlStream.SimpleWrite(@"]add ");
                    if (!column.IsNull && column.DefaultValue == null)
                    {
                        column.DefaultValue = column.DbType.getDefaultValue();
                        if (column.DefaultValue == null) column.IsNull = true;
                    }
                    appendColumn(sqlStream, column);
                    if (column.UpdateValue != null) isUpdateValue = true;
                }
                if (isUpdateValue)
                {
                    sqlStream.SimpleWrite(@"
update[");
                    sqlStream.SimpleWrite(tableName);
                    sqlStream.SimpleWrite("]set ");
                    foreach (Column column in columnCollection.Columns)
                    {
                        if (column.UpdateValue != null)
                        {
                            if (!isUpdateValue) sqlStream.Write(',');
                            constantConverter.ConvertNameToSqlStream(sqlStream, column.Name);
                            sqlStream.Write('=');
                            sqlStream.Write(column.UpdateValue);
                            isUpdateValue = false;
                        }
                    }
                    sqlStream.SimpleWrite(" from[");
                    sqlStream.SimpleWrite(tableName);
                    sqlStream.SimpleWrite("]with(nolock)");
                }
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
            sqlStream.Write(" in(select top ");
            AutoCSer.Extensions.NumberExtension.ToString(createQuery.GetCount, sqlStream);
            sqlStream.Write(' ');
            sqlStream.SimpleWrite(keyName);
            sqlStream.SimpleWrite(" from[");
            sqlStream.SimpleWrite(sqlTool.TableName);
            sqlStream.Write("]with(nolock)where ");
            int whereSize, whereIndex;
            if (createQuery.Where.IsWhereTrue) whereSize = whereIndex = 0;
            else
            {
                sqlStream.Write('(');
                whereIndex = sqlStream.Length;
                sqlTool.Client.GetSql(createQuery.Where.Expression, sqlStream, ref query);
                if ((whereSize = sqlStream.Length - whereIndex) == 0) sqlStream.Data.CurrentIndex -= sizeof(char);
                else sqlStream.Write(")and ");
            }
            sqlStream.SimpleWrite(keyName);
            sqlStream.Write(" not in(select top ");
            AutoCSer.Extensions.NumberExtension.ToString(query.SkipCount, sqlStream);
            sqlStream.Write(' ');
            sqlStream.SimpleWrite(keyName);
            sqlStream.SimpleWrite(" from[");
            sqlStream.SimpleWrite(sqlTool.TableName);
            sqlStream.SimpleWrite("]with(nolock)");
            if (whereSize != 0)
            {
                sqlStream.SimpleWrite("where ");
                sqlStream.Write(sqlStream.Char + whereIndex, whereSize);
            }
            whereIndex = sqlStream.Length;
            createQuery.WriteOrder(sqlTool, sqlStream, constantConverter, ref query);
            whereSize = sqlStream.Length - whereIndex;
            sqlStream.Write(')');
            if (whereSize != 0) sqlStream.Write(sqlStream.Char + whereIndex, whereSize);
            sqlStream.Write(')');
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
                    else selectNoOrder(sqlTool, ref createQuery, ref query, sqlStream);
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
                sqlStream.SimpleWrite("select top 1 ");
                DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                sqlStream.SimpleWrite(" from[");
                sqlStream.SimpleWrite(sqlTool.TableName);
                sqlStream.Write("]with(nolock)where ");
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
                sqlStream.SimpleWrite("select top 1 ");
                DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap, constantConverter);
                sqlStream.SimpleWrite(" from[");
                sqlStream.SimpleWrite(sqlTool.TableName);
                sqlStream.Write("]with(nolock)where ");
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
        internal override ReturnType Update<valueType, modelType>
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
                        sqlStream.Write(@"update[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select top 1 ");
                        DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        int size = sqlStream.Data.CurrentIndex >> 1;
                        sqlStream.Write(@"
if @@ROWCOUNT<>0 begin
 update[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.SimpleWrite(@"
 ");
                        sqlStream.Write(sqlStream.Char, size);
                        sqlStream.SimpleWrite(@"
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return ReturnType.Success;
                }
                if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    if (query.NotQuery)
                    {
                        sqlStream.Write(@"update[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select top 1 ");
                        DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        int size = sqlStream.Data.CurrentIndex >> 1;
                        sqlStream.Write(@"
if @@ROWCOUNT<>0 begin
 update[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]set ");
                        DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, constantConverter, sqlTool);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWrite(@"
 ");
                        sqlStream.Write(sqlStream.Char, size);
                        sqlStream.SimpleWrite(@"
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
                if (query.NotQuery)
                {
                    if (executeNonQuery(connection, query.UpdateSql) > 0)
                    {
                        sqlTool.CallOnUpdated(value, null, memberMap);
                        return ReturnType.Success;
                    }
                    return ReturnType.NotFoundData;
                }
                else
                {
                    ReturnType returnType = ReturnType.Unknown;
                    using (DbCommand command = getCommand(connection, query.Sql, CommandType.Text))
                    {
                        try
                        {
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
                            returnType = ReturnType.NotFoundData;
                        }
                        finally
                        {
                            if (returnType == ReturnType.Unknown) sqlTool.Log.Error(query.Sql, LogLevel.Error | LogLevel.AutoCSer);
                        }
                    }
                    return returnType;
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
                if (executeNonQuery(transaction, query.UpdateSql) > 0)
                {
                    sqlTool.CallOnUpdated(transaction, value, memberMap);
                    return ReturnType.Success;
                }
                return ReturnType.ExecuteFailed;
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
                    sqlStream.SimpleWrite("insert into[");
                    sqlStream.SimpleWrite(sqlTool.TableName);
                    sqlStream.SimpleWrite("](");
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap, constantConverter);
                    sqlStream.SimpleWrite(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, constantConverter, sqlTool);
                    if(!query.NotQuery)
                    {
                        sqlStream.Write(@")
if @@ROWCOUNT<>0 begin
 select top 1 ");
                        DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.SimpleWrite(@"
end");
                    }
                    else sqlStream.Write(')');
                }
                else
                {
                    sqlStream.SimpleWrite("insert into[");
                    sqlStream.SimpleWrite(sqlTool.TableName);
                    sqlStream.SimpleWrite("](");
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap, constantConverter);
                    sqlStream.SimpleWrite(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, constantConverter, sqlTool);
                    if (!query.NotQuery && DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                    {
                        sqlStream.Write(@")
if @@ROWCOUNT<>0 begin
 select top 1 ");
                        DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWrite(@"
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
        internal override ReturnValue<SubArray<valueType>> Insert<valueType, modelType>(Sql.Table<valueType, modelType> sqlTool, ref DbConnection connection, ref SubArray<valueType> array)
        {
            if (connection == null) connection = GetConnection();
            if (connection != null)
            {
                DataTable data = sqlTool.GetDataTable(ref array);
                using (SqlBulkCopy copy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.UseInternalTransaction, null))
                {
                    import(copy, data);
                }
                sqlTool.CallOnInserted(ref array);
                return array;
            }
            return ReturnType.ConnectionFailed;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="copy"></param>
        /// <param name="data"></param>
        private void import(SqlBulkCopy copy, DataTable data)
        {
            copy.BatchSize = data.Rows.Count;
            copy.DestinationTableName = data.TableName;
            foreach (DataColumn column in data.Columns) copy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
            copy.WriteToServer(data);
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
                DataTable data = sqlTool.GetDataTable(ref array);
                using (SqlBulkCopy copy = new SqlBulkCopy((SqlConnection)transaction.Connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction.DbTransaction))
                {
                    import(copy, data);
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
                        sqlStream.Write(@"delete[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select top 1 ");
                        DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.Write(@"
if @@ROWCOUNT<>0 begin
 delete[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        constantConverter.ConvertNameToSqlStream(sqlStream, DataModel.Model<modelType>.Identity.FieldInfo.Name);
                        sqlStream.Write('=');
                        AutoCSer.Extensions.NumberExtension.ToString(identity, sqlStream);
                        sqlStream.SimpleWrite(@"
end");
                    }
                    query.Sql = sqlStream.ToString();
                    return ReturnType.Success;
                }
                else if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    if (query.NotQuery)
                    {
                        sqlStream.Write(@"delete[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                    }
                    else
                    {
                        sqlStream.SimpleWrite("select top 1 ");
                        DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap, constantConverter);
                        sqlStream.SimpleWrite(" from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.Write(@"
if @@ROWCOUNT<>0 begin
 delete[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.SimpleWrite("]from[");
                        sqlStream.SimpleWrite(sqlTool.TableName);
                        sqlStream.Write("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, constantConverter);
                        sqlStream.SimpleWrite(@"
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
            GetQuery<modelType> getQuery = new GetQuery<modelType> { MemberMap = sqlTool.SelectMemberMap, Sql = query.Sql };
            ReturnType returnType = Get(sqlTool, ref connection, value, ref getQuery);
            if (returnType == ReturnType.Success)
            {
                sqlTool.CallOnDeleted(value);
                return ReturnType.Success;
            }
            return returnType;
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
