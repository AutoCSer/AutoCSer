using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// XML节点
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct Node
    {
        /// <summary>
        /// 属性集合
        /// </summary>
        private KeyValue<Range, Range>[] attributes;
        /// <summary>
        /// 子节点集合
        /// </summary>
        private KeyValue<SubString, Node>[] nodes;
        /// <summary>
        /// 子节点集合
        /// </summary>
        internal LeftArray<KeyValue<SubString, Node>> Nodes
        {
            get { return new LeftArray<KeyValue<SubString, Node>> { Array = nodes, Length = String.Length }; }
        }
        /// <summary>
        /// 字符串
        /// </summary>
        internal SubString String;
        /// <summary>
        /// 类型
        /// </summary>
        public NodeType Type { get; internal set; }
        /// <summary>
        /// 根据名称获取 XML 节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Node this[string name]
        {
            get
            {
                if (Type == NodeType.Node)
                {
                    int count = String.Length;
                    if (count != 0)
                    {
                        foreach (KeyValue<SubString, Node> node in nodes)
                        {
                            if (node.Key.Equals(name)) return node.Value;
                            if (--count == 0) break;
                        }
                    }
                }
                return default(Node);
            }
        }
        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetString(string value)
        {
            String.Set(value, 0, value.Length);
            Type = NodeType.String;
        }
        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetString(string value, int startIndex, int length)
        {
            String.Set(value, startIndex, length);
            Type = NodeType.String;
        }
        /// <summary>
        /// 设置子节点集合
        /// </summary>
        /// <param name="nodes"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetNode(ref LeftArray<KeyValue<SubString, Node>> nodes)
        {
            this.nodes = nodes.Array;
            String.Length = nodes.Length;
            Type = NodeType.Node;
        }
        /// <summary>
        /// 属性集合
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="attributes"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void SetAttribute(string xml, KeyValue<Range, Range>[] attributes)
        {
            String.String = xml;
            this.attributes = attributes;
        }
        /// <summary>
        /// 获取属性索引位置
        /// </summary>
        /// <param name="nameStart"></param>
        /// <param name="nameSize"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal unsafe bool GetAttribute(char* nameStart, int nameSize, ref Range index)
        {
            if (attributes != null)
            {
                foreach (KeyValue<Range, Range> attribute in attributes)
                {
                    if (attribute.Key.Length == nameSize)
                    {
                        fixed (char* xmlFixed = String.String)
                        {
                            if (AutoCSer.Memory.SimpleEqualNotNull((byte*)(xmlFixed + attribute.Key.StartIndex), (byte*)nameStart, nameSize << 1))
                            {
                                index = attribute.Value;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
