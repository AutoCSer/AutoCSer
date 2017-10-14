using System;

namespace AutoCSer.OpenAPI._51Nod
{
    /// <summary>
    /// 题目
    /// </summary>
    public sealed class Problem
    {
        /// <summary>
        /// 题目ID
        /// </summary>
        public int Id;
        /// <summary>
        /// 超时毫秒限制基数
        /// </summary>
        public int TimeLimit;
        /// <summary>
        /// 内存限制字节数基数
        /// </summary>
        public long MemoryLimit;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 题目描述HTML
        /// </summary>
        public string Description = string.Empty;
        /// <summary>
        /// 输入数据描述
        /// </summary>
        public string InputDescription = string.Empty;
        /// <summary>
        /// 输入数据样例
        /// </summary>
        public string Input = string.Empty;
        /// <summary>
        /// 输出数据描述
        /// </summary>
        public string OutputDescription = string.Empty;
        /// <summary>
        /// 输出数据样例
        /// </summary>
        public string Output = string.Empty;
        /// <summary>
        /// 输入数据格式化定义
        /// </summary>
        public string FormatInput = string.Empty;
        /// <summary>
        /// 题目来源
        /// </summary>
        public string Source = string.Empty;
        /// <summary>
        /// 题目来源连接
        /// </summary>
        public string SourceLink = string.Empty;
        /// <summary>
        /// 基础分数
        /// </summary>
        public int BasePoint;
    }
}
