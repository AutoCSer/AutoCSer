using System;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 成员选择
    /// </summary>
    public abstract class MemberFilterAttribute : Attribute
    {
        /// <summary>
        /// 成员选择类型
        /// </summary>
        public abstract MemberFilters MemberFilters { get; }
        /// <summary>
        /// 成员是否匹配自定义属性类型，默认为 false 表示选择所有成员。
        /// </summary>
        public bool IsAttribute;
        /// <summary>
        /// 指定是否搜索该成员的继承链以查找这些特性，参考System.Reflection.MemberInfo.GetCustomAttributes(bool inherit)。
        /// </summary>
        public bool IsBaseTypeAttribute;
        ///// <summary>
        ///// 成员匹配自定义属性是否可继承，true 表示允许申明默认配置类型的派生类型并且选择继承深度最小的申明配置。
        ///// </summary>
        //public bool IsInheritAttribute;
        /// <summary>
        /// 默认公有动态成员
        /// </summary>
        public abstract class Instance : MemberFilterAttribute
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public MemberFilters Filter = MemberFilters.Instance;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override MemberFilters MemberFilters
            {
                get { return Filter & MemberFilters.Instance; }
            }
        }
        /// <summary>
        /// 默认非公有成员
        /// </summary>
        public abstract class NonPublic : MemberFilterAttribute
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public MemberFilters Filter = MemberFilters.NonPublic;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override MemberFilters MemberFilters
            {
                get { return Filter; }
            }
        }
        /// <summary>
        /// 默认公有动态成员
        /// </summary>
        public abstract class PublicInstance : MemberFilterAttribute
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public MemberFilters Filter = MemberFilters.PublicInstance;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override MemberFilters MemberFilters
            {
                get { return Filter & MemberFilters.Instance; }
            }
        }
        /// <summary>
        /// 默认公有动态字段成员
        /// </summary>
        public abstract class PublicInstanceField : MemberFilterAttribute
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public MemberFilters Filter = MemberFilters.PublicInstanceField;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override MemberFilters MemberFilters
            {
                get { return Filter & MemberFilters.Instance; }
            }
        }
        /// <summary>
        /// 默认动态字段成员
        /// </summary>
        public abstract class InstanceField : MemberFilterAttribute
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public MemberFilters Filter = MemberFilters.InstanceField;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override MemberFilters MemberFilters
            {
                get { return Filter & MemberFilters.Instance; }
            }
        }
        /// <summary>
        /// 默认动态属性成员
        /// </summary>
        public abstract class PublicInstanceProperty : MemberFilterAttribute
        {
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public MemberFilters Filter = MemberFilters.PublicInstanceProperty;
            /// <summary>
            /// 成员选择类型
            /// </summary>
            public override MemberFilters MemberFilters
            {
                get { return Filter & MemberFilters.Instance; }
            }
        }
    }
}
