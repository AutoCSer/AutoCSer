using AutoCSer.Memory;
using System;

namespace AutoCSer.Sql.Excel
{
    /// <summary>
    /// Excel连接信息
    /// </summary>
    public sealed class Connection
    {
        /// <summary>
        /// 默认表格名称
        /// </summary>
        public const string DefaultTableName = "Sheet1$";

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
            AutoCSer.Memory.Pointer buffer = UnmanagedPool.Default.GetPointer();
            try
            {
                using (CharStream connectionStream = new CharStream(ref buffer))
                {
                    connectionStream.SimpleWrite("Provider=");
                    connectionStream.Write(provider.Name);
                    connectionStream.SimpleWrite(";Data Source=");
                    connectionStream.Write(DataSource);
                    if (Password != null)
                    {
                        connectionStream.Write(";Database Password=");
                        connectionStream.SimpleWrite(Password);
                    }
                    connectionStream.Write(";Extended Properties='");
                    connectionStream.Write(provider.Excel);
                    connectionStream.Write(IsTitleColumn ? ";HDR=YES;IMEX=" : ";HDR=NO;IMEX=");
                    AutoCSer.Extensions.NumberExtension.ToString((byte)Intermixed, connectionStream);
                    connectionStream.Write('\'');
                    return new Sql.Connection { Type = ClientKind.Excel, ConnectionString = connectionStream.ToString(), IsPool = isPool };
                }
            }
            finally { UnmanagedPool.Default.Push(ref buffer); }
        }
    }
}
