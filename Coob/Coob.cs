using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Coob.CoobEventArgs;
using Coob.Game;
using Coob.Packets;
using Jint.Native;

namespace Coob
{
    public class Coob
    {
        public ConcurrentQueue<Packet.Base> MessageQueue;
        public delegate Packet.Base PacketParserDel(Client client, Coob coob);
        public Dictionary<int, PacketParserDel> PacketParsers;
        public Dictionary<ulong, Client> Clients;
        public CoobOptions Options;
        public World World { get; private set; }

        private readonly TcpListener clientListener;
        private Thread messageHandlerThread;

        private readonly Stopwatch elapsedDt;
        private float dtSinceLastWorldUpdate;
        private float dtSinceLastServerUpdate;
        private float accumulator;

        public bool Running { get; private set; }

        public Coob(CoobOptions options)
        {
            Options = options;

            elapsedDt = new Stopwatch();

            MessageQueue = new ConcurrentQueue<Packet.Base>();
            PacketParsers = new Dictionary<int, PacketParserDel>();
            Clients = new Dictionary<ulong, Client>();
            World = new World(options.WorldSeed, this);

            // TODO Refactor with new design later
            PacketParsers.Add((int)CSPacketIDs.EntityUpdate, Packet.EntityUpdate.Parse);
            PacketParsers.Add((int)CSPacketIDs.Interact, Packet.Interact.Parse);
            PacketParsers.Add((int)CSPacketIDs.Hit, Packet.Hit.Parse);
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
                clientListener.BeginAcceptTcpClient(OnClientConnect, null);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10048)
                    Log.Error("Something is already running on port " + options.Port + ". Can't start server.");
                else
                    Log.Error("Unknown error occured while trying to start server:\n" + e);

                Log.Display();
                Environment.Exit(1);
            }

            Log.Info("Listening on port: " + options.Port);
        }

        void OnClientConnect(IAsyncResult result)
        {
            var tcpClient = clientListener.EndAcceptTcpClient(result);

            string ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();

            var clientConnectArgs = new ClientConnectEventArgs(ip);
            if (!Program.ScriptManager.CallEvent("OnClientConnect", clientConnectArgs).Canceled)
            {
                var newClient = new Client(tcpClient, this);
                Clients.Add(newClient.Id, newClient);
            }
            else
                tcpClient.Close();

            clientListener.BeginAcceptTcpClient(OnClientConnect, null);
        }

        public void HandleRecvPacket(int id, Client client)
        {
            if (!PacketParsers.ContainsKey(id))
            {
                Log.Error("Unknown packet: {0} from client {1}", id, client.Id);
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

            messageHandlerThread = new Thread(MessageHandler);
            messageHandlerThread.Start();

            Log.Info("Started message handler and world.");
        }

        public void StopServer()
        {
            Running = false;
        }

        public void KickPlayer(string name)
        {
            var player = Clients.FirstOrDefault(plr => plr.Value.Entity.Name.ToLower() == name);
            if (player.Value != null)
            {
                player.Value.Disconnect();
            }
            else
            {
                Log.Warning("No such player '{0}'", name);
            }
        }

        public void Broadcast(string Message)
        {
            foreach (var c in Clients.Values)
            {
                c.SendServerMessage("[SERVER] " + Message);
            }
        }

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
                dtSinceLastServerUpdate += dt * 1000f;

                if (dtSinceLastWorldUpdate >= Globals.WorldTickPerSecond)
                {
                    World.Update(dt + dtSinceLastWorldUpdate);
                    dtSinceLastWorldUpdate = 0;
                }

                if (dtSinceLastServerUpdate >= Globals.EntityUpdatesPerSecond)
                {
                    World.SendServerUpdate();
                    dtSinceLastServerUpdate = 0;
                }

                Log.Display();
            }
            //Console.WriteLine("------------------------------------");
            //Console.WriteLine("Updated " + times + " times");
        }

        void MessageHandler()
        {
            while (Running)
            {
                UpdateWorld();

                Packet.Base message;
                while (MessageQueue.TryDequeue(out message))
                {
                    try
                    {
                        if (!message.CallScript())
                            continue;

                        message.Process();
                    }
                    catch (JsException ex)
                    {
                        var messageText = (ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message);
                        Log.Error("JS Error on {0}: {1} - {2}", message.PacketTypeName, messageText, ex.Value);
                    }
                    catch (IOException)
                    {
                        // Client disconnected while we were writing to its netwriter.
                    }
                }

                Thread.Sleep(5); // Avoid maxing the cpu (as much).
            }
        }

        /// <summary>
        /// Returns the lowest unused client ID. Returns -1 if limit is exceeded.
        /// </summary>
        /// <returns></returns>
        public ulong CreateId()
        {
            for (ulong i = 1; i < Options.MaxClients; i++)
            {
                if (!Clients.ContainsKey(i))
                    return i;
            }

            return 0;
        }


        public Client[] GetClients()
        {
            return GetClients(null);
        }

        public Client[] GetClients(Client except)
        {
            return Clients.Values.Where(cl => cl != except && cl.Joined).ToArray();
        }

        public Client GetClient(ulong id)
        {
            return Clients.ContainsKey(id) ? Clients[id] : null;
        }

        public Client GetClient(string name)
        {
            name = name.ToLower();
            return Clients.Values.FirstOrDefault(client => client.Entity.Name.ToLower() == name);
        }
    }
}
