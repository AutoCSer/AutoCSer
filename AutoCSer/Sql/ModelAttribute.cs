using System;
using AutoCSer.Metadata;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 数据表格模型配置
    /// </summary>
    public class ModelAttribute : Attribute//MemberFilterAttribute.PublicInstanceField
    {
        /// <summary>
        /// 默认空属性
        /// </summary>
        internal static readonly ModelAttribute Default = new ModelAttribute();

        /// <summary>
        /// 成员选择类型
        /// </summary>
        public MemberFilters MemberFilters = MemberFilters.InstanceField;
        /// <summary>
        /// 日志队列 TCP 调用名称
        /// </summary>
        public string LogServerName;
        /// <summary>
        /// 删除列名称集合，对于组合列必须是数据库中的实际展开名称
        /// </summary>
        public string[] DeleteColumnNames;
        /// <summary>
        /// 默认缓存类型
        /// </summary>
        public Cache.Whole.Event.Type CacheType;
        /// <summary>
        /// 默认计数缓存类型
        /// </summary>
        public Cache.Counter.Type CounterCacheType;
        /// <summary>
        /// 日志流是否客户端队列模式
        /// </summary>
        public bool IsLogClientQueue;
        /// <summary>
        /// 默认为 true 表示生成加载缓存事件代码
        /// </summary>
        public bool IsLoadedCache = true;
        /// <summary>
        /// 是否绑定成员缓存类型
        /// </summary>
        public bool IsMemberCache;
        /// <summary>
        /// 默认为 true 表示日志序列化需要支持成员位图
        /// </summary>
        public bool IsLogMemberMap = true;
        /// <summary>
        /// 默认为 true 表示日志队列输出参数二进制序列化需要检测循环引用，如果可以保证参数没有循环引用而且对象无需重用则应该设置为 false 减少 CPU 开销。
        /// </summary>
        public bool IsLogSerializeReferenceMember = true;
        /// <summary>
        /// 默认为 true 表示生成日志流客户端获取缓存调用
        /// </summary>
        public bool IsLogClientGetCache = true;
        /// <summary>
        /// 默认为 true 生成默认序列化配置 [AutoCSer.Json.Serialize] + [AutoCSer.Json.Parse] + [AutoCSer.BinarySerialize.Serialize(IsReferenceMember = false)]
        /// </summary>
        public bool IsDefaultSerialize = true;
        /// <summary>
        /// 默认为 true 表示 null 字符串自动转换为 string.Empty
        /// </summary>
        public bool IsNullStringEmpty = true;
        /// <summary>
        /// 默认二进制序列化是否序列化成员位图
        /// </summary>
        public bool IsDefaultSerializeIsMemberMap;
        /// <summary>
        /// 默认为 false 表示不生成数据更新成员位图
        /// </summary>
        public bool IsUpdateMemberMap;
        /// <summary>
        /// 默认为 false 生成数据更新成员位图类型使用 struct 定义，否则采用 class
        /// </summary>
        public bool IsUpdateMemberMapClassType;
        /// <summary>
        /// 默认为 true 表示为 getSqlCache 函数生成 [AutoCSer.Net.TcpStaticServer.RemoteKey] 
        /// </summary>
        public bool IsRemoteKey = true;
    }
}
