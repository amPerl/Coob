using Coob.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Coob
{
    public class Client
    {
        public bool Joined;
        public NetReader Reader;
        public BinaryWriter Writer;
        public NetworkStream NetStream;
        public long ID;
        public Entity Entity;
        public string IP;

        TcpClient tcp;
        byte[] recvBuffer;

        public Client(TcpClient tcpClient)
        {
            Joined = false;
            Entity = null;

            tcp = tcpClient;
            IP = tcp.Client.RemoteEndPoint.ToString().Split(':')[0];
            NetStream = tcp.GetStream();
            Reader = new NetReader(NetStream);
            Writer = new BinaryWriter(NetStream);

            recvBuffer = new byte[4];
            NetStream.BeginRead(recvBuffer, 0, 4, idCallback, null);
        }

        void idCallback(IAsyncResult result)
        {
            if (!tcp.Connected)
            {
                Disconnect("Connection reset by peer.");
                return;
            }

            int bytesRead = 0;
            try
            {
                bytesRead = NetStream.EndRead(result);

                if (bytesRead == 4)
                {
                    Root.Coob.HandleRecvPacket(BitConverter.ToInt32(recvBuffer, 0), this);
                }
                NetStream.BeginRead(recvBuffer, 0, 4, idCallback, null);
            }
            catch { Disconnect("Read error."); }
        }

        public void Disconnect(string reason = "")
        {
            Joined = false;
            Root.JavaScript.Engine.CallFunction("onClientDisconnect", this);

            Log.WriteInfo("Client " + ID + " disconnected (" + reason + ").");
            tcp.Close();

            if (Root.Coob.Clients.Contains(this))
            {
                Root.Coob.Clients.Remove(this);
                Log.WriteInfo("Clients count: " + Root.Coob.Clients.Count);
            }
        }

        public void SendMessage(long id, string message)
        {
            byte[] msgBuffer = Encoding.Unicode.GetBytes(message);
            int msgLength = msgBuffer.Length / 2;

            Writer.Write(10);
            Writer.Write(id);
            Writer.Write(msgLength);
            Writer.Write(msgBuffer);
        }

        public void SendServerMessage(string message)
        {
            SendMessage(0, message);
        }
    }
}
