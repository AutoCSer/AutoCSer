using System;

namespace AutoCSer.TestCase
{
    /// <summary>
    /// TCP 服务端口号
    /// </summary>
    internal enum ServerPort
    {
        TcpInternalServer_Session = 12000,
        TcpInternalServer_Member = 12001,
        TcpInternalServer_Json = 12002,
        TcpStaticServer_Session = 12003,
        TcpStaticServer_Member = 12004,
        TcpStaticServer_Json = 12005,
        TcpOpenServer_Session = 12006,
        TcpOpenServer_Member = 12007,
        TcpOpenServer_Json = 12008,
        TcpInternalServer_Emit = 12009,
        TcpOpenServer_Emit = 12010,

        TcpInternalServerPerformance = 12100,
        TcpOpenServerPerformance = 12101,
        TcpInternalServerPerformance_Emit = 12102,
        TcpOpenServerPerformance_Emit = 12103,
        Nuget_Emit = 12104,

        HttpFilePerformance = 12200,
        WebPerformance = 12201,

        SqlTableDataReader = 12300,
        SqlTableDataWriter = 12301,
        SqlTableDataLog = 12302,
        SqlTableWeb = 12303,

        ChatServer = 12400,

        DocumentWeb = 12500,

        Example_TcpInterfaceServer_RefOut = 12600,
        Example_TcpInterfaceServer_SendOnly = 12601,
        Example_TcpInterfaceServer_Asynchronous = 12602,
        Example_TcpInterfaceServer_KeepCallback = 12603,
        Example_TcpInterfaceServer_Inherit = 12604,

        Example_TcpInterfaceOpenServer_RefOut = 12700,
        Example_TcpInterfaceOpenServer_SendOnly = 12701,
        Example_TcpInterfaceOpenServer_Asynchronous = 12702,
        Example_TcpInterfaceOpenServer_KeepCallback = 12703,
        Example_TcpInterfaceOpenServer_Inherit = 12704,

        Example_TcpStaticServer_1 = 12800,
        Example_TcpStaticServer_2 = 12801,

        Example_TcpInternalServer_NoAttribute = 12900,
        Example_TcpInternalServer_Static = 12901,
        Example_TcpInternalServer_Field = 12902,
        Example_TcpInternalServer_Property = 12903,
        Example_TcpInternalServer_RefOut = 12904,
        Example_TcpInternalServer_ClientAsynchronous = 12905,
        Example_TcpInternalServer_SendOnly = 12906,
        Example_TcpInternalServer_Asynchronous = 12907,
        Example_TcpInternalServer_KeepCallback = 12908,
        Example_TcpInternalServer_ClientTaskAsync = 12909,

        Example_TcpOpenServer_NoAttribute = 13000,
        Example_TcpOpenServer_Static = 13001,
        Example_TcpOpenServer_Field = 13002,
        Example_TcpOpenServer_Property = 13003,
        Example_TcpOpenServer_RefOut = 13004,
        Example_TcpOpenServer_ClientAsynchronous = 13005,
        Example_TcpOpenServer_SendOnly = 13006,
        Example_TcpOpenServer_Asynchronous = 13007,
        Example_TcpOpenServer_KeepCallback = 13008,
        Example_TcpOpenServer_ClientTaskAsync = 13009,

        Example_WebView = 14000,
    }
}
