using System;

namespace AutoCSer
{
    /// <summary>
    /// 数据记录范围
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct FormatRange
    {
        /// <summary>
        /// 起始位置
        /// </summary>
        private int startIndex;
        /// <summary>
        /// 跳过记录数
        /// </summary>
        public int SkipCount
        {
            get { return startIndex; }
        }
        /// <summary>
        /// 结束位置
        /// </summary>
        private int endIndex;
        /// <summary>
        /// 结束位置
        /// </summary>
        public int EndIndex
        {
            get { return endIndex; }
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        public int GetCount
        {
            get { return endIndex - startIndex; }
        }

        /// <summary>
        /// 数据记录范围
        /// </summary>
        /// <param name="count">数据总量</param>
        /// <param name="skipCount">跳过记录数</param>
        /// <param name="getCount">获取记录数</param>
        public FormatRange(int count, int skipCount, int getCount)
        {
            count = count < 0 ? 0 : count;
            if (skipCount < count && getCount != 0)
            {
                if (getCount > 0)
                {
                    if (skipCount >= 0)
                    {
                        startIndex = skipCount;
                        if ((endIndex = skipCount + getCount) > count) endIndex = count;
                    }
                    else
                    {
                        startIndex = 0;
                        if ((endIndex = skipCount + getCount) > count) endIndex = count;
                        else if (endIndex < 0) endIndex = 0;
                    }
                }
                else
                {
                    startIndex = skipCount >= 0 ? skipCount : 0;
                    endIndex = count;
                }
            }
            else startIndex = endIndex = 0;
        }
    }
}
