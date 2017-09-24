using System;
#pragma warning disable

namespace AutoCSer.Net.Packet
{
    /// <summary>
    /// 数据链接层帧类型
    /// </summary>
    public enum Frame : ushort
    {
        Arp = 0x806,
        IpV4 = 0x800,
        IpV6 = 0x86dd,
        ReverseArp = 0x8035,

        AppleTalk = 0x809b,
        AppleTalkArp = 0x80f3,
        AtaOverEthernet = 0x88a2,
        AvbTransportProtocol = 0x88b5,
        CircuitEmulationServicesOverEthernet = 0x88d8,
        CobraNet = 0x8819,
        ConnectivityFaultManagementOrOperationsAdministrationManagement = 0x8902,
        Echo = 0x200,
        EtherCatProtocol = 0x88a4,
        ExtensibleAuthenticationProtocolOverLan = 0x888e,
        FibreChannelOverEthernet = 0x8906,
        FibreChannelOverEthernetInitializationProtocol = 0x8914,
        HomePlug = 0x88e1,
        HyperScsi = 0x889a,
        LLDP = 0x88cc,
        Loop = 0x60,
        MacControl = 0x8808,
        MacSecurity = 0x88e5,
        MultiprotocolLabelSwitchingMulticast = 0x8848,
        MultiprotocolLabelSwitchingUnicast = 0x8847,
        None = 0,
        Novell = 0x8138,
        NovellInternetworkPacketExchange = 0x8137,
        PointToPointProtocolOverEthernetDiscoveryStage = 0x8863,
        PointToPointProtocolOverEthernetSessionStage = 0x8864,
        PrecisionTimeProtocol = 0x88f7,
        ProviderBridging = 0x88a8,
        QInQ = 0x9100,
        SerialRealTimeCommunicationSystemIii = 0x88cd,
        VeritasLowLatencyTransport = 0xcafe,
        VLanTaggedFrame = 0x8100,
    }
}
