using System;
using System.Runtime.CompilerServices;
using AutoCSer.Xml;

namespace AutoCSer.Extension
{
    /// <summary>
    /// XML 序列化扩展操作
    /// </summary>
    public static partial class XmlSerialize
    {
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>序列化结果</returns>
        public static SerializeResult toXml<valueType>(this valueType value, SerializeConfig config = null)
        {
            return Serializer.Serialize(value, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="xmlStream">Xml输出缓冲区</param>
        /// <param name="config">配置参数</param>
        /// <returns>警告提示状态</returns>
        public static SerializeWarning toXml<valueType>(this valueType value, CharStream xmlStream, SerializeConfig config = null)
        {
            return Serializer.Serialize(value, xmlStream, config);
        }


        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult fromXml<valueType>(this valueType value, SubString xml, ParseConfig config = null)
        {
            return Parser.Parse(ref xml, ref value, config);
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult fromXml<valueType>(this valueType value, ref SubString xml, ParseConfig config = null)
        {
            return Parser.Parse(ref xml, ref value, config);
        }
        /// <summary>
        /// XML 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult fromXml<valueType>(this valueType value, string xml, ParseConfig config = null)
        {
            return Parser.Parse(xml, ref value, config);
        }
    }
}
