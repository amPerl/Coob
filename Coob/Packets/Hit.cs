using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Coob.CoobEventArgs;
using Coob.Structures;

namespace Coob.Packets
{
    public partial class Packet
    {
        public class Hit : Base
        {
            public ulong EntityID;
            public ulong TargetID;
            public float Damage;
            public byte Critical;
            public uint StunDuration;
            public uint Something8;
            public QVector3 Position;
            public Vector3 HitDirection;
            public byte Skill;
            public byte HitType;
            public byte ShowLight;

            public Hit(Client client) : base(client) { }

            public static Base Parse(Client client, Coob coob)
            {
                var hit = new Hit(client);

                hit.EntityID = client.Reader.ReadUInt64();
                hit.TargetID = client.Reader.ReadUInt64();
                hit.Damage = client.Reader.ReadSingle();
                hit.Critical = client.Reader.ReadByte();
                client.Reader.ReadBytes(3);
                hit.StunDuration = client.Reader.ReadUInt32();
                hit.Something8 = client.Reader.ReadUInt32();
                hit.Position = client.Reader.ReadQVector3();
                hit.HitDirection = client.Reader.ReadVector3();
                hit.Skill = client.Reader.ReadByte();
                hit.HitType = client.Reader.ReadByte();
                hit.ShowLight = client.Reader.ReadByte();
                client.Reader.ReadBytes(1);
                return hit;
            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Process()
            {
                // Todo: Process Hit in world.
            }
        }
    }
}
