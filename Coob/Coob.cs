using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Coob.Structures;
using Coob.Packets;
using Jint.Native;

namespace Coob
{
    public class CoobOptions
    {
        public int Port = 12345;
        public int WorldSeed;
        public uint MaxClients = 1024;
    }

    public class Coob
    {
        public ConcurrentQueue<Packet.Base> MessageQueue;
        public delegate Packet.Base PacketParserDel(Client client);
        public Dictionary<int, PacketParserDel> PacketParsers;
        public Dictionary<ulong, Client> Clients;
        public ConcurrentDictionary<ulong, Entity> Entities;
        public CoobOptions Options;

        TcpListener clientListener;

        Thread messageHandlerThread;

        public bool Running { get; private set; }

        public Coob(CoobOptions options)
        {
            this.Options = options;
            MessageQueue = new ConcurrentQueue<Packet.Base>();
            PacketParsers = new Dictionary<int, PacketParserDel>();
            Clients = new Dictionary<ulong, Client>();
            Entities = new ConcurrentDictionary<ulong, Entity>();

            PacketParsers.Add((int)CSPacketIDs.EntityUpdate, Packet.EntityUpdate.Parse);
            PacketParsers.Add((int)CSPacketIDs.Interact, Packet.Interact.Parse);
            PacketParsers.Add((int)CSPacketIDs.Shoot, Packet.Shoot.Parse);
            PacketParsers.Add((int)CSPacketIDs.ClientChatMessage, Packet.ChatMessage.Parse);
            PacketParsers.Add((int)CSPacketIDs.ChunkDiscovered, Packet.UpdateChunk.Parse);
            PacketParsers.Add((int)CSPacketIDs.SectorDiscovered, Packet.UpdateSector.Parse);
            PacketParsers.Add((int)CSPacketIDs.ClientVersion, Packet.ClientVersion.Parse);

            clientListener = new TcpListener(IPAddress.Any, options.Port);
            clientListener.Start();
            clientListener.BeginAcceptTcpClient(onClientConnect, null);
        }

        void onClientConnect(IAsyncResult result)
        {
            var tcpClient = clientListener.EndAcceptTcpClient(result);

            string ip = (tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString();

            if (Root.Scripting.CallFunction<bool>("onClientConnect", ip))
            {
                var newClient = new Client(tcpClient);
                Clients.Add(newClient.ID, newClient);
            }
            else
            {
                tcpClient.Close();
            }

            clientListener.BeginAcceptTcpClient(onClientConnect, null);
        }

        public void HandleRecvPacket(int id, Client client)
        {
            if (!PacketParsers.ContainsKey(id))
            {
                Log.Error("Unknown packet: {0} from client {1}", id, client.ID);
                client.Disconnect("Unknown data");
                return;
            }

            Packet.Base message = PacketParsers[id].Invoke(client);
            MessageQueue.Enqueue(message);

            if (!(message is Packet.EntityUpdate) &&
                !(message is Packet.UpdateChunk) &&
                !(message is Packet.UpdateSector))
            {
                Log.Info("queueing {0}", message);
            }
        }

        public void StartMessageHandler()
        {
            Running = true;
            messageHandlerThread = new Thread(messageHandler);
            messageHandlerThread.Start();

            Log.Info("Started message handler.");
        }

        public void StopMessageHandler()
        {
            Running = false;
        }

        void messageHandler()
        {
            while (Running)
            {
                Packet.Base message = null;
                if (!MessageQueue.TryDequeue(out message)) goto displayLog;

                try
                {
                    if (!message.CallScript()) goto displayLog;
                }
                catch (JsException ex)
                {
                    var messageText = (ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message);
                    Log.Error("JS Error on {0}: {1} - {2}", message.PacketTypeName, messageText, ex.Value);
                    goto displayLog;
                }

                message.Process();

            displayLog:
                Log.Display();
            }
        }

        /// <summary>
        /// Returns the lowest unused client ID. Returns -1 if limit is exceeded.
        /// </summary>
        /// <returns></returns>
        public ulong CreateID()
        {
            for (ulong i = 1; i < Options.MaxClients; i++)
            {
                if (Clients.ContainsKey(i) == false)
                {
                    return i;
                }
            }

            return 0;
        }

        public void BroadcastChat(ulong id, string message)
        {
            byte[] msgBuffer = Encoding.Unicode.GetBytes(message);
            int msgLength = msgBuffer.Length / 2;

            foreach (var client in GetClients())
            {
                client.Writer.Write(SCPacketIDs.ServerChatMessage);
                client.Writer.Write(id);
                client.Writer.Write(msgLength);
                client.Writer.Write(msgBuffer);
            }
        }

        public Client[] GetClients()
        {
            return Clients.Values.ToArray();
        }

        public void SendServerMessage(string message)
        {
            Clients.Select(cl => cl.Value)
                .Where(cl => cl.Joined)
                .ToList()
                .ForEach(
                    cl => cl.SendServerMessage(message)
                );
        }

        public void SetTime(uint day, float time)
        {
            Clients.Select(cl => cl.Value)
                .Where(cl => cl.Joined)
                .ToList()
                .ForEach(cl => cl.SetTime(day, time));
        }
    }
}
