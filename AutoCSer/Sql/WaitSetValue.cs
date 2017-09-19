using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Sql
{
    /// <summary>
    /// 等待设置数据
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct WaitSetValue<valueType> where valueType : class
    {
        /// <summary>
        /// 同步等待锁
        /// </summary>
        private AutoCSer.Threading.WaitHandle wait;
        /// <summary>
        /// 等待设置的数据
        /// </summary>
        private valueType value;
        /// <summary>
        /// 等待设置数据
        /// </summary>
        /// <param name="value">设置的数据</param>
        public WaitSetValue(valueType value)
        {
            this.value = value;
            wait = new AutoCSer.Threading.WaitHandle();
            if (value == null) wait.Set(0);
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value">设置的数据</param>
        /// <returns>设置的数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType Set(valueType value)
        {
            if (value != null)
            {
                this.value = value;
                wait.Set();
            }
            return value;
        }
        /// <summary>
        /// 等待设置数据
        /// </summary>
        /// <returns>设置的数据</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public valueType Wait()
        {
            if (value == null) wait.Wait();
            return value;
        }
    }
}
