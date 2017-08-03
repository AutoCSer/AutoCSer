using System;

namespace AutoCSer.Web
{
    /// <summary>
    /// WEB服务器
    /// </summary>
    public partial class WebServer
    {
        /// <summary>
        /// 是否采用静态文件缓存控制策略
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected unsafe override bool isStaticFileCacheControl(SubArray<byte> path)
        {
            fixed (byte* pathFixed = path.BufferArray)
            {
                byte* pathStart = pathFixed + path.StartIndex;
                return (*(int*)pathStart | 0x202000) == '/' + ('j' << 8) + ('s' << 16) + ('/' << 24)
                    //|| (*(int*)(pathStart + 1) | 0x202020) == 'c' + ('s' << 8) + ('s' << 16) + ('/' << 24)
                    || ((*(int*)(pathStart + (path.Count - 3)) & 0xffffff) | 0x202000) == '.' + ('j' << 8) + ('s' << 16);
            }
        }
    }
}
