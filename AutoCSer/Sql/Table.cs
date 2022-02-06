using System;
using System.Reflection;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Data;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据表格
    /// </summary>
    public abstract class Table : IDisposable
    {
        /// <summary>
        /// 自增ID生成器
        /// </summary>
        protected long identity64;
        /// <summary>
        /// 当前自增ID
        /// </summary>
        internal long Identity64
        {
            get { return identity64; }
            set
            {
                if (Attribute.IsSetIdentity)
                {
                    do
                    {
                        long identity = identity64;
                        if (value <= identity) return;
                        if (Interlocked.CompareExchange(ref identity64, value, identity) == identity) return;
                        Log.Debug("Id " + identity.toString() + " 被修改", LogLevel.Debug | LogLevel.Info | LogLevel.AutoCSer);
                    }
                    while (true);
                }
                if (!Attribute.IsLoadIdentity) identity64 = Math.Max(value, Attribute.BaseIdentity - 1);
            }
        }
        /// <summary>
        /// 自增ID
        /// </summary>
        internal long NextIdentity
        {
            get { return Interlocked.Increment(ref identity64); }
        }
        /// <summary>
        /// 数据库表格配置
        /// </summary>
        internal readonly TableAttribute Attribute;
        /// <summary>
        /// 表格名称
        /// </summary>
        internal readonly string TableName;
        /// <summary>
        /// SQL操作客户端
        /// </summary>
        internal readonly Client Client;
        /// <summary>
        /// 当前时间间隔毫秒数
        /// </summary>
        public int NowTimeMilliseconds
        {
            get { return Client.NowTimeMilliseconds; }
        }
        /// <summary>
        /// 当前时间数组
        /// </summary>
        private NowTime[] nowTimes;
        /// <summary>
        /// 操作队列
        /// </summary>
        private readonly Threading.LinkQueueTask queue;
        /// <summary>
        /// 日志处理
        /// </summary>
        internal readonly AutoCSer.ILog Log;
        /// <summary>
        /// 缓存加载完毕事件
        /// </summary>
        public event Action OnCacheLoaded;
        /// <summary>
        /// 计算列加载完成事件
        /// </summary>
        internal event Action OnLogMemberLoaded;
        /// <summary>
        /// 创建缓存等待
        /// </summary>
        private AutoCSer.Threading.WaitHandle createCacheWait;
        /// <summary>
        /// 缓存加载等待
        /// </summary>
        internal AutoCSer.Threading.WaitHandle CacheLoadWait;
        /// <summary>
        /// 是否已经加载缓存
        /// </summary>
        protected int isLoadCache;
        /// <summary>
        /// 日志流数据是否加载完成
        /// </summary>
        protected int isLoadLogStream;
        /// <summary>
        /// 成员名称是否忽略大小写
        /// </summary>
        protected readonly bool ignoreCase;
        /// <summary>
        /// 是否仅允许队列操作
        /// </summary>
        internal bool IsOnlyQueue;
        /// <summary>
        /// 是否初始化错误
        /// </summary>
        internal bool IsError;
        /// <summary>
        /// 待创建一级索引的成员名称集合
        /// </summary>
        internal AutoCSer.StateSearcher.AsciiSearcher<string> NoIndexMemberNames;
        /// <summary>
        /// 数据库表格操作工具
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, string tableName, NowTime[] nowTimes, bool isCreateCacheWait)
            : this(Connection.GetConnection(attribute.ConnectionType), attribute, tableName, nowTimes, isCreateCacheWait) { }
        /// <summary>
        /// 数据库表格操作工具
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(Connection connection, TableAttribute attribute, string tableName, NowTime[] nowTimes, bool isCreateCacheWait)
        {
            Attribute = attribute;
            IsOnlyQueue = attribute.IsOnlyQueue;
            if (attribute.OnLogStreamCount <= 0) attribute.OnLogStreamCount = TableAttribute.DefaultOnLogStreamCount;
            Client = connection.Client;
            Log = connection.Log ?? AutoCSer.LogHelper.Default;
            ignoreCase = connection.ClientAttribute.IgnoreCase;
            TableName = tableName;
            this.nowTimes = nowTimes;
            queue = new Threading.LinkQueueTask(Client, AutoCSer.Threading.ThreadPool.Tiny);
            CacheLoadWait.Set(0);
            if (isCreateCacheWait) createCacheWait.Set(0);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual unsafe void Dispose()
        {
            if (NoIndexMemberNames != null)
            {
                NoIndexMemberNames.Dispose();
                NoIndexMemberNames = null;
            }
        }
        /// <summary>
        /// 创建一个事务处理对象
        /// </summary>
        /// <param name="isolationLevel">默认为 RepeatableRead</param>
        /// <returns></returns>
        public Transaction CreateTransaction(IsolationLevel isolationLevel = IsolationLevel.RepeatableRead)
        {
            return Client.CreateTransaction(isolationLevel);
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name">列名称</param>
        /// <param name="isUnique">是否唯一值</param>
        internal void CreateIndex(DbConnection connection, string name, bool isUnique)
        {
            if (Attribute.IsCheckTable)
            {
                int index = ignoreCase ? NoIndexMemberNames.Searcher.SearchLower(name) : NoIndexMemberNames.Searcher.Search(name);
                if (index >= 0 && NoIndexMemberNames.Array[index] != null)
                {
                    Exception exception = null;
                    try
                    {
                        Client.CreateIndex(connection, TableName, new ColumnCollection
                        {
                            Columns = new Column[] { new Column { Name = name } },
                            Type = isUnique ? ColumnCollectionType.UniqueIndex : ColumnCollectionType.Index
                        });
                        NoIndexMemberNames.Array[index] = null;
                    }
                    catch (Exception error)
                    {
                        exception = error;
                    }
                    if (exception != null) Log.Exception(exception, "索引 " + TableName + "." + name + " 创建失败", LogLevel.Exception | LogLevel.AutoCSer);
                }
            }
        }
        /// <summary>
        /// 等待创建缓存
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WaitCreateCache()
        {
            createCacheWait.Wait();
        }
        /// <summary>
        /// 缓存对象创建完毕
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void CacheCreated()
        {
            createCacheWait.Set();
        }
        /// <summary>
        /// 等待缓存加载
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WaitLoad()
        {
            CacheLoadWait.Wait();
        }
        /// <summary>
        /// 添加操作队列
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void AddQueue(Threading.LinkQueueTaskNode value)
        {
            queue.Add(value);
        }
        /// <summary>
        /// 缓存加载完毕事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        protected void callOnCacheLoaded()
        {
            if (OnCacheLoaded != null) OnCacheLoaded();
        }
        /// <summary>
        /// 计算列加载完成事件
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnLogMemberLoaded()
        {
            if (OnLogMemberLoaded != null) OnLogMemberLoaded();
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="memberIndex"></param>
        /// <returns></returns>
        internal DateTime GetNowTime(DateTime time, int memberIndex)
        {
            NowTime nowTime = nowTimes[memberIndex];
            return nowTime == null ? time : nowTime.Next;
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="table"></param>
        /// <param name="time"></param>
        /// <param name="memberIndex"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DateTime GetNowTime(Table table, DateTime time, int memberIndex)
        {
            return table.GetNowTime(time, memberIndex);
        }

        /// <summary>
        /// 同步执行 SQL 语句
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="reader">读取数据委托</param>
        /// <returns>是否成功</returns>
        public ReturnValue CustomReader(string sql, Action<DbDataReader> reader)
        {
            return IsOnlyQueue ? CustomReaderQueue(sql, reader) : Client.CustomReader(sql, reader, Log);
        }
        /// <summary>
        /// 同步执行 SQL 语句
        /// </summary>
        internal sealed class SqlReader : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table table;
            /// <summary>
            /// SQL 语句
            /// </summary>
            private readonly string sql;
            /// <summary>
            /// 读取数据委托
            /// </summary>
            private readonly Action<DbDataReader> reader;
            /// <summary>
            /// 获取数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 是否操作成功
            /// </summary>
            private ReturnValue returnValue;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="sql"></param>
            /// <param name="reader"></param>
            internal SqlReader(Table table, string sql, Action<DbDataReader> reader)
            {
                this.table = table;
                this.sql = sql;
                this.reader = reader;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue.ReturnType = table.CustomReaderQueue(ref connection, sql, reader);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待获取数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue Wait()
            {
                wait.Set(0);
                table.AddQueue(this);
                wait.Wait();
                return returnValue;
            }
        }
        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="reader">读取数据委托</param>
        /// <returns></returns>
        public ReturnValue CustomReaderQueue(string sql, Action<DbDataReader> reader)
        {
            return new SqlReader(this, sql, reader).Wait();
        }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnType CustomReaderQueue(ref DbConnection connection, string sql, Action<DbDataReader> reader)
        {
            return Client.CustomReader(ref connection, sql, reader, Log, 0);
        }

        /// <summary>
        /// 字符串验证
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="value">成员值</param>
        /// <param name="length">最大长度</param>
        /// <param name="isAscii">是否ASCII</param>
        /// <param name="isNull">是否可以为null</param>
        /// <returns></returns>
        internal unsafe bool StringVerify(string memberName, string value, int length, bool isAscii, bool isNull)
        {
            if (!isNull && value == null)
            {
                NullVerify(memberName);
                return false;
            }
            if (length != 0 && value != null)
            {
                if (isAscii)
                {
                    int nextLength = length - value.Length;
                    if (nextLength >= 0 && value.Length > (length >> 1))
                    {
                        fixed (char* valueFixed = value)
                        {
                            for (char* start = valueFixed, end = valueFixed + value.Length; start != end; ++start)
                            {
                                if ((*start & 0xff00) != 0 && --nextLength < 0) break;
                            }
                        }
                    }
                    if (nextLength < 0)
                    {
                        Log.Error(TableName + "." + memberName + " 超长 " + length.toString(), LogLevel.Error | LogLevel.AutoCSer);
                        return false;
                    }
                }
                else if (value.Length > length)
                {
                    Log.Error(TableName + "." + memberName + " 超长 " + value.Length.toString() + " > " + length.toString(), LogLevel.Error | LogLevel.AutoCSer);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 字符串验证
        /// </summary>
        /// <param name="table"></param>
        /// <param name="memberName">成员名称</param>
        /// <param name="value">成员值</param>
        /// <param name="length">最大长度</param>
        /// <param name="isAscii">是否ASCII</param>
        /// <param name="isNull">是否可以为null</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static bool StringVerify(Table table, string memberName, string value, int length, bool isAscii, bool isNull)
        {
            return table.StringVerify(memberName, value, length, isAscii, isNull);
        }
        /// <summary>
        /// 成员值不能为null
        /// </summary>
        /// <param name="memberName">成员名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void NullVerify(string memberName)
        {
            Log.Error(TableName + "." + memberName + " 不能为null", LogLevel.Error | LogLevel.AutoCSer);
        }
        /// <summary>
        /// 成员值不能为null
        /// </summary>
        /// <param name="table"></param>
        /// <param name="memberName">成员名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static void NullVerify(Table table, string memberName)
        {
            table.NullVerify(memberName);
        }
#if !NOJIT
        /// <summary>
        /// 数据库字符串验证函数
        /// </summary>
        internal static readonly MethodInfo StringVerifyMethod = ((Func<Table, string, string, int, bool, bool, bool>)Table.StringVerify).Method;
        /// <summary>
        /// 数据库字段空值验证
        /// </summary>
        internal static readonly MethodInfo NullVerifyMethod = ((Action<Table, string>)Table.NullVerify).Method;
#endif
    }
    /// <summary>
    /// 数据表格
    /// </summary>
    /// <typeparam name="modelType">模型类型</typeparam>
    public abstract class Table<modelType> : Table
        where modelType : class
    {
        /// <summary>
        /// 更新查询SQL数据成员
        /// </summary>
        internal MemberMap<modelType> SelectMemberMap = new MemberMap<modelType>();
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, string tableName, NowTime[] nowTimes, bool isCreateCacheWait) : base(attribute, tableName, nowTimes, isCreateCacheWait) { }
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="tableName">表格名称</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(Connection connection, TableAttribute attribute, string tableName, NowTime[] nowTimes, bool isCreateCacheWait) : base(connection, attribute, tableName, nowTimes, isCreateCacheWait) { }
        /// <summary>
        /// 数据完成类型注册
        /// </summary>
        /// <param name="onLoaded">数据完成操作</param>
        /// <param name="types">待加载类型集合</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void AddLogStreamLoadedType(Action<LogStream.LoadedType> onLoaded, params LogStream.LoadedType[] types)
        {
            if (Interlocked.CompareExchange(ref isLoadLogStream, 1, 0) == 0) LogStream.LoadedType.Add(typeof(modelType), Attribute.TableNumber, onLoaded, types);
        }
        /// <summary>
        /// 设置更新查询SQL数据成员
        /// </summary>
        /// <param name="member">字段表达式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void SetSelectMember<returnType>(Expression<Func<modelType, returnType>> member)
        {
            SelectMemberMap.SetMember(member);
        }
        /// <summary>
        /// 获取更新查询SQL数据成员
        /// </summary>
        /// <param name="memberMap">查询SQL数据成员</param>
        /// <returns>更新查询SQL数据成员</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal MemberMap<modelType> GetSelectMemberMap(MemberMap<modelType> memberMap)
        {
            MemberMap<modelType> value = SelectMemberMap.Copy();
            value.Or(memberMap);
            return value;
        }
        /// <summary>
        /// 条件表达式转换为 SQL 字符串
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public string GetWhere(Expression<Func<modelType, bool>> where)
        {
            return Client.GetWhere(where.Body);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override unsafe void Dispose()
        {
            base.Dispose();
            if (SelectMemberMap != null)
            {
                SelectMemberMap.Dispose();
                SelectMemberMap = null;
            }
        }
    }
    /// <summary>
    /// 数据表格
    /// </summary>
    /// <typeparam name="tableType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    public class Table<tableType, modelType> : Table<modelType>
        where tableType : class, modelType
        where modelType : class
    {
        /// <summary>
        /// 数据更新事件
        /// </summary>
        /// <param name="newValue">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新数据成员</param>
        public delegate void OnTableUpdated(tableType newValue, tableType oldValue, MemberMap<modelType> memberMap);
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(Connection connection, TableAttribute attribute, NowTime[] nowTimes, bool isCreateCacheWait)
            : base(connection, attribute, attribute.GetTableName(typeof(tableType)), nowTimes, isCreateCacheWait)
        {
            if (attribute.IsCheckTable)
            {
                DbConnection dbConnection = Client.GetConnection();
                if (dbConnection == null)
                {
                    IsError = true;
                    Log.Fatal(TableName + " 初始化失败", LogLevel.Fatal | LogLevel.AutoCSer);
                    return;
                }
                using (dbConnection)
                {
                    TableColumnCollection memberTable = DataModel.Model<modelType>.GetTable(Client, TableName);
                    Client.ToSqlColumn(memberTable);
                    TableColumnCollection table = Client.GetTable(dbConnection, TableName);
                    if (table == null)
                    {
                        Client.CreateTable(dbConnection, memberTable);
                        table = memberTable;
                    }
                    else
                    {
                        ModelAttribute modelAttribute = TypeAttribute.GetAttribute<ModelAttribute>(typeof(modelType), false);
                        if (modelAttribute != null && modelAttribute.DeleteColumnNames.length() != 0)
                        {
                            HashSet<string> deleteNames = modelAttribute.DeleteColumnNames.getHash(value => ignoreCase ? value.toLower() : value);
                            Column[] deleteColumns = table.Columns.Columns.getFindArray(value => deleteNames.Contains(ignoreCase ? value.Name.ToLower() : value.Name));
                            if (deleteColumns.Length != 0)
                            {
                                table.Columns.Columns = table.Columns.Columns.getFindArray(value => !deleteNames.Contains(ignoreCase ? value.Name.ToLower() : value.Name));
                                Client.DeleteFields(dbConnection, new ColumnCollection { Name = memberTable.Columns.Name, Columns = deleteColumns });
                            }
                        }
                        using (AutoCSer.StateSearcher.AsciiSearcher<Column> sqlColumnNames = new AutoCSer.StateSearcher.AsciiSearcher<Column>(ignoreCase ? table.Columns.Columns.getArray(value => value.Name.ToLower()) : table.Columns.Columns.getArray(value => value.Name), table.Columns.Columns, false))
                        {
                            LeftArray<Column> newColumns;
                            if (ignoreCase) newColumns = memberTable.Columns.Columns.getFind(value => sqlColumnNames.Searcher.SearchLower(value.Name) < 0);
                            else newColumns = memberTable.Columns.Columns.getFind(value => sqlColumnNames.Searcher.Search(value.Name) < 0);
                            if (newColumns.Length != 0 && Client.IsAddField)
                            {
                                Client.AddFields(dbConnection, new ColumnCollection { Name = memberTable.Columns.Name, Columns = newColumns.ToArray() });
                                newColumns.Add(table.Columns.Columns);
                                table.Columns.Columns = newColumns.ToArray();
                            }
                            if (ignoreCase) newColumns = memberTable.Columns.Columns.getFind(value => !value.IsMatch(this, sqlColumnNames.Get(value.Name.ToLower()), ignoreCase, attribute.IsIgnoreMatchDateTime));
                            else newColumns = memberTable.Columns.Columns.getFind(value => !value.IsMatch(this, sqlColumnNames.Get(value.Name), ignoreCase, attribute.IsIgnoreMatchDateTime));
                            if (newColumns.count() != 0)
                            {
                                Log.Error("表格 " + memberTable.Columns.Name + " 字段类型不匹配 : " + newColumns.JoinString(",", value => value.Name), LogLevel.Error | LogLevel.AutoCSer);
                            }
                        }
                    }
                    string[] names = ignoreCase ? table.Columns.Columns.getArray(value => value.Name.ToLower()) : table.Columns.Columns.getArray(value => value.Name);
                    NoIndexMemberNames = new AutoCSer.StateSearcher.AsciiSearcher<string>(names, names, false);
                    if (table.Indexs != null)
                    {
                        foreach (ColumnCollection column in table.Indexs) names[ignoreCase ? NoIndexMemberNames.Searcher.SearchLower(column.Columns[0].Name) : NoIndexMemberNames.Searcher.Search(column.Columns[0].Name)] = null;
                    }
                    if (table.PrimaryKey != null) names[ignoreCase ? NoIndexMemberNames.Searcher.SearchLower(table.PrimaryKey.Columns[0].Name) : NoIndexMemberNames.Searcher.Search(table.PrimaryKey.Columns[0].Name)] = null;
                    Field identity = DataModel.Model<modelType>.Identity;
                    if (identity != null)
                    {
                        if (Client.IsIndex) CreateIndex(dbConnection, identity.FieldInfo.Name, true);
                        SelectMemberMap.SetMember(identity.MemberMapIndex);
                        if (attribute.IsLoadIdentity)
                        {
                            using (MemberMap<modelType> memberMap = new MemberMap<modelType>())
                            {
                                memberMap.SetMember(identity.MemberMapIndex);
                                CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(1, new KeyValue<Field, bool>(identity, true));
                                SelectQuery<modelType> query = new SelectQuery<modelType> { MemberMap = memberMap };
                                Client.GetSelectQuery(this, ref createQuery, ref query);
                                IConvertible identityConvertible = Client.GetValue(dbConnection, query.Sql) as IConvertible;
                                identity64 = identityConvertible == null ? attribute.BaseIdentity - 1 : identityConvertible.ToInt64(null);
                            }
                        }
                    }
                    foreach (Field field in DataModel.Model<modelType>.PrimaryKeys)
                    {
                        SelectMemberMap.SetMember(field.MemberMapIndex);
                    }
                }
            }
        }
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, NowTime[] nowTimes, bool isCreateCacheWait)
            : this(Connection.GetConnection(attribute.ConnectionType), attribute, nowTimes, isCreateCacheWait)
        {
        }
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        /// <param name="nowTimes">当前时间数组</param>
        public static Table<tableType, modelType> Get(bool isCreateCacheWait, NowTime[] nowTimes = null)
        {
            Type type = typeof(tableType);
            TableAttribute attribute = TypeAttribute.GetAttribute<TableAttribute>(type, false);
            if (attribute != null && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1)
            {
                Table<tableType, modelType> table = new Table<tableType, modelType>(attribute, nowTimes, isCreateCacheWait);
                if (!table.IsError) return table;
            }
            return null;
        }
        /// <summary>
        /// 添加数据之前的处理事件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancel"></param>
        public delegate void OnInsertCancel(tableType value, ref AnyCancel cancel);
        /// <summary>
        /// 添加数据之前的处理事件
        /// </summary>
        public event OnInsertCancel OnInsert;
        /// <summary>
        /// 添加数据之前的处理事件
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CallOnInsert(tableType value)
        {
            if (OnInsert != null)
            {
                AnyCancel cancel = default(AnyCancel);
                OnInsert(value, ref cancel);
                return !cancel.IsCancelValue;
            }
            return true;
        }
        /// <summary>
        /// 添加数据之前的处理事件
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CallOnInsert(ref SubArray<tableType> array)
        {
            if (OnInsert != null)
            {
                AnyCancel cancel = default(AnyCancel);
                foreach (tableType value in array)
                {
                    OnInsert(value, ref cancel);
                    if (cancel.IsCancelValue) return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 添加数据之后的处理事件
        /// </summary>
        public event Action<tableType> OnInserted;
        /// <summary>
        /// 添加数据之后的处理事件
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnInserted(tableType value)
        {
            if (OnInserted != null) OnInserted(value);
        }
        /// <summary>
        /// 添加数据之后的处理事件
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnInserted(Transaction transaction, tableType value)
        {
            if (OnInserted != null) transaction.OnCommitted += () => OnInserted(value);
        }
        /// <summary>
        /// 添加数据之后的处理事件
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="array"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnInserted(Transaction transaction, SubArray<tableType> array)
        {
            if (OnInserted != null)
            {
                transaction.OnCommitted += () =>
                {
                    foreach (tableType value in array) OnInserted(value);
                };
            }
        }
        /// <summary>
        /// 添加数据之后的处理事件
        /// </summary>
        /// <param name="array"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnInserted(ref SubArray<tableType> array)
        {
            if (OnInserted != null)
            {
                foreach (tableType value in array) OnInserted(value);
            }
        }
        /// <summary>
        /// 更新数据之前的处理事件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
        /// <param name="cancel"></param>
        public delegate void OnUpdateCancel(tableType value, MemberMap<modelType> memberMap, ref AnyCancel cancel);
        /// <summary>
        /// 更新数据之前的处理事件
        /// </summary>
        public event OnUpdateCancel OnUpdate;
        /// <summary>
        /// 更新数据之前的处理事件
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">更新成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CallOnUpdate(tableType value, MemberMap<modelType> memberMap)
        {
            if (OnUpdate != null)
            {
                AnyCancel cancel = default(AnyCancel);
                OnUpdate(value, memberMap, ref cancel);
                return !cancel.IsCancelValue;
            }
            return true;
        }
        /// <summary>
        /// 更新数据之后的处理事件 [更新后的数据 + 更新前的数据 + 更新成员位图]
        /// </summary>
        public event OnTableUpdated OnUpdated;
        /// <summary>
        /// 更新数据之后的处理事件
        /// </summary>
        /// <param name="value">更新后的数据</param>
        /// <param name="oldValue">更新前的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnUpdated(tableType value, tableType oldValue, MemberMap<modelType> memberMap)
        {
            if (OnUpdated != null) OnUpdated(value, oldValue, memberMap);
        }
        /// <summary>
        /// 更新数据之后的处理事件
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="value">更新后的数据</param>
        /// <param name="memberMap">更新成员位图</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnUpdated(Transaction transaction, tableType value, MemberMap<modelType> memberMap)
        {
            if (OnUpdated != null) transaction.OnCommitted += () => OnUpdated(value, null, memberMap);
        }
        /// <summary>
        /// 删除数据之前的处理事件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancel"></param>
        public delegate void OnDeleteCancel(tableType value, ref AnyCancel cancel);
        /// <summary>
        /// 删除数据之前的处理事件
        /// </summary>
        public event OnDeleteCancel OnDelete;
        /// <summary>
        /// 删除数据之前的处理事件
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool CallOnDelete(tableType value)
        {
            if (OnDelete != null)
            {
                AnyCancel cancel = default(AnyCancel);
                OnDelete(value, ref cancel);
                return !cancel.IsCancelValue;
            }
            return true;
        }
        /// <summary>
        /// 删除数据之后的处理事件
        /// </summary>
        public event Action<tableType> OnDeleted;
        /// <summary>
        /// 删除数据之后的处理事件
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnDeleted(tableType value)
        {
            if (OnDeleted != null) OnDeleted(value);
        }
        /// <summary>
        /// 删除数据之后的处理事件
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CallOnDeleted(Transaction transaction, tableType value)
        {
            if (OnDeleted != null) transaction.OnCommitted += () => OnDeleted(value);
        }
        /// <summary>
        /// 缓存数据加载完成
        /// </summary>
        /// <param name="onInserted">添加记录事件</param>
        /// <param name="onUpdated">更新记录事件</param>
        /// <param name="onDeleted">删除记录事件</param>
        /// <param name="isLoadMemberCache">是否加载缓存依赖类型</param>
        /// <param name="isSqlStreamTypeCount">是否日志流计数完成类型注册</param>
        public void CacheLoaded(Action<tableType> onInserted = null, OnTableUpdated onUpdated = null, Action<tableType> onDeleted = null, bool isLoadMemberCache = true, bool isSqlStreamTypeCount = true)
        {
            if (Interlocked.CompareExchange(ref isLoadCache, 1, 0) == 0)
            {
                if (onInserted != null) OnInserted += onInserted;
                if (onUpdated != null) OnUpdated += onUpdated;
                if (onDeleted != null) OnDeleted += onDeleted;
                CacheLoaded(isLoadMemberCache, isSqlStreamTypeCount);
            }
        }
        /// <summary>
        /// 缓存数据加载完成
        /// </summary>
        /// <param name="isLoadMemberCache">是否加载缓存依赖类型</param>
        /// <param name="isSqlStreamTypeCount">是否日志流计数完成类型注册</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CacheLoaded(bool isLoadMemberCache, bool isSqlStreamTypeCount)
        {
            CacheLoadWait.Set();
            callOnCacheLoaded();

            Type memberCacheType = null;
            if (isLoadMemberCache)
            {
                for (Type type = typeof(tableType); type != typeof(modelType); type = type.BaseType)
                {
                    if (type.IsGenericType && TypeAttribute.GetAttribute<MemberCacheLinkAttribute>(type, false) != null)
                    {
                        Type[] types = type.GetGenericArguments();
                        if (types.Length == 2) memberCacheType = types[1];
                        break;
                    }
                }
            }
            LoadMemberCache(memberCacheType);
            if (isSqlStreamTypeCount) LogStream.LoadedType.Add(typeof(modelType), Attribute.TableNumber);
            if (memberCacheType != null) WaitMemberCache();
        }
        /// <summary>
        /// 加载成员缓存初始化依赖类型
        /// </summary>
        /// <param name="memberCacheType">成员缓存绑定类型</param>
        public void LoadMemberCache(Type memberCacheType = null)
        {
            HashSet<Type> cacheTypes = AutoCSer.HashSetCreator.CreateOnly<Type>();
            if (memberCacheType != null)
            {
                foreach (FieldInfo field in memberCacheType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    foreach (object attribute in field.GetCustomAttributes(false))
                    {
                        MemberCacheLinkAttribute memberCacheAttribute = attribute as MemberCacheLinkAttribute;
                        if (memberCacheAttribute != null && memberCacheAttribute.CacheType != null)
                        {
                            cacheTypes.Add(memberCacheAttribute.CacheType);
                            break;
                        }
                    }
                }
            }
            MemberCacheLinkWait.Load(typeof(tableType), cacheTypes);
        }
        /// <summary>
        /// 等待成员扩展缓存初始化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void WaitMemberCache()
        {
            MemberCacheLinkWait.Wait(typeof(tableType));
        }

        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据库记录集合</returns>
        public ReturnValue<LeftArray<tableType>> Select(Expression<Func<modelType, bool>> where = null, MemberMap<modelType> memberMap = null)
        {
            if (IsOnlyQueue) return SelectQueue(where, memberMap);
            CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(where);
            if (createQuery.Where.Type != WhereExpression.ConvertType.Unknown)
            {
                if (!createQuery.Where.IsWhereFalse)
                {
                    SelectQuery<modelType> selectQuery = default(SelectQuery<modelType>);
                    Client.GetSelectQuery(this, memberMap, ref createQuery, ref selectQuery);
                    DbConnection connection = null;
                    try
                    {
                        ReturnValue<LeftArray<tableType>> array = SelectQueue(ref connection, ref selectQuery);
                        Client.FreeConnection(ref connection);
                        return array;
                    }
                    catch (Exception error)
                    {
                        Client.CloseErrorConnection(ref connection);
                        return error;
                    }
                }
                return new LeftArray<tableType>(0);
            }
            return ReturnType.UnknownWhereExpression;
        }
        /// <summary>
        /// 同步获取数据
        /// </summary>
        internal sealed class Selecter : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            internal ReturnValue<LeftArray<tableType>> returnValue;
            /// <summary>
            /// 查询信息
            /// </summary>
            internal SelectQuery<modelType> Query;
            /// <summary>
            /// 获取数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            /// <param name="table"></param>
            internal Selecter(Table<tableType, modelType> table)
            {
                this.table = table;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue = table.SelectQueue(ref connection, ref Query);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待获取数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue<LeftArray<tableType>> Wait()
            {
                wait.Set(0);
                table.AddQueue(this);
                wait.Wait();
                return returnValue;
            }
        }
        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据库记录集合</returns>
        public ReturnValue<LeftArray<tableType>> SelectQueue(Expression<Func<modelType, bool>> where = null, MemberMap<modelType> memberMap = null)
        {
            CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(where);
            if (createQuery.Where.Type != WhereExpression.ConvertType.Unknown)
            {
                if (!createQuery.Where.IsWhereFalse)
                {
                    Selecter selecter = new Selecter(this);
                    Client.GetSelectQuery(this, memberMap, ref createQuery, ref selecter.Query);
                    return selecter.Wait();
                }
                return new LeftArray<tableType>(0);
            }
            return ReturnType.UnknownWhereExpression;
        }
        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="where">查询条件</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据库记录集合</returns>
        internal ReturnValue<LeftArray<tableType>> SelectQueue(ref DbConnection connection, Expression<Func<modelType, bool>> where = null, MemberMap<modelType> memberMap = null)
        {
            CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(where);
            if (createQuery.Where.Type != WhereExpression.ConvertType.Unknown)
            {
                if (!createQuery.Where.IsWhereFalse)
                {
                    SelectQuery<modelType> query = default(SelectQuery<modelType>);
                    Client.GetSelectQuery(this, memberMap, ref createQuery, ref query);
                    return SelectQueue(ref connection, ref query);
                }
                return new LeftArray<tableType>(0);
            }
            return ReturnType.UnknownWhereExpression;
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="createQuery"></param>
        /// <param name="query"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetSelect(MemberMap<modelType> memberMap, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query)
        {
            Client.GetSelectQuery(this, memberMap, ref createQuery, ref query);
        }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<LeftArray<tableType>> SelectQueue(ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            return Client.Select(this, ref connection, ref query);
        }

        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="readValue">读取数据委托</param>
        /// <param name="where">查询条件</param>
        /// <returns>数据库记录集合</returns>
        public ReturnValue<LeftArray<tableType>> Select(Func<DbDataReader, tableType> readValue, Expression<Func<modelType, bool>> where = null)
        {
            if (IsOnlyQueue) return SelectQueue(readValue, where);
            CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(where);
            if (createQuery.Where.Type != WhereExpression.ConvertType.Unknown)
            {
                if (!createQuery.Where.IsWhereFalse)
                {
                    SelectQuery<modelType> selectQuery = default(SelectQuery<modelType>);
                    Client.GetSelectQuery(this, ref createQuery, ref selectQuery);
                    DbConnection connection = null;
                    try
                    {
                        ReturnValue<LeftArray<tableType>> array = SelectQueue(readValue, ref connection, ref selectQuery);
                        Client.FreeConnection(ref connection);
                        return array;
                    }
                    catch (Exception error)
                    {
                        Client.CloseErrorConnection(ref connection);
                        return error;
                    }
                }
                return new LeftArray<tableType>(0);
            }
            return ReturnType.UnknownWhereExpression;
        }
        /// <summary>
        /// 同步获取数据
        /// </summary>
        internal sealed class CustomSelecter : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 读取数据委托
            /// </summary>
            private readonly Func<DbDataReader, tableType> readValue;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            internal ReturnValue<LeftArray<tableType>> returnValue;
            /// <summary>
            /// 查询信息
            /// </summary>
            internal SelectQuery<modelType> Query;
            /// <summary>
            /// 获取数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="readValue"></param>
            internal CustomSelecter(Table<tableType, modelType> table, Func<DbDataReader, tableType> readValue)
            {
                this.table = table;
                this.readValue = readValue;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue = table.SelectQueue(readValue, ref connection, ref Query);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待获取数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue<LeftArray<tableType>> Wait()
            {
                wait.Set(0);
                table.AddQueue(this);
                wait.Wait();
                return returnValue;
            }
        }
        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="readValue">读取数据委托</param>
        /// <param name="where">查询条件</param>
        /// <returns>数据库记录集合，IsNull 表示失败</returns>
        public ReturnValue<LeftArray<tableType>> SelectQueue(Func<DbDataReader, tableType> readValue, Expression<Func<modelType, bool>> where = null)
        {
            CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(where);
            if (createQuery.Where.Type != WhereExpression.ConvertType.Unknown)
            {
                if (!createQuery.Where.IsWhereFalse)
                {
                    CustomSelecter selecter = new CustomSelecter(this, readValue);
                    Client.GetSelectQuery(this, ref createQuery, ref selecter.Query);
                    return selecter.Wait();
                }
                return new LeftArray<tableType>(0);
            }
            return ReturnType.UnknownWhereExpression;
        }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="readValue"></param>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<LeftArray<tableType>> SelectQueue(Func<DbDataReader, tableType> readValue, ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            return Client.Select(this, ref connection, ref query, readValue);
        }

        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="readValue">读取数据委托</param>
        /// <returns>数据库记录集合</returns>
        public ReturnValue<LeftArray<tableType>> Select(string sql, Func<DbDataReader, tableType> readValue)
        {
            return IsOnlyQueue ? SelectQueue(sql, readValue) : Client.Select(sql, readValue, Log);
        }
        /// <summary>
        /// 同步获取数据
        /// </summary>
        internal sealed class SqlSelecter : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// SQL 语句
            /// </summary>
            private readonly string sql;
            /// <summary>
            /// 读取数据委托
            /// </summary>
            private readonly Func<DbDataReader, tableType> readValue;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            internal ReturnValue<LeftArray<tableType>> returnValue;
            /// <summary>
            /// 获取数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="sql"></param>
            /// <param name="readValue"></param>
            internal SqlSelecter(Table<tableType, modelType> table, string sql, Func<DbDataReader, tableType> readValue)
            {
                this.table = table;
                this.sql = sql;
                this.readValue = readValue;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue = table.SelectQueue(ref connection, sql, readValue);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待获取数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue<LeftArray<tableType>> Wait()
            {
                wait.Set(0);
                table.AddQueue(this);
                wait.Wait();
                return returnValue;
            }
        }
        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="readValue">读取数据委托</param>
        /// <returns>数据库记录集合</returns>
        public ReturnValue<LeftArray<tableType>> SelectQueue(string sql, Func<DbDataReader, tableType> readValue)
        {
            return new SqlSelecter(this, sql, readValue).Wait();
        }
        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="readValue"></param>
        /// <returns>数据集合</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal ReturnValue<LeftArray<tableType>> SelectQueue(ref DbConnection connection, string sql, Func<DbDataReader, tableType> readValue)
        {
            return Client.Select(ref connection, sql, readValue, Log);
        }

        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        public ReturnValue<tableType> Get(long identity, MemberMap<modelType> memberMap = null)
        {
            if (IsOnlyQueue) return GetQueue(identity, memberMap);
            GetQuery<modelType> getQuery = default(GetQuery<modelType>);
            tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
            DataModel.Model<modelType>.SetIdentity(value, identity);
            Client.GetByIdentity(this, value, memberMap, ref getQuery);
            return Get(value, ref getQuery);
        }
        /// <summary>
        /// 数据库记录
        /// </summary>
        /// <param name="value"></param>
        /// <param name="getQuery"></param>
        /// <returns></returns>
        internal ReturnValue<tableType> Get(tableType value, ref GetQuery<modelType> getQuery)
        {
            DbConnection connection = null;
            try
            {
                ReturnType returnType = Client.Get(this, ref connection, value, ref getQuery);
                Client.FreeConnection(ref connection);
                switch (returnType)
                {
                    case ReturnType.Success: return value;
                    case ReturnType.NotFoundData: return (tableType)null;
                }
                return returnType;
            }
            catch (Exception error)
            {
                Client.CloseErrorConnection(ref connection);
                return error;
            }
        }
        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<tableType> Get(int identity, MemberMap<modelType> memberMap = null)
        {
            return Get((long)identity, memberMap);
        }
        /// <summary>
        /// 同步获取数据
        /// </summary>
        internal sealed class Getter : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            internal ReturnValue<tableType> ReturnValue;
            /// <summary>
            /// 单条记录查询信息
            /// </summary>
            internal GetQuery<modelType> Query;
            /// <summary>
            /// 获取数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            internal Getter(Table<tableType, modelType> table, tableType value)
            {
                this.table = table;
                ReturnValue.Value = value;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    ReturnValue.ReturnType = table.Client.Get(table, ref connection, ReturnValue.Value, ref Query);
                    switch (ReturnValue.ReturnType)
                    {
                        case ReturnType.Success: break;
                        case ReturnType.NotFoundData: ReturnValue = (tableType)null; break;
                        default: ReturnValue.Value = null; break;
                    }
                }
                catch (Exception error)
                {
                    ReturnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 添加操作队列
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue<tableType> Wait()
            {
                wait.Set(0);
                table.AddQueue(this);
                wait.Wait();
                return ReturnValue;
            }
        }
        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        public ReturnValue<tableType> GetQueue(long identity, MemberMap<modelType> memberMap = null)
        {
            tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
            DataModel.Model<modelType>.SetIdentity(value, identity);
            Getter getter = new Getter(this, value);
            Client.GetByIdentity(this, value, memberMap, ref getter.Query);
            return getter.Wait();
        }
        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<tableType> GetQueue(int identity, MemberMap<modelType> memberMap = null)
        {
            return GetQueue((long)identity, memberMap);
        }
        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        public ReturnValue<tableType> GetQueue(ref DbConnection connection, long identity, MemberMap<modelType> memberMap = null)
        {
            tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
            DataModel.Model<modelType>.SetIdentity(value, identity);
            GetQuery<modelType> query = default(GetQuery<modelType>);
            Client.GetByIdentity(this, value, memberMap, ref query);
            ReturnType returnType = Client.Get(this, ref connection, value, ref query);
            switch (returnType)
            {
                case ReturnType.Success: return value;
                case ReturnType.NotFoundData: return (tableType)null;
            }
            return returnType;
        }
        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<tableType> GetQueue(ref DbConnection connection, int identity, MemberMap<modelType> memberMap = null)
        {
            return GetQueue(ref connection, (long)identity, memberMap);
        }

        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <param name="memberMap">需要生成 SQL 语句的字段成员位图</param>
        /// <returns>添加是否成功</returns>
        public ReturnValue Insert(tableType value, bool isIgnoreTransaction = true, MemberMap<modelType> memberMap = null)
        {
            if (IsOnlyQueue) return InsertQueue(value, isIgnoreTransaction, memberMap);
            if (value != null)
            {
                InsertQuery query = new InsertQuery { NotQuery = true };
                ReturnType returnType = Client.Insert(this, value, memberMap, ref query);
                if (returnType == ReturnType.Success)
                {
                    DbConnection connection = null;
                    try
                    {
                        returnType = Client.Insert(this, ref connection, value, ref query, isIgnoreTransaction);
                        Client.FreeConnection(ref connection);
                    }
                    catch (Exception error)
                    {
                        Client.CloseErrorConnection(ref connection);
                        return error;
                    }
                }
                return returnType;
            }
            return ReturnType.ArgumentNull;
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="transaction">事务操作</param>
        /// <param name="memberMap">需要生成 SQL 语句的字段成员位图</param>
        /// <returns>添加是否成功</returns>
        public ReturnValue Insert(tableType value, Transaction transaction, MemberMap<modelType> memberMap = null)
        {
            if (transaction == null) return Insert(value, false, memberMap);
            if (!IsOnlyQueue)
            {
                if (value != null)
                {
                    InsertQuery query = new InsertQuery { NotQuery = true };
                    ReturnType returnType = Client.Insert(this, value, memberMap, ref query);
                    return returnType == ReturnType.Success ? Client.Insert(this, transaction, value, ref query) : returnType;
                }
                return ReturnType.ArgumentNull;
            }
            return ReturnType.QueueNotSupportSqlTransaction;
        }
        /// <summary>
        /// 同步添加数据
        /// </summary>
        internal sealed class Inserter : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private readonly tableType value;
            /// <summary>
            /// 添加数据查询信息
            /// </summary>
            private InsertQuery query;
            /// <summary>
            /// 添加数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 添加数据是否成功
            /// </summary>
            private ReturnValue returnValue;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            internal Inserter(Table<tableType, modelType> table, tableType value, bool isIgnoreTransaction)
            {
                this.table = table;
                this.value = value;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue.ReturnType = table.Client.Insert(table, ref connection, value, ref query, isIgnoreTransaction);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待添加数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue Wait(MemberMap<modelType> memberMap)
            {
                ReturnType returnType = table.Client.Insert(table, value, memberMap, ref query);
                if (returnType == ReturnType.Success)
                {
                    wait.Set(0);
                    table.AddQueue(this);
                    wait.Wait();
                    return returnValue;
                }
                return returnType;
            }
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <param name="memberMap">需要生成 SQL 语句的字段成员位图</param>
        /// <returns>添加是否成功</returns>
        public ReturnValue InsertQueue(tableType value, bool isIgnoreTransaction = false, MemberMap<modelType> memberMap = null)
        {
            if (value != null) return new Inserter(this, value, isIgnoreTransaction).Wait(memberMap);
            return ReturnType.ArgumentNull;
        }

        /// <summary>
        /// 异步添加数据
        /// </summary>
        internal sealed class AsynchronousInserter : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private readonly tableType value;
            /// <summary>
            /// 添加数据回调
            /// </summary>
            private readonly Action<ReturnValue<tableType>> onInserted;
            /// <summary>
            /// 添加数据查询信息
            /// </summary>
            private InsertQuery query;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="onInserted">添加数据回调</param>
            /// <param name="value"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            internal AsynchronousInserter(Table<tableType, modelType> table, tableType value, Action<ReturnValue<tableType>> onInserted, bool isIgnoreTransaction)
            {
                this.onInserted = onInserted;
                this.table = table;
                this.value = value;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                ReturnValue<tableType> returnValue = default(ReturnValue<tableType>);
                try
                {
                    returnValue.ReturnType = table.Client.Insert(table, ref connection, value, ref query, isIgnoreTransaction);
                    if (returnValue.ReturnType == ReturnType.Success) returnValue.Value = value;
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { onInserted(returnValue); }
            }
            /// <summary>
            /// 添加队列操作
            /// </summary>
            /// <param name="memberMap"></param>
            /// <returns></returns>
            internal ReturnType AddQueue(MemberMap<modelType> memberMap)
            {
                ReturnType returnType = table.Client.Insert(table, value, memberMap, ref query);
                if (returnType == ReturnType.Success)
                {
                    table.AddQueue(this);
                    return ReturnType.Success;
                }
                return returnType;
            }
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="onInserted">添加数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <param name="memberMap">需要生成 SQL 语句的字段成员位图</param>
        public void InsertQueue(tableType value, Action<ReturnValue<tableType>> onInserted, bool isIgnoreTransaction = false, MemberMap<modelType> memberMap = null)
        {
            ReturnType returnType = ReturnType.Unknown;
            try
            {
                if (value != null) returnType = new AsynchronousInserter(this, value, onInserted, isIgnoreTransaction).AddQueue(memberMap);
                else returnType = ReturnType.ArgumentNull;
            }
            finally
            {
                if (returnType != ReturnType.Success) onInserted(returnType);
            }
        }

        /// <summary>
        /// 数据集合转DataTable
        /// </summary>
        /// <param name="array">数据集合</param>
        /// <returns>数据集合</returns>
        internal DataTable GetDataTable(ref SubArray<tableType> array)
        {
            DataTable dataTable = new DataTable("["+ TableName + "]");
            foreach (KeyValue<string, Type> column in DataModel.Model<modelType>.ToArray.DataColumns) dataTable.Columns.Add(new DataColumn(column.Key, column.Value));
            foreach (tableType value in array)
            {
                object[] memberValues = new object[dataTable.Columns.Count];
                int index = 0;
                DataModel.Model<modelType>.ToArray.Write(value, memberValues, ref index, this);
                dataTable.Rows.Add(memberValues);
            }
            return dataTable;
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>添加是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<SubArray<tableType>> Insert(tableType[] array, bool isIgnoreTransaction = true)
        {
            return Insert(new SubArray<tableType>(array), isIgnoreTransaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>添加是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<SubArray<tableType>> Insert(LeftArray<tableType> array, bool isIgnoreTransaction = true)
        {
            return Insert(new SubArray<tableType>(ref array), isIgnoreTransaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>添加是否成功</returns>
        public ReturnValue<SubArray<tableType>> Insert(SubArray<tableType> array, bool isIgnoreTransaction = true)
        {
            if (IsOnlyQueue) return InsertQueue(array, isIgnoreTransaction);
            if (array.Count != 0)
            {
                ReturnType returnType = Client.Insert(this, ref array);
                if (returnType == ReturnType.Success)
                {
                    DbConnection connection = null;
                    try
                    {
                        ReturnValue<SubArray<tableType>> returnValue = Client.Insert(this, ref connection, ref array, isIgnoreTransaction);
                        Client.FreeConnection(ref connection);
                        return returnValue;
                    }
                    catch (Exception error)
                    {
                        Client.CloseErrorConnection(ref connection);
                        return error;
                    }
                }
                return returnType;
            }
            return new SubArray<tableType>();
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="transaction">事务操作</param>
        /// <returns>添加是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<SubArray<tableType>> Insert(tableType[] array, Transaction transaction)
        {
            return Insert(new SubArray<tableType>(array), transaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="transaction">事务操作</param>
        /// <returns>添加是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<SubArray<tableType>> Insert(LeftArray<tableType> array, Transaction transaction)
        {
            return Insert(new SubArray<tableType>(ref array), transaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="transaction">事务操作</param>
        /// <returns>添加是否成功</returns>
        public ReturnValue<SubArray<tableType>> Insert(SubArray<tableType> array, Transaction transaction)
        {
            if (transaction == null) return Insert(array, false);
            if (!IsOnlyQueue)
            {
                if (array.Count != 0)
                {
                    ReturnType returnType = Client.Insert(this, ref array);
                    if (returnType == ReturnType.Success)
                    {
                        returnType = Client.Insert(this, transaction, ref array);
                        if (returnType == ReturnType.Success) return array;
                    }
                    return returnType;
                }
                return new SubArray<tableType>();
            }
            return ReturnType.QueueNotSupportSqlTransaction;
        }
        /// <summary>
        /// 同步添加数据
        /// </summary>
        internal sealed class Importer : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象数组
            /// </summary>
            private ReturnValue<SubArray<tableType>> returnValue;
            /// <summary>
            /// 添加数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            internal Importer(Table<tableType, modelType> table, ref SubArray<tableType> array, bool isIgnoreTransaction)
            {
                this.table = table;
                returnValue.Value = array;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue = table.Client.Insert(table, ref connection, ref returnValue.Value, isIgnoreTransaction);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待添加数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue<SubArray<tableType>> Wait()
            {
                wait.Set(0);
                table.AddQueue(this);
                wait.Wait();
                return returnValue;
            }
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>添加是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<SubArray<tableType>> InsertQueue(tableType[] array, bool isIgnoreTransaction = false)
        {
            return InsertQueue(new SubArray<tableType>(array), isIgnoreTransaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>添加是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue<SubArray<tableType>> InsertQueue(LeftArray<tableType> array, bool isIgnoreTransaction = false)
        {
            return InsertQueue(new SubArray<tableType>(ref array), isIgnoreTransaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>添加是否成功</returns>
        public ReturnValue<SubArray<tableType>> InsertQueue(SubArray<tableType> array, bool isIgnoreTransaction = false)
        {
            if (array.Count != 0)
            {
                ReturnType returnType = Client.Insert(this, ref array);
                if (returnType == ReturnType.Success) return new Importer(this, ref array, isIgnoreTransaction).Wait();
                return returnType;
            }
            return new SubArray<tableType>();
        }

        /// <summary>
        /// 异步添加数据
        /// </summary>
        internal sealed class AsynchronousImporter : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象数组
            /// </summary>
            private SubArray<tableType> array;
            /// <summary>
            /// 添加数据回调
            /// </summary>
            private readonly Action<ReturnValue<SubArray<tableType>>> onInserted;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="onInserted">添加数据回调</param>
            /// <param name="array"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal AsynchronousImporter(Table<tableType, modelType> table, ref SubArray<tableType> array, Action<ReturnValue<SubArray<tableType>>> onInserted, bool isIgnoreTransaction)
            {
                this.onInserted = onInserted;
                this.table = table;
                this.array = array;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                ReturnValue<SubArray<tableType>> returnValue = default(ReturnValue<SubArray<tableType>>);
                try
                {
                    returnValue = table.Client.Insert(table, ref connection, ref array, isIgnoreTransaction);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { onInserted(returnValue); }
            }
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="onInserted">添加数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void InsertQueue(tableType[] array, Action<ReturnValue<SubArray<tableType>>> onInserted, bool isIgnoreTransaction = false)
        {
            InsertQueue(new SubArray<tableType>(array), onInserted, isIgnoreTransaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="onInserted">添加数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void InsertQueue(LeftArray<tableType> array, Action<ReturnValue<SubArray<tableType>>> onInserted, bool isIgnoreTransaction = false)
        {
            InsertQueue(new SubArray<tableType>(ref array), onInserted, isIgnoreTransaction);
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="array">待添加数据数组</param>
        /// <param name="onInserted">添加数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        public void InsertQueue(SubArray<tableType> array, Action<ReturnValue<SubArray<tableType>>> onInserted, bool isIgnoreTransaction = false)
        {
            ReturnType returnType = ReturnType.Unknown;
            try
            {
                if (array.Count != 0)
                {
                    returnType = Client.Insert(this, ref array);
                    if (returnType == ReturnType.Success)
                    {
                        returnType = ReturnType.Unknown;
                        AddQueue(new AsynchronousImporter(this, ref array, onInserted, isIgnoreTransaction));
                        returnType = ReturnType.Success;
                    }
                }
                else
                {
                    returnType = ReturnType.Success;
                    onInserted(new SubArray<tableType>());
                }
            }
            finally
            {
                if (returnType != ReturnType.Success) onInserted(returnType);
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">更新数据字段成员位图</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>更新是否成功</returns>
        public ReturnValue Update(tableType value, MemberMap<modelType> memberMap, bool isIgnoreTransaction = true)
        {
            if (IsOnlyQueue) return UpdateQueue(value, memberMap, isIgnoreTransaction);
            if (memberMap != null && value != null)
            {
                if (DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, this))
                {
                    UpdateQuery<modelType> updateQuery = new UpdateQuery<modelType> { NotQuery = true };
                    ReturnType returnType = Client.Update(this, value, memberMap, ref updateQuery);
                    if (returnType == ReturnType.Success)
                    {
                        DbConnection connection = null;
                        try
                        {
                            returnType = Client.Update(this, ref connection, value, memberMap, ref updateQuery, isIgnoreTransaction);
                            Client.FreeConnection(ref connection);
                        }
                        catch (Exception error)
                        {
                            Client.CloseErrorConnection(ref connection);
                            return error;
                        }
                    }
                    return returnType;
                }
                return ReturnType.VerifyError;
            }
            return ReturnType.ArgumentNull;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">更新数据字段成员位图</param>
        /// <param name="transaction">事务操作</param>
        /// <returns>更新是否成功</returns>
        public ReturnValue Update(tableType value, MemberMap<modelType> memberMap, Transaction transaction)
        {
            if (transaction == null) return Update(value, memberMap, false);
            if (!IsOnlyQueue)
            {
                if (memberMap != null && value != null)
                {
                    if (DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, this))
                    {
                        UpdateQuery<modelType> updateQuery = new UpdateQuery<modelType> { NotQuery = true };
                        ReturnType returnType = Client.Update(this, value, memberMap, ref updateQuery);
                        return returnType == ReturnType.Success ? Client.Update(this, transaction, value, memberMap, ref updateQuery) : returnType;
                    }
                    return ReturnType.VerifyError;
                }
                return ReturnType.ArgumentNull;
            }
            return ReturnType.QueueNotSupportSqlTransaction;
        }
        /// <summary>
        /// 同步更新数据
        /// </summary>
        internal sealed class Updater : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private readonly tableType value;
            /// <summary>
            /// 更新成员位图
            /// </summary>
            private readonly MemberMap<modelType> memberMap;
            /// <summary>
            /// 更新记录查询信息
            /// </summary>
            internal UpdateQuery<modelType> Query;
            /// <summary>
            /// 添加数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 更新数据是否成功
            /// </summary>
            private ReturnValue returnValue;
            /// <summary>
            /// 同步更新数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="memberMap"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            internal Updater(Table<tableType, modelType> table, tableType value, MemberMap<modelType> memberMap, bool isIgnoreTransaction)
            {
                this.table = table;
                this.value = value;
                this.memberMap = memberMap;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue.ReturnType = table.Client.Update(table, ref connection, value, memberMap, ref Query, isIgnoreTransaction);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待添加数据
            /// </summary>
            /// <returns></returns>
            internal ReturnValue Wait()
            {
                ReturnType returnType = table.Client.Update(table, value, memberMap, ref Query);
                if (returnType == ReturnType.Success)
                {
                    wait.Set(0);
                    table.AddQueue(this);
                    wait.Wait();
                    return returnValue;
                }
                return returnType;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">更新数据字段成员位图</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>更新是否成功</returns>
        public ReturnValue UpdateQueue(tableType value, MemberMap<modelType> memberMap, bool isIgnoreTransaction = false)
        {
            if (memberMap != null && value != null)
            {
                if (DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, this))
                {
                    return new Updater(this, value, memberMap, isIgnoreTransaction).Wait();
                }
                return ReturnType.VerifyError;
            }
            return ReturnType.ArgumentNull;
        }

        /// <summary>
        /// 异步更新数据
        /// </summary>
        internal sealed class AsynchronousUpdater : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private readonly tableType value;
            /// <summary>
            /// 更新数据成员位图
            /// </summary>
            private readonly MemberMap<modelType> memberMap;
            /// <summary>
            /// 更新数据回调
            /// </summary>
            private readonly Action<ReturnValue<tableType>> onUpdated;
            /// <summary>
            /// 更新记录查询信息
            /// </summary>
            internal UpdateQuery<modelType> Query;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="memberMap"></param>
            /// <param name="onUpdated"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            internal AsynchronousUpdater(Table<tableType, modelType> table, tableType value, MemberMap<modelType> memberMap, Action<ReturnValue<tableType>> onUpdated, bool isIgnoreTransaction)
            {
                this.onUpdated = onUpdated;
                this.table = table;
                this.memberMap = memberMap;
                this.value = value;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                ReturnValue<tableType> returnValue = default(ReturnValue<tableType>);
                try
                {
                    returnValue.ReturnType = table.Client.Update(table, ref connection, value, memberMap, ref Query, isIgnoreTransaction);
                    if (returnValue.ReturnType == ReturnType.Success) returnValue.Value = value;
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { onUpdated(returnValue); }
            }
            /// <summary>
            /// 添加操作队列
            /// </summary>
            /// <returns></returns>
            internal ReturnType AddQueue()
            {
                ReturnType returnType = table.Client.Update(table, value, memberMap, ref Query);
                if (returnType == ReturnType.Success)
                {
                    table.AddQueue(this);
                    return ReturnType.Success;
                }
                return returnType;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">更新数据字段成员位图</param>
        /// <param name="onUpdated">更新数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        public void UpdateQueue(tableType value, MemberMap<modelType> memberMap, Action<ReturnValue<tableType>> onUpdated, bool isIgnoreTransaction = false)
        {
            ReturnType returnType = ReturnType.Unknown;
            try
            {
                if (memberMap != null && value != null)
                {
                    if (DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, this))
                    {
                        returnType = new AsynchronousUpdater(this, value, memberMap, onUpdated, isIgnoreTransaction).AddQueue();
                    }
                    else returnType = ReturnType.VerifyError;
                }
                else returnType = ReturnType.ArgumentNull;
            }
            finally
            {
                if (returnType != ReturnType.Success) onUpdated(returnType);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>删除是否成功</returns>
        public ReturnValue Delete(tableType value, bool isIgnoreTransaction = true)
        {
            if (IsOnlyQueue) return DeleteQueue(value, isIgnoreTransaction);
            if (value != null)
            {
                InsertQuery query = new InsertQuery { NotQuery = true };
                ReturnType returnType = Client.Delete(this, value, ref query);
                if (returnType == ReturnType.Success)
                {
                    DbConnection connection = null;
                    try
                    {
                        returnType = Client.Delete(this, ref connection, value, ref query, isIgnoreTransaction);
                        Client.FreeConnection(ref connection);
                    }
                    catch (Exception error)
                    {
                        Client.CloseErrorConnection(ref connection);
                        return error;
                    }
                }
                return returnType;
            }
            return ReturnType.ArgumentNull;
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否删除成功</returns>
        public ReturnValue Delete(long identity, bool isIgnoreTransaction = true)
        {
            if (DataModel.Model<modelType>.Identity != null)
            {
                tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
                DataModel.Model<modelType>.SetIdentity(value, identity);
                return Delete(value, isIgnoreTransaction);
            }
            return ReturnType.InvalidOperation;
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否删除成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue Delete(int identity, bool isIgnoreTransaction = true)
        {
            return Delete((long)identity, isIgnoreTransaction);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="transaction">事务操作</param>
        /// <returns>删除是否成功</returns>
        public ReturnValue Delete(tableType value, Transaction transaction)
        {
            if (transaction == null) return Delete(value, false);
            if (!IsOnlyQueue)
            {
                if (value != null)
                {
                    InsertQuery query = new InsertQuery { NotQuery = true };
                    ReturnType returnType = Client.Delete(this, value, ref query);
                    return returnType == ReturnType.Success ? Client.Delete(this, transaction, value, ref query) : returnType;
                }
                return ReturnType.ArgumentNull;
            }
            return ReturnType.QueueNotSupportSqlTransaction;
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="transaction">事务操作</param>
        /// <returns>是否删除成功</returns>
        public ReturnValue Delete(long identity, Transaction transaction)
        {
            if (DataModel.Model<modelType>.Identity != null)
            {
                tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
                DataModel.Model<modelType>.SetIdentity(value, identity);
                return Delete(value, transaction);
            }
            return ReturnType.DataModelLessIdentity;
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="transaction">事务操作</param>
        /// <returns>是否删除成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue Delete(int identity, Transaction transaction)
        {
            return Delete((long)identity, transaction);
        }
        /// <summary>
        /// 同步删除数据
        /// </summary>
        internal sealed class Deletor : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private readonly tableType value;
            /// <summary>
            /// 删除数据 SQL 语句
            /// </summary>
            private InsertQuery query;
            /// <summary>
            /// 删除数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 删除数据是否成功
            /// </summary>
            private ReturnValue returnValue;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal Deletor(Table<tableType, modelType> table, tableType value, bool isIgnoreTransaction)
            {
                this.table = table;
                this.value = value;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                try
                {
                    returnValue.ReturnType = table.Client.Delete(table, ref connection, value, ref query, isIgnoreTransaction);
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { wait.Set(); }
            }
            /// <summary>
            /// 等待删除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal ReturnValue Wait()
            {
                ReturnType returnType = table.Client.Delete(table, value, ref query);
                if (returnType == ReturnType.Success)
                {
                    wait.Set(0);
                    table.AddQueue(this);
                    wait.Wait();
                    return returnValue;
                }
                return returnType;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>删除是否成功</returns>
        public ReturnValue DeleteQueue(tableType value, bool isIgnoreTransaction = false)
        {
            if (value != null) return new Deletor(this, value, isIgnoreTransaction).Wait();
            return ReturnType.ArgumentNull;
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否删除成功</returns>
        public ReturnValue DeleteQueue(long identity, bool isIgnoreTransaction = false)
        {
            if (DataModel.Model<modelType>.Identity != null)
            {
                tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
                DataModel.Model<modelType>.SetIdentity(value, identity);
                return DeleteQueue(value, isIgnoreTransaction);
            }
            return ReturnType.DataModelLessIdentity;
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否删除成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public ReturnValue DeleteQueue(int identity, bool isIgnoreTransaction = false)
        {
            return DeleteQueue((long)identity, isIgnoreTransaction);
        }

        /// <summary>
        /// 异步删除数据
        /// </summary>
        internal sealed class AsynchronousDeletor : Threading.LinkQueueTaskNode
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private readonly Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private readonly tableType value;
            /// <summary>
            /// 删除数据回调
            /// </summary>
            private readonly Action<ReturnValue<tableType>> onDeleted;
            /// <summary>
            /// 删除数据 SQL 语句
            /// </summary>
            private InsertQuery query;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private readonly bool isIgnoreTransaction;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="onDeleted">删除数据回调</param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            internal AsynchronousDeletor(Table<tableType, modelType> table, tableType value, Action<ReturnValue<tableType>> onDeleted, bool isIgnoreTransaction)
            {
                this.onDeleted = onDeleted;
                this.table = table;
                this.value = value;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override void RunLinkQueueTask(ref DbConnection connection)
            {
                ReturnValue<tableType> returnValue = default(ReturnValue<tableType>);
                try
                {
                    returnValue.ReturnType = table.Client.Delete(table, ref connection, value, ref query, isIgnoreTransaction);
                    if (returnValue.ReturnType == ReturnType.Success) returnValue.Value = value;
                }
                catch (Exception error)
                {
                    returnValue = error;
                }
                finally { onDeleted(returnValue); }
            }
            /// <summary>
            /// 添加操作队列
            /// </summary>
            /// <returns></returns>
            internal ReturnType AddQueue()
            {
                ReturnType returnType = table.Client.Delete(table, value, ref query);
                if (returnType == ReturnType.Success)
                {
                    table.AddQueue(this);
                    return ReturnType.Success;
                }
                return returnType;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="onDeleted">删除数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        public void DeleteQueue(tableType value, Action<ReturnValue<tableType>> onDeleted, bool isIgnoreTransaction = false)
        {
            ReturnType returnType = ReturnType.Unknown;
            try
            {
                if (value != null)
                {
                    returnType = new AsynchronousDeletor(this, value, onDeleted, isIgnoreTransaction).AddQueue();
                }
                else returnType = ReturnType.ArgumentNull;
            }
            finally
            {
                if (returnType != ReturnType.Success) onDeleted(returnType);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="onDeleted">删除数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        public void DeleteQueue(long identity, Action<ReturnValue<tableType>> onDeleted, bool isIgnoreTransaction = false)
        {
            ReturnType returnType = ReturnType.DataModelLessIdentity;
            try
            {
                if (DataModel.Model<modelType>.Identity != null)
                {
                    tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
                    DataModel.Model<modelType>.SetIdentity(value, identity);
                    returnType = ReturnType.Success;
                    DeleteQueue(value, onDeleted, isIgnoreTransaction);
                }
            }
            finally
            {
                if (returnType != ReturnType.Success) onDeleted(returnType);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="onDeleted">删除数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void DeleteQueue(int identity, Action<ReturnValue<tableType>> onDeleted, bool isIgnoreTransaction = false)
        {
            DeleteQueue((long)identity, onDeleted, isIgnoreTransaction);
        }
    }
    /// <summary>
    /// 数据表格
    /// </summary>
    /// <typeparam name="tableType">表格类型</typeparam>
    /// <typeparam name="modelType">模型类型</typeparam>
    /// <typeparam name="keyType">关键字类型</typeparam>
    public class Table<tableType, modelType, keyType> : Table<tableType, modelType>
        where tableType : class, modelType
        where modelType : class
        where keyType : IEquatable<keyType>
    {
        /// <summary>
        /// 获取数据委托
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key">关键字</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns></returns>
        public delegate ReturnValue<tableType> GetValue(ref DbConnection connection, keyType key, MemberMap<modelType> memberMap);
        /// <summary>
        /// 设置关键字
        /// </summary>
        public readonly Action<modelType, keyType> SetPrimaryKey;
        /// <summary>
        /// 获取关键字
        /// </summary>
        internal readonly Func<modelType, keyType> GetPrimaryKey;
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="connection">SQL 数据库连接信息</param>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(Connection connection, TableAttribute attribute, NowTime[] nowTimes, bool isCreateCacheWait)
            : base(connection, attribute, nowTimes, isCreateCacheWait)
        {
            FieldInfo[] primaryKeys = DataModel.Model<modelType>.PrimaryKeys.getArray(value => value.FieldInfo);
            GetPrimaryKey = AutoCSer.Data.Model<modelType>.GetPrimaryKeyGetter<keyType>("GetSqlPrimaryKey", primaryKeys);
            SetPrimaryKey = AutoCSer.Data.Model<modelType>.GetPrimaryKeySetter<keyType>("SetSqlPrimaryKey", primaryKeys);
        }
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="nowTimes">当前时间数组</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, NowTime[] nowTimes, bool isCreateCacheWait)
            : this(Connection.GetConnection(attribute.ConnectionType), attribute, nowTimes, isCreateCacheWait)
        {
        }
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        /// <param name="nowTimes">当前时间数组</param>
        public new static Table<tableType, modelType, keyType> Get(bool isCreateCacheWait, NowTime[] nowTimes = null)
        {
            Type type = typeof(tableType);
            TableAttribute attribute = TypeAttribute.GetAttribute<TableAttribute>(type, false);
            if (attribute != null && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1)
            {
                Table<tableType, modelType, keyType> table = new Table<tableType, modelType, keyType>(attribute, nowTimes, isCreateCacheWait);
                if (!table.IsError) return table;
            }
            return null;
        }
        /// <summary>
        /// 根据关键字获取数据对象
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        public ReturnValue<tableType> GetByPrimaryKey(keyType key, MemberMap<modelType> memberMap = null)
        {
            if (IsOnlyQueue) return GetByPrimaryKeyQueue(key, memberMap);
            GetQuery<modelType> getQuery = default(GetQuery<modelType>);
            tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
            SetPrimaryKey(value, key);
            Client.GetByPrimaryKey(this, value, memberMap, ref getQuery);
            return Get(value, ref getQuery);
        }
        /// <summary>
        /// 根据关键字获取数据对象
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        public ReturnValue<tableType> GetByPrimaryKeyQueue(keyType key, MemberMap<modelType> memberMap = null)
        {
            tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
            SetPrimaryKey(value, key);
            Getter getter = new Getter(this, value);
            Client.GetByPrimaryKey(this, value, memberMap, ref getter.Query);
            return getter.Wait();
        }
        /// <summary>
        /// 根据关键字获取数据对象
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="key">关键字</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        public ReturnValue<tableType> GetByPrimaryKeyQueue(ref DbConnection connection, keyType key, MemberMap<modelType> memberMap = null)
        {
            tableType value = AutoCSer.Metadata.DefaultConstructor<tableType>.Constructor();
            SetPrimaryKey(value, key);
            GetQuery<modelType> query = default(GetQuery<modelType>);
            Client.GetByPrimaryKey(this, value, memberMap, ref query);
            ReturnType returnType = Client.Get(this, ref connection, value, ref query);
            switch(returnType)
            {
                case ReturnType.Success: return value;
                case ReturnType.NotFoundData: return (tableType)null;
            }
            return returnType;
        }
    }
}
