using System;

namespace AutoCSer.Example.WebView.Template
{
    /// <summary>
    /// 客户端模板 测试
    /// </summary>
    partial class Client : AutoCSer.WebView.View<Client>
    {
        /// <summary>
        /// 服务端数据
        /// </summary>
        public int ServerData
        {
            get { return 1; }
        }
    }
}
