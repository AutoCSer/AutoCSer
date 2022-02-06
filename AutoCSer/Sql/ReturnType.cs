using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 返回值类型
    /// </summary>
    public enum ReturnType : ushort
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 异常
        /// </summary>
        Exception,
        /// <summary>
        /// 连接失败
        /// </summary>
        ConnectionFailed,
        /// <summary>
        /// 事件要求取消执行
        /// </summary>
        EventCancel,
        /// <summary>
        /// 应用程序进程内事务申请失败取消执行
        /// </summary>
        TransactionCancel,
        /// <summary>
        /// 执行失败或者执行结果不符合预期值
        /// </summary>
        ExecuteFailed,
        /// <summary>
        /// 没有找到自增ID或者主键
        /// </summary>
        NotFoundPrimaryKey,
        /// <summary>
        /// 没有找到查询SQL语句
        /// </summary>
        NotFoundSql,
        /// <summary>
        /// 缺少必要参数
        /// </summary>
        ArgumentNull,
        /// <summary>
        /// 条件表达式不可解析
        /// </summary>
        UnknownWhereExpression,
        /// <summary>
        /// 没有找到执行数据
        /// </summary>
        NotFoundData,
        /// <summary>
        /// 数据模型缺少自增ID定义
        /// </summary>
        DataModelLessIdentity,
        /// <summary>
        /// 数据验证失败
        /// </summary>
        VerifyError,
        /// <summary>
        /// 队列模式不支持数据库事务
        /// </summary>
        QueueNotSupportSqlTransaction,
        /// <summary>
        /// 不支持的操作
        /// </summary>
        InvalidOperation,
    }
}
