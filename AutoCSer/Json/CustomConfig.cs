using System;
using AutoCSer.Memory;

namespace AutoCSer.Json
{
    /// <summary>
    /// 自定义全局配置
    /// </summary>
    public class CustomConfig
    {
        /// <summary>
        /// 写入浮点数（.NET Standard 2.1 可以自己重写配合 CharStream.GetPrepChar + float.TryFormat 处理）
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(CharStream charStream, float value)
        {
            charStream.SimpleWrite(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return 0;
        }
        /// <summary>
        /// 写入浮点数（.NET Standard 2.1 可以自己重写 CharStream.GetPrepChar + double.TryFormat 处理）
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(CharStream charStream, double value)
        {
            charStream.Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return 0;
        }
        /// <summary>
        /// 写入小数（.NET Standard 2.1 可以自己重写配合 CharStream.GetPrepChar + decimal.TryFormat 处理）
        /// </summary>
        /// <param name="charStream"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(CharStream charStream, decimal value)
        {
            charStream.SimpleWrite(value.ToString());
            return 0;
        }
        /// <summary>
        /// 写入时间值
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int Write(JsonSerializer jsonSerializer, DateTime value)
        {
            jsonSerializer.SerializeDateTime(value);
            return 0;
        }
        /// <summary>
        /// 自定义序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        /// <returns>未写入字符数量</returns>
        public virtual int NotSupport<T>(JsonSerializer jsonSerializer, T value)
        {
            if (value != null)
            {
                Type type = typeof(T);
                if (type.IsInterface) jsonSerializer.CallSerialize((object)value);
                else
                {
                    if (!type.IsArray) jsonSerializer.CharStream.WriteJsonObject();
                    else jsonSerializer.CharStream.WriteJsonArray();
                }
            }
            else jsonSerializer.CharStream.WriteJsonNull();
            return 0;
        }

        /// <summary>
        /// 自定义反序列化浮点数（.NET Standard 2.1 可以自己重写 float.TryParse 处理）
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool DeSerialize(JsonDeSerializer jsonDeSerializer, ReadOnlySpan<char> buffer, ref float value)
        {
            string stringBuffer = jsonDeSerializer.GetStringBuffer(buffer.Length);
            return stringBuffer.Length != 0 && float.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化浮点数（.NET Standard 2.1 可以自己重写 double.TryParse 处理）
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool DeSerialize(JsonDeSerializer jsonDeSerializer, ReadOnlySpan<char> buffer, ref double value)
        {
            string stringBuffer = jsonDeSerializer.GetStringBuffer(buffer.Length);
            return stringBuffer.Length != 0 && double.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化小数（.NET Standard 2.1 可以自己重写 decimal.TryParse 处理）
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool DeSerialize(JsonDeSerializer jsonDeSerializer, ReadOnlySpan<char> buffer, ref decimal value)
        {
            string stringBuffer = jsonDeSerializer.GetStringBuffer(buffer.Length);
            return stringBuffer.Length != 0 && decimal.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool DeSerialize(JsonDeSerializer jsonDeSerializer, ref DateTime value)
        {
            string stringBuffer = jsonDeSerializer.GetQuoteStringBuffer();
            return stringBuffer.Length != 0 && DateTime.TryParse(stringBuffer, out value);
        }
        /// <summary>
        /// 自定义反序列化时间值
        /// </summary>
        /// <param name="jsonDeSerializer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool DeSerializeNotDateTime(JsonDeSerializer jsonDeSerializer, ref DateTime value)
        {
            jsonDeSerializer.DeSerializeState = DeSerializeState.NotDateTime;
            return false;
        }
        /// <summary>
        /// 自定义反序列化不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonDeSerializer">JSON 反序列化</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual DeSerializeState NotSupport<T>(JsonDeSerializer jsonDeSerializer, ref T value)
        {
            jsonDeSerializer.Ignore();
            return jsonDeSerializer.DeSerializeState;
            //return DeSerializeState.NotSupport;
        }
        /// <summary>
        /// 自定义反序列化浮点数（.NET Standard 2.1 可以自己重写 float.TryParse 处理）
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool DeSerialize(ref SubString buffer, out double value)
        {
            return double.TryParse(buffer, out value);
        }
    }
}
