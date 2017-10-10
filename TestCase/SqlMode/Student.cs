using System;

namespace AutoCSer.TestCase.SqlModel
{
    /// <summary>
    /// 学生数据定义
    /// </summary>
    [AutoCSer.WebView.ClientType(PrefixName = Pub.WebViewClientTypePrefixName)]
    [AutoCSer.Sql.Model(CacheType = AutoCSer.Sql.Cache.Whole.Event.Type.IdentityArray, LogServerName = Pub.LogServerName, IsLogSerializeReferenceMember = false)]
    public partial class Student
    {
        /// <summary>
        /// 学生标识（默认自增）
        /// </summary>
        [AutoCSer.WebView.OutputAjax]
        [AutoCSer.Net.TcpStaticServer.RemoteKey]
        public int Id;
        /// <summary>
        /// 电子邮箱（关键字）
        /// </summary>
        [AutoCSer.Sql.Member(PrimaryKeyIndex = 1, MaxStringLength = 256, IsAscii = true)]
        public string Email;
        /// <summary>
        /// 密码
        /// </summary>
        [AutoCSer.Sql.Member(MaxStringLength = 32)]
        public string Password;
        /// <summary>
        /// 头像
        /// </summary>
        [AutoCSer.Sql.Member(MaxStringLength = 256, IsAscii = true)]
        [AutoCSer.WebView.OutputAjax(BindingName = "Gender")]
        //[AutoCSer.WebView.OutputAjax(BindingName = nameof(Gender))] VS2010 不支持
        public string Image = string.Empty;
        /// <summary>
        /// 姓名
        /// </summary>
        [AutoCSer.Sql.Member(MaxStringLength = 64)]
        public string Name;
        /// <summary>
        /// 出生日期（自定义字段）
        /// </summary>
        public AutoCSer.Sql.Member.IntDate Birthday;
        /// <summary>
        /// 性别（自定义 enum 字段）
        /// </summary>
        [AutoCSer.Sql.Member]
        public Member.Gender Gender;
        /// <summary>
        /// 按加入时间排序的班级集合（不可识别的字段映射为 JSON 字符串）
        /// </summary>
        [AutoCSer.Sql.Member(MaxStringLength = 256, IsAscii = true, IsMemberIndex = true)]
        public Member.ClassDate[] Classes;
        /// <summary>
        /// 当前班级标识
        /// </summary>
        public int ClassId
        {
            get
            {
                if (Classes == null) return 0;
                return Classes[Classes.Length - 1].ClassId;
            }
        }
    }
}
