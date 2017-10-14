using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 群发消息
    /// </summary>
    public abstract class BulkMessageBase : MessageBase
    {
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
        /// 卡券消息
        /// </summary>
        public BulkCardMessage wxcard;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        protected void toJson(AutoCSer.Json.Serializer serializer)
        {
            serializer.CustomWriteNextName("msgtype");
            serializer.CustomSerialize(msgtype.ToString());
            serializer.CustomWriteNextName(msgtype.ToString());
            switch (msgtype)
            {
                case BulkMessageType.mpnews:
                    serializer.CustomSerialize(mpnews);
                    break;
                case BulkMessageType.text:
                    serializer.CustomSerialize(text);
                    break;
                case BulkMessageType.image:
                    serializer.CustomSerialize(image);
                    break;
                case BulkMessageType.voice:
                    serializer.CustomSerialize(voice);
                    break;
                case BulkMessageType.mpvideo:
                    serializer.CustomSerialize(mpvideo);
                    break;
                case BulkMessageType.wxcard:
                    serializer.CustomSerialize(wxcard);
                    break;
            }
            serializer.CustomObjectEnd();
        }
    }
}
