using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer
{
    /// <summary>
    /// JSON 对象缓存文件
    /// </summary>
    /// <typeparam name="valueType">对象类型</typeparam>
    public sealed class JsonFile<valueType> where valueType : class
    {
        /// <summary>
        /// 缓存文件名称
        /// </summary>
        private readonly string fileName;
        /// <summary>
        /// 编码
        /// </summary>
        private readonly Encoding encoding;
        /// <summary>
        /// 文件访问锁
        /// </summary>
        private readonly object fileLock = new object();
        /// <summary>
        /// 日志处理
        /// </summary>
        private readonly AutoCSer.Log.ILog log;
        /// <summary>
        /// 数据对象
        /// </summary>
        public valueType Value { get; private set; }
        /// <summary>
        /// JSON对象缓存文件
        /// </summary>
        /// <param name="fileName">缓存文件名称</param>
        /// <param name="value">数据对象</param>
        /// <param name="encoding"></param>
        /// <param name="log">日志处理</param>
        public JsonFile(string fileName, valueType value = null, Encoding encoding = null, AutoCSer.Log.ILog log = null)
        {
            this.fileName = fileName;
            this.encoding = encoding ?? AutoCSer.Config.Pub.Default.Encoding;
            this.log = log ?? AutoCSer.Log.Pub.Log;
            bool isFile = false, isJson = false;
            try
            {
                if (File.Exists(fileName))
                {
                    isFile = true;
                    if (AutoCSer.Json.Parser.Parse(File.ReadAllText(fileName, this.encoding), ref value))
                    {
                        Value = value;
                        isJson = true;
                    }
                }
            }
            catch (Exception error)
            {
                log.Add(Log.LogType.Error, error, fileName);
            }
            if (isFile && !isJson) AutoCSer.IO.File.MoveBak(fileName);
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="value">新的对象值</param>
        /// <returns>是否修改成功</returns>
        public bool Rework(valueType value)
        {
            if (value == null) throw new ArgumentNullException();
            string json = AutoCSer.Json.Serializer.Serialize(value);
            Monitor.Enter(fileLock);
            try
            {
                if (write(json))
                {
                    Value = value;
                    return true;
                }
            }
            finally { Monitor.Exit(fileLock); }
            return false;
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <returns>是否成功</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Write()
        {
            return Rework(Value);
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>是否成功</returns>
        private bool write(string json)
        {
            try
            {
                if (File.Exists(fileName)) AutoCSer.IO.File.MoveBak(fileName);
                File.WriteAllText(fileName, json, this.encoding);
                return true;
            }
            catch (Exception error)
            {
                log.Add(Log.LogType.Error, error, fileName);
            }
            return false;
        }
    }
}
