using System;
using System.Linq.Expressions;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using AutoCSer.Extension;
using System.Collections.Generic;
using AutoCSer.Metadata;

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
        /// SQL客户端操作
        /// </summary>
        /// <param name="connection">SQL连接信息</param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal Sql2000(Connection connection) : base(connection) { }
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
        /// <returns>SQL命令</returns>
        protected override DbCommand getCommand(DbConnection connection, string sql)
        {
            DbCommand command = new SqlCommand(sql, new UnionType { Value = connection }.SqlConnection);
            command.CommandType = CommandType.Text;
            return command;
        }
        /// <summary>
        /// 获取数据适配器
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <returns>数据适配器</returns>
        protected override DbDataAdapter getAdapter(DbCommand command)
        {
            return new SqlDataAdapter(new UnionType { Value = command }.SqlCommand);
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
                else if (!memberAttribute.IsFixedLength && memberAttribute.MaxStringLength == -1)
                {
                    sqlType = memberAttribute.IsAscii ? SqlDbType.VarChar : SqlDbType.NVarChar;
                    size = memberAttribute.MaxStringLength <= maxStringSize ? memberAttribute.MaxStringLength : maxStringSize;
                }
                else
                {
                    sqlType = memberAttribute.IsAscii ? SqlDbType.Text : SqlDbType.NText;
                    size = int.MaxValue;
                }
            }
            else
            {
                sqlType = memberType.formCSharpType();
                size = sqlType.getSize();
            }
            return new Column
            {
                DbType = sqlType,
                Size = size,
                IsNull = memberAttribute.IsDefaultMember && memberType != typeof(string) ? type.isNull() : memberAttribute.IsNull,
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
 select columnproperty(id,name,'IsIdentity')as isidentity,id,xusertype,name,length,isnullable,colid,isnull((select top 1 text from syscomments where id=syscolumns.cdefault and colid=1),'')as defaultValue,isnull((select top 1 cast(value as varchar(256))from sysproperties where id=syscolumns.id and smallid=syscolumns.colid),'')as remark from syscolumns where id=@id order by colid
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
            using (DbCommand command = getCommand(connection, getTableSql(tableName)))
            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.Default))
            {
                Column identity = null;
                Dictionary<short, Column> columns = DictionaryCreator.CreateShort<Column>();
                while (reader.Read())
                {
                    SqlDbType type = DbType.GetType((short)reader["xusertype"]);
                    int size = (int)(short)reader["length"];
                    if (type == SqlDbType.NChar || type == SqlDbType.NVarChar) size >>= 1;
                    else if (type == SqlDbType.Text || type == SqlDbType.NText) size = int.MaxValue;
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
                LeftArray<ColumnCollection> columnCollections = default(LeftArray<ColumnCollection>);
                if (reader.NextResult())
                {
                    short indexId = -1;
                    string indexName = null;
                    ColumnCollectionType columnType = ColumnCollectionType.Index;
                    LeftArray<short> columnId = default(LeftArray<short>);
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
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                sqlStream.SimpleWriteNotNull("create table[");
                sqlStream.SimpleWriteNotNull(this.Connection.Owner);
                sqlStream.SimpleWriteNotNull("].[");
                sqlStream.SimpleWriteNotNull(tableName);
                sqlStream.SimpleWriteNotNull("](");
                bool isTextImage = false, isNext = false;
                foreach (Column column in table.Columns.Columns)
                {
                    if (isNext) sqlStream.Write(',');
                    appendColumn(sqlStream, column);
                    if (!isTextImage) isTextImage = column.DbType.isTextImageType() != 0;
                    isNext = true;
                }
                ColumnCollection primaryKey = table.PrimaryKey;
                if (primaryKey != null && primaryKey.Columns.length() != 0)
                {
                    isNext = false;
                    sqlStream.SimpleWriteNotNull(",primary key(");
                    foreach (Column column in primaryKey.Columns)
                    {
                        if (isNext) sqlStream.Write(',');
                        sqlStream.SimpleWriteNotNull(column.SqlName);
                        isNext = true;
                    }
                    sqlStream.Write(')');
                }
                sqlStream.SimpleWriteNotNull(")on[primary]");
                if (isTextImage) sqlStream.WriteNotNull(" textimage_on[primary]");
                foreach (Column column in table.Columns.Columns)
                {
                    if (!string.IsNullOrEmpty(column.Remark))
                    {
                        sqlStream.WriteNotNull(@"
exec dbo.sp_addextendedproperty @name=N'MS_Description',@value=N");
                        ConstantConverter.Default.Convert(sqlStream, column.Remark);
                        sqlStream.WriteNotNull(",@level0type=N'USER',@level0name=N'");
                        sqlStream.SimpleWriteNotNull(this.Connection.Owner);
                        sqlStream.WriteNotNull("',@level1type=N'TABLE',@level1name=N'");
                        sqlStream.SimpleWriteNotNull(tableName);
                        sqlStream.WriteNotNull("', @level2type=N'COLUMN',@level2name=N'");
                        sqlStream.SimpleWriteNotNull(column.SqlName);
                        sqlStream.Write('\'');
                    }
                }
                if (table.Indexs != null)
                {
                    foreach (ColumnCollection columns in table.Indexs)
                    {
                        if (columns != null && columns.Columns.length() != 0)
                        {
                            sqlStream.SimpleWriteNotNull(@"
create");
                            if (columns.Type == ColumnCollectionType.UniqueIndex) sqlStream.SimpleWriteNotNull(" unique");
                            sqlStream.SimpleWriteNotNull(" index[");
                            AppendIndexName(sqlStream, tableName, columns);
                            sqlStream.SimpleWriteNotNull("]on[");
                            sqlStream.SimpleWriteNotNull(this.Connection.Owner);
                            sqlStream.SimpleWriteNotNull("].[");
                            sqlStream.SimpleWriteNotNull(tableName);
                            sqlStream.SimpleWriteNotNull("](");
                            isNext = false;
                            foreach (Column column in columns.Columns)
                            {
                                if (isNext) sqlStream.Write(',');
                                //sqlStream.Write('[');
                                sqlStream.SimpleWriteNotNull(column.SqlName);
                                //sqlStream.Write(']');
                                isNext = true;
                            }
                            sqlStream.SimpleWriteNotNull(")on[primary]");
                        }
                    }
                }
                sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
            //sqlStream.Write('[');
            sqlStream.SimpleWriteNotNull(column.SqlName);
            sqlStream.Write(' ');
            //sqlStream.Write(']');
            sqlStream.SimpleWriteNotNull(column.DbType.ToString());
            //if (isIdentity) sqlStream.Write(" identity(1,1)not");
            //else
            //{
            if (column.DbType.isStringType() != 0 && column.Size != int.MaxValue)
            {
                sqlStream.Write('(');
                sqlStream.SimpleWriteNotNull(column.Size == -1 ? "max" : column.Size.toString());
                sqlStream.Write(')');
            }
            if (column.DefaultValue != null)
            {
                sqlStream.SimpleWriteNotNull(" default ");
                sqlStream.SimpleWriteNotNull(column.DefaultValue);
            }
            if (!column.IsNull) sqlStream.SimpleWriteNotNull(" not");
            //}
            sqlStream.SimpleWriteNotNull(" null");
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
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            string sql;
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                sqlStream.WriteNotNull(columnCollection.Type == ColumnCollectionType.UniqueIndex ? @"
create unique index[" : @"
create index[");
                AppendIndexName(sqlStream, tableName, columnCollection);
                sqlStream.SimpleWriteNotNull("]on[");
                sqlStream.SimpleWriteNotNull(this.Connection.Owner);
                sqlStream.SimpleWriteNotNull("].[");
                sqlStream.SimpleWriteNotNull(tableName);
                sqlStream.SimpleWriteNotNull("](");
                bool isNext = false;
                foreach (Column column in columnCollection.Columns)
                {
                    if (isNext) sqlStream.Write(',');
                    //sqlStream.Write('[');
                    sqlStream.SimpleWriteNotNull(column.SqlName);
                    //sqlStream.Write(']');
                    isNext = true;
                }
                sqlStream.SimpleWriteNotNull(")on[primary]");
                sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                foreach (Column column in columnCollection.Columns)
                {
                    sqlStream.WriteNotNull(@"
alter table [");
                    sqlStream.SimpleWriteNotNull(this.Connection.Owner);
                    sqlStream.SimpleWriteNotNull("].[");
                    sqlStream.SimpleWriteNotNull(tableName);
                    sqlStream.SimpleWriteNotNull(@"]drop column ");
                    sqlStream.SimpleWriteNotNull(column.SqlName);
                }
                sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
            if (sqlStream == null) sqlStream = new CharStream(null, 0);
            bool isUpdateValue = false;
            byte* buffer = null;
            try
            {
                sqlStream.Reset(buffer = AutoCSer.UnmanagedPool.Default.Get(), AutoCSer.UnmanagedPool.DefaultSize);
                foreach (Column column in columnCollection.Columns)
                {
                    sqlStream.WriteNotNull(@"
alter table [");
                    sqlStream.SimpleWriteNotNull(this.Connection.Owner);
                    sqlStream.SimpleWriteNotNull("].[");
                    sqlStream.SimpleWriteNotNull(tableName);
                    sqlStream.SimpleWriteNotNull(@"]add ");
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
                    sqlStream.SimpleWriteNotNull(@"
update[");
                    sqlStream.SimpleWriteNotNull(tableName);
                    sqlStream.SimpleWriteNotNull("]set ");
                    foreach (Column column in columnCollection.Columns)
                    {
                        if (column.UpdateValue != null)
                        {
                            if (!isUpdateValue) sqlStream.Write(',');
                            sqlStream.SimpleWriteNotNull(column.SqlName);
                            sqlStream.Write('=');
                            sqlStream.WriteNotNull(column.UpdateValue);
                            isUpdateValue = false;
                        }
                    }
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(tableName);
                    sqlStream.SimpleWriteNotNull("]with(nolock)");
                }
                sql = sqlStream.ToString();
            }
            finally
            {
                if (buffer != null) AutoCSer.UnmanagedPool.Default.Push(buffer);
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
            sqlStream.SimpleWriteNotNull("select ");
            DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap);
            sqlStream.SimpleWriteNotNull(" from[");
            sqlStream.SimpleWriteNotNull(sqlTool.TableName);
            sqlStream.WriteNotNull("]with(nolock)where ");
            sqlStream.SimpleWriteNotNull(keyName);
            sqlStream.WriteNotNull(" in(select top ");
            AutoCSer.Extension.Number.ToString(createQuery.GetCount, sqlStream);
            sqlStream.Write(' ');
            sqlStream.SimpleWriteNotNull(keyName);
            sqlStream.SimpleWriteNotNull(" from[");
            sqlStream.SimpleWriteNotNull(sqlTool.TableName);
            sqlStream.WriteNotNull("]with(nolock)where ");
            int whereSize, whereIndex;
            if (createQuery.Where == null) whereSize = whereIndex = 0;
            else
            {
                sqlStream.Write('(');
                whereIndex = sqlStream.Length;
                sqlTool.Client.GetSql(createQuery.Where, sqlStream, ref query);
                if ((whereSize = sqlStream.Length - whereIndex) == 0) sqlStream.ByteSize -= sizeof(char);
                else sqlStream.Write(")and ");
            }
            sqlStream.SimpleWriteNotNull(keyName);
            sqlStream.WriteNotNull(" not in(select top ");
            AutoCSer.Extension.Number.ToString(query.SkipCount, sqlStream);
            sqlStream.Write(' ');
            sqlStream.SimpleWriteNotNull(keyName);
            sqlStream.SimpleWriteNotNull(" from[");
            sqlStream.SimpleWriteNotNull(sqlTool.TableName);
            sqlStream.SimpleWriteNotNull("]with(nolock)");
            if (whereSize != 0)
            {
                sqlStream.SimpleWriteNotNull("where ");
                sqlStream.Write(sqlStream.Char + whereIndex, whereSize);
            }
            whereIndex = sqlStream.Length;
            createQuery.WriteOrder(sqlTool, sqlStream, ref query);
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
            sqlStream.SimpleWriteNotNull("select ");
            int count = query.SkipCount + createQuery.GetCount;
            if (count != 0)
            {
                sqlStream.SimpleWriteNotNull("top ");
                AutoCSer.Extension.Number.ToString(count, sqlStream);
                sqlStream.Write(' ');
            }
            DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap);
            sqlStream.SimpleWriteNotNull(" from [");
            sqlStream.SimpleWriteNotNull(sqlTool.TableName);
            sqlStream.SimpleWriteNotNull("]with(nolock)");
            createQuery.WriteWhere(sqlTool, sqlStream, ref query);
            createQuery.WriteOrder(sqlTool, sqlStream, ref query);
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
                if (query.SkipCount != 0 && createQuery.IsOrder)
                {
                    if (DataModel.Model<modelType>.PrimaryKeys.Length == 1) selectKeys(sqlTool, ref createQuery, ref query, DataModel.Model<modelType>.PrimaryKeys[0].SqlFieldName, sqlStream);
                    else if (DataModel.Model<modelType>.Identity != null) selectKeys(sqlTool, ref createQuery, ref query, DataModel.Model<modelType>.IdentitySqlName, sqlStream);
                    else selectNoOrder(sqlTool, ref createQuery, ref query, sqlStream);
                }
                else selectNoOrder(sqlTool, ref createQuery, ref query, sqlStream);
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
                sqlStream.SimpleWriteNotNull("select top 1 ");
                DataModel.Model<modelType>.GetNames(sqlStream, query.MemberMap);
                sqlStream.SimpleWriteNotNull(" from[");
                sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                sqlStream.WriteNotNull("]with(nolock)where ");
                sqlStream.SimpleWriteNotNull(DataModel.Model<modelType>.IdentitySqlName);
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
                sqlStream.SimpleWriteNotNull("select top 1 ");
                DataModel.Model<modelType>.GetNames(sqlStream, memberMap);
                sqlStream.SimpleWriteNotNull(" from[");
                sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                sqlStream.WriteNotNull("]with(nolock)where ");
                DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, ConstantConverter.Default);
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
                    sqlStream.SimpleWriteNotNull("select top 1 ");
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap);
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    sqlStream.SimpleWriteNotNull(DataModel.Model<modelType>.IdentitySqlName);
                    sqlStream.Write('=');
                    AutoCSer.Extension.Number.ToString(identity, sqlStream);
                    int size = sqlStream.ByteSize >> 1;
                    sqlStream.WriteNotNull(@"
if @@ROWCOUNT<>0 begin
 update[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.SimpleWriteNotNull("]set ");
                    DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, ConstantConverter.Default);
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    sqlStream.SimpleWriteNotNull(DataModel.Model<modelType>.IdentitySqlName);
                    sqlStream.Write('=');
                    AutoCSer.Extension.Number.ToString(identity, sqlStream);
                    sqlStream.SimpleWriteNotNull(@"
 ");
                    sqlStream.WriteNotNull(sqlStream.Char, size);
                    sqlStream.SimpleWriteNotNull(@"
end");
                    query.Sql = sqlStream.ToString();
                    return true;
                }
                if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    sqlStream.SimpleWriteNotNull("select top 1 ");
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, query.MemberMap);
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, ConstantConverter.Default);
                    int size = sqlStream.ByteSize >> 1;
                    sqlStream.WriteNotNull(@"
if @@ROWCOUNT<>0 begin
 update[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.SimpleWriteNotNull("]set ");
                    DataModel.Model<modelType>.Updater.Update(sqlStream, memberMap, value, ConstantConverter.Default);
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, ConstantConverter.Default);
                    sqlStream.SimpleWriteNotNull(@"
 ");
                    sqlStream.WriteNotNull(sqlStream.Char, size);
                    sqlStream.SimpleWriteNotNull(@"
end");
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
                using (DbCommand command = getCommand(connection, query.Sql))
                {
                    try
                    {
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
                                    return true;
                                }
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        sqlTool.Log.add(AutoCSer.Log.LogType.Error, error, query.Sql);
                    }
                }
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
                    sqlStream.SimpleWriteNotNull("insert into[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.SimpleWriteNotNull("](");
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap);
                    sqlStream.SimpleWriteNotNull(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, ConstantConverter.Default);
                    sqlStream.WriteNotNull(@")
if @@ROWCOUNT<>0 begin
 select top 1 ");
                    DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap);
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    sqlStream.SimpleWriteNotNull(DataModel.Model<modelType>.IdentitySqlName);
                    sqlStream.Write('=');
                    AutoCSer.Extension.Number.ToString(identity, sqlStream);
                    sqlStream.SimpleWriteNotNull(@"
end");
                }
                else
                {
                    sqlStream.SimpleWriteNotNull("insert into[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.SimpleWriteNotNull("](");
                    DataModel.Model<modelType>.Inserter.GetColumnNames(sqlStream, memberMap);
                    sqlStream.SimpleWriteNotNull(")values(");
                    DataModel.Model<modelType>.Inserter.Insert(sqlStream, memberMap, value, ConstantConverter.Default);
                    if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                    {
                        sqlStream.WriteNotNull(@")
if @@ROWCOUNT<>0 begin
 select top 1 ");
                        DataModel.Model<modelType>.GetNames(sqlStream, DataModel.Model<modelType>.MemberMap);
                        sqlStream.SimpleWriteNotNull(" from[");
                        sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                        sqlStream.WriteNotNull("]with(nolock)where ");
                        DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, ConstantConverter.Default);
                        sqlStream.SimpleWriteNotNull(@"
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
            if (DataModel.Model<modelType>.Identity != null || DataModel.Model<modelType>.PrimaryKeys.Length != 0)
            {
                GetQuery<modelType> getQuery = new GetQuery<modelType> { MemberMap = DataModel.Model<modelType>.MemberMap, Sql = query.Sql };
                if (!Get(sqlTool, ref connection, value, ref getQuery)) return false;
            }
            else if (executeNonQuery(ref connection, query.Sql) <= 0) return false;
            sqlTool.CallOnInserted(value);
            return true;
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
                    sqlStream.SimpleWriteNotNull("select top 1 ");
                    DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap);
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    sqlStream.SimpleWriteNotNull(DataModel.Model<modelType>.IdentitySqlName);
                    sqlStream.Write('=');
                    AutoCSer.Extension.Number.ToString(identity, sqlStream);
                    sqlStream.WriteNotNull(@"
if @@ROWCOUNT<>0 begin
 delete[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.SimpleWriteNotNull("]from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    sqlStream.SimpleWriteNotNull(DataModel.Model<modelType>.IdentitySqlName);
                    sqlStream.Write('=');
                    AutoCSer.Extension.Number.ToString(identity, sqlStream);
                    sqlStream.SimpleWriteNotNull(@"
end");
                    query.Sql = sqlStream.ToString();
                    return true;
                }
                else if (DataModel.Model<modelType>.PrimaryKeys.Length != 0)
                {
                    sqlStream.SimpleWriteNotNull("select top 1 ");
                    DataModel.Model<modelType>.GetNames(sqlStream, sqlTool.SelectMemberMap);
                    sqlStream.SimpleWriteNotNull(" from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, ConstantConverter.Default);
                    sqlStream.WriteNotNull(@"
if @@ROWCOUNT<>0 begin
 delete[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.SimpleWriteNotNull("]from[");
                    sqlStream.SimpleWriteNotNull(sqlTool.TableName);
                    sqlStream.WriteNotNull("]with(nolock)where ");
                    DataModel.Model<modelType>.PrimaryKeyWhere.Write(sqlStream, value, ConstantConverter.Default);
                    sqlStream.SimpleWriteNotNull(@"
end");
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
            GetQuery<modelType> getQuery = new GetQuery<modelType> { MemberMap = sqlTool.SelectMemberMap, Sql = query.Sql };
            if (Get(sqlTool, ref connection, value, ref getQuery))
            {
                sqlTool.CallOnDeleted(value);
                return true;
            }
            return false;
        }
    }
}
