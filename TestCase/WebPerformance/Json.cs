using System;
using System.Text;

namespace AutoCSer.TestCase.WebPerformance
{
    /// <summary>
    /// JSON 调用测试
    /// </summary>
    [AutoCSer.WebView.Call]
    internal sealed class Json : AutoCSer.WebView.CallAsynchronous<Json>
    {
        /// <summary>
        /// JSON 测试数据
        /// </summary>
        private static readonly Message message = new Message();

        /// <summary>
        /// JSON 调用测试
        /// </summary>
        [AutoCSer.WebView.CallMethod(IsOnlyPost = false, FullName = @"/json")]
        public void GetMessage()
        {
            RepsonseEndJsonSerialize(message);
        }
    }
}
