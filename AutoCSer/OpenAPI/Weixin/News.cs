using System;

namespace AutoCSer.OpenAPI.Weixin
{
    /// <summary>
    /// 修改永久图文素材
    /// </summary>
    public sealed class News
    {
        /// <summary>
        /// 要修改的图文消息的id
        /// </summary>
        public string media_id;
        /// <summary>
        /// 要更新的文章在图文消息中的位置（多图文消息时，此字段才有意义），第一篇为0
        /// </summary>
        public int index;
        /// <summary>
        /// 图文消息素材
        /// </summary>
        public Article articles;
    }
}
