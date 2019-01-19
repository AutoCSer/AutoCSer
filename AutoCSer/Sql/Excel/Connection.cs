using System;

namespace AutoCSer.Sql.Excel
{
    /// <summary>
    /// Excel连接信息
    /// </summary>
    public sealed class Connection
    {
        /// <summary>
        /// 数据接口属性
        /// </summary>
        public Provider Provider = Provider.Ace12;
        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password;
        /// <summary>
        /// 混合数据处理方式
        /// </summary>
        public Intermixed Intermixed = Intermixed.WriteAndRead;
        /// <summary>
        /// 第一行是否列名
        /// </summary>
        public bool IsTitleColumn = true;

        /// <summary>
        /// 获取数据库连接信息
        /// </summary>
        /// <param name="isPool">是否启用数据库连接池</param>
        /// <returns>数据库连接信息</returns>
        public unsafe Sql.Connection Get(bool isPool = false)
        {
            ProviderAttribute provider = EnumAttribute<Provider, ProviderAttribute>.Array((byte)Provider);
            byte* buffer = AutoCSer.UnmanagedPool.Default.Get();
            try
            {
                using (CharStream connectionStream = new CharStream((char*)buffer, AutoCSer.UnmanagedPool.DefaultSize >> 1))
                {
                    connectionStream.SimpleWriteNotNull("Provider=");
                    connectionStream.Write(provider.Name);
                    connectionStream.SimpleWriteNotNull(";Data Source=");
                    connectionStream.Write(DataSource);
                    if (Password != null)
                    {
                        connectionStream.WriteNotNull(";Database Password=");
                        connectionStream.SimpleWriteNotNull(Password);
                    }
                    connectionStream.WriteNotNull(";Extended Properties='");
                    connectionStream.Write(provider.Excel);
                    connectionStream.WriteNotNull(IsTitleColumn ? ";HDR=YES;IMEX=" : ";HDR=NO;IMEX=");
                    AutoCSer.Extension.Number.ToString((byte)Intermixed, connectionStream);
                    connectionStream.Write('\'');
                    return new Sql.Connection { Type = ClientKind.Excel, ConnectionString = connectionStream.ToString(), IsPool = isPool };
                }
            }
            finally { AutoCSer.UnmanagedPool.Default.Push(buffer); }
        }
    }
}
