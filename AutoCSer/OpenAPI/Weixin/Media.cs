using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 上传图文消息素材结果
    /// </summary>
    public sealed class Media : Return
    {
        /// <summary>
        /// 媒体文件/图文消息上传后获取的唯一标识
        /// </summary>
        public string media_id;
        /// <summary>
        /// 媒体文件上传时间
        /// </summary>
        public long created_at;
        /// <summary>
        /// 媒体文件类型
        /// </summary>
        public MediaType type;
    }
}
