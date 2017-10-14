using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 提交测试
    /// </summary>
    public sealed class Judge
    {
        /// <summary>
        /// 提交测试ID
        /// </summary>
        public int Id;
        /// <summary>
        /// 题目ID
        /// </summary>
        public int ProblemId;
        /// <summary>
        /// 程序内容
        /// </summary>
        public string ProgramContent;
        /// <summary>
        /// 程序语言
        /// </summary>
        public JudgeLanguage Language;
    }
}
