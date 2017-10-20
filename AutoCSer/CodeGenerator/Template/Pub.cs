using System;

namespace AutoCSer.CodeGenerator.Template
{
    /// <summary>
    /// CSharp模板公用模糊类型
    /// </summary>
    internal partial class Pub
    {
        /// <summary>
        /// 类型
        /// </summary>
        public partial class Type : Pub
        {
        }
        /// <summary>
        /// 类型全名
        /// </summary>
        public partial class FullName : Pub
        {
            public int CompareTo(FullName other) { return 0; }
        }
        /// <summary>
        /// 成员类型
        /// </summary>
        public class MemberType : Pub
        {
        }
        /// <summary>
        /// 参数类型
        /// </summary>
        public partial class ParameterType : Pub { }
        ///// <summary>
        ///// 返回值类型
        ///// </summary>
        //public class ReturnType : Pub { }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public class MethodReturnType : Pub { }
        /// <summary>
        /// 带引用修饰的参数类型名称
        /// </summary>
        public class ParameterTypeRefName : Pub { }
        /// <summary>
        /// 参数类型名称
        /// </summary>
        public class ParameterTypeName : Pub { }
        /// <summary>
        /// 成员索引
        /// </summary>
        public const int MemberIndex = 0;
        /// <summary>
        /// 成员名称
        /// </summary>
        public Pub MemberName = null;
        /// <summary>
        /// 字段/属性
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>返回值</returns>
        public object PropertyName
        {
            get { return null; }
            set { }
        }
        /// <summary>
        /// 函数调用
        /// </summary>
        /// <param name="name">参数</param>
        /// <returns>返回值</returns>
        public object this[params object[] name]
        {
            get { return null; }
            set { }
        }
        /// <summary>
        /// 函数调用
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>返回值</returns>
        public object MethodName(params object[] value)
        {
            return null;
        }
        /// <summary>
        /// 字段/属性
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>返回值</returns>
        public static object StaticPropertyName
        {
            get { return null; }
            set { }
        }
        /// <summary>
        /// 函数调用
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>返回值</returns>
        public static object StaticMethodName(params object[] value)
        {
            return null;
        }
    }
}
