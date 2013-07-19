using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.Structures;
using System.IO;

namespace Coob.Packets
{
    public partial class Packet
    {
        public class EntityUpdate : Base
        {
            public Entity Entity;
            public Entity Changes;
            public long UpdateBitmask;
            public bool IsJoin;

            public EntityUpdate(Entity entity, Entity changes, bool join, Client client)
                : base(client)
            {
                this.Entity = entity;
                this.Changes = changes;
                this.IsJoin = join;
            }

            public static Base Parse(Client client)
            {
                int length = client.Reader.ReadInt32();

                byte[] compressedData = client.Reader.ReadBytes(length);
                byte[] maskedData = ZlibHelper.UncompressBuffer(compressedData);

                Entity entity;

                using (var ms = new MemoryStream(maskedData))
                using (var br = new BinaryReader(ms))
                {
                    long id = br.ReadInt64();

                    if (id != client.ID)
                        throw new NotImplementedException();

                    if (client.Entity == null)
                    {
                        entity = new Entity();

                        client.Entity = entity;
                        Root.Coob.Entities[client.ID] = client.Entity;

                        entity.ReadByMask(br);

                        return new EntityUpdate(client.Entity, client.Entity, true, client);
                    }
                    else
                    {
                        entity = Root.Coob.Entities[id];

                        Entity changes = new Entity();
                        changes.ReadByMask(br);

                        return new EntityUpdate(client.Entity, changes, false, client);
                    }
                }
            }

            public override bool CallScript()
            {
                if (IsJoin)
                    return (bool)Root.JavaScript.Engine.CallFunction("onClientJoin", Sender);
                else
                    return (bool)Root.JavaScript.Engine.CallFunction("onEntityUpdate", Entity, Changes, Sender);
            }

            public override void Process()
            {
                Entity.CopyByMask(Changes);

                using (var ms = new MemoryStream())
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Entity.ID);
                    bw.Write(Changes.LastBitmask);
                    Entity.WriteByMask(Changes.LastBitmask, bw);
                }
            }
        }
    }
}
