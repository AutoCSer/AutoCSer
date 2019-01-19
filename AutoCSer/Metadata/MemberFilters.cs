using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员选择类型
    /// </summary>
    [Flags]
    public enum MemberFilters
    {
        /// <summary>
        /// 未知成员
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 公共动态字段
        /// </summary>
        PublicInstanceField = 1,
        /// <summary>
        /// 非公共动态字段
        /// </summary>
        NonPublicInstanceField = 2,
        /// <summary>
        /// 公共动态属性
        /// </summary>
        PublicInstanceProperty = 4,
        /// <summary>
        /// 非公共动态属性
        /// </summary>
        NonPublicInstanceProperty = 8,
        /// <summary>
        /// 公共静态字段
        /// </summary>
        PublicStaticField = 0x10,
        /// <summary>
        /// 非公共静态字段
        /// </summary>
        NonPublicStaticField = 0x20,
        /// <summary>
        /// 公共静态属性
        /// </summary>
        PublicStaticProperty = 0x40,
        /// <summary>
        /// 非公共静态属性
        /// </summary>
        NonPublicStaticProperty = 0x80,
        /// <summary>
        /// 公共动态成员
        /// </summary>
        PublicInstance = PublicInstanceField + PublicInstanceProperty,
        /// <summary>
        /// 非公共动态成员
        /// </summary>
        NonPublicInstance = NonPublicInstanceField + NonPublicInstanceProperty,
        /// <summary>
        /// 公共静态成员
        /// </summary>
        PublicStatic = PublicStaticField + PublicStaticProperty,
        /// <summary>
        /// 非公共静态成员
        /// </summary>
        NonPublicStatic = NonPublicStaticField + NonPublicStaticProperty,
        /// <summary>
        /// 动态字段成员
        /// </summary>
        InstanceField = PublicInstanceField + NonPublicInstanceField,
        /// <summary>
        /// 动态属性成员
        /// </summary>
        InstanceProperty = PublicInstanceProperty + NonPublicInstanceProperty,
        /// <summary>
        /// 静态字段成员
        /// </summary>
        StaticField = PublicStaticField + NonPublicStaticField,
        /// <summary>
        /// 静态属性成员
        /// </summary>
        StaticProperty = PublicStaticProperty + NonPublicStaticProperty,
        /// <summary>
        /// 公共成员
        /// </summary>
        Public = PublicInstance + PublicStatic,
        /// <summary>
        /// 非公共成员
        /// </summary>
        NonPublic = NonPublicInstance + NonPublicStatic,
        /// <summary>
        /// 动态成员
        /// </summary>
        Instance = PublicInstance + NonPublicInstance,
        /// <summary>
        /// 静态成员
        /// </summary>
        Static = PublicStatic + NonPublicStatic,
        /// <summary>
        /// 字段
        /// </summary>
        Field = InstanceField + StaticField,
        /// <summary>
        /// 属性
        /// </summary>
        Property = InstanceProperty + StaticProperty,
    }
}
