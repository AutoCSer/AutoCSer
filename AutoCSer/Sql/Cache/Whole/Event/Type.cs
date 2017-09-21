using System;

namespace AutoCSer.Sql.Cache.Whole.Event
{
    /// <summary>
    /// 默认缓存类型
    /// </summary>
    public enum Type : byte
    {
        /// <summary>
        /// 未知/自定义缓存类型
        /// </summary>
        Unknown,
        /// <summary>
        /// 将 ID 作为数组索引的缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityArray&lt;valueType, modelType, valueType&gt;
        /// </summary>
        IdentityArray,
        /// <summary>
        /// 将 ID 作为数组索引并且支持分页查询的缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityTree&lt;valueType, modelType, valueType&gt;
        /// </summary>
        IdentityTree,
        /// <summary>
        /// K-V 缓存， AutoCSer.Sql.Cache.Whole.Event.PrimaryKey&lt;valueType, modelType, valueType, keyType&gt;[256]
        /// </summary>
        PrimaryKeyArray,
        /// <summary>
        /// K-V 缓存， AutoCSer.Sql.Cache.Whole.Event.PrimaryKey&lt;valueType, modelType, valueType, keyType&gt;
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// 将 ID 作为数组索引的缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityArray&lt;valueType, modelType, valueType&gt;
        /// </summary>
        CreateIdentityArray,
        /// <summary>
        /// 将 ID 作为数组索引的缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityArrayWhere&lt;valueType, modelType, valueType&gt;
        /// </summary>
        CreateIdentityArrayWhere,
        /// <summary>
        /// 将 ID 作为数组索引的缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityArrayWhereExpression&lt;valueType, modelType, valueType&gt;
        /// </summary>
        CreateIdentityArrayWhereExpression,
        /// <summary>
        /// 将 ID 作为数组索引并且支持分页查询的缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityTree&lt;valueType, modelType, valueType&gt;
        /// </summary>
        CreateIdentityTree,
        /// <summary>
        /// K-V 缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityTree&lt;valueType, modelType, valueType, keyType&gt;
        /// </summary>
        CreatePrimaryKeyArray,
        /// <summary>
        /// K-V 缓存， AutoCSer.Sql.Cache.Whole.Event.IdentityTree&lt;valueType, modelType, valueType, keyType&gt;
        /// </summary>
        CreatePrimaryKey,
        /// <summary>
        /// 成员绑定缓存， AutoCSer.Sql.Cache.Whole.Event.MemberKey&lt;valueType, modelType, memberCacheType, keyType, memberKeyType, targetType&gt;
        /// </summary>
        CreateMemberKey,
        /// <summary>
        /// 自定义缓存，用于生成基于缓存的远程调用链与日志流处理，需要继承自 AutoCSer.Sql.Cache.Whole.Event.Cache&lt;valueType, modelType&gt;
        /// </summary>
        Custom
    }
}
