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
        /// JSON 字符流
        /// </summary>
        private readonly CharStream jsonStream;
        /// <summary>
        /// JSON 序列化
        /// </summary>
        private readonly AutoCSer.Json.Serializer jsonSerializer;
        /// <summary>
        /// JSON 调用测试
        /// </summary>
        public Json()
        {
            jsonSerializer = AutoCSer.Json.Serializer.CreateDefault(out jsonStream);
        }
        /// <summary>
        /// JSON 调用测试
        /// </summary>
        [AutoCSer.WebView.CallMethod(IsOnlyPost = false, FullName = @"/json")]
        public void GetMessage()
        {
            jsonStream.Clear();
            jsonSerializer.TypeSerialize(message);
            RepsonseEnd(jsonStream);
        }
    }
}
