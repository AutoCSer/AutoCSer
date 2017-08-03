using System;
using System.Reflection;

namespace AutoCSer.Web
{
    /// <summary>
    /// HTTP 域名注册信息
    /// </summary>
    struct HttpDomain
    {
        ///// <summary>
        ///// 当前执行程序集
        ///// </summary>
        //private static readonly string assemblyFile = Assembly.GetExecutingAssembly().Location;
        /// <summary>
        /// 域名集合
        /// </summary>
        public AutoCSer.Net.HttpRegister.Domain[] Domains;
        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServerType;
        /// <summary>
        /// 是否调用过 停止域名服务
        /// </summary>
        private bool isStop;
        /// <summary>
        /// 是否成功调用过 启动域名服务
        /// </summary>
        private bool isStart;
        /// <summary>
        /// 停止域名服务
        /// </summary>
        /// <param name="httpClient"></param>
        public void Stop(AutoCSer.Net.HttpRegister.Server.TcpInternalClient httpClient)
        {
            if (!isStop)
            {
                isStop = true;
                httpClient.stop(Domains);
            }
        }
        /// <summary>
        /// 启动域名服务
        /// </summary>
        /// <returns></returns>
        public bool Start(AutoCSer.Net.HttpRegister.Server.TcpInternalClient httpClient)
        {
            if (!isStart)
            {
                AutoCSer.Net.TcpServer.ReturnValue<AutoCSer.Net.HttpRegister.RegisterState> state = httpClient.start(ServerType.Assembly.Location, ServerType.FullName, Domains, true);
                if (state.Type == AutoCSer.Net.TcpServer.ReturnType.Success)
                {
                    if (state.Value == AutoCSer.Net.HttpRegister.RegisterState.Success) return isStart = true;
                    Console.WriteLine(ServerType.Name + " " + state.Value.ToString());
                }
                else Console.WriteLine(ServerType.Name + " " + state.Type.ToString());
                return false;
            }
            return true;
        }
    }
}
