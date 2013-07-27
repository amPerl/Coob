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

      
            public Hit(Client client) : base(client)
            {
                 
            }

            public static Base Parse(Client client, Coob coob)
            {
                Hit hit = new Hit(client);

                hit.EntityID = client.Reader.ReadUInt64();
                hit.TargetID = client.Reader.ReadUInt64();
                hit.Damage = client.Reader.ReadSingle();
                hit.Critical = client.Reader.ReadByte();
                client.Reader.ReadBytes(3);
                hit.StunDuration = client.Reader.ReadUInt32();
                hit.Something8 = client.Reader.ReadUInt32();
                hit.Position = new QVector3
                   {
                       X = client.Reader.ReadInt64(),
                       Y = client.Reader.ReadInt64(),
                       Z = client.Reader.ReadInt64()
                   };
                hit.HitDirection = new Vector3
                   {
                       X = client.Reader.ReadSingle(),
                       Y = client.Reader.ReadSingle(),
                       Z = client.Reader.ReadSingle()
                   };
                hit.Skill = client.Reader.ReadByte();
                hit.HitType = client.Reader.ReadByte();
                hit.ShowLight = client.Reader.ReadByte();
                client.Reader.ReadBytes(1);
                coob.World.HitPackets.Add(hit);
                return hit;
            }

            public void write(BinaryWriter bw)
            {
                bw.Write(EntityID);
                bw.Write(TargetID);
                bw.Write(Damage);
                bw.Write(Critical);
                bw.Pad(3);
                bw.Write(StunDuration);
                bw.Write(Something8);
                Position.Write(bw);
                HitDirection.Write(bw);
                bw.Write(Skill);
                bw.Write(HitType);
                bw.Write(ShowLight);
                bw.Pad(1);
            }
        

            public override bool CallScript()
            {
                return false;
            }

            public override void Process()
            {
                
            }
        }
    }
}
