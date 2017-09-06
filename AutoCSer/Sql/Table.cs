using System;
using System.Reflection;
using AutoCSer.Extension;
using AutoCSer.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Common;
using System.Runtime.InteropServices;

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
                        Log.add(AutoCSer.Log.LogType.Debug | AutoCSer.Log.LogType.Info, "Id " + identity.toString() + " 被修改");
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
        /// 操作队列
        /// </summary>
        private readonly LinkQueueTask queue;
        /// <summary>
        /// 日志处理
        /// </summary>
        internal readonly AutoCSer.Log.ILog Log;
        /// <summary>
        /// 缓存加载完毕事件
        /// </summary>
        public event Action OnCacheLoaded;
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
        protected volatile int isLoadCache;
        /// <summary>
        /// 日志流数据是否加载完成
        /// </summary>
        protected volatile int isLoadLogStream;
        /// <summary>
        /// 成员名称是否忽略大小写
        /// </summary>
        protected readonly bool ignoreCase;
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
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, string tableName, bool isCreateCacheWait)
        {
            Connection connection = Connection.GetConnection(attribute.ConnectionType);
            this.Attribute = attribute;
            if (attribute.OnLogStreamCount <= 0) attribute.OnLogStreamCount = TableAttribute.DefaultOnLogStreamCount;
            this.Client = connection.SqlClient;
            this.Log = connection.Log ?? AutoCSer.Log.Pub.Log;
            ignoreCase = connection.ClientAttribute.IgnoreCase;
            TableName = tableName;
            queue = new LinkQueueTask(Client, AutoCSer.Threading.ThreadPool.Tiny);
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
        /// 创建索引
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="name">列名称</param>
        /// <param name="isUnique">是否唯一值</param>
        internal void CreateIndex(DbConnection connection, string name, bool isUnique)
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
                if (exception != null) Log.add(AutoCSer.Log.LogType.Error, exception, "索引 " + TableName + "." + name + " 创建失败");
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
        internal void AddQueue(LinkQueueTaskNode value)
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
        /// 字符串验证
        /// </summary>
        /// <param name="memberName">成员名称</param>
        /// <param name="value">成员值</param>
        /// <param name="length">最大长度</param>
        /// <param name="isAscii">是否ASCII</param>
        /// <param name="isNull">是否可以为null</param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
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
                        Log.add(AutoCSer.Log.LogType.Error, TableName + "." + memberName + " 超长 " + length.toString());
                        return false;
                    }
                }
                else if (value.Length > length)
                {
                    Log.add(AutoCSer.Log.LogType.Error, TableName + "." + memberName + " 超长 " + value.Length.toString() + " > " + length.toString());
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 成员值不能为null
        /// </summary>
        /// <param name="memberName">成员名称</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        [AutoCSer.IOS.Preserve(Conditional = true)]
        internal void NullVerify(string memberName)
        {
            Log.add(AutoCSer.Log.LogType.Error, TableName + "." + memberName + " 不能为null");
        }
#if !NOJIT
        /// <summary>
        /// 数据库字符串验证函数
        /// </summary>
        internal static readonly MethodInfo StringVerifyMethod = typeof(Table).GetMethod("StringVerify", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), typeof(string), typeof(int), typeof(bool), typeof(bool) }, null);
        /// <summary>
        /// 数据库字段空值验证
        /// </summary>
        internal static readonly MethodInfo NullVerifyMethod = typeof(Table).GetMethod("NullVerify", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string) }, null);
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
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, string tableName, bool isCreateCacheWait) : base(attribute, tableName, isCreateCacheWait) { }
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
        /// 数据表格
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, bool isCreateCacheWait) : base(attribute, attribute.GetTableName(typeof(tableType)), isCreateCacheWait)
        {
            DbConnection connection = Client.GetConnection();
            if (connection == null)
            {
                IsError = true;
                Log.add(AutoCSer.Log.LogType.Fatal, TableName + " 初始化失败");
                return;
            }
            using (connection)
            {
                TableColumnCollection memberTable = DataModel.Model<modelType>.GetTable(Client, TableName);
                Client.ToSqlColumn(memberTable);
                TableColumnCollection table = Client.GetTable(connection, TableName);
                if (table == null)
                {
                    Client.CreateTable(connection, memberTable);
                    table = memberTable;
                }
                else
                {
                    ModelAttribute modelAttribute = TypeAttribute.GetAttribute<ModelAttribute>(typeof(modelType), false);
                    if (modelAttribute != null && modelAttribute.DeleteColumnNames.length() != 0)
                    {
                        HashSet<string> deleteNames = modelAttribute.DeleteColumnNames.getHash(value => ignoreCase ? value.toLower() : value);
                        Column[] deleteColumns = table.Columns.Columns.getFindArray(value => deleteNames.Contains(ignoreCase ? value.SqlName.ToLower() : value.SqlName));
                        if (deleteColumns.Length != 0)
                        {
                            table.Columns.Columns = table.Columns.Columns.getFindArray(value => !deleteNames.Contains(ignoreCase ? value.SqlName.ToLower() : value.SqlName));
                            Client.DeleteFields(connection, new ColumnCollection { Name = memberTable.Columns.Name, Columns = deleteColumns });
                        }
                    }
                    using (AutoCSer.StateSearcher.AsciiSearcher<Column> sqlColumnNames = new AutoCSer.StateSearcher.AsciiSearcher<Column>(ignoreCase ? table.Columns.Columns.getArray(value => value.SqlName.ToLower()) : table.Columns.Columns.getArray(value => value.SqlName), table.Columns.Columns, false))
                    {
                        LeftArray<Column> newColumns;
                        if (ignoreCase) newColumns = memberTable.Columns.Columns.getFind(value => sqlColumnNames.Searcher.SearchLower(value.SqlName) < 0);
                        else newColumns = memberTable.Columns.Columns.getFind(value => sqlColumnNames.Searcher.Search(value.SqlName) < 0);
                        if (newColumns.Length != 0 && Client.IsAddField)
                        {
                            Client.AddFields(connection, new ColumnCollection { Name = memberTable.Columns.Name, Columns = newColumns.ToArray() });
                            newColumns.Add(table.Columns.Columns);
                            table.Columns.Columns = newColumns.ToArray();
                        }
                        if (ignoreCase) newColumns = memberTable.Columns.Columns.getFind(value => !value.IsMatch(sqlColumnNames.Get(value.SqlName.ToLower()), ignoreCase));
                        else newColumns = memberTable.Columns.Columns.getFind(value => !value.IsMatch(sqlColumnNames.Get(value.SqlName), ignoreCase));
                        if (newColumns.count() != 0)
                        {
                            Log.add(AutoCSer.Log.LogType.Error, "表格 " + memberTable.Columns.Name + " 字段类型不匹配 : " + newColumns.JoinString(",", value => value.SqlName));
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
                    if (Client.IsIndex) CreateIndex(connection, identity.FieldInfo.Name, true);
                    SelectMemberMap.SetMember(identity.MemberMapIndex);
                    if (attribute.IsLoadIdentity)
                    {
                        using (MemberMap<modelType> memberMap = new MemberMap<modelType>())
                        {
                            memberMap.SetMember(identity.MemberMapIndex);
                            CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(1, new KeyValue<Field, bool>(identity, true));
                            SelectQuery<modelType> query = new SelectQuery<modelType> { MemberMap = memberMap };
                            Client.GetSelectQuery(this, ref createQuery, ref query);
                            IConvertible identityConvertible = Client.GetValue(connection, query.Sql) as IConvertible;
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
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        public static Table<tableType, modelType> Get(bool isCreateCacheWait)
        {
            Type type = typeof(tableType);
            TableAttribute attribute = TypeAttribute.GetAttribute<TableAttribute>(type, false);
            if (attribute != null && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1)
            {
                Table<tableType, modelType> table = new Table<tableType, modelType>(attribute, isCreateCacheWait);
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
        public event Action<tableType, tableType, MemberMap<modelType>> OnUpdated;
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
        /// 缓存数据加载完成
        /// </summary>
        /// <param name="onInserted">添加记录事件</param>
        /// <param name="onUpdated">更新记录事件</param>
        /// <param name="onDeleted">删除记录事件</param>
        /// <param name="isSqlStreamTypeCount">是否日志流计数完成类型注册</param>
        public void CacheLoaded(Action<tableType> onInserted = null, Action<tableType, tableType, MemberMap<modelType>> onUpdated = null, Action<tableType> onDeleted = null, bool isSqlStreamTypeCount = true)
        {
            if (Interlocked.CompareExchange(ref isLoadCache, 1, 0) == 0)
            {
                if (onInserted != null) OnInserted += onInserted;
                if (onUpdated != null) OnUpdated += onUpdated;
                if (onDeleted != null) OnDeleted += onDeleted;
                CacheLoadWait.Set();
                callOnCacheLoaded();
                if (isSqlStreamTypeCount) LogStream.LoadedType.Add(typeof(modelType), Attribute.TableNumber);
            }
        }
        /// <summary>
        /// 获取查询信息
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="createQuery"></param>
        /// <param name="query"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void GetSelectQuery(MemberMap<modelType> memberMap, ref CreateSelectQuery<modelType> createQuery, ref SelectQuery<modelType> query)
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
        internal LeftArray<tableType> Select(ref DbConnection connection, ref SelectQuery<modelType> query)
        {
            return Client.Select(this, ref connection, ref query);
        }
        /// <summary>
        /// 同步获取数据
        /// </summary>
        internal sealed class Selecter : LinkQueueTaskNode<Selecter>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            internal Table<tableType, modelType> Table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            internal LeftArray<tableType> Value;
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
            internal Selecter()
            {
                wait.Set(0);
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    Value = Table.Select(ref connection, ref Query);
                }
                finally { wait.Set(); }
                LinkNext = null;
                return next;
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                Table = null;
                Value.Null();
                Query.Free();
                YieldPool.Default.PushNotNull(this);
            }
            /// <summary>
            /// 等待获取数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal LeftArray<tableType> Wait()
            {
                wait.Wait();
                return Value;
            }
        }
        /// <summary>
        /// 获取数据库记录集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据库记录集合</returns>
        public LeftArray<tableType> Select(Expression<Func<modelType, bool>> where = null, MemberMap<modelType> memberMap = null)
        {
            Selecter selecter = (Selecter.YieldPool.Default.Pop() as Selecter) ?? new Selecter();
            try
            {
                CreateSelectQuery<modelType> createQuery = new CreateSelectQuery<modelType>(where);
                Client.GetSelectQuery(selecter.Table = this, memberMap ?? SelectMemberMap, ref createQuery, ref selecter.Query);
                AddQueue(selecter);
                return selecter.Wait();
            }
            finally { selecter.Push(); }
        }
        /// <summary>
        /// 同步获取数据
        /// </summary>
        internal sealed class Getter : LinkQueueTaskNode<Getter>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            internal tableType Value;
            /// <summary>
            /// 单条记录查询信息
            /// </summary>
            internal GetQuery<modelType> Query;
            /// <summary>
            /// 获取数据等待锁
            /// </summary>
            private AutoCSer.Threading.AutoWaitHandle wait;
            /// <summary>
            /// 是否获取到数据
            /// </summary>
            private bool isValue;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            internal Getter()
            {
                wait.Set(0);
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    isValue = table.Client.Get(table, ref connection, Value, ref Query);
                }
                finally { wait.Set(); }
                LinkNext = null;
                return next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(Table<tableType, modelType> table, tableType value)
            {
                this.table = table;
                Value = value;
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                table = null;
                Value = null;
                isValue = false;
                Query.Free();
                YieldPool.Default.PushNotNull(this);
            }
            /// <summary>
            /// 等待获取数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool Wait()
            {
                wait.Wait();
                return isValue;
            }
        }
        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        public tableType Get(long identity, MemberMap<modelType> memberMap = null)
        {
            tableType value = AutoCSer.Emit.Constructor<tableType>.New();
            DataModel.Model<modelType>.SetIdentity(value, identity);
            Getter getter = (Getter.YieldPool.Default.Pop() as Getter) ?? new Getter();
            try
            {
                Client.GetByIdentity(this, value, memberMap, ref getter.Query);
                getter.Set(this, value);
                AddQueue(getter);
                if (!getter.Wait()) return null;
            }
            finally { getter.Push(); }
            return value;
        }
        /// <summary>
        /// 根据自增 Id 获取数据库记录
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="memberMap">成员位图</param>
        /// <returns>数据对象</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public tableType Get(int identity, MemberMap<modelType> memberMap = null)
        {
            return Get((long)identity, memberMap);
        }
        /// <summary>
        /// 同步添加数据
        /// </summary>
        internal sealed class Inserter : LinkQueueTaskNode<Inserter>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private tableType value;
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
            private bool isIgnoreTransaction;
            /// <summary>
            /// 添加数据是否成功
            /// </summary>
            private bool isValue;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            internal Inserter()
            {
                wait.Set(0);
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    isValue = table.Client.Insert(table, ref connection, value, ref query, isIgnoreTransaction);
                }
                finally { wait.Set(); }
                LinkNext = null;
                return next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="query"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(Table<tableType, modelType> table, tableType value, ref InsertQuery query, bool isIgnoreTransaction)
            {
                this.table = table;
                this.value = value;
                this.query = query;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                table = null;
                value = null;
                query.Clear();
                isValue = false;
                YieldPool.Default.PushNotNull(this);
            }
            /// <summary>
            /// 等待添加数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool Wait()
            {
                wait.Wait();
                return isValue;
            }
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <param name="memberMap">需要生成 SQL 语句的字段成员位图</param>
        /// <returns>添加是否成功</returns>
        public bool Insert(tableType value, bool isIgnoreTransaction = false, MemberMap<modelType> memberMap = null)
        {
            if (value != null)
            {
                InsertQuery query = new InsertQuery();
                if (Client.Insert(this, value, memberMap, ref query))
                {
                    Inserter inserter = (Inserter.YieldPool.Default.Pop() as Inserter) ?? new Inserter();
                    try
                    {
                        inserter.Set(this, value, ref query, isIgnoreTransaction);
                        AddQueue(inserter);
                        return inserter.Wait();
                    }
                    finally { inserter.Push(); }
                }
            }
            return false;
        }
        /// <summary>
        /// 异步添加数据
        /// </summary>
        internal sealed class AsynchronousInserter : LinkQueueTaskNode<AsynchronousInserter>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private tableType value;
            /// <summary>
            /// 添加数据回调
            /// </summary>
            private Action<tableType> onInserted;
            /// <summary>
            /// 添加数据查询信息
            /// </summary>
            private InsertQuery query;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private bool isIgnoreTransaction;
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    if (table.Client.Insert(table, ref connection, value, ref query, isIgnoreTransaction))
                    {
                        Action<tableType> onInserted = this.onInserted;
                        if (onInserted != null)
                        {
                            this.onInserted = null;
                            onInserted(value);
                        }
                    }
                }
                finally
                {
                    if (onInserted != null) onInserted(null);
                }
                LinkNext = null;
                table = null;
                value = null;
                onInserted = null;
                query.Clear();
                YieldPool.Default.PushNotNull(this);
                return next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="onInserted">添加数据回调</param>
            /// <param name="value"></param>
            /// <param name="query"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(Table<tableType, modelType> table, tableType value, ref Action<tableType> onInserted, ref InsertQuery query, bool isIgnoreTransaction)
            {
                this.onInserted = onInserted;
                this.table = table;
                this.value = value;
                this.query = query;
                this.isIgnoreTransaction = isIgnoreTransaction;
                onInserted = null;
            }
        }
        /// <summary>
        /// 将数据添加到数据库
        /// </summary>
        /// <param name="value">待添加数据</param>
        /// <param name="onInserted">添加数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <param name="memberMap">需要生成 SQL 语句的字段成员位图</param>
        public void Insert(tableType value, Action<tableType> onInserted, bool isIgnoreTransaction = false, MemberMap<modelType> memberMap = null)
        {
            try
            {
                if (value != null)
                {
                    InsertQuery query = new InsertQuery();
                    if (Client.Insert(this, value, memberMap, ref query))
                    {
                        AsynchronousInserter inserter = (AsynchronousInserter.YieldPool.Default.Pop() as AsynchronousInserter) ?? new AsynchronousInserter();
                        inserter.Set(this, value, ref onInserted, ref query, isIgnoreTransaction);
                        AddQueue(inserter);
                    }
                }
            }
            finally
            {
                if (onInserted != null) onInserted(null);
            }
        }
        /// <summary>
        /// 同步更新数据
        /// </summary>
        internal sealed class Updater : LinkQueueTaskNode<Updater>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private tableType value;
            /// <summary>
            /// 更新成员位图
            /// </summary>
            private MemberMap<modelType> memberMap;
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
            private bool isIgnoreTransaction;
            /// <summary>
            /// 更新数据是否成功
            /// </summary>
            private bool isValue;
            /// <summary>
            /// 同步更新数据
            /// </summary>
            internal Updater()
            {
                wait.Set(0);
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    isValue = table.Client.Update(table, ref connection, value, memberMap, ref Query, isIgnoreTransaction);
                }
                finally { wait.Set(); }
                LinkNext = null;
                return next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="memberMap"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(Table<tableType, modelType> table, tableType value, MemberMap<modelType> memberMap, bool isIgnoreTransaction)
            {
                this.table = table;
                this.value = value;
                this.memberMap = memberMap;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                table = null;
                value = null;
                memberMap = null;
                Query.Free();
                isValue = false;
                YieldPool.Default.PushNotNull(this);
            }
            /// <summary>
            /// 等待添加数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool Wait()
            {
                wait.Wait();
                return isValue;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">更新数据字段成员位图</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>更新是否成功</returns>
        public bool Update(tableType value, MemberMap<modelType> memberMap, bool isIgnoreTransaction = false)
        {
            if (memberMap != null && value != null && DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, this))
            {
                Updater updater = (Updater.YieldPool.Default.Pop() as Updater) ?? new Updater();
                try
                {
                    if (Client.Update(this, value, memberMap, ref updater.Query))
                    {
                        updater.Set(this, value, memberMap, isIgnoreTransaction);
                        AddQueue(updater);
                        return updater.Wait();
                    }
                }
                finally { updater.Push(); }
            }
            return false;
        }
        /// <summary>
        /// 异步更新数据
        /// </summary>
        internal sealed class AsynchronousUpdater : LinkQueueTaskNode<AsynchronousUpdater>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private tableType value;
            /// <summary>
            /// 更新数据回调
            /// </summary>
            private Action<tableType> onUpdated;
            /// <summary>
            /// 更新成员位图
            /// </summary>
            private MemberMap<modelType> memberMap;
            /// <summary>
            /// 更新记录查询信息
            /// </summary>
            internal UpdateQuery<modelType> Query;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private bool isIgnoreTransaction;
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    if (table.Client.Update(table, ref connection, value, memberMap, ref Query, isIgnoreTransaction))
                    {
                        Action<tableType> onUpdated = this.onUpdated;
                        if (onUpdated != null)
                        {
                            this.onUpdated = null;
                            onUpdated(value);
                        }
                    }
                }
                finally
                {
                    if (onUpdated != null) onUpdated(null);
                }
                LinkNext = null;
                table = null;
                value = null;
                onUpdated = null;
                memberMap = null;
                Query.Free();
                YieldPool.Default.PushNotNull(this);
                return next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="memberMap"></param>
            /// <param name="onUpdated"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(Table<tableType, modelType> table, tableType value, MemberMap<modelType> memberMap, ref Action<tableType> onUpdated, bool isIgnoreTransaction)
            {
                this.onUpdated = onUpdated;
                this.table = table;
                this.value = value;
                this.memberMap = memberMap;
                this.isIgnoreTransaction = isIgnoreTransaction;
                onUpdated = null;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="value">更新数据</param>
        /// <param name="memberMap">更新数据字段成员位图</param>
        /// <param name="onUpdated">更新数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        public void Update(tableType value, MemberMap<modelType> memberMap, Action<tableType> onUpdated, bool isIgnoreTransaction = false)
        {
            try
            {
                if (memberMap != null && value != null && DataModel.Model<modelType>.Verifyer.Verify(value, memberMap, this))
                {
                    AsynchronousUpdater updater = (AsynchronousUpdater.YieldPool.Default.Pop() as AsynchronousUpdater) ?? new AsynchronousUpdater();
                    if (Client.Update(this, value, memberMap, ref updater.Query))
                    {
                        updater.Set(this, value, memberMap, ref onUpdated, isIgnoreTransaction);
                        AddQueue(updater);
                    }
                }
            }
            finally
            {
                if (onUpdated != null) onUpdated(null);
            }
        }
        /// <summary>
        /// 同步删除数据
        /// </summary>
        internal sealed class Deletor : LinkQueueTaskNode<Deletor>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private tableType value;
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
            private bool isIgnoreTransaction;
            /// <summary>
            /// 删除数据是否成功
            /// </summary>
            private bool isValue;
            /// <summary>
            /// 同步获取数据
            /// </summary>
            internal Deletor()
            {
                wait.Set(0);
            }
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    isValue = table.Client.Delete(table, ref connection, value, ref query, isIgnoreTransaction);
                }
                finally { wait.Set(); }
                LinkNext = null;
                return next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="query"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(Table<tableType, modelType> table, tableType value, ref InsertQuery query, bool isIgnoreTransaction)
            {
                this.table = table;
                this.value = value;
                this.query = query;
                this.isIgnoreTransaction = isIgnoreTransaction;
            }
            /// <summary>
            /// 释放对象
            /// </summary>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Push()
            {
                table = null;
                value = null;
                query.Clear();
                isValue = false;
                YieldPool.Default.PushNotNull(this);
            }
            /// <summary>
            /// 等待删除数据
            /// </summary>
            /// <returns></returns>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal bool Wait()
            {
                wait.Wait();
                return isValue;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>删除是否成功</returns>
        public bool Delete(tableType value, bool isIgnoreTransaction = false)
        {
            if (value != null)
            {
                InsertQuery query = new InsertQuery();
                if (Client.Delete(this, value, ref query))
                {
                    Deletor deletor = (Deletor.YieldPool.Default.Pop() as Deletor) ?? new Deletor();
                    try
                    {
                        deletor.Set(this, value, ref query, isIgnoreTransaction);
                        AddQueue(deletor);
                        return deletor.Wait();
                    }
                    finally { deletor.Push(); }
                }
            }
            return false;
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否删除成功</returns>
        public bool Delete(long identity, bool isIgnoreTransaction = false)
        {
            if (DataModel.Model<modelType>.Identity == null) throw new InvalidOperationException();
            tableType value = AutoCSer.Emit.Constructor<tableType>.New();
            DataModel.Model<modelType>.SetIdentity(value, identity);
            return Delete(value, isIgnoreTransaction);
        }
        /// <summary>
        /// 根据自增 Id 删除数据库数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        /// <returns>是否删除成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Delete(int identity, bool isIgnoreTransaction = false)
        {
            return Delete((long)identity, isIgnoreTransaction);
        }
        /// <summary>
        /// 异步删除数据
        /// </summary>
        internal sealed class AsynchronousDeletor : LinkQueueTaskNode<AsynchronousDeletor>
        {
            /// <summary>
            /// 数据表格
            /// </summary>
            private Table<tableType, modelType> table;
            /// <summary>
            /// 目标数据对象
            /// </summary>
            private tableType value;
            /// <summary>
            /// 删除数据回调
            /// </summary>
            private Action<tableType> onDeleted;
            /// <summary>
            /// 删除数据 SQL 语句
            /// </summary>
            private InsertQuery query;
            /// <summary>
            /// 是否忽略应用程序事务
            /// </summary>
            private bool isIgnoreTransaction;
            /// <summary>
            /// 执行任务
            /// </summary>
            /// <param name="connection"></param>
            internal override LinkQueueTaskNode RunLinkQueueTask(ref DbConnection connection)
            {
                LinkQueueTaskNode next = LinkNext;
                try
                {
                    if (table.Client.Delete(table, ref connection, value, ref query, isIgnoreTransaction))
                    {
                        Action<tableType> onDeleted = this.onDeleted;
                        if (onDeleted != null)
                        {
                            this.onDeleted = null;
                            onDeleted(value);
                        }
                    }
                }
                finally
                {
                    if (onDeleted != null) onDeleted(null);
                }
                LinkNext = null;
                table = null;
                value = null;
                onDeleted = null;
                query.Clear();
                YieldPool.Default.PushNotNull(this);
                return next;
            }
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="table"></param>
            /// <param name="value"></param>
            /// <param name="onDeleted">删除数据回调</param>
            /// <param name="query"></param>
            /// <param name="isIgnoreTransaction">是否忽略应用程序事务</param>
            [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
            internal void Set(Table<tableType, modelType> table, tableType value, ref Action<tableType> onDeleted, ref InsertQuery query, bool isIgnoreTransaction)
            {
                this.onDeleted = onDeleted;
                this.table = table;
                this.value = value;
                this.query = query;
                this.isIgnoreTransaction = isIgnoreTransaction;
                onDeleted = null;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value">待删除数据</param>
        /// <param name="onDeleted">删除数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        public void Delete(tableType value, Action<tableType> onDeleted, bool isIgnoreTransaction = false)
        {
            try
            {
                if (value != null)
                {
                    InsertQuery query = new InsertQuery();
                    if (Client.Delete(this, value, ref query))
                    {
                        AsynchronousDeletor deletor = (AsynchronousDeletor.YieldPool.Default.Pop() as AsynchronousDeletor) ?? new AsynchronousDeletor();
                        deletor.Set(this, value, ref onDeleted, ref query, isIgnoreTransaction);
                        AddQueue(deletor);
                    }
                }
            }
            finally
            {
                if (onDeleted != null) onDeleted(null);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="onDeleted">删除数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        public void Delete(long identity, Action<tableType> onDeleted, bool isIgnoreTransaction = false)
        {
            if (DataModel.Model<modelType>.Identity == null) throw new InvalidOperationException();
            tableType value = AutoCSer.Emit.Constructor<tableType>.New();
            DataModel.Model<modelType>.SetIdentity(value, identity);
            Delete(value, onDeleted, isIgnoreTransaction);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="identity">自增 Id</param>
        /// <param name="onDeleted">删除数据回调</param>
        /// <param name="isIgnoreTransaction">是否忽略应用程序事务（不是数据库事务）</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Delete(int identity, Action<tableType> onDeleted, bool isIgnoreTransaction = false)
        {
            Delete((long)identity, onDeleted, isIgnoreTransaction);
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
        /// 设置关键字
        /// </summary>
        private Action<modelType, keyType> setPrimaryKey;
        /// <summary>
        /// 获取关键字
        /// </summary>
        internal Func<modelType, keyType> GetPrimaryKey;
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="attribute">数据库表格配置</param>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        protected Table(TableAttribute attribute, bool isCreateCacheWait) : base(attribute, isCreateCacheWait)
        {
            FieldInfo[] primaryKeys = DataModel.Model<modelType>.PrimaryKeys.getArray(value => value.FieldInfo);
            GetPrimaryKey = AutoCSer.Data.Model<modelType>.GetPrimaryKeyGetter<keyType>("GetSqlPrimaryKey", primaryKeys);
            setPrimaryKey = AutoCSer.Data.Model<modelType>.GetPrimaryKeySetter<keyType>("SetSqlPrimaryKey", primaryKeys);
        }
        /// <summary>
        /// 获取数据库表格操作工具
        /// </summary>
        /// <returns>数据库表格操作工具</returns>
        /// <param name="isCreateCacheWait">是否等待创建缓存</param>
        public new static Table<tableType, modelType, keyType> Get(bool isCreateCacheWait)
        {
            Type type = typeof(tableType);
            TableAttribute attribute = TypeAttribute.GetAttribute<TableAttribute>(type, false);
            if (attribute != null && Array.IndexOf(ConfigLoader.Config.CheckConnectionNames, attribute.ConnectionType) != -1)
            {
                Table<tableType, modelType, keyType> table = new Table<tableType, modelType, keyType>(attribute, isCreateCacheWait);
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
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public tableType GetByPrimaryKey(keyType key, MemberMap<modelType> memberMap = null)
        {
            tableType value = AutoCSer.Emit.Constructor<tableType>.New();
            setPrimaryKey(value, key);
            Getter getter = (Getter.YieldPool.Default.Pop() as Getter) ?? new Getter();
            try
            {
                Client.GetByPrimaryKey(this, value, memberMap, ref getter.Query);
                getter.Set(this, value);
                AddQueue(getter);
                if (!getter.Wait()) return null;
            }
            finally { getter.Push(); }
            return value;
        }
    }
}
