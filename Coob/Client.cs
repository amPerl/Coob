using Coob.Exceptions;
using Coob.Packets;
using Coob.Structures;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Coob.CoobEventArgs;

namespace Coob
{
    public class Client
    {
        public bool Joined;
        public NetReader Reader;
        public BinaryWriter Writer;
        public NetworkStream NetStream;
        public ulong Id { get; private set; }
        public Entity Entity;
        public string Ip;
        public Coob Coob { get; private set; }
        public bool Pvp;
        private bool disconnecting;

        private readonly TcpClient tcp;
        private readonly byte[] recvBuffer;

        public Client(TcpClient tcpClient, Coob coob)
        {
            Joined = false;
            Entity = null;
            disconnecting = false;
            tcp = tcpClient;
            Ip = ((IPEndPoint)tcp.Client.RemoteEndPoint).Address.ToString();
            NetStream = tcp.GetStream();
            Reader = new NetReader(NetStream);
            Writer = new BinaryWriter(NetStream);
            Coob = coob;

            Id = Coob.CreateId();

            if (Id == 0)
                throw new UserLimitReachedException();

            recvBuffer = new byte[4];
            NetStream.BeginRead(recvBuffer, 0, 4, IdCallback, null);
        }

        void IdCallback(IAsyncResult result)
        {
            if (!tcp.Connected)
            {
                Disconnect("Connection reset by peer.");
                return;
            }

            if (disconnecting)
                return;

            try
            {
                int bytesRead = NetStream.EndRead(result);

                if (bytesRead == 4)
                    Coob.HandleRecvPacket(BitConverter.ToInt32(recvBuffer, 0), this);

                NetStream.BeginRead(recvBuffer, 0, 4, IdCallback, null);
            }
            catch
            {
                Disconnect("Read error");
            }
        }

        public void Disconnect(string reason = "")
        {
            Joined = false;
            disconnecting = true;
            tcp.Close();

            if (!Coob.Clients.ContainsKey(Id))
                return;

            var client = Coob.Clients[Id];
            Coob.Clients.Remove(Id);

            Entity removedEntity;
            if (!Coob.World.Entities.TryRemove(Id, out removedEntity))
                throw new ArgumentException("Failed to remove entity from Entities");

            Program.ScriptManager.CallEvent("OnClientDisconnect", new ClientDisconnectEventArgs(client, reason));
        }

        public void SendMessage(ulong id, string message)
        {
            byte[] msgBuffer = Encoding.Unicode.GetBytes(message);
            int msgLength = msgBuffer.Length / 2;

            Writer.Write(ScPacketIDs.ServerChatMessage);
            Writer.Write(id);
            Writer.Write(msgLength);
            Writer.Write(msgBuffer);
        }

        public void SendServerMessage(string message)
        {
            SendMessage(0, message);
        }

        /// <summary>
        /// Sets the current day and time for the client.
        /// </summary>
        /// <param name="day">The current day (not sure what use this has).</param>
        /// <param name="time">The elapsed hours in 0-24 range.</param>
        public void SetTime(uint day, float time)
        {
            Writer.Write(ScPacketIDs.CurrentTime);
            Writer.Write(day);
            Writer.Write((uint)(60f * 60f * time * 1000f));
        }
    }
}
