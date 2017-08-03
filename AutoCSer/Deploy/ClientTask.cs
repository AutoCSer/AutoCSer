using System;

namespace AutoCSer.Deploy
{
    /// <summary>
    /// 部署客户端任务信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ClientTask
    {
        /// <summary>
        /// 服务器端目录
        /// </summary>
        public string ServerPath;
        /// <summary>
        /// 客户端目录
        /// </summary>
        public string ClientPath;
        /// <summary>
        /// 运行文件名称
        /// </summary>
        public string RunFileName;
        /// <summary>
        /// 任务信息索引位置
        /// </summary>
        public int TaskIndex;
        /// <summary>
        /// 运行前休眠
        /// </summary>
        public int RunSleep;
        /// <summary>
        /// 文件搜索匹配集合
        /// </summary>
        public string[] FileSearchPatterns;
        /// <summary>
        /// 任务类型
        /// </summary>
        public TaskType Type;
    }
}
