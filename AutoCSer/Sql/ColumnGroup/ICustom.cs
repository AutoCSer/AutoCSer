using System;
using System.Data.Common;

namespace AutoCSer.Sql.ColumnGroup
{
#if NOJIT
    /// <summary>
    /// 自定义类型处理接口
    /// </summary>
    public interface ICustom
    {
        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="reader">字段读取器物理存储</param>
        /// <param name="value">目标数据</param>
        /// <param name="index">当前读取位置</param>
        void Set(DbDataReader reader, object value, ref int index);
        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sqlTool"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        bool Verify(object value, AutoCSer.emit.sqlTable.sqlToolBase sqlTool, string columnName);
        /// <summary>
        /// 获取,分割列名集合
        /// </summary>
        /// <param name="name">列名前缀</param>
        /// <returns></returns>
        string GetColumnNames(string name);
        /// <summary>
        /// 获取成员名称与类型集合
        /// </summary>
        /// <param name="name">列名前缀</param>
        /// <returns></returns>
        keyValue<string, Type>[] GetDataColumns(string name);
        /// <summary>
        /// 获取插入数据SQL表达式
        /// </summary>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="value">数据列</param>
        /// <param name="converter">SQL常量转换</param>
        void Insert(charStream sqlStream, object value, constantConverter converter);
        /// <summary>
        /// 获取更新数据SQL表达式
        /// </summary>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="value">数据列</param>
        /// <param name="converter">SQL常量转换</param>
        /// <param name="columnName">列名前缀</param>
        void Update(charStream sqlStream, object value, constantConverter converter, string columnName);
        /// <summary>
        /// 读取字段值
        /// </summary>
        /// <param name="value">数据列</param>
        /// <param name="values">目标数组</param>
        /// <param name="index">当前写入位置</param>
        void ToArray(object value, object[] values, ref int index);
        /// <summary>
        /// 获取添加SQL表达式
        /// </summary>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="value">数据列</param>
        /// <param name="converter">SQL常量转换</param>
        /// <param name="columnName">列名前缀</param>
        void Where(charStream sqlStream, object value, constantConverter converter, string columnName);
    }
#else
    /// <summary>
    /// 自定义类型处理接口
    /// </summary>
    /// <typeparam name="valueType"></typeparam>
    public interface ICustom<valueType>
    {
        /// <summary>
        /// 设置字段值
        /// </summary>
        /// <param name="reader">字段读取器物理存储</param>
        /// <param name="value">目标数据</param>
        /// <param name="index">当前读取位置</param>
        void Set(DbDataReader reader, ref valueType value, ref int index);
        /// <summary>
        /// 数据验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sqlTool"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        bool Verify(valueType value, Table sqlTool, string columnName);
        /// <summary>
        /// 获取,分割列名集合
        /// </summary>
        /// <param name="name">列名前缀</param>
        /// <returns></returns>
        string GetColumnNames(string name);
        /// <summary>
        /// 获取成员名称与类型集合
        /// </summary>
        /// <param name="name">列名前缀</param>
        /// <returns></returns>
        KeyValue<string, Type>[] GetDataColumns(string name);
        /// <summary>
        /// 获取插入数据SQL表达式
        /// </summary>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="value">数据列</param>
        /// <param name="converter">SQL常量转换</param>
        void Insert(CharStream sqlStream, valueType value, ConstantConverter converter);
        /// <summary>
        /// 获取更新数据SQL表达式
        /// </summary>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="value">数据列</param>
        /// <param name="converter">SQL常量转换</param>
        /// <param name="columnName">列名前缀</param>
        void Update(CharStream sqlStream, valueType value, ConstantConverter converter, string columnName);
        /// <summary>
        /// 读取字段值
        /// </summary>
        /// <param name="value">数据列</param>
        /// <param name="values">目标数组</param>
        /// <param name="index">当前写入位置</param>
        void ToArray(valueType value, object[] values, ref int index);
        /// <summary>
        /// 获取添加SQL表达式
        /// </summary>
        /// <param name="sqlStream">SQL表达式流</param>
        /// <param name="value">数据列</param>
        /// <param name="converter">SQL常量转换</param>
        /// <param name="columnName">列名前缀</param>
        void Where(CharStream sqlStream, valueType value, ConstantConverter converter, string columnName);
    }
#endif
}
