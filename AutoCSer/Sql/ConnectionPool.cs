using System;
using System.Data.Common;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 连接池
    /// </summary>
    internal sealed class ConnectionPool
    {
        /// <summary>
        /// 连接池标识
        /// </summary>
        private Key poolKey;
        /// <summary>
        /// 连接数组
        /// </summary>
        private DbConnection[] array;
        /// <summary>
        /// 连接数组访问锁
        /// </summary>
        private AutoCSer.Threading.SpinLock arrayLock;
        /// <summary>
        /// 当前位置
        /// </summary>
        private int index;
        /// <summary>
        /// 是否启用连接池
        /// </summary>
        internal bool IsPool;
        /// <summary>
        /// 连接池
        /// </summary>
        /// <param name="isPool">是否启用连接池</param>
        private ConnectionPool(bool isPool)
        {
            if (this.IsPool = isPool) array = new DbConnection[ConfigLoader.Config.ConnectionPoolSize];
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal DbConnection Pop()
        {
            arrayLock.EnterYield();
            if (index != 0)
            {
                DbConnection value = array[--index];
                arrayLock.Exit();
                return value;
            }
            arrayLock.Exit();
            return null;
        }
        /// <summary>
        /// 连接池处理
        /// </summary>
        /// <param name="connection"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Push(ref DbConnection connection)
        {
            if (connection != null)
            {
                if (IsPool)
                {
                    arrayLock.EnterYield();
                    if (index != array.Length)
                    {
                        array[index++] = connection;
                        arrayLock.Exit();
                    }
                    else
                    {
                        arrayLock.Exit();
                        connection.Dispose();
                    }
                }
                else connection.Dispose();
                connection = null;
            }
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private void clear(int count)
        {
            while (index > count)
            {
                DbConnection connection = Pop();
                if (connection == null) return;
                connection.Dispose();
            }
        }

        /// <summary>
        /// 连接池标识
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct Key : IEquatable<Key>
        {
            /// <summary>
            /// 连接字符串
            /// </summary>
            public string Connection;
            /// <summary>
            /// 哈希值
            /// </summary>
            public int HashCode;
            /// <summary>
            /// SQL 客户端处理类型
            /// </summary>
            public Type Type;
            /// <summary>
            /// 连接池标识
            /// </summary>
            /// <param name="type">SQL 客户端处理类型</param>
            /// <param name="connection">连接字符串</param>
            public Key(Type type, string connection)
            {
                HashCode = (Type = type).GetHashCode() ^ (Connection = connection).GetHashCode();
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(Key other)
            {
                return Type == other.Type && Connection == other.Connection;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCode;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return Equals((Key)obj);
            }
        }
        /// <summary>
        /// 获取连接池标识
        /// </summary>
        /// <param name="connectionPool"></param>
        /// <returns></returns>
        private static Key getPoolKey(ConnectionPool connectionPool)
        {
            return connectionPool.poolKey;
        }
        /// <summary>
        /// 连接池集合访问锁
        /// </summary>
        private static AutoCSer.Threading.LockLastDictionary<Key, ConnectionPool> poolLock = new AutoCSer.Threading.LockLastDictionary<Key, ConnectionPool>(getPoolKey);
        /// <summary>
        /// 获取连接池
        /// </summary>
        /// <param name="type">SQL 客户端处理类型</param>
        /// <param name="connection">连接字符串</param>
        /// <param name="isPool">是否启用连接池</param>
        /// <returns></returns>
        internal static ConnectionPool Get(Type type, string connection, bool isPool)
        {
            if (isPool)
            {
                ConnectionPool pool;
                Key key = new Key(type, connection);
                if (!poolLock.TryGetValue(key, out pool))
                {
                    try
                    {
                        pool = new ConnectionPool(true);
                        pool.poolKey = key;
                        poolLock.Set(key, pool);
                    }
                    finally { poolLock.Exit(); }
                }
                return pool;
            }
            else return new ConnectionPool(false);
        }
        /// <summary>
        /// 清除缓存数据
        /// </summary>
        /// <param name="count">保留缓存数据数量</param>
        private static void clearCache(int count)
        {
            poolLock.DictionaryLock.EnterSleepFlag();
            try
            {
                foreach (ConnectionPool pool in poolLock.Dictionary.Values) pool.clear(count);
            }
            finally { poolLock.DictionaryLock.ExitSleepFlag(); }
        }
        static ConnectionPool()
        {
            AutoCSer.Memory.Common.AddClearCache(clearCache, typeof(ConnectionPool));
        }
    }
}
