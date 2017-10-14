using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 外部提交结果
    /// </summary>
    public sealed class JudgeResult
    {
        /// <summary>
        /// 提交测试ID
        /// </summary>
        public int Id;
        ///// <summary>
        ///// 最大用时毫秒数
        ///// </summary>
        //public int TimeUse;
        ///// <summary>
        ///// 最大内存使用字节数
        ///// </summary>
        //public long MemoryUse;
        /// <summary>
        /// 编译信息
        /// </summary>
        public string Message;
        /// <summary>
        /// 外部提交测试结果集合
        /// </summary>
        public JudgeItem[] Items;
        /// <summary>
        /// 判题结果
        /// </summary>
        public JudgeValue JudgeValue;
    }
}
