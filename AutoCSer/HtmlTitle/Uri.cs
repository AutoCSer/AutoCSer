using System;
using System.Text;
using AutoCSer.Extension;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.HtmlTitle
{
    /// <summary>
    /// Uri 与回调函数信息
    /// </summary>
    internal sealed class Uri : AutoCSer.Threading.Link<Uri>
    {
        /// <summary>
        /// Uri字符串
        /// </summary>
        internal string UriString;
        /// <summary>
        /// Uri
        /// </summary>
        internal SubArray<byte> UriBytes;
        /// <summary>
        /// 获取HTML标题回调函数
        /// </summary>
        private Action<string> onGet;
        /// <summary>
        /// 默认编码格式
        /// </summary>
        internal Encoding Encoding;
        /// <summary>
        /// 获取Uri与回调函数信息
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="onGet">获取HTML标题回调函数</param>
        /// <param name="encoding">默认编码格式</param>
        /// <returns>Uri与回调函数信息</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(string uri, Action<string> onGet, Encoding encoding)
        {
            UriString = uri;
            this.onGet = onGet;
            this.Encoding = encoding;
        }
        /// <summary>
        /// 获取Uri与回调函数信息
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="onGet">获取HTML标题回调函数</param>
        /// <param name="encoding">默认编码格式</param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public void Set(ref SubArray<byte> uri, Action<string> onGet, Encoding encoding)
        {
            this.UriBytes = uri;
            this.onGet = onGet;
            this.Encoding = encoding;
        }
        /// <summary>
        /// 取消调用
        /// </summary>
        public void Cancel()
        {
            Action<string> onGet = this.onGet;
            UriString = null;
            UriBytes.SetNull();
            LinkNext = null;
            this.onGet = null;
            AutoCSer.Threading.RingPool<Uri>.Default.PushNotNull(this);
            onGet(null);
        }
        /// <summary>
        /// 获取HTML标题回调
        /// </summary>
        /// <param name="title">HTML标题</param>
        internal void Callback(string title)
        {
            Action<string> onGet = this.onGet;
            UriString = null;
            UriBytes.SetNull();
            LinkNext = null;
            this.onGet = null;
            AutoCSer.Threading.RingPool<Uri>.Default.PushNotNull(this);
            onGet(title);
        }
        /// <summary>
        /// 取消调用
        /// </summary>
        /// <param name="log"></param>
        internal void CancelQueue(AutoCSer.Log.ILog log)
        {
            Uri value = this;
            do
            {
                Uri nextValue = value.LinkNext;
                try
                {
                    value.Cancel();
                }
                catch (Exception error)
                {
                    log.Add(Log.LogType.Error, error);
                }
                value = nextValue;
            }
            while (value != null);
        }
    }
}
