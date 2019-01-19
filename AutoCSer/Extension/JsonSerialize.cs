using System;
using System.Runtime.CompilerServices;
using AutoCSer.Json;

namespace AutoCSer.Extension
{
    /// <summary>
    /// JSON 序列化扩展操作
    /// </summary>
    public static partial class JsonSerialize
    {
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static SerializeResult toJson<valueType>(this valueType value, SerializeConfig config = null)
        {
            return Serializer.Serialize(value, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        public static SerializeWarning toJson<valueType>(this valueType value, CharStream jsonStream, SerializeConfig config = null)
        {
            return Serializer.Serialize(value, jsonStream, config);
        }

        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult fromJson<valueType>(this valueType value, SubString json, ParseConfig config = null)
        {
            return Parser.Parse(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult fromJson<valueType>(this valueType value, ref SubString json, ParseConfig config = null)
        {
            return Parser.Parse(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ParseResult fromJson<valueType>(this valueType value, string json, ParseConfig config = null)
        {
            return Parser.Parse(json, ref value, config);
        }

        /// <summary>
        /// JSON 节点解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 节点 + 解析状态结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static KeyValue<ParseResult, Node> toJsonNode<valueType>(this SubString json, ParseConfig config = null)
        {
            KeyValue<ParseResult, Node> value = default(KeyValue<ParseResult, Node>);
            value.Key = Parser.Parse(ref json, ref value.Value, config);
            return value;
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 节点 + 解析状态结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static KeyValue<ParseResult, Node> toJsonNode<valueType>(this string json, ParseConfig config = null)
        {
            KeyValue<ParseResult, Node> value = default(KeyValue<ParseResult, Node>);
            value.Key = Parser.Parse(json, ref value.Value, config);
            return value;
        }
    }
}
