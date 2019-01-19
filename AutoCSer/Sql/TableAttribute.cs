using System;
using AutoCSer.Extension;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据库表格配置
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 表格名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 获取表格名称
        /// </summary>
        /// <param name="type">表格绑定类型</param>
        /// <returns>表格名称</returns>
        internal unsafe string GetTableName(Type type)
        {
            if (Name != null) return Name;
            string name = null;
            if (ConfigLoader.Config.TableNamePrefixs.Length != 0)
            {
                name = type.fullName();
                foreach (string perfix in ConfigLoader.Config.TableNamePrefixs)
                {
                    if (name.Length > perfix.Length && name.StartsWith(perfix, StringComparison.Ordinal) && name[perfix.Length] == '.')
                    {
                        return name.Substring(perfix.Length + 1);
                    }
                }
            }
            int depth = ConfigLoader.Config.TableNameDepth;
            if (depth <= 0) return type.Name;
            if (name == null) name = type.fullName();
            fixed (char* nameFixed = name)
            {
                char* start = nameFixed, end = nameFixed + name.Length;
                do
                {
                    while (start != end && *start != '.') ++start;
                    if (start == end) return type.Name;
                    ++start;
                }
                while (--depth != 0);
                int index = (int)(start - nameFixed);
                while (start != end)
                {
                    if (*start == '.') *start = '_';
                    ++start;
                }
                return name.Substring(index);
            }
        }
        /// <summary>
        /// 数据库连接配置名称
        /// </summary>
        public string ConnectionName;
        /// <summary>
        /// 数据库连接配置名称
        /// </summary>
        public virtual string ConnectionType
        {
            get { return ConnectionName; }
        }
        /// <summary>
        /// 表格编号，主要使用枚举识别同一数据模型下的不同表格
        /// </summary>
        public int TableNumber;
        /// <summary>
        /// 自增ID起始值
        /// </summary>
        public int BaseIdentity = 1;
        /// <summary>
        /// 最大日志流客户端数量
        /// </summary>
        internal const int DefaultLogStreamCount = 16;
        /// <summary>
        /// 最大日志流客户端数量，默认为 16
        /// </summary>
        public int MaxLogStreamCount = DefaultLogStreamCount;
        /// <summary>
        /// 日志流推送休眠数量
        /// </summary>
        internal const int DefaultOnLogStreamCount = 1 << 20;
        /// <summary>
        /// 日志流推送休眠数量，默认为 1M
        /// </summary>
        public int OnLogStreamCount = DefaultOnLogStreamCount;
        /// <summary>
        /// 计数成员缓存数量
        /// </summary>
        internal const int DefaultCountMemberCacheSize = 32 << 10;
        /// <summary>
        /// 计数成员缓存数量，默认为 32K
        /// </summary>
        public int CountMemberCacheSize = DefaultCountMemberCacheSize;
        /// <summary>
        /// 默认为 true 表示添加数据时自动设置自增标识
        /// </summary>
        public bool IsSetIdentity = true;
        /// <summary>
        /// 默认为 true 表示数据库表格初始化时自动从数据库获取当前最大的自增标识，如果指定了全表缓存可以设置为 false 在缓存初始化的时候处理
        /// </summary>
        public bool IsLoadIdentity = true;
        /// <summary>
        /// 默认为 true 表示仅允许队列操作
        /// </summary>
        public bool IsOnlyQueue = true;
        /// <summary>
        /// 初始化时是否检测匹配表格信息
        /// </summary>
        public bool IsCheckTable = true;
    }
}
