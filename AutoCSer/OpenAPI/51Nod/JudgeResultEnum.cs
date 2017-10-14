using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 测试结果
    /// </summary>
    public enum JudgeResultEnum : byte
    {
        /// <summary>
        /// 运行中
        /// </summary>
        Running = 0,
        /// <summary>
        /// AC
        /// </summary>
        Accepted = 1,
        /// <summary>
        /// 输出错误
        /// </summary>
        WrongAnswer = 2,
        /// <summary>
        /// 超内存
        /// </summary>
        MemoryLimitExceed = 3,
        /// <summary>
        /// 超时
        /// </summary>
        TimeLimitExceed = 4,
        /// <summary>
        /// 运行时异常错误
        /// </summary>
        RunTimeError = 5,
        /// <summary>
        /// 判题服务没有找到测试数据
        /// </summary>
        DataFileLost = 6,
        /// <summary>
        /// 程序试图创建进程错误
        /// </summary>
        CreateProcess = 8,
        /// <summary>
        /// 程序阻塞超时
        /// </summary>
        Blocked = 9,

        /// <summary>
        /// 题目配置错误
        /// </summary>
        ProblemError = 21,
        /// <summary>
        /// 输出数据格式化验证错误
        /// </summary>
        FormatError = 22,
        /// <summary>
        /// 数据错误
        /// </summary>
        DataError = 23,
        /// <summary>
        /// 数据正常
        /// </summary>
        DataOk = 24,
        /// <summary>
        /// 挑战失败
        /// </summary>
        ChallangeFail = 25,
        /// <summary>
        /// 挑战成功
        /// </summary>
        Succeed = 26,

        /// <summary>
        /// 未知错误重试判题
        /// </summary>
        Retry = 100,
    }
}
