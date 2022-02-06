using System;
using System.Runtime.CompilerServices;
using AutoCSer.Json;
using AutoCSer.Memory;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// JSON 序列化扩展操作
    /// </summary>
    public static partial class JsonSerialize
    {
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static string toJson<T>(this T value, SerializeConfig config = null)
        {
            return JsonSerializer.Serialize(ref value, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">配置参数</param>
        /// <returns></returns>
        public static SerializeWarning toJson<T>(this T value, CharStream jsonStream, SerializeConfig config = null)
        {
            return JsonSerializer.Serialize(ref value, jsonStream, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 字符串</returns>
        public static string toJsonThreadStatic<T>(this T value, SerializeConfig config = null)
        {
            return JsonSerializer.ThreadStaticSerialize(ref value, config);
        }

        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult fromJson<T>(this T value, SubString json, DeSerializeConfig config = null)
        {
            return JsonDeSerializer.DeSerialize(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult fromJson<T>(this T value, ref SubString json, DeSerializeConfig config = null)
        {
            return JsonDeSerializer.DeSerialize(ref json, ref value, config);
        }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">目标数据</param>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static DeSerializeResult fromJson<T>(this T value, string json, DeSerializeConfig config = null)
        {
            return JsonDeSerializer.DeSerialize(json, ref value, config);
        }

        /// <summary>
        /// JSON 节点解析
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 节点 + 解析状态结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static KeyValue<DeSerializeResult, Node> toJsonNode<T>(this SubString json, DeSerializeConfig config = null)
        {
            KeyValue<DeSerializeResult, Node> value = default(KeyValue<DeSerializeResult, Node>);
            value.Key = JsonDeSerializer.DeSerialize(ref json, ref value.Value, config);
            return value;
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="config">配置参数</param>
        /// <returns>JSON 节点 + 解析状态结果</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static KeyValue<DeSerializeResult, Node> toJsonNode<T>(this string json, DeSerializeConfig config = null)
        {
            KeyValue<DeSerializeResult, Node> value = default(KeyValue<DeSerializeResult, Node>);
            value.Key = JsonDeSerializer.DeSerialize(json, ref value.Value, config);
            return value;
        }
    }
}
