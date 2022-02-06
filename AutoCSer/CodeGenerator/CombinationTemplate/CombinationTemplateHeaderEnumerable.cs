using System;
using System.Collections.Generic;

namespace AutoCSer.CodeGenerator
{
    /// <summary>
    /// 自定义模板标记枚举
    /// </summary>
    internal sealed class CombinationTemplateHeaderEnumerable
    {
        /// <summary>
        /// 模板标记集合
        /// </summary>
        /// <returns></returns>
        private readonly LeftArray<string[][]> headerArray;
        /// <summary>
        /// 模板标记索引集合
        /// </summary>
        private readonly int[] indexArray;
        /// <summary>
        /// 模板代码片段数组
        /// </summary>
        private readonly string[] codeArray;
        /// <summary>
        /// 模板标记替代索引集合
        /// </summary>
        private readonly CombinationTemplateLink[] linkArray;
        /// <summary>
        /// 自定义模板标记枚举
        /// </summary>
        /// <param name="headerArray"></param>
        /// <param name="codeArray"></param>
        /// <param name="linkArray"></param>
        internal CombinationTemplateHeaderEnumerable(ref LeftArray<string[][]> headerArray, string[] codeArray, CombinationTemplateLink[] linkArray)
        {
            this.headerArray = headerArray;
            this.codeArray = codeArray;
            this.linkArray = linkArray;
            indexArray = new int[headerArray.Length];
        }
        /// <summary>
        /// 枚举代码
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<string> GetCode()
        {
            bool isTemplate = true;
            foreach (int _ in enumerable(0))
            {
                if (isTemplate) isTemplate = false;
                else
                {
                    foreach (CombinationTemplateLink link in linkArray)
                    {
                        codeArray[link.Index] = headerArray[link.Row][indexArray[link.Row]][link.Col];
                    }
                    yield return string.Concat(codeArray);
                }
            }
        }
        /// <summary>
        /// 组合枚举
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private IEnumerable<int> enumerable(int index)
        {
            int nextIndex = index + 1;
            foreach (int enumerableIndex in AutoCSer.Extensions.EnumerableExtension.Range(0, headerArray[index].Length))
            {
                indexArray[index] = enumerableIndex;
                if (nextIndex == headerArray.Length) yield return 0;
                else
                {
                    foreach (int _ in enumerable(nextIndex)) yield return 0;
                }
            }
        }
    }
}
