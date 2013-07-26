using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Coob.CoobEventArgs;
using Coob.Game;
using Coob.Structures;
using Coob.Packets;
using Jint.Native;

namespace Coob
{
    public class CoobOptions
    {
        public int Port = 12345;
        public uint MaxClients = 1024;
        public int WorldSeed;
    }

    public class Coob
    {
        public ConcurrentQueue<Packet.Base> MessageQueue;
        public delegate Packet.Base PacketParserDel(Client client, Coob coob);
        public Dictionary<int, PacketParserDel> PacketParsers;
        public Dictionary<ulong, Client> Clients;
        public CoobOptions Options;
        public World World { get; private set; }

        TcpListener clientListener;

        Thread messageHandlerThread;

        public bool Running { get; private set; }

        public Coob(CoobOptions options)
        {
            this.Options = options;
            MessageQueue = new ConcurrentQueue<Packet.Base>();
            PacketParsers = new Dictionary<int, PacketParserDel>();
            Clients = new Dictionary<ulong, Client>();
            World = new World(options.WorldSeed, this);

            PacketParsers.Add((int)CSPacketIDs.EntityUpdate, Packet.EntityUpdate.Parse);
            PacketParsers.Add((int)CSPacketIDs.Interact, Packet.Interact.Parse);
            // TODO: PacketParsers.Add((int)CSPacketIDs.Hit, Packet.Hit.Parse);
            // TODO: PacketParsers.Add((int)CSPacketIDs.Stealth, Packet.Stealth.Parse);
            PacketParsers.Add((int)CSPacketIDs.Shoot, Packet.Shoot.Parse);
            PacketParsers.Add((int)CSPacketIDs.ClientChatMessage, Packet.ChatMessage.Parse);
            PacketParsers.Add((int)CSPacketIDs.ChunkDiscovered, Packet.UpdateChunk.Parse);
            PacketParsers.Add((int)CSPacketIDs.SectorDiscovered, Packet.UpdateSector.Parse);
            PacketParsers.Add((int)CSPacketIDs.ClientVersion, Packet.ClientVersion.Parse);

            try
            {
                clientListener = new TcpListener(IPAddress.Any, options.Port);
                clientListener.Start();
                clientListener.BeginAcceptTcpClient(onClientConnect, null);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10048)
                {
                    Log.Error("Something is already running on port " + options.Port + ". Can't start server.");
                }
                else
                {
                    Log.Error("Unknown error occured while trying to start server:\n" + e);
                }

                Log.Display();
                Environment.Exit(1);
            }
        }

        void onClientConnect(IAsyncResult result)
        {
            var tcpClient = clientListener.EndAcceptTcpClient(result);

            string ip = (tcpClient.Client.RemoteEndPoint as IPEndPoint).Address.ToString();

            var clientConnectArgs = new ClientConnectEventArgs(ip);
            if (!Root.ScriptManager.CallEvent("OnClientConnect", clientConnectArgs).Canceled)
            {
                var newClient = new Client(tcpClient, this);
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

            Packet.Base message = PacketParsers[id].Invoke(client, this);
            MessageQueue.Enqueue(message);

            if (!(message is Packet.EntityUpdate) &&
                !(message is Packet.UpdateChunk) &&
                !(message is Packet.UpdateSector))
            {
                Log.Info("queueing {0}", message);
            }
        }

        public void StartServer()
        {
            Running = true;

            messageHandlerThread = new Thread(messageHandler);
            messageHandlerThread.Start();

            Log.Info("Started message handler and world.");
        }

        public void StopServer()
        {
            Running = false;
        }

        readonly Stopwatch elapsedDt = new Stopwatch();
        float dtSinceLastWorldUpdate = 0;
        float dtSinceLastEntityUpdate = 0;
        float accumulator = 0;

        private void UpdateWorld()
        {
            var totalDt = (float)elapsedDt.Elapsed.TotalSeconds;
            accumulator = totalDt;
            elapsedDt.Restart();

            int times = 0;
            //Console.WriteLine("------------------------------------");
            while (accumulator > 0 && times < 4) // max 4 updates per update so that we don't risk updating like 300 times in a row if something froze temporarily.
            {
                ++times;
                float dt = Math.Min(accumulator, Globals.WorldTickPerSecond / 1000f);
                accumulator -= dt;
                //Console.WriteLine(dt);

                dtSinceLastWorldUpdate += dt * 1000f; // * 1000 because counting in milliseconds below.
                dtSinceLastEntityUpdate += dt * 1000f;

                if (dtSinceLastWorldUpdate >= Globals.WorldTickPerSecond)
                {
                    World.Update(dt + dtSinceLastWorldUpdate);
                    dtSinceLastWorldUpdate = 0;
                }

                if (dtSinceLastEntityUpdate >= Globals.EntityUpdatesPerSecond)
                {
                    World.SendEntityUpdates();
                    dtSinceLastEntityUpdate = 0;
                }

                Log.Display();
            }
            //Console.WriteLine("------------------------------------");
            //Console.WriteLine("Updated " + times + " times");
        }

        void messageHandler()
        {
            while (Running)
            {
                UpdateWorld();

                Packet.Base message = null;
                if (!MessageQueue.TryDequeue(out message)) goto sleep;

                try
                {
                    if (!message.CallScript()) goto sleep;
                }
                catch (JsException ex)
                {
                    var messageText = (ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message);
                    Log.Error("JS Error on {0}: {1} - {2}", message.PacketTypeName, messageText, ex.Value);
                    goto sleep;
                }

                message.Process();

            sleep:
                Thread.Sleep(5); // Avoid maxing the cpu (as much).
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
       

        public Client[] GetClients()
        {
            return GetClients(null);
        }

        public Client[] GetClients(Client except)
        {
            return Clients.Values.Where(cl => cl != except).ToArray();
        }



    }
}
