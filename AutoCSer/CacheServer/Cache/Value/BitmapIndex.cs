using System;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 获取位图数据
    /// </summary>
    internal struct BitmapIndex
    {
        /// <summary>
        /// 获取的位图
        /// </summary>
        internal byte[] Map;
        /// <summary>
        /// 位图内的字节偏移
        /// </summary>
        private int mapIndex;
        /// <summary>
        /// 位图内的二进制位偏移
        /// </summary>
        private uint index;
        /// <summary>
        /// 获取位图数据
        /// </summary>
        /// <param name="maps"></param>
        /// <param name="parser"></param>
        internal BitmapIndex(byte[][] maps, ref OperationParameter.NodeParser parser)
        {
            if (parser.ValueData.Type == ValueData.DataType.UInt)
            {
                if ((mapIndex = (int)((index = parser.ValueData.Int64.UInt) >> (Bitmap.MaxMapBits + 3))) < maps.Length)
                {
                    byte[] map = maps[mapIndex];
                    if (map != null && (mapIndex = (int)((index &= ((1U << (Bitmap.MaxMapBits + 3)) - 1)) >> 3)) < map.Length)
                    {
                        Map = map;
                        return;
                    }
                }
                Map = null;
                parser.ReturnParameter.ReturnParameterSet(false);
            }
            else
            {
                index = 0;
                mapIndex = 0;
                Map = null;
                parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
        }
        /// <summary>
        /// 获取位图数据
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="parser"></param>
        internal BitmapIndex(Bitmap bitmap, ref OperationParameter.NodeParser parser)
        {
            if (parser.ValueData.Type == ValueData.DataType.UInt)
            {
                mapIndex = (int)((index = parser.ValueData.Int64.UInt) >> (Bitmap.MaxMapBits + 3));
                byte[][] maps = bitmap.Maps;
                if (mapIndex >= maps.Length) bitmap.Maps = maps = maps.copyNew(Math.Max(maps.Length << 1, mapIndex + 1));
                index &= ((1U << (Bitmap.MaxMapBits + 3)) - 1);
                if ((Map = maps[mapIndex]) == null)
                {
                    if (mapIndex != 0)
                    {
                        int nullIndex = mapIndex;
                        while (nullIndex != 0 && maps[nullIndex - 1] == null) --nullIndex;
                        switch (nullIndex)
                        {
                            case 0:
                                maps[0] = new byte[Bitmap.MaxMapSize];
                                nullIndex = 1;
                                break;
                            case 1:
                                if ((Map = maps[0]).Length != Bitmap.MaxMapSize)
                                {
                                    byte[] newMap = new byte[Bitmap.MaxMapSize];
                                    System.Buffer.BlockCopy(Map, 0, newMap, 0, Map.Length);
                                    maps[0] = newMap;
                                }
                                break;
                        }
                        while (nullIndex != mapIndex) maps[nullIndex++] = new byte[Bitmap.MaxMapSize];
                        maps[mapIndex] = Map = new byte[Bitmap.MaxMapSize];
                        mapIndex = (int)(index >> 3);
                    }
                    else
                    {
                        mapIndex = (int)(index >> 3);
                        maps[0] = Map = new byte[mapIndex + 1];
                    }
                }
                else
                {
                    mapIndex = (int)(index >> 3);
                    if (Map.Length <= mapIndex)
                    {
                        byte[] newMap = new byte[Math.Min(Math.Max(Map.Length << 1, mapIndex + 1), Bitmap.MaxMapSize)];
                        System.Buffer.BlockCopy(Map, 0, newMap, 0, Map.Length);
                        maps[0] = Map = newMap;
                    }
                }
            }
            else
            {
                index = 0;
                mapIndex = 0;
                Map = null;
                parser.ReturnParameter.ReturnType = ReturnType.ValueDataLoadError;
            }
        }
        /// <summary>
        /// 获取数据位
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int Get()
        {
            return Map[mapIndex] & (1 << (int)(index & 7));
        }
        /// <summary>
        /// 是否进行了清除数据操作
        /// </summary>
        /// <returns></returns>
        internal bool IsOperationClear()
        {
            byte value = (byte)(1 << (int)(index & 7));
            if ((Map[mapIndex] & value) != 0)
            {
                Map[mapIndex] ^= value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否进行了设置数据操作
        /// </summary>
        /// <returns></returns>
        internal bool IsOperationSet()
        {
            byte value = (byte)(1 << (int)(index & 7));
            if ((Map[mapIndex] & value) == 0)
            {
                Map[mapIndex] ^= value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取反操作
        /// </summary>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal int SetNegate()
        {
            byte value = (byte)(1 << (int)(index & 7));
            return (Map[mapIndex] ^= value) & value;
        }
    }
}
