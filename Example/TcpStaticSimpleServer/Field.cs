using System;

namespace AutoCSer.Example.TcpStaticSimpleServer
{
    /// <summary>
    /// 字段支持 示例
    /// </summary>
    [AutoCSer.Net.TcpStaticSimpleServer.Server(Name = ServerName.Example1)]
    partial class Field
    {
        /// <summary>
        /// 只读字段支持
        /// </summary>
        [AutoCSer.Net.TcpStaticSimpleServer.Method]
        static int GetField;

        /// <summary>
        /// 可写字段支持
        /// </summary>
        [AutoCSer.Net.TcpStaticSimpleServer.Method(IsOnlyGetMember = false)]
        static int SetField;

        /// <summary>
        /// 字段支持 测试
        /// </summary>
        /// <returns></returns>
        internal static bool TestCase()
        {
            GetField = 2;
            AutoCSer.Net.TcpServer.ReturnValue<int> value = AutoCSer.Example.TcpStaticSimpleServer.TcpCallSimple.Field.GetField;
            if (value.Type != AutoCSer.Net.TcpServer.ReturnType.Success || value.Value != 2)
            {
                return false;
            }

            SetField = 0;
            AutoCSer.Example.TcpStaticSimpleServer.TcpCallSimple.Field.SetField = 3;
            if (SetField != 3)
            {
                return false;
            }

            return true;
        }
    }
}
