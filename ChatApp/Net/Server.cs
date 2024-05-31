using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ChatApp.Net.IO;

namespace ChatApp.Net
{
    class Server
    {
        TcpClient _client;
        PacketBuilder _packetBuilder;
        public Server()
        {
            _client = new TcpClient();
        }
        public void ConnectToServer(string username)
        {
            if(!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7000);
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOPCode(0);
                connectPacket.WriteString(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }
        }
    }
}
