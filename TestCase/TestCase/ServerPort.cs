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

        TcpInternalSimpleServer_Session = 12020,
        TcpInternalSimpleServer_Member = 12021,
        TcpInternalSimpleServer_Json = 12022,
        TcpStaticSimpleServer_Session = 12023,
        TcpStaticSimpleServer_Member = 12024,
        TcpStaticSimpleServer_Json = 12025,
        TcpOpenSimpleServer_Session = 12026,
        TcpOpenSimpleServer_Member = 12027,
        TcpOpenSimpleServer_Json = 12028,
        TcpInternalSimpleServer_Emit = 12029,
        TcpOpenSimpleServer_Emit = 12030,

        TcpInternalStreamServer_Session = 12040,
        TcpInternalStreamServer_Member = 12041,
        TcpInternalStreamServer_Json = 12042,
        TcpStaticStreamServer_Session = 12043,
        TcpStaticStreamServer_Member = 12044,
        TcpStaticStreamServer_Json = 12045,
        TcpOpenStreamServer_Session = 12046,
        TcpOpenStreamServer_Member = 12047,
        TcpOpenStreamServer_Json = 12048,
        TcpInternalStreamServer_Emit = 12049,
        TcpOpenStreamServer_Emit = 12050,

        TcpInternalServerPerformance = 12100,
        TcpOpenServerPerformance = 12101,
        TcpInternalServerPerformance_Emit = 12102,
        TcpOpenServerPerformance_Emit = 12103,
        TcpInternalSimpleServerPerformance = 12104,
        TcpOpenSimpleServerPerformance = 12105,
        TcpInternalSimpleServerPerformance_Emit = 12106,
        TcpOpenSimpleServerPerformance_Emit = 12107,
        TcpInternalStreamServerPerformance_Synchronous = 12108,
        TcpInternalStreamServerPerformance_TcpQueue = 12109,
        TcpInternalStreamServerPerformance_Queue = 12110,
        TcpOpenStreamServerPerformance_Synchronous = 12111,
        TcpOpenStreamServerPerformance_TcpQueue = 12112,
        TcpOpenStreamServerPerformance_Queue = 12113,
        TcpInternalStreamServerPerformance_Synchronous_Emit = 12114,
        TcpInternalStreamServerPerformance_TcpQueue_Emit = 12115,
        TcpInternalStreamServerPerformance_Queue_Emit = 12116,
        TcpOpenStreamServerPerformance_Synchronous_Emit = 12117,
        TcpOpenStreamServerPerformance_TcpQueue_Emit = 12118,
        TcpOpenStreamServerPerformance_Queue_Emit = 12119,
        Nuget_Emit = 12120,

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

        Example_TcpInternalSimpleServer_NoAttribute = 13100,
        Example_TcpInternalSimpleServer_Static = 13101,
        Example_TcpInternalSimpleServer_Field = 13102,
        Example_TcpInternalSimpleServer_Property = 13103,
        Example_TcpInternalSimpleServer_RefOut = 13104,
        Example_TcpInternalSimpleServer_Asynchronous = 13105,

        Example_TcpStaticSimpleServer_1 = 13200,
        Example_TcpStaticSimpleServer_2 = 13201,

        Example_TcpOpenSimpleServer_NoAttribute = 13300,
        Example_TcpOpenSimpleServer_Static = 13301,
        Example_TcpOpenSimpleServer_Field = 13302,
        Example_TcpOpenSimpleServer_Property = 13303,
        Example_TcpOpenSimpleServer_RefOut = 13304,
        Example_TcpOpenSimpleServer_Asynchronous = 13305,

        Example_TcpInterfaceSimpleServer_RefOut = 13400,
        Example_TcpInterfaceSimpleServer_Inherit = 13401,

        Example_TcpInterfaceOpenSimpleServer_RefOut = 13500,
        Example_TcpInterfaceOpenSimpleServer_Inherit = 13501,

        Example_TcpInternalStreamServer_NoAttribute = 13600,
        Example_TcpInternalStreamServer_Static = 13601,
        Example_TcpInternalStreamServer_Field = 13602,
        Example_TcpInternalStreamServer_Property = 13603,
        Example_TcpInternalStreamServer_RefOut = 13604,
        Example_TcpInternalStreamServer_ClientAsynchronous = 13605,
        Example_TcpInternalStreamServer_SendOnly = 13606,
        Example_TcpInternalStreamServer_ClientTaskAsync = 13607,

        Example_TcpOpenStreamServer_NoAttribute = 13700,
        Example_TcpOpenStreamServer_Static = 13701,
        Example_TcpOpenStreamServer_Field = 13702,
        Example_TcpOpenStreamServer_Property = 13703,
        Example_TcpOpenStreamServer_RefOut = 13704,
        Example_TcpOpenStreamServer_ClientAsynchronous = 13705,
        Example_TcpOpenStreamServer_SendOnly = 13706,
        Example_TcpOpenStreamServer_ClientTaskAsync = 13707,

        Example_TcpInterfaceStreamServer_RefOut = 13800,
        Example_TcpInterfaceStreamServer_SendOnly = 13801,
        Example_TcpInterfaceStreamServer_Inherit = 13802,

        Example_WebView = 14000,
    }
}
