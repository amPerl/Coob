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
        public int MaxClients = 1024;
    }

    public class Coob
    {
        public ConcurrentQueue<Packet.Base> MessageQueue;
        public delegate Packet.Base PacketParserDel(Client client);
        public Dictionary<int, PacketParserDel> PacketParsers;
        public Dictionary<long, Client> Clients;
        public ConcurrentDictionary<long, Entity> Entities;
        public CoobOptions Options;

        TcpListener clientListener;

        Thread messageHandlerThread;

        public Coob(CoobOptions options)
        {
            this.Options = options;
            MessageQueue = new ConcurrentQueue<Packet.Base>();
            PacketParsers = new Dictionary<int, PacketParserDel>();
            Clients = new Dictionary<long, Client>();
            Entities = new ConcurrentDictionary<long, Entity>();

            PacketParsers.Add(0, Packet.EntityUpdate.Parse);
            PacketParsers.Add(6, Packet.Interact.Parse);
            PacketParsers.Add(9, Packet.Shoot.Parse);
            PacketParsers.Add(10, Packet.ChatMessage.Parse);
            PacketParsers.Add(11, Packet.UpdateChunk.Parse);
            PacketParsers.Add(12, Packet.UpdateSector.Parse);
            PacketParsers.Add(17, Packet.ClientVersion.Parse);

            clientListener = new TcpListener(IPAddress.Any, options.Port);
            clientListener.Start();
            clientListener.BeginAcceptTcpClient(onClientConnect, null);
            
        }

        void onClientConnect(IAsyncResult result)
        {
            var tcpClient = clientListener.EndAcceptTcpClient(result);

            string ip = tcpClient.Client.RemoteEndPoint.ToString().Split(':')[0];

            if ((bool)Root.JavaScript.Engine.CallFunction("onClientConnect", ip))
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
                Log.WriteError("Unknown packet: " + id + " from client " + client.ID);
                client.Disconnect("Unknown data");
                return;
            }

            Packet.Base message = PacketParsers[id].Invoke(client);
            MessageQueue.Enqueue(message);

            if (!(message is Packet.EntityUpdate) &&
                !(message is Packet.UpdateChunk) &&
                !(message is Packet.UpdateSector))
            {
                Log.WriteInfo("queueing " + message);
            }
        }

        public void StartMessageHandler()
        {
            messageHandlerThread = new Thread(messageHandler);
            messageHandlerThread.Start();

            Log.WriteInfo("Started message handler.");
        }

        void messageHandler()
        {
            while (true)
            {
                Packet.Base message = null;
                if (!MessageQueue.TryDequeue(out message)) goto displayLog;

                try
                {
                    if (!message.CallScript()) goto displayLog;
                }
                catch (JsException ex)
                {
                    Log.WriteError("JS Error on " + message.PacketTypeName + ": " + (ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message) + " - " + ex.Value);
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
        public long CreateID()
        {
            for (long i = 0; i < Options.MaxClients; i++)
            {
                if (Clients.ContainsKey(i) == false)
                {
                    return i;
                }
            }

            return -1;
        }

        public void BroadcastChat(long id, string message)
        {
            byte[] msgBuffer = Encoding.Unicode.GetBytes(message);
            int msgLength = msgBuffer.Length / 2;

            foreach (var client in GetClients())
            {
                client.Writer.Write(10);
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
    }
}
