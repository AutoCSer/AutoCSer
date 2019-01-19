using System;
using System.Collections.Generic;
using AutoCSer.Extension;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 拓扑排序
    /// </summary>
    public static class TopologySort
    {
        /// <summary>
        /// 拓扑排序器
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct Sorter<valueType>
        {
            /// <summary>
            /// 图
            /// </summary>
            private Dictionary<valueType, ListArray<valueType>> graph;
            /// <summary>
            /// 排序结果
            /// </summary>
            private valueType[] values;
            /// <summary>
            /// 当前排序位置
            /// </summary>
            private int index;
            /// <summary>
            /// 是否反向排序
            /// </summary>
            private bool isDesc;
            /// <summary>
            /// 拓扑排序器
            /// </summary>
            /// <param name="graph">图</param>
            /// <param name="points">单点集合</param>
            /// <param name="isDesc">是否反向排序</param>
            public Sorter(Dictionary<valueType, ListArray<valueType>> graph, ref LeftArray<valueType> points, bool isDesc)
            {
                this.graph = graph;
                this.isDesc = isDesc;
                values = new valueType[graph.Count + points.Length];
                if (isDesc)
                {
                    index = points.Length;
                    points.CopyTo(values, 0);
                }
                else points.CopyTo(values, index = graph.Count);
            }
            /// <summary>
            /// 拓扑排序
            /// </summary>
            /// <returns>排序结果</returns>
            public valueType[] Sort()
            {
                ListArray<valueType> points;
                if (isDesc)
                {
                    foreach (valueType point in graph.getArray(value => value.Key))
                    {
                        if (graph.TryGetValue(point, out points))
                        {
                            graph[point] = null;
                            foreach (valueType nextPoint in points) popDesc(nextPoint);
                            graph.Remove(point);
                            values[index++] = point;
                        }
                    }
                }
                else
                {
                    foreach (valueType point in graph.getArray(value => value.Key))
                    {
                        if (graph.TryGetValue(point, out points))
                        {
                            graph[point] = null;
                            foreach (valueType nextPoint in points) pop(nextPoint);
                            graph.Remove(point);
                            values[--index] = point;
                        }
                    }
                }
                return values;
            }
            /// <summary>
            /// 排序子节点
            /// </summary>
            /// <param name="point">子节点</param>
            private void pop(valueType point)
            {
                ListArray<valueType> points;
                if (graph.TryGetValue(point, out points))
                {
                    if (points == null) throw new OverflowException("拓扑排序循环");
                    graph[point] = null;
                    foreach (valueType nextPoint in points) pop(nextPoint);
                    graph.Remove(point);
                    values[--index] = point;
                }
            }
            /// <summary>
            /// 排序子节点
            /// </summary>
            /// <param name="point">子节点</param>
            private void popDesc(valueType point)
            {
                ListArray<valueType> points;
                if (graph.TryGetValue(point, out points))
                {
                    if (points == null) throw new OverflowException("拓扑排序循环");
                    graph[point] = null;
                    foreach (valueType nextPoint in points) popDesc(nextPoint);
                    graph.Remove(point);
                    values[index++] = point;
                }
            }
        }
        /// <summary>
        /// 拓扑排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="edges">边集合</param>
        /// <param name="points">无边点集合</param>
        /// <param name="isDesc">是否反向排序</param>
        /// <returns>排序结果</returns>
        public static valueType[] Sort<valueType>(ICollection<KeyValue<valueType, valueType>> edges, HashSet<valueType> points, bool isDesc = false)
        {
            if (edges.count() == 0) return points.getArray();
            Dictionary<valueType, ListArray<valueType>> graph = DictionaryCreator.CreateAny<valueType, ListArray<valueType>>();
            if (points == null) points = HashSetCreator.CreateAny<valueType>();
            ListArray<valueType> values;
            foreach (KeyValue<valueType, valueType> edge in edges)
            {
                if (!graph.TryGetValue(edge.Key, out values)) graph.Add(edge.Key, values = new ListArray<valueType>());
                values.Add(edge.Value);
                points.Add(edge.Value);
            }
            LeftArray<valueType> pointList = new LeftArray<valueType>(points.Count);
            foreach (valueType point in points)
            {
                if (!graph.ContainsKey(point)) pointList.UnsafeAdd(point);
            }
            return new Sorter<valueType>(graph, ref pointList, isDesc).Sort();
        }
    }
}
