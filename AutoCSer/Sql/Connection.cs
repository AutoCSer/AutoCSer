using System;
using System.Reflection;

namespace AutoCSer.Sql
{
    /// <summary>
    /// SQL 数据库连接信息
    /// </summary>
    public sealed unsafe class Connection
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString;
        /// <summary>
        /// 数据库表格所有者
        /// </summary>
        public string Owner = "dbo";
        /// <summary>
        /// SQL 客户端类型默认配置信息
        /// </summary>
        public ClientKindAttribute Attribute;
        /// <summary>
        /// SQL 客户端类型默认配置信息
        /// </summary>
        internal ClientKindAttribute ClientAttribute
        {
            get
            {
                if (Attribute == null)
                {
                    if (Type != ClientKind.ThirdParty) Attribute = EnumAttribute<ClientKind, ClientKindAttribute>.Array((byte)Type) ?? EnumAttribute<ClientKind, ClientKindAttribute>.Array((byte)ClientKind.Sql2000);
                }
                return Attribute;
            }
        }
        /// <summary>
        /// SQL 客户端
        /// </summary>
        private Client client;
        /// <summary>
        /// SQL 客户端
        /// </summary>
        public Client Client
        {
            get
            {
                if (client == null)
                {
                    client = (Client)ClientAttribute.ClientType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Connection) }, null).Invoke(new object[] { this });
                }
                return client;
            }
        }
        /// <summary>
        /// 日志处理
        /// </summary>
        public AutoCSer.Log.ILog Log;
        /// <summary>
        /// SQL 客户端类型
        /// </summary>
        public ClientKind Type;
        /// <summary>
        /// 是否启用连接池
        /// </summary>
        public bool IsPool = true;

        /// <summary>
        /// 连接集合
        /// </summary>
        private static readonly AutoCSer.Threading.LockEquatableLastDictionary<HashString, Connection> connections = new AutoCSer.Threading.LockEquatableLastDictionary<HashString, Connection>();
        /// <summary>
        /// 根据连接类型获取连接信息
        /// </summary>
        /// <param name="type">连接类型</param>
        /// <returns>连接信息</returns>
        internal static Connection GetConnection(string type)
        {
            if (type != null)
            {
                Connection value;
                HashString key = type;
                if (!connections.TryGetValue(ref key, out value)) connections.Set(ref key, value = ConfigLoader.GetUnion(typeof(Connection), type).Connection ?? new Connection());
                return value;
            }
            return null;
        }
    }
}
