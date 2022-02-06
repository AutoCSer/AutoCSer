using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 预览消息
    /// </summary>
    public sealed class PreviewMessage : MessageBase
    {
        /// <summary>
        /// 微信号,优先于touser
        /// </summary>
        public string towxname;
        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string touser;
        /// <summary>
        /// 消息类型
        /// </summary>
        public BulkMessageType msgtype;
        /// <summary>
        /// 图文消息
        /// </summary>
        public MediaMessage mpnews;
        /// <summary>
        /// 视频
        /// </summary>
        public MediaMessage mpvideo;
        /// <summary>
        /// 卡券
        /// </summary>
        public CardMessage wxcard;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        private void toJson(AutoCSer.JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(towxname))
            {
                serializer.CustomWriteFirstName("touser");
                serializer.CallSerialize(touser);
            }
            else
            {
                serializer.CustomWriteFirstName("towxname");
                serializer.CallSerialize(towxname);
            }
            serializer.CustomWriteNextName("msgtype");
            serializer.CallSerialize(msgtype.ToString());
            serializer.CustomWriteNextName(msgtype.ToString());
            switch (msgtype)
            {
                case BulkMessageType.text:
                    serializer.TypeSerialize(ref text);
                    break;
                case BulkMessageType.image:
                    serializer.TypeSerialize(ref image);
                    break;
                case BulkMessageType.voice:
                    serializer.TypeSerialize(ref voice);
                    break;
                case BulkMessageType.mpvideo:
                    serializer.TypeSerialize(ref mpvideo);
                    break;
                case BulkMessageType.mpnews:
                    serializer.TypeSerialize(ref mpnews);
                    break;
                case BulkMessageType.wxcard:
                    serializer.TypeSerialize(ref wxcard);
                    break;
            }
            serializer.CustomObjectEnd();
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.JsonSerializeCustom]
        private static void toJson(AutoCSer.JsonSerializer serializer, PreviewMessage value)
        {
            value.toJson(serializer);
        }
    }
}
