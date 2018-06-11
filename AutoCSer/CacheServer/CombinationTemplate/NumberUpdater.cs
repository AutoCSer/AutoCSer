using System;
/*Type:ulong;long;uint;int;ushort;short;byte;sbyte;double;float;decimal*/

namespace AutoCSer.Extension
{
    /// <summary>
    /// 数字更新
    /// </summary>
    public static partial class NumberUpdater
    {
        /// <summary>
        /// 获取数字更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>数字更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Number</*Type[0]*/ulong/*Type[0]*/> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray</*Type[0]*/ulong/*Type[0]*/> node, int index)
        {
            return node.GetNumber(index);
        }
        /// <summary>
        /// 获取数字更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>数字更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Number</*Type[0]*/ulong/*Type[0]*/> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, /*Type[0]*/ulong/*Type[0]*/> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetNumber(key);
        }
        /// <summary>
        /// 获取数字更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>数字更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Number</*Type[0]*/ulong/*Type[0]*/> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, /*Type[0]*/ulong/*Type[0]*/> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}
