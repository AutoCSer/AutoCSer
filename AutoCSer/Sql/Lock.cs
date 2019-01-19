using System;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 锁类型
    /// </summary>
    [Flags]
    public enum Lock : uint
    {
        /// <summary>
        /// 不增加任何锁信息
        /// </summary>
        None = 0,
        /// <summary>
        /// NOLOCK 不添加共享锁和排它锁，允许脏读
        /// </summary>
        No = 1,
        /// <summary>
        /// READPAST 跳过已经加锁的数据行，仅应用于 READ COMMITTED 隔离性级别下事务操作中的 SELECT
        /// </summary>
        ReadPast = 2,

        /// <summary>
        /// ROWLOCK 行级锁
        /// </summary>
        Row = 0x10,
        /// <summary>
        /// PAGLOCK 页锁（否则通常可能添加表锁）
        /// </summary>
        Page = 0x20,
        /// <summary>
        /// TABLOCK 表级锁，如果同时指定了HOLDLOCK，该锁一直保持到这个事务结束
        /// </summary>
        Table = 0x40,
        /// <summary>
        /// TABLOCKX 在表上使用排它锁
        /// </summary>
        TableX = 0x80,

        /// <summary>
        /// UPDLOCK 在读表中数据时设置更新锁，允许先读取数据（不阻塞其他用户读数据），并且保证在后来更新数据时，这一段时间内这些数据没有被其他用户修改
        /// </summary>
        Update = 0x100,
        /// <summary>
        /// XLOCK 排它锁（DELELTE 锁）
        /// </summary>
        X = 0x200,
        /// <summary>
        /// HOLDLOCK 在该表上保持共享锁，直到整个事务结束
        /// </summary>
        Hold = 0x400,
    }
}
