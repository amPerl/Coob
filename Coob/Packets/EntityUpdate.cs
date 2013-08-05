using System;
using Coob.CoobEventArgs;
using Coob.Structures;
using System.IO;

namespace Coob.Packets
{
    public partial class Packet
    {
        public class EntityUpdate : Base
        {
            public static readonly byte[] FullBitmask = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00 };

            public Entity Entity;
            public Entity Changes;
            public long UpdateBitmask;
            public bool IsJoin;

            public EntityUpdate(Entity entity, Entity changes, bool join, Client client)
                : base(client)
            {
                Entity = entity;
                Changes = changes;
                IsJoin = join;
            }

            public static Base Parse(Client client, Coob coob)
            {
                int length = client.Reader.ReadInt32();

                byte[] compressedData = client.Reader.ReadBytes(length);
                byte[] maskedData = ZlibHelper.UncompressBuffer(compressedData);

                using (var ms = new MemoryStream(maskedData))
                using (var br = new BinaryReader(ms))
                {
                    ulong id = br.ReadUInt64();

                    if (id != client.Id)
                        throw new NotImplementedException();

                    if (client.Entity == null)
                    {
                        Entity entity = new Entity(coob, null);

                        client.Entity = entity;
                        entity.Client = client;
                        entity.Id = client.Id;
                        coob.World.Entities[client.Id] = client.Entity;

                        entity.ReadByMask(br);

                        return new EntityUpdate(client.Entity, client.Entity, true, client);
                    }

                    Entity changes = new Entity(coob, client);
                    changes.ReadByMask(br);

                    return new EntityUpdate(client.Entity, changes, false, client);
                }
            }

            public override bool CallScript()
            {
                if (IsJoin)
                {
                    bool joined = Program.ScriptManager.CallEvent("OnClientJoin", new ClientJoinEventArgs(Sender)).Canceled == false;

                    if (joined)
                        Sender.Joined = true;

                    return joined;
                }

                return Program.ScriptManager.CallEvent("OnEntityUpdate", new EntityUpdateEventArgs(Sender, Changes)).Canceled == false;
            }

            public override void Process()
            {
                Entity.CopyByMask(Changes);

                byte[] compressed;

                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Entity.Id);
                    bw.Write(Changes.LastBitmask);
                    Entity.WriteByMask(Changes.LastBitmask, bw);

                    byte[] uncompressed = ms.ToArray();
                    compressed = ZlibHelper.CompressBuffer(uncompressed);
                }

                foreach (var peer in Sender.Coob.GetClients(Sender))
                {
                    peer.Writer.Write(ScPacketIDs.EntityUpdate);
                    peer.Writer.Write(compressed.Length);
                    peer.Writer.Write(compressed);

                    if (!IsJoin)
                        continue;

                    byte[] peerCompressed;
                    using (var ms = new MemoryStream())
                    using (var bw = new BinaryWriter(ms))
                    {
                        bw.Write(peer.Entity.Id);
                        bw.Write(Changes.LastBitmask);
                        peer.Entity.WriteByMask(FullBitmask, bw);

                        byte[] uncompressed = ms.ToArray();
                        peerCompressed = ZlibHelper.CompressBuffer(uncompressed);
                    }

                    Sender.Writer.Write(ScPacketIDs.EntityUpdate);
                    Sender.Writer.Write(peerCompressed.Length);
                    Sender.Writer.Write(peerCompressed);
                }
            }
        }
    }
}
