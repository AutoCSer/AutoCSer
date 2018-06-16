//本文件由程序自动生成,请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<long> GetLong(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetLong(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<uint> GetUInt(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetUInt(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<int> GetInt(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetInt(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<ushort> GetUShort(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetUShort(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<short> GetShort(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetShort(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<byte> GetByte(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetByte(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<sbyte> GetSByte(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetSByte(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<double> GetDouble(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetDouble(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<float> GetFloat(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetFloat(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<decimal> GetDecimal(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetDecimal(value.Type);
        }
    }
}

namespace AutoCSer.CacheServer
{
    /// <summary>
    /// 客户端
    /// </summary>
    public sealed partial class Client
    {
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal static ReturnValue<bool> GetBool(AutoCSer.Net.TcpServer.ReturnValue<ReturnParameter> value)
        {
            return value.Value.Parameter.GetBool(value.Type);
        }
    }
}

namespace AutoCSer.Extension
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public static partial class IntegerUpdater
    {
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<long> GetIntegerUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<long> node, int index)
        {
            return node.GetInteger(index);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<long> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, long> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetInteger(key);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<long> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, long> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetInteger(key);
        }
    }
}

namespace AutoCSer.Extension
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public static partial class IntegerUpdater
    {
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<uint> GetIntegerUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<uint> node, int index)
        {
            return node.GetInteger(index);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<uint> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, uint> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetInteger(key);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<uint> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, uint> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetInteger(key);
        }
    }
}

namespace AutoCSer.Extension
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public static partial class IntegerUpdater
    {
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<int> GetIntegerUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<int> node, int index)
        {
            return node.GetInteger(index);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<int> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, int> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetInteger(key);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<int> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, int> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetInteger(key);
        }
    }
}

namespace AutoCSer.Extension
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public static partial class IntegerUpdater
    {
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<ushort> GetIntegerUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<ushort> node, int index)
        {
            return node.GetInteger(index);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<ushort> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, ushort> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetInteger(key);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<ushort> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, ushort> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetInteger(key);
        }
    }
}

namespace AutoCSer.Extension
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public static partial class IntegerUpdater
    {
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<short> GetIntegerUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<short> node, int index)
        {
            return node.GetInteger(index);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<short> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, short> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetInteger(key);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<short> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, short> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetInteger(key);
        }
    }
}

namespace AutoCSer.Extension
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public static partial class IntegerUpdater
    {
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<byte> GetIntegerUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<byte> node, int index)
        {
            return node.GetInteger(index);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<byte> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, byte> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetInteger(key);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<byte> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, byte> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetInteger(key);
        }
    }
}

namespace AutoCSer.Extension
{
    /// <summary>
    /// 整数更新
    /// </summary>
    public static partial class IntegerUpdater
    {
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">数组节点</param>
        /// <param name="index">数组索引位置</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<sbyte> GetIntegerUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<sbyte> node, int index)
        {
            return node.GetInteger(index);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<sbyte> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, sbyte> node, keyType key)
            where keyType : IEquatable<keyType>
        {
            return node.GetInteger(key);
        }
        /// <summary>
        /// 获取整数更新
        /// </summary>
        /// <param name="node">字典节点</param>
        /// <param name="key">关键字</param>
        /// <returns>整数更新</returns>
        [System.Runtime.CompilerServices.MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public static AutoCSer.CacheServer.OperationUpdater.Integer<sbyte> GetIntegerUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, sbyte> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetInteger(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<long> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<long> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<long> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, long> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<long> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, long> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<uint> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<uint> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<uint> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, uint> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<uint> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, uint> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<int> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<int> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<int> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, int> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<int> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, int> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<ushort> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<ushort> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<ushort> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, ushort> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<ushort> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, ushort> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<short> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<short> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<short> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, short> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<short> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, short> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<byte> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<byte> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<byte> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, byte> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<byte> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, byte> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<sbyte> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<sbyte> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<sbyte> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, sbyte> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<sbyte> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, sbyte> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<double> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<double> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<double> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, double> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<double> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, double> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<float> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<float> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<float> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, float> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<float> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, float> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

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
        public static AutoCSer.CacheServer.OperationUpdater.Number<decimal> GetNumberUpdater(this AutoCSer.CacheServer.DataStructure.Abstract.ValueArray<decimal> node, int index)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<decimal> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.Abstract.ValueDictionary<keyType, decimal> node, keyType key)
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
        public static AutoCSer.CacheServer.OperationUpdater.Number<decimal> GetNumberUpdater<keyType>(this AutoCSer.CacheServer.DataStructure.ValueSearchTreeDictionary<keyType, decimal> node, keyType key)
            where keyType : IEquatable<keyType>, IComparable<keyType>
        {
            return node.GetNumber(key);
        }
    }
}

#endif