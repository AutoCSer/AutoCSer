using System;
using System.Reflection;
using System.Collections.Generic;
using AutoCSer.Extensions;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;
using AutoCSer.Json;

namespace AutoCSer
{
    /// <summary>
    /// JSON 解析
    /// </summary>
    public unsafe sealed partial class JsonDeSerializer
    {
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <param name="config">配置参数</param>
        /// <returns>是否解析成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static DeSerializeResult DeSerializeNotEmpty<valueType>(string json, ref valueType value, DeSerializeConfig config = null)
        {
            JsonDeSerializer deSerializer = YieldPool.Default.Pop() ?? new JsonDeSerializer();
            try
            {
                return deSerializer.deSerialize<valueType>(json, ref value, config);
            }
            finally { deSerializer.Free(); }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetTcpServer()
        {
            Config = DefaultConfig;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="data">数据</param>
        /// <param name="value">目标对象</param>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal bool DeSerializeTcpServer<valueType>(ref SubArray<byte> data, ref valueType value)
        {
            fixed (byte* dataFixed = data.GetFixedBuffer())
            {
                end = (jsonFixed = Current = (char*)(dataFixed + data.Start)) + (data.Length >> 1);
                return deSerializeTcpServer(ref value);
            }
        }
        ///// <summary>
        ///// 反序列化
        ///// </summary>
        ///// <typeparam name="valueType">数据类型</typeparam>
        ///// <param name="data">数据</param>
        ///// <param name="value">目标对象</param>
        ///// <returns>是否成功</returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //internal bool DeSerializeTcpServer<valueType>(byte[] data, ref valueType value)
        //{
        //    fixed (byte* dataFixed = data)
        //    {
        //        end = (jsonFixed = Current = (char*)dataFixed) + (data.Length >> 1);
        //        return deSerializeTcpServer(ref value);
        //    }
        //}
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool deSerializeTcpServer<valueType>(ref valueType value)
        {
            //MemberMap = null;
            //anonymousTypes.Null();
            if (endChar != *(end - 1))
            {
                if (((endChar = *(end - 1)) & 0xff00) == 0)
                {
                    isEndSpace = (bits[(byte)endChar] & AutoCSer.JsonDeSerializer.DeSerializeSpaceBit) == 0;
                    if ((uint)(endChar - '0') < 10) isEndDigital = isEndHex = isEndNumber = true;
                    else
                    {
                        isEndDigital = false;
                        if ((uint)((endChar | 0x20) - 'a') < 6) isEndHex = isEndNumber = true;
                        else
                        {
                            isEndHex = false;
                            isEndNumber = (bits[(byte)endChar] & AutoCSer.JsonDeSerializer.DeSerializeNumberBit) == 0;
                        }
                    }
                }
                else isEndSpace = isEndDigital = isEndHex = isEndNumber = false;
            }
            DeSerializeState = DeSerializeState.Success;
            TypeDeSerializer<valueType>.DeSerializeTcpServer(this, ref value);
            if (DeSerializeState == DeSerializeState.Success)
            {
                if (Current == end) return true;
                space();
                return DeSerializeState == DeSerializeState.Success && Current == end;
            }
            return false;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetEnum()
        {
            Config = DefaultConfig;
            endChar = '"';
            isEndSpace = isEndDigital = isEndHex = isEndNumber = false;
        }
        /// <summary>
        /// JSON 解析
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <param name="value">目标数据</param>
        /// <returns>是否解析成功</returns>
        internal bool DeSerializeEnum<valueType>(string json, ref valueType value)
        {
            //MemberMap = null;
            //anonymousTypes.Null();
            fixed (char* jsonFixed = json)
            {
                Current = this.jsonFixed = jsonFixed;
                end = jsonFixed + json.Length;
                DeSerializeState = DeSerializeState.Success;
                TypeDeSerializer<valueType>.DefaultDeSerializer(this, ref value);
            }
            return DeSerializeState == DeSerializeState.Success && Current == end;
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
        internal bool DeSerializeWebViewNotEmpty<valueType>(ref valueType value, string json, DeSerializeConfig config)
        {
            this.Config = config ?? DefaultConfig;
            fixed (char* jsonFixed = (this.json = json))
            {
                Current = this.jsonFixed = jsonFixed;
                end = jsonFixed + json.Length;
                return deSerializeTcpServer(ref value);
            }
        }

        /// <summary>
        /// 预编译类型
        /// </summary>
        /// <param name="types"></param>
        internal static void Compile(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type != null) GenericType.Get(type).JsonSerializeCompile();
            }
        }
    }
}
