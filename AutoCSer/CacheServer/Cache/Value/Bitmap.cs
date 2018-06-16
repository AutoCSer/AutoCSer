using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extension;

namespace AutoCSer.CacheServer.Cache.Value
{
    /// <summary>
    /// 位图 数据节点
    /// </summary>
    internal sealed class Bitmap : Node
    {
        /// <summary>
        /// 最大位图字节大小二进制位数
        /// </summary>
        internal const int MaxMapBits = 20;
        /// <summary>
        /// 最大位图字节大小
        /// </summary>
        internal const int MaxMapSize = 1 << MaxMapBits;
        /// <summary>
        /// 最大位图大小
        /// </summary>
        internal const uint MaxMapBitSize = MaxMapSize << 3;

        /// <summary>
        /// 位图
        /// </summary>
        internal byte[][] Maps = NullValue<byte[]>.Array;
        /// <summary>
        /// 位图 数据节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        private Bitmap(Cache.Node parent, ref OperationParameter.NodeParser parser) : base(parent) { }

        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetOperationNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void OperationEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.Remove: clear(ref parser); return;
                case OperationParameter.OperationType.SetValue: set(ref parser); return;
                case OperationParameter.OperationType.SetNegate: setNegate(ref parser); return;
                case OperationParameter.OperationType.Clear:
                    if (Maps.Length != 0)
                    {
                        Maps = NullValue<byte[]>.Array;
                        parser.IsOperation = true;
                    }
                    parser.ReturnParameter.ReturnParameterSet(true);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }
        /// <summary>
        /// 清除数据位
        /// </summary>
        /// <param name="parser">参数解析</param>
        private void clear(ref OperationParameter.NodeParser parser)
        {
            BitmapIndex index = new BitmapIndex(Maps, ref parser);
            if (index.Map != null)
            {
                if (index.IsOperationClear()) parser.SetOperationReturnParameter();
                else parser.ReturnParameter.ReturnParameterSet(false);
            }
        }
        /// <summary>
        /// 设置数据位
        /// </summary>
        /// <param name="parser">参数解析</param>
        private void set(ref OperationParameter.NodeParser parser)
        {
            BitmapIndex index = new BitmapIndex(this, ref parser);
            if (index.Map != null)
            {
                if (index.IsOperationSet()) parser.SetOperationReturnParameterFalse();
                else parser.ReturnParameter.ReturnParameterSet(true);
            }
        }
        /// <summary>
        /// 数据位取反
        /// </summary>
        /// <param name="parser">参数解析</param>
        private void setNegate(ref OperationParameter.NodeParser parser)
        {
            BitmapIndex index = new BitmapIndex(this, ref parser);
            if (index.Map != null)
            {
                bool isBit = index.SetNegate() != 0;
                parser.ReturnParameter.ReturnParameterSet(isBit);
                parser.SetOperationType(isBit ? OperationParameter.OperationType.SetValue : OperationParameter.OperationType.Remove);
            }
        }
        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        internal override Cache.Node GetQueryNext(ref OperationParameter.NodeParser parser)
        {
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="parser">参数解析</param>
        internal override void QueryEnd(ref OperationParameter.NodeParser parser)
        {
            switch (parser.OperationType)
            {
                case OperationParameter.OperationType.GetValue:
                    BitmapIndex index = new BitmapIndex(Maps, ref parser);
                    if (index.Map != null) parser.ReturnParameter.ReturnParameterSet(index.Get() != 0);
                    return;
            }
            parser.ReturnParameter.ReturnType = ReturnType.OperationTypeError;
        }

        /// <summary>
        /// 创建缓存快照
        /// </summary>
        /// <returns></returns>
        internal unsafe override Snapshot.Node CreateSnapshot()
        {
            int count = Maps.Length;
            while (count != 0 && Maps[count - 1] == null) --count;
            if (count != 0)
            {
                byte[][] newMaps = new byte[count][];
                count = 0;
                foreach (byte[] map in Maps)
                {
                    byte[] newMap = new byte[map.Length];
                    System.Buffer.BlockCopy(map, 0, newMap, 0, map.Length);
                    newMaps[count] = newMap;
                    if (++count == newMaps.Length) break;
                }
                return new Snapshot.Value.Bitmap(newMaps);
            }
            return new Snapshot.Value.Bitmap(NullValue<byte[]>.Array);
        }
#if NOJIT
        /// <summary>
        /// 创建位图节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parser"></param>
        /// <returns></returns>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        private static Bitmap create(Cache.Node parent, ref OperationParameter.NodeParser parser)
        {
            return new Bitmap(parent, ref parser);
        }
#endif
        /// <summary>
        /// 节点信息
        /// </summary>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        private static readonly NodeInfo<Bitmap> nodeInfo;
        static Bitmap()
        {
            nodeInfo = new NodeInfo<Bitmap>
            {
#if NOJIT
                Constructor = (Constructor<Bitmap>)Delegate.CreateDelegate(typeof(Constructor<Bitmap>), typeof(Bitmap).GetMethod(CreateMethodName, BindingFlags.Static | BindingFlags.NonPublic, null, NodeConstructorParameterTypes, null))
#else
                Constructor = (Constructor<Bitmap>)AutoCSer.Emit.Constructor.CreateCache(typeof(Bitmap), NodeConstructorParameterTypes)
#endif
            };
        }
    }
}
