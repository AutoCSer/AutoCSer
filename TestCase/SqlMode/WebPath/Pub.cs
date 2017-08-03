using System;

namespace AutoCSer.TestCase.SqlModel.WebPath
{
    /// <summary>
    /// 公共全局 URL
    /// </summary>
    [AutoCSer.WebView.ClientType(Name = AutoCSer.WebView.PathAttribute.AutoCSerPubPathName, MemberName = null)]
    [AutoCSer.WebView.Path]
    public struct Pub
    {
        ///// <summary>
        ///// 404
        ///// </summary>
        //[AutoCSer.WebView.OutputAjax(IsIgnoreCurrent = true)]
        //public string NotFound404
        //{
        //    get { return "/404.html"; }
        //}
        /// <summary>
        /// 班级列表
        /// </summary>
        [AutoCSer.WebView.OutputAjax(IsIgnoreCurrent = true)]
        public string ClassList
        {
            get { return "/ClassList.html"; }
        }
    }
}
