using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.DataStructure.Value
{
    /// <summary>
    /// 返回值扩展方法
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> Get<valueType>(this ReturnValueNode<Binary<valueType>> value)
        {
            return value.Value.GetBinary<valueType>();
        }
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> Get<valueType>(this ReturnValueNode<Json<valueType>> value)
        {
            return value.Value.GetJson<valueType>();
        }
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> Get<valueType>(this ReturnValueNode<Value<valueType>> value)
        {
            return value.Value.Get(ValueData.Data<valueType>.GetData);
        }
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> Get<valueType>(this ReturnValueNode<Number<valueType>> value)
        {
            return value.Value.Get(ValueData.Data<valueType>.GetData);
        }
        /// <summary>
        /// 获取返回值数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static ReturnValue<valueType> Get<valueType>(this ReturnValueNode<Integer<valueType>> value)
        {
            return value.Value.Get(ValueData.Data<valueType>.GetData);
        }
    }
}
