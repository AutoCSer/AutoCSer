using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 客服消息
    /// </summary>
    public sealed class Message : MessageBase
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string touser;
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType msgtype;
        /// <summary>
        /// 视频消息
        /// </summary>
        public VideoMessage video;
        /// <summary>
        /// 音乐消息
        /// </summary>
        public MusicMessage music;
        /// <summary>
        /// 图文消息
        /// </summary>
        public NewsMessage news;
        /// <summary>
        /// 卡券
        /// </summary>
        public CardMessage wxcard;
        /// <summary>
        /// 以某个客服帐号来发消息
        /// </summary>
        public MessageAccount customservice;
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        private void toJson(AutoCSer.Json.Serializer serializer)
        {
            serializer.CustomWriteFirstName("touser");
            serializer.CallSerialize(touser);
            serializer.CustomWriteNextName("msgtype");
            serializer.CallSerialize(msgtype.ToString());
            serializer.CustomWriteNextName(msgtype.ToString());
            switch (msgtype)
            {
                case MessageType.news:
                    serializer.TypeSerialize(ref news);
                    break;
                case MessageType.text:
                    serializer.TypeSerialize(ref text);
                    break;
                case MessageType.image:
                    serializer.TypeSerialize(ref image);
                    break;
                case MessageType.voice:
                    serializer.TypeSerialize(ref voice);
                    break;
                case MessageType.video:
                    serializer.TypeSerialize(ref video);
                    break;
                case MessageType.music:
                    serializer.TypeSerialize(ref music);
                    break;
                case MessageType.wxcard:
                    serializer.TypeSerialize(ref wxcard);
                    break;
            }
            if (customservice.kf_account != null)
            {
                serializer.CustomWriteNextName("customservice");
                serializer.TypeSerialize(ref customservice);
            }
            serializer.CustomObjectEnd();
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [AutoCSer.IOS.Preserve(Conditional = true)]
        [AutoCSer.Json.SerializeCustom]
        private static void toJson(AutoCSer.Json.Serializer serializer, Message value)
        {
            value.toJson(serializer);
        }
    }
}
