using System;

namespace AutoCSer
{
    /// <summary>
    /// 分页记录范围
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct PageCount
    {
        /// <summary>
        /// 数据总量
        /// </summary>
        private int count;
        /// <summary>
        /// 分页尺寸
        /// </summary>
        private int pageSize;
        /// <summary>
        /// 分页总数
        /// </summary>
        private int pageCount;
        /// <summary>
        /// 当前页号
        /// </summary>
        private int currentPage;
        /// <summary>
        /// 跳过记录数
        /// </summary>
        private int skipCount;
        /// <summary>
        /// 跳过记录数
        /// </summary>
        public int SkipCount
        {
            get { return skipCount; }
        }
        /// <summary>
        /// 逆序跳过记录数
        /// </summary>
        public int DescSkipCount
        {
            get { return count - skipCount - currentPageSize; }
        }
        /// <summary>
        /// 当前页记录数
        /// </summary>
        private int currentPageSize;
        /// <summary>
        /// 当前页记录数
        /// </summary>
        public int CurrentPageSize
        {
            get { return currentPageSize; }
        }
        /// <summary>
        /// 分页记录范围
        /// </summary>
        /// <param name="count">数据总量</param>
        /// <param name="pageSize">分页尺寸</param>
        /// <param name="currentPage">页号</param>
        internal PageCount(int count, int pageSize, int currentPage)
        {
            this.pageSize = pageSize > 0 ? pageSize : 10;
            pageCount = (count + this.pageSize - 1) / this.pageSize;
            this.count = count;
            this.currentPage = Math.Max(currentPage <= pageCount ? currentPage : pageCount, 1);
            skipCount = (this.currentPage - 1) * this.pageSize;
            currentPageSize = Math.Min(skipCount + this.pageSize, count) - skipCount;
        }
    }
}
