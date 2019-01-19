using System;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 事务操作
    /// </summary>
    public sealed class Transaction : IDisposable
    {
        /// <summary>
        /// SQL 操作客户端
        /// </summary>
        private readonly Client client;
        /// <summary>
        /// 数据库连接
        /// </summary>
        private DbConnection connection;
        /// <summary>
        /// 数据库连接
        /// </summary>
        internal DbConnection Connection
        {
            get
            {
                if (connection != null) return connection;
                return !isDisposed ? connection = client.GetConnection() : null;
            }
        }
        /// <summary>
        /// 数据库事务
        /// </summary>
        private DbTransaction dbTransaction;
        /// <summary>
        /// 数据库事务
        /// </summary>
        internal DbTransaction DbTransaction
        {
            get
            {
                return dbTransaction ?? (dbTransaction = Connection.BeginTransaction(isolationLevel));
            }
        }
        /// <summary>
        /// 提交事务以后的处理
        /// </summary>
        internal event Action OnCommitted;
        /// <summary>
        /// 隔离级别
        /// </summary>
        private readonly IsolationLevel isolationLevel;
        /// <summary>
        /// 是否已经提交处理
        /// </summary>
        private bool isCommit;
        /// <summary>
        /// 是否已经释放
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 事务操作
        /// </summary>
        /// <param name="client">SQL 操作客户端</param>
        /// <param name="isolationLevel">隔离级别</param>
        internal Transaction(Client client, IsolationLevel isolationLevel)
        {
            this.client = client;
            this.isolationLevel = isolationLevel;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        internal void Commit()
        {
            DbTransaction.Commit();
            isCommit = true;
            try
            {
                Dispose();
            }
            finally
            {
                if (OnCommitted != null) OnCommitted();
            }
        }
        /// <summary>
        /// 释放事务
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                try
                {
                    if (dbTransaction != null)
                    {
                        if (!isCommit) dbTransaction.Rollback();
                        dbTransaction.Dispose();
                        dbTransaction = null;
                    }
                }
                finally { client.FreeConnection(ref connection); }
            }
        }
    }
}
