using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 群发消息
    /// </summary>
    public sealed class BulkMessage : BulkMessageBase
    {
        /// <summary>
        /// 用于设定图文消息的接收者
        /// </summary>
        public BulkMessageFilter filter;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.JsonSerializeCustom]
        private static void toJson(AutoCSer.JsonSerializer serializer, BulkMessage value)
        {
            serializer.CustomWriteFirstName("filter");
            serializer.TypeSerialize(ref value.filter);
            value.toJson(serializer);
        }
    }
}
