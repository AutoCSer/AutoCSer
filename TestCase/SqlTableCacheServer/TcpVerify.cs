using System;

namespace AutoCSer.TestCase.SqlTableCacheServer
{
    /// <summary>
    /// TCP 调用验证
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = Config.DataReaderServer, Host = "127.0.0.1", Port = 12300, IsServer = true, VerifyString = DataReaderTcpVerify.VerifyString, IsMarkData = true, IsRememberCommand = false)]
    internal partial class DataReaderTcpVerify : AutoCSer.Net.TcpStaticServer.TimeVerify<DataReaderTcpVerify>
    {
        /// <summary>
        /// TCP 服务验证字符串
        /// </summary>
        internal const string VerifyString = "1";
    }
    /// <summary>
    /// TCP 调用验证
    /// </summary>
    [AutoCSer.Net.TcpStaticServer.Server(Name = SqlModel.Pub.LogServerName, Host = "127.0.0.1", Port = 12302, IsServer = true, VerifyString = DataReaderTcpVerify.VerifyString, IsMarkData = true, IsRememberCommand = false)]
    internal partial class DataLogTcpVerify : AutoCSer.Net.TcpStaticServer.TimeVerify<DataLogTcpVerify>
    {
    }
}
