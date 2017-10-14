using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 判题结果
    /// </summary>
    public enum JudgeValue : byte
    {
        /// <summary>
        /// 默认未处理状态
        /// </summary>
        None = 0,
        /// <summary>
        /// 处理中
        /// </summary>
        Processing = 1,
        /// <summary>
        /// AC
        /// </summary>
        Accepted = 2,
        /// <summary>
        /// 输出错误
        /// </summary>
        WrongAnswer = 3,
        /// <summary>
        /// 超内存
        /// </summary>
        MemoryLimitExceed = 4,
        /// <summary>
        /// 超时
        /// </summary>
        TimeLimitExceed = 5,
        /// <summary>
        /// 运行时异常错误
        /// </summary>
        RunTimeError = 6,
        /// <summary>
        /// 判题服务错误
        /// </summary>
        JudgeError = 7,
        /// <summary>
        /// 编译错误
        /// </summary>
        CompileError = 8,

        /// <summary>
        /// 挑战成功
        /// </summary>
        ChallangeSucceed = 20,
        /// <summary>
        /// 挑战失败
        /// </summary>
        ChallangeFail = 21,
    }
}
