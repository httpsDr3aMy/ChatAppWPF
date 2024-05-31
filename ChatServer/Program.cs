using System;
using System.Net.Sockets;
using System.Net;
using ChatServer.Net.IO;

namespace ChatServer
{
    internal class Program
    {
        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);
                BroadcastConnection();
            }
        }

        static void BroadcastConnection()
        {
            foreach(var user in _users)
            {
                foreach(var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOPCode(1);
                    broadcastPacket.WriteMessage(usr.Username);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach(var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOPCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }
        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            foreach (var user in _users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOPCode(10);
                msgPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
            BroadcastMessage($"[{disconnectedUser.Username}] Disconnected!");
        }
    }
}