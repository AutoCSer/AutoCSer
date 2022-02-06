using System;
using System.Runtime.CompilerServices;
using AutoCSer.Xml;
using AutoCSer.Memory;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// XML 序列化扩展操作
    /// </summary>
    public static partial class XmlSerialize
    {
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>Xml 字符串</returns>
        public static string toXml<T>(this T value, SerializeConfig config = null)
        {
            return XmlSerializer.Serialize(value, config);
        }
        /// <summary>
        /// 对象转换 XML 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="xmlStream">Xml输出缓冲区</param>
        /// <param name="config">配置参数</param>
        /// <returns>警告提示状态</returns>
        public static SerializeWarning toXml<T>(this T value, CharStream xmlStream, SerializeConfig config = null)
        {
            return XmlSerializer.Serialize(value, xmlStream, config);
        }


        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult fromXml<T>(this T value, SubString xml, DeSerializeConfig config = null)
        {
            return XmlDeSerializer.DeSerialize(ref xml, ref value, config);
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult fromXml<T>(this T value, ref SubString xml, DeSerializeConfig config = null)
        {
            return XmlDeSerializer.DeSerialize(ref xml, ref value, config);
        }
        /// <summary>
        /// XML 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="xml">XML 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>XML 解析结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult fromXml<T>(this T value, string xml, DeSerializeConfig config = null)
        {
            return XmlDeSerializer.DeSerialize(xml, ref value, config);
        }
    }
}
