using System;
using System.Collections.Generic;

namespace AutoCSer.Example.WebView.Template
{
    /// <summary>
    /// 循环数据作用域 测试
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false)]
    partial class Loop : AutoCSer.WebView.View<Loop>
    {
        /// <summary>
        /// 测试数据
        /// </summary>
        struct TestData
        {
            /// <summary>
            /// 测试数据
            /// </summary>
            public int Value;
        }
        IEnumerable<TestData> LoopData
        {
            get
            {
                yield return new TestData { Value = 1 };
                yield return new TestData { Value = 2 };
            }
        }
    }
}
