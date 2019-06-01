using System;
using AutoCSer.Net.TcpServer;

namespace AutoCSer.Deploy.AssemblyEnvironment
{
    /// <summary>
    /// 程序集环境检测客户端
    /// </summary>
    public static class CheckClient
    {
        /// <summary>
        /// 程序集环境检测
        /// </summary>
        /// <param name="client"></param>
        /// <param name="parameters"></param>
        public static void Check(CheckServer.TcpInternalClient client, string[] parameters)
        {
            CheckResult result = new CheckResult();
            result.Tick = long.Parse(parameters[0]);
            result.TaskId = int.Parse(parameters[1]);
            try
            {
                ReturnValue<CheckTask> task = client.get(result.Tick, result.TaskId);
                if (task.Type == ReturnType.Success) task.Value.Check(result);
            }
            finally
            {
                client.setResult(result);
            }
        }
        /// <summary>
        /// 程序集环境检测
        /// </summary>
        /// <param name="parameters"></param>
        public static void Check(string[] parameters)
        {
            using (CheckServer.TcpInternalClient client = new CheckServer.TcpInternalClient()) Check(client, parameters);
        }
    }
}
