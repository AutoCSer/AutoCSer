using System;

namespace AutoCSer.Example.WebView.Template
{
    /// <summary>
    /// 数据作用域 测试
    /// </summary>
    [AutoCSer.WebView.View(IsPage = false)]
    partial class Value : AutoCSer.WebView.View<Value>
    {
        /// <summary>
        /// 外层作用域测试数据
        /// </summary>
        struct Test1
        {
            /// <summary>
            /// 数据定义
            /// </summary>
            public string String1;
        }
        /// <summary>
        /// 内层作用域测试数据
        /// </summary>
        class Test2
        {
            /// <summary>
            /// 数据定义
            /// </summary>
            public string String2;
        }

        /// <summary>
        /// 外层作用域测试数据
        /// </summary>
        Test1 Data1
        {
            get
            {
                return new Test1 { String1 = "我是外层数据" };
            }
        }
        Test2 Data2
        {
            get
            {
                return new Test2 { String2 = "我是内层数据" };
            }
        }
    }
}
