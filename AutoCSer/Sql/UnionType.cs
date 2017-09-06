using System;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using System.Data.SqlClient;
#if !NETSTANDARD2_0
using System.Data.OleDb;
#endif

namespace AutoCSer.Sql
{
    /// <summary>
    /// 类型转换
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionType
    {
        /// <summary>
        /// 回调对象
        /// </summary>
        [FieldOffset(0)]
        public object Value;
        /// <summary>
        /// SQL 配置
        /// </summary>
        [FieldOffset(0)]
        public Config Config;
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        [FieldOffset(0)]
        public Connection Connection;
        /// <summary>
        /// 数据库命令
        /// </summary>
        [FieldOffset(0)]
        public SqlCommand SqlCommand;
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        [FieldOffset(0)]
        public SqlConnection SqlConnection;
#if !NETSTANDARD2_0
        /// <summary>
        /// 数据库命令
        /// </summary>
        [FieldOffset(0)]
        public OleDbCommand OleDbCommand;
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        [FieldOffset(0)]
        public OleDbConnection OleDbConnection;
#endif

        /// <summary>
        /// 表示包含二元运算符的表达式
        /// </summary>
        [FieldOffset(0)]
        public BinaryExpression BinaryExpression;
        /// <summary>
        /// 表示包含条件运算符的表达式
        /// </summary>
        [FieldOffset(0)]
        public ConditionalExpression ConditionalExpression;
        /// <summary>
        /// 表示具有常量值的表达式
        /// </summary>
        [FieldOffset(0)]
        public ConstantExpression ConstantExpression;
        /// <summary>
        /// 表示访问字段或属性
        /// </summary>
        [FieldOffset(0)]
        public MemberExpression MemberExpression;
        /// <summary>
        /// 表示对静态方法或实例方法的调用
        /// </summary>
        [FieldOffset(0)]
        public MethodCallExpression MethodCallExpression;
        /// <summary>
        /// 表示包含一元运算符的表达式
        /// </summary>
        [FieldOffset(0)]
        public UnaryExpression UnaryExpression;
    }
}
