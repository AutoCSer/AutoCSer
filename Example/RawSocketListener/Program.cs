using System;
using System.Net;
using System.Threading;
using AutoCSer.Extension;

namespace AutoCSer.Example.RawSocketListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"http://www.AutoCSer.com/Index.html
");
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
                if (ips.Length == 0) Console.WriteLine("没有找到可用的本地 IP 地址");
                else
                {
                    int index = 0;
                    foreach (IPAddress ip in ips)
                    {
                        Console.WriteLine(index.toString() + " -> " + ip.ToString());
                        ++index;
                    }
                    string indexString = Console.ReadLine();
                    if (!int.TryParse(indexString, out index) || (uint)index >= ips.Length) index = 0;
                    Console.WriteLine(ips[index].ToString());

                    using (AutoCSer.Net.RawSocketListener.Listener socket = new AutoCSer.Net.RawSocketListener.Listener(ips[index], onPacket))
                    {
                        Thread.Sleep(3000);
                        if (socket.IsError) Console.WriteLine("套接字监听失败，可能需要管理员权限。");
                        else
                        {
                            Console.WriteLine(@"如果只能监听到本地发出的 TCP 数据包，而不是监听到本地接收的 TCP 数据包，可能需要配置防火墙策略或者彻底关闭防火墙。");
                            Console.WriteLine("Press quit to exit.");
                            while (Console.ReadLine() != "quit") ;
                            return;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
            Console.ReadKey();
        }

        /// <summary>
        /// 数据包处理
        /// </summary>
        /// <param name="buffer">数据包</param>
        private static void onPacket(AutoCSer.Net.RawSocketListener.Buffer buffer)
        {
            using (buffer)
            {
                AutoCSer.Net.Packet.Ip ip4 = buffer.Ip;
                if (ip4.IsPacket)
                {
                    switch (ip4.Protocol)
                    {
                        case AutoCSer.Net.Packet.Ip.ProtocolEnum.Icmp:
                            AutoCSer.Net.Packet.Icmp icmp = new AutoCSer.Net.Packet.Icmp(ip4.Packet);
                            if (icmp.IsPacket)
                            {
                                Console.WriteLine(ip4.Source.toHex() + " -> " + ip4.Destination.toHex() + " " + ip4.Protocol.ToString() + " " + icmp.Type.ToString() + " " + icmp.Code.ToString());
                            }
                            else Console.WriteLine("Unknown");
                            break;
                        case AutoCSer.Net.Packet.Ip.ProtocolEnum.Igmp:
                            AutoCSer.Net.Packet.Igmp igmp = new AutoCSer.Net.Packet.Igmp(ip4.Packet);
                            if (igmp.IsPacket)
                            {
                                Console.WriteLine(ip4.Source.toHex() + " -> " + ip4.Destination.toHex() + " " + ip4.Protocol.ToString());
                            }
                            else Console.WriteLine("Unknown");
                            break;
                        case AutoCSer.Net.Packet.Ip.ProtocolEnum.Tcp:
                            AutoCSer.Net.Packet.Tcp tcp = new AutoCSer.Net.Packet.Tcp(ip4.Packet);
                            if (tcp.IsPacket)
                            {
                                Console.WriteLine(ip4.Source.toHex() + ":" + ((ushort)tcp.SourcePort).toHex() + " -> " + ip4.Destination.toHex() + ":" + ((ushort)tcp.DestinationPort).toHex() + " " + ip4.Protocol.ToString());
                                //if (ip4.Destination == 0xb962c48b)
                                //{
                                //    SubArray<byte> data = tcp.Packet;
                                //    Console.WriteLine("+" + System.Text.Encoding.ASCII.GetString(data.BufferArray, data.StartIndex, data.Count) + "+");
                                //}
                                //if (ip4.Source == 0xb962c48b)
                                //{
                                //    SubArray<byte> data = tcp.Packet;
                                //    Console.WriteLine("-" + System.Text.Encoding.ASCII.GetString(data.BufferArray, data.StartIndex, data.Count) + "-");
                                //}
                            }
                            else Console.WriteLine("Unknown");
                            break;
                        case AutoCSer.Net.Packet.Ip.ProtocolEnum.Udp:
                            AutoCSer.Net.Packet.Udp udp = new AutoCSer.Net.Packet.Udp(ip4.Packet);
                            if (udp.IsPacket)
                            {
                                Console.WriteLine(ip4.Source.toHex() + ":" + ((ushort)udp.SourcePort).toHex() + " -> " + ip4.Destination.toHex() + ":" + ((ushort)udp.DestinationPort).toHex() + " " + ip4.Protocol.ToString());
                            }
                            else Console.WriteLine("Unknown");
                            break;
                        default:
                            Console.WriteLine(ip4.Source.toHex() + " -> " + ip4.Destination.toHex() + " " + ip4.Protocol.ToString());
                            break;
                    }
                }
                else Console.WriteLine("Unknown");
            }
        }
    }
}
