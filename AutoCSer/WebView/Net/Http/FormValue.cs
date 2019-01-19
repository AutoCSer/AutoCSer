using System;
using System.IO;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.Http
{
    /// <summary>
    /// HTTP 请求表单值
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct FormValue
    {
        /// <summary>
        /// 名称
        /// </summary>
        private SubArray<byte> name;
        /// <summary>
        /// 名称
        /// </summary>
        public SubArray<byte> Name
        {
            get { return name; }
        }
        /// <summary>
        /// 表单值
        /// </summary>
        private SubArray<byte> value;
        /// <summary>
        /// 表单值
        /// </summary>
        public SubArray<byte> Value
        {
            get { return value; }
        }
        /// <summary>
        /// 客户端文件名称
        /// </summary>
        public byte[] FileName { get; private set; }
        /// <summary>
        /// 服务器端文件名称
        /// </summary>
        public string SaveFileName { get; private set; }
        /// <summary>
        /// 清表单数据
        /// </summary>
        internal void Clear()
        {
            name.Array = null;
            value.Array = null;
            FileName = null;
            if (SaveFileName != null)
            {
                try
                {
                    if (File.Exists(SaveFileName)) File.Delete(SaveFileName);
                }
                catch (Exception error)
                {
                    AutoCSer.Log.Pub.Log.Add(AutoCSer.Log.LogType.Debug, error);
                }
                SaveFileName = null;
            }
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Null()
        {
            name.Array = null;
            value.Array = null;
            FileName = null;
            SaveFileName = null;
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void Set(byte[] name, byte[] value)
        {
            this.name.Set(name, 0, name.Length);
            if (value == null) this.value.SetNull();
            else this.value.Set(value, 0, value.Length);
        }
        /// <summary>
        /// 设置文件数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="saveFileName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetFile(byte[] name, byte[] fileName, string saveFileName)
        {
            this.name.Set(name, 0, name.Length);
            value.SetNull();
            FileName = fileName;
            SaveFileName = saveFileName;
        }
        /// <summary>
        /// 设置文件数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="fileName"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetFile(byte[] name, byte[] value, byte[] fileName)
        {
            Set(name, value);
            FileName = fileName;
            SaveFileName = null;
        }
        /// <summary>
        /// 设置表单名称
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetName(byte[] buffer, int startIndex, int length)
        {
            name.Set(buffer, startIndex, length);
            value.Set(0, 0);
            FileName = null;
        }
        /// <summary>
        /// 设置表单数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="valueIndex"></param>
        /// <param name="valueLength"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetNameValue(byte[] buffer, int startIndex, int length, int valueIndex, int valueLength)
        {
            name.Set(buffer, startIndex, length);
            value.Set(buffer, valueIndex, valueLength);
            FileName = null;
        }
    }
}
