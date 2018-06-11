using System;

namespace AutoCSer.CacheServer.OperationUpdater
{
    /// <summary>
    /// 服务端数据处理
    /// </summary>
    internal unsafe static class Data
    {
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref ulong value, ulong updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if (value >= updateValue)
                        {
                            value %= updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Xor:
                    if (updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value ^= ulong.MaxValue; return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref long value, long updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if ((updateValue = value % updateValue) != value)
                        {
                            value = updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Xor:
                    if ((ulong)updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value = (long)((ulong)value ^ ulong.MaxValue); return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref uint value, uint updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if (value >= updateValue)
                        {
                            value %= updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Xor:
                    if (updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value ^= uint.MaxValue; return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref int value, int updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if ((updateValue = value % updateValue) != value)
                        {
                            value = updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Xor:
                    if ((uint)updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value = (int)((uint)value ^ uint.MaxValue); return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref ushort value, ushort updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if (value >= updateValue)
                        {
                            value %= updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Xor:
                    if (updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value ^= ushort.MaxValue; return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref short value, short updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if ((updateValue = (short)(value % updateValue)) != value)
                        {
                            value = updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Xor:
                    if ((ushort)updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value = (short)(ushort)((uint)(ushort)value ^ uint.MaxValue); return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref byte value, byte updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if (value >= updateValue)
                        {
                            value %= updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Xor:
                    if (updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value ^= byte.MaxValue; return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref sbyte value, sbyte updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    switch (updateValue)
                    {
                        case 0: return ReturnType.DivideByZero;
                        case 1: return ReturnType.Unknown;
                        default: value /= updateValue; return ReturnType.Success;
                    }
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if ((updateValue = (sbyte)(value % updateValue)) != value)
                        {
                            value = updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
                case OperationType.Xor:
                    if ((byte)updateValue != 0)
                    {
                        value ^= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.And:
                    if ((updateValue &= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Or:
                    if ((updateValue |= value) != value)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Not: value = (sbyte)(byte)((uint)(byte)value ^ uint.MaxValue); return ReturnType.Success;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref double value, double updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    if (updateValue != 1)
                    {
                        if (updateValue != 0)
                        {
                            value /= updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.DivideByZero;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if ((updateValue = value % updateValue) != value)
                        {
                            value = updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref float value, float updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    if (updateValue != 1)
                    {
                        if (updateValue != 0)
                        {
                            value /= updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.DivideByZero;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if ((updateValue = value % updateValue) != value)
                        {
                            value = updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
            }
            return ReturnType.UpdateOperationTypeError;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="updateValue"></param>
        internal static ReturnType Update(OperationType type, ref decimal value, decimal updateValue)
        {
            switch (type)
            {
                case OperationType.Set:
                    if (value != updateValue)
                    {
                        value = updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Add:
                    if (updateValue != 0)
                    {
                        value += updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Sub:
                    if (updateValue != 0)
                    {
                        value -= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mul:
                    if (updateValue != 1)
                    {
                        value *= updateValue;
                        return ReturnType.Success;
                    }
                    return ReturnType.Unknown;
                case OperationType.Div:
                    if (updateValue != 1)
                    {
                        if (updateValue != 0)
                        {
                            value /= updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.DivideByZero;
                    }
                    return ReturnType.Unknown;
                case OperationType.Mod:
                    if (updateValue != 0)
                    {
                        if ((updateValue = value % updateValue) != value)
                        {
                            value = updateValue;
                            return ReturnType.Success;
                        }
                        return ReturnType.Unknown;
                    }
                    return ReturnType.DivideByZero;
            }
            return ReturnType.UpdateOperationTypeError;
        }

        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, ulong value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, long value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, uint value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, int value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, ushort value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, short value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, byte value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, sbyte value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, double value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, float value)
        {
            parser.UpdateOperation(value);
        }
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        /// <param name="parser"></param>
        /// <param name="value"></param>
        internal static void UpdateOperation(ref OperationParameter.NodeParser parser, decimal value)
        {
            parser.UpdateOperation(value);
        }

        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, ulong value, ulong logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, long value, long logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, uint value, uint logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, int value, int logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, ushort value, ushort logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, short value, short logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, byte value, byte logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, sbyte value, sbyte logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, double value, double logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, float value, float logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="logicValue"></param>
        /// <returns></returns>
        internal static bool IsLogicData(LogicType type, decimal value, decimal logicValue)
        {
            switch (type)
            {
                case LogicType.Equal: return value == logicValue;
                case LogicType.NotEqual: return value != logicValue;
                case LogicType.MoreOrEqual: return value >= logicValue;
                case LogicType.More: return value > logicValue;
                case LogicType.LessOrEqual: return value <= logicValue;
                case LogicType.Less: return value < logicValue;
            }
            return false;
        }
    }
    /// <summary>
    /// 服务端数据处理
    /// </summary>
    /// <typeparam name="valueType">数据类型</typeparam>
    internal static class Data<valueType>
    {
        /// <summary>
        /// 修改数据委托
        /// </summary>
        internal static readonly UpdateData<valueType> UpdateData;
        /// <summary>
        /// 修改操作数据包
        /// </summary>
        internal static readonly UpdateOperationData<valueType> UpdateOperationData;
        /// <summary>
        /// 数据逻辑运算判断
        /// </summary>
        internal static readonly Func<LogicType, valueType, valueType, bool> IsLogicData;

        static Data()
        {
            if (ValueData.Data<valueType>.IsUpdate)
            {
                Type type = typeof(valueType);
                if (type == typeof(int))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<int>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<int>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, int, int, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(long))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<long>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<long>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, long, long, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(ulong))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<ulong>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<ulong>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, ulong, ulong, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(uint))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<uint>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<uint>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, uint, uint, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(short))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<short>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<short>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, short, short, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(ushort))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<ushort>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<ushort>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, ushort, ushort, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(byte))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<byte>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<byte>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, byte, byte, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(sbyte))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<sbyte>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<sbyte>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, sbyte, sbyte, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(float))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<float>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<float>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, float, float, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(double))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<double>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<double>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, double, double, bool>)Data.IsLogicData;
                    return;
                }
                if (type == typeof(decimal))
                {
                    UpdateData = (UpdateData<valueType>)(object)(UpdateData<decimal>)Data.Update;
                    UpdateOperationData = (UpdateOperationData<valueType>)(object)(UpdateOperationData<decimal>)Data.UpdateOperation;
                    IsLogicData = (Func<LogicType, valueType, valueType, bool>)(object)(Func<LogicType, decimal, decimal, bool>)Data.IsLogicData;
                    return;
                }
            }
        }
    }
}
