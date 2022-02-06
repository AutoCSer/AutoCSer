using System;
using System.Reflection;
using AutoCSer.Extensions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AutoCSer.Metadata;
using AutoCSer.Memory;
using AutoCSer.Json;

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer
    {
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="onSerializeStream">序列化以后的数据流处理事件</param>
        /// <param name="config">配置参数</param>
        private void serialize<valueType>(ref valueType value, Action<CharStream> onSerializeStream, SerializeConfig config)
        {
            Config = config ?? DefaultConfig;
            AutoCSer.Memory.Pointer buffer = UnmanagedPool.Default.GetPointer();
            try
            {
                CharStream.Reset(ref buffer);
                using (CharStream)
                {
                    serialize(ref value);
                    onSerializeStream(CharStream);
                }
            }
            finally { UnmanagedPool.Default.PushOnly(ref buffer); }
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="valueType">目标数据类型</typeparam>
        /// <param name="value">数据对象</param>
        /// <param name="onSerializeStream">序列化以后的数据流处理事件</param>
        /// <param name="config">配置参数</param>
        public static SerializeWarning Serialize<valueType>(ref valueType value, Action<CharStream> onSerializeStream, SerializeConfig config = null)
        {
            if (onSerializeStream == null) throw new ArgumentNullException();
            JsonSerializer jsonSerializer = YieldPool.Default.Pop() ?? new JsonSerializer();
            try
            {
                jsonSerializer.serialize<valueType>(ref value, onSerializeStream, config);
                return jsonSerializer.Warning;
            }
            finally { jsonSerializer.Free(); }
        }
        ///// <summary>
        ///// 创建默认配置 JSON 序列化
        ///// </summary>
        ///// <param name="jsonStream"></param>
        ///// <returns></returns>
        //[MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        //public static Serializer CreateDefault(out CharStream jsonStream)
        //{
        //    Serializer serializer = new Serializer();
        //    serializer.SetTcpServer();
        //    (jsonStream = serializer.CharStream).Reset();
        //    return serializer;
        //}
        /// <summary>
        /// 初始化
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetTcpServer()
        {
            Config = AutoCSer.Net.TcpServer.Server.JsonConfig;
            if (checkLoopDepth != Config.CheckLoopDepth || isLoopObject)
            {
                if (Config.CheckLoopDepth <= 0)
                {
                    checkLoopDepth = 0;
                    if (forefather == null) forefather = new object[sizeof(int)];
                }
                else checkLoopDepth = Config.CheckLoopDepth;
            }
            isLoopObject = false;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="valueType"></typeparam>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        internal void SerializeTcpServer<valueType>(ref valueType value, UnmanagedStream stream)
            //where valueType : struct
        {
            int index = stream.Data.CurrentIndex;
            if (forefatherCount != 0)
            {
                System.Array.Clear(forefather, 0, forefatherCount);
                forefatherCount = 0;
            }
            CharStream.From(stream);
            try
            {
                //Warning = SerializeWarning.None;
                TypeSerializer<valueType>.SerializeTcpServer(this, ref value);
            }
            finally { stream.From(CharStream); }
            if (((stream.Data.CurrentIndex - index) & 2) != 0) stream.Write(' ');
        }

        /// <summary>
        /// 预编译类型
        /// </summary>
        /// <param name="types"></param>
        internal static void Compile(Type[] types)
        {
            foreach (Type type in types)
            {
                if (type != null) GenericType.Get(type).JsonDeSerializeCompile(); 
            }
        }
    }
}
