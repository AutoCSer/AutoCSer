using System;
using AutoCSer.Extension;
using System.IO;

namespace AutoCSer.Web
{
    /// <summary>
    /// 主页重定向
    /// </summary>
    internal sealed class LocationIndex : AutoCSer.WebView.Call<LocationIndex>
    {
        /// <summary>
        /// 主页地址
        /// </summary>
        private static readonly LocationVersion locationIndex = new LocationVersion(@"/Index.html");
        /// <summary>
        /// 主页重定向
        /// </summary>
        [AutoCSer.WebView.CallMethod(FullName = "")]
        public void Load()
        {
            locationIndex.Location(this);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[AutoCSer.WebView.CallMethod(FullName = "RMB")]
        //public void RMB()
        //{
        //    using (System.Net.WebClient webClient = new System.Net.WebClient())
        //    {
        //        HttpResponse.SetContentType(AutoCSer.Net.Http.ResponseContentType.Txt);
        //        Response(webClient.DownloadData("http://www.chinamoney.com.cn/webdata/fe/rmb_fx_spot.json?t=" + Date.Now.Ticks.toString()));
        //    }
        //}
    }
}
