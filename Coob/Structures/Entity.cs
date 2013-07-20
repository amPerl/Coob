using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coob.Structures
{
    /* Structure info mostly stolen from mat^2 ;) */

    public class Entity
    {
        #region Fields
        public ulong ID;
        public byte[] LastBitmask;

        public QVector3 Position;
        public Vector3 Rotation;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public Vector3 ExtraVelocity;
        public float LookPitch;
        public uint PhysicsFlags;
        public byte SpeedFlags;
        public uint EntityType;
        public byte CurrentMode;
        public uint LastShootTime;
        public uint HitCounter;
        public uint LastHitTime;
        public Appearance Appearance;
        public byte Flags1;
        public byte Flags2;
        public uint RollTime;
        public int StunTime;
        public uint SlowedTime;
        public uint MakeBlueTime;
        public uint SpeedUpTime;
        public float ShowPatchTime;
        public byte ClassType;
        public byte Specialization;
        public float ChargedMP;
        public Vector3 RayHit;
        public float HP;
        public float MP;
        public float BlockPower;
        public float MaxHPMultiplier;
        public float ShootSpeed;
        public float DamageMultiplier;
        public float ArmorMultiplier;
        public float ResistanceMultiplier;
        public uint Level;
        public uint CurrentXP;
        public Item ItemData;
        public Item[] Equipment;
        public uint IceBlockFour;
        public uint[] Skills;
        public string Name;

        public uint unknown_or_not_used1;
        public uint unknown_or_not_used2;
        public byte unknown_or_not_used3;
        public uint unknown_or_not_used4;
        public uint unknown_or_not_used5;
        public uint not_used1;
        public uint not_used2;
        public uint not_used3;
        public uint not_used4;
        public uint not_used5;
        public uint not_used6;
        public byte not_used7;
        public byte not_used8;
        public ulong ParentOwner;
        public uint not_used11;
        public uint not_used12;
        public uint not_used13;
        public uint not_used14;
        public uint not_used15;
        public uint not_used16;
        public uint not_used17;
        public uint not_used18;
        public uint not_used20;
        public uint not_used21;
        public uint not_used22;
        public byte not_used19;

        #endregion

        public Entity()
        {
            Position = new QVector3();
            Rotation = new Vector3();
            Velocity = new Vector3();
            Acceleration = new Vector3();
            ExtraVelocity = new Vector3();

            Appearance = new Appearance();

            RayHit = new Vector3();

            ItemData = new Item();

            Equipment = new Item[13];
            for (int i = 0; i < 13; i++)
                Equipment[i] = new Item();

            Skills = new uint[11];
        }

        public void ReadByMask(BinaryReader reader)
        {
            LastBitmask = reader.ReadBytes(8);
            BitArray bitArray = new BitArray(LastBitmask);

            if (bitArray.Get(0))
            {
                Position = new QVector3
                {
                    X = reader.ReadInt64(),
                    Y = reader.ReadInt64(),
                    Z = reader.ReadInt64()
                };
            }
            if (bitArray.Get(1))
            {
                Rotation = new Vector3
                {
                    Roll = reader.ReadSingle(),
                    Pitch = reader.ReadSingle(),
                    Yaw = reader.ReadSingle()
                };
            }
            if (bitArray.Get(2))
            {
                Velocity = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            }
            if (bitArray.Get(3))
            {
                Acceleration = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            }
            if (bitArray.Get(4))
            {
                ExtraVelocity = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            }
            if (bitArray.Get(5))
            {
                LookPitch = reader.ReadSingle();
            }
            if (bitArray.Get(6))
            {
                PhysicsFlags = reader.ReadUInt32();
            }
            if (bitArray.Get(7))
            {
                SpeedFlags = reader.ReadByte();
            }
            if (bitArray.Get(8))
            {
                EntityType = reader.ReadUInt32();
            }
            if (bitArray.Get(9))
            {
                CurrentMode = reader.ReadByte();
            }
            if (bitArray.Get(10))
            {
                LastShootTime = reader.ReadUInt32();
            }
            if (bitArray.Get(11))
            {
                HitCounter = reader.ReadUInt32();
            }
            if (bitArray.Get(12))
            {
                LastHitTime = reader.ReadUInt32();
            }
            if (bitArray.Get(13))
            {
                Appearance.Read(reader);
            }
            if (bitArray.Get(14))
            {
                Flags1 = reader.ReadByte();
                Flags2 = reader.ReadByte();
            }
            if (bitArray.Get(15))
            {
                RollTime = reader.ReadUInt32();
            }
            if (bitArray.Get(16))
            {
                StunTime = reader.ReadInt32();
            }
            if (bitArray.Get(17))
            {
                SlowedTime = reader.ReadUInt32();
            }
            if (bitArray.Get(18))
            {
                MakeBlueTime = reader.ReadUInt32();
            }
            if (bitArray.Get(19))
            {
                SpeedUpTime = reader.ReadUInt32();
            }
            if (bitArray.Get(20))
            {
                ShowPatchTime = reader.ReadSingle();
            }
            if (bitArray.Get(21))
            {
                ClassType = reader.ReadByte();
            }
            if (bitArray.Get(22))
            {
                Specialization = reader.ReadByte();
            }
            if (bitArray.Get(23))
            {
                ChargedMP = reader.ReadSingle();
            }
            if (bitArray.Get(24))
            {
                not_used1 = reader.ReadUInt32();
                not_used2 = reader.ReadUInt32();
                not_used3 = reader.ReadUInt32();
            }
            if (bitArray.Get(25))
            {
                not_used4 = reader.ReadUInt32();
                not_used5 = reader.ReadUInt32();
                not_used6 = reader.ReadUInt32();
            }
            if (bitArray.Get(26))
            {
                RayHit = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            }
            if (bitArray.Get(27))
            {
                HP = reader.ReadSingle();
            }
            if (bitArray.Get(28))
            {
                MP = reader.ReadSingle();
            }
            if (bitArray.Get(29))
            {
                BlockPower = reader.ReadSingle();
            }
            if (bitArray.Get(30))
            {
                MaxHPMultiplier = reader.ReadSingle();
                ShootSpeed = reader.ReadSingle();
                DamageMultiplier = reader.ReadSingle();
                ArmorMultiplier = reader.ReadSingle();
                ResistanceMultiplier = reader.ReadSingle();
            }
            if (bitArray.Get(31))
            {
                not_used7 = reader.ReadByte();
            }
            if (bitArray.Get(32))
            {
                not_used8 = reader.ReadByte();
            }
            if (bitArray.Get(33))
            {
                Level = reader.ReadUInt32();
            }
            if (bitArray.Get(34))
            {
                CurrentXP = reader.ReadUInt32();
            }
            if (bitArray.Get(35))
            {
                ParentOwner = reader.ReadUInt64();
            }
            if (bitArray.Get(36))
            {
                unknown_or_not_used1 = reader.ReadUInt32();
                unknown_or_not_used2 = reader.ReadUInt32();
            }
            if (bitArray.Get(37))
            {
                unknown_or_not_used3 = reader.ReadByte();
            }
            if (bitArray.Get(38))
            {
                unknown_or_not_used4 = reader.ReadUInt32();
            }
            if (bitArray.Get(39))
            {
                unknown_or_not_used5 = reader.ReadUInt32();
                not_used11 = reader.ReadUInt32();
                not_used12 = reader.ReadUInt32();
            }
            if (bitArray.Get(40))
            {
                not_used13 = reader.ReadUInt32();
                not_used14 = reader.ReadUInt32();
                not_used15 = reader.ReadUInt32();
                not_used16 = reader.ReadUInt32();
                not_used17 = reader.ReadUInt32();
                not_used18 = reader.ReadUInt32();
            }
            if (bitArray.Get(41))
            {
                not_used20 = reader.ReadUInt32();
                not_used21 = reader.ReadUInt32();
                not_used22 = reader.ReadUInt32();
            }
            if (bitArray.Get(42))
            {
                not_used19 = reader.ReadByte();
            }
            if (bitArray.Get(43))
            {
                ItemData.Read(reader);
            }
            if (bitArray.Get(44))
            {
                for (int i = 0; i < 13; i++)
                {
                    Item item = new Item();
                    item.Read(reader);
                    Equipment[i] = item;
                }
            }
            if (bitArray.Get(45))
            {
                Name = Encoding.ASCII.GetString(reader.ReadBytes(16));
                Name = Name.TrimEnd(' ', '\0');
            }
            if (bitArray.Get(46))
            {
                Skills = new uint[11];
                for (int i = 0; i < 11; i++)
                {
                    Skills[i] = reader.ReadUInt32();
                }
            }
            if (bitArray.Get(47))
            {
                IceBlockFour = reader.ReadUInt32();
            }
        }

        public void CopyByMask(Entity from)
        {
            LastBitmask = from.LastBitmask;

            BitArray bitArray = new BitArray(LastBitmask);

            if (bitArray.Get(0))
            {
                Position = from.Position.Clone();
            }
            if (bitArray.Get(1))
            {
                Rotation = new Vector3
                {
                    Pitch = from.Rotation.Pitch,
                    Roll = from.Rotation.Roll,
                    Yaw = from.Rotation.Yaw
                };
            }
            if (bitArray.Get(2))
            {
                Velocity = from.Velocity.Clone();
            }
            if (bitArray.Get(3))
            {
                Acceleration = from.Acceleration.Clone();
            }
            if (bitArray.Get(4))
            {
                ExtraVelocity = from.ExtraVelocity.Clone();
            }
            if (bitArray.Get(5))
            {
                LookPitch = from.LookPitch;
            }
            if (bitArray.Get(6))
            {
                PhysicsFlags = from.PhysicsFlags;
            }
            if (bitArray.Get(7))
            {
                SpeedFlags = from.SpeedFlags;
            }
            if (bitArray.Get(8))
            {
                EntityType = from.EntityType;
            }
            if (bitArray.Get(9))
            {
                CurrentMode = from.CurrentMode;
            }
            if (bitArray.Get(10))
            {
                LastShootTime = from.LastShootTime;
            }
            if (bitArray.Get(11))
            {
                HitCounter = from.HitCounter;
            }
            if (bitArray.Get(12))
            {
                LastHitTime = from.LastHitTime;
            }
            if (bitArray.Get(13))
            {
                Appearance.CopyFrom(from.Appearance);
            }
            if (bitArray.Get(14))
            {
                Flags1 = from.Flags1;
                Flags2 = from.Flags2;
            }
            if (bitArray.Get(15))
            {
                RollTime = from.RollTime;
            }
            if (bitArray.Get(16))
            {
                StunTime = from.StunTime;
            }
            if (bitArray.Get(17))
            {
                SlowedTime = from.SlowedTime;
            }
            if (bitArray.Get(18))
            {
                MakeBlueTime = from.MakeBlueTime;
            }
            if (bitArray.Get(19))
            {
                SpeedUpTime = from.SpeedUpTime;
            }
            if (bitArray.Get(20))
            {
                ShowPatchTime = from.ShowPatchTime;
            }
            if (bitArray.Get(21))
            {
                ClassType = from.ClassType;
            }
            if (bitArray.Get(22))
            {
                Specialization = from.Specialization;
            }
            if (bitArray.Get(23))
            {
                ChargedMP = from.ChargedMP;
            }
            if (bitArray.Get(24))
            {
                not_used1 = from.not_used1;
                not_used2 = from.not_used2;
                not_used3 = from.not_used3;
            }
            if (bitArray.Get(25))
            {
                not_used4 = from.not_used4;
                not_used5 = from.not_used5;
                not_used6 = from.not_used6;
            }
            if (bitArray.Get(26))
            {
                RayHit = from.RayHit.Clone();
            }
            if (bitArray.Get(27))
            {
                HP = from.HP;
            }
            if (bitArray.Get(28))
            {
                MP = from.MP;
            }
            if (bitArray.Get(29))
            {
                BlockPower = from.BlockPower;
            }
            if (bitArray.Get(30))
            {
                MaxHPMultiplier = from.MaxHPMultiplier;
                ShootSpeed = from.ShootSpeed;
                DamageMultiplier = from.DamageMultiplier;
                ArmorMultiplier = from.ArmorMultiplier;
                ResistanceMultiplier = from.ResistanceMultiplier;
            }
            if (bitArray.Get(31))
            {
                not_used7 = from.not_used7;
            }
            if (bitArray.Get(32))
            {
                not_used8 = from.not_used8;
            }
            if (bitArray.Get(33))
            {
                Level = from.Level;
            }
            if (bitArray.Get(34))
            {
                CurrentXP = from.CurrentXP;
            }
            if (bitArray.Get(35))
            {
                ParentOwner = from.ParentOwner;
            }
            if (bitArray.Get(36))
            {
                unknown_or_not_used1 = from.unknown_or_not_used1;
                unknown_or_not_used2 = from.unknown_or_not_used2;
            }
            if (bitArray.Get(37))
            {
                unknown_or_not_used3 = from.unknown_or_not_used3;
            }
            if (bitArray.Get(38))
            {
                unknown_or_not_used4 = from.unknown_or_not_used4;
            }
            if (bitArray.Get(39))
            {
                unknown_or_not_used5 = from.unknown_or_not_used5;
                not_used11 = from.not_used11;
                not_used12 = from.not_used12;
            }
            if (bitArray.Get(40))
            {
                not_used13 = from.not_used13;
                not_used14 = from.not_used14;
                not_used15 = from.not_used15;
                not_used16 = from.not_used16;
                not_used17 = from.not_used17;
                not_used18 = from.not_used18;
            }
            if (bitArray.Get(41))
            {
                not_used20 = from.not_used20;
                not_used21 = from.not_used21;
                not_used22 = from.not_used22;
            }
            if (bitArray.Get(42))
            {
                not_used19 = from.not_used19;
            }
            if (bitArray.Get(43))
            {
                ItemData.CopyFrom(from.ItemData);
            }
            if (bitArray.Get(44))
            {
                for (int i = 0; i < 13; i++)
                {
                    Item item = new Item();
                    item.CopyFrom(from.Equipment[i]);
                    Equipment[i] = item;
                }
            }
            if (bitArray.Get(45))
            {
                Name = from.Name;
            }
            if (bitArray.Get(46))
            {
                Skills = new uint[11];
                for (int i = 0; i < 11; i++)
                {
                    Skills[i] = from.Skills[i];
                }
            }
            if (bitArray.Get(47))
            {
                IceBlockFour = from.IceBlockFour;
            }
        }


        public void WriteByMask(byte[] bitmask, BinaryWriter writer)
        {
            BitArray bitArray = new BitArray(bitmask);

            if (bitArray.Get(0))
            {
                Position.Write(writer);
            }
            if (bitArray.Get(1))
            {
                Rotation.Write(writer);
            }
            if (bitArray.Get(2))
            {
                Velocity.Write(writer);
            }
            if (bitArray.Get(3))
            {
                Acceleration.Write(writer);
            }
            if (bitArray.Get(4))
            {
                ExtraVelocity.Write(writer);
            }
            if (bitArray.Get(5))
            {
                writer.Write(LookPitch);
            }
            if (bitArray.Get(6))
            {
                writer.Write(PhysicsFlags);
            }
            if (bitArray.Get(7))
            {
                writer.Write(SpeedFlags);
            }
            if (bitArray.Get(8))
            {
                writer.Write(EntityType);
            }
            if (bitArray.Get(9))
            {
                writer.Write(CurrentMode);
            }
            if (bitArray.Get(10))
            {
                writer.Write(LastShootTime);
            }
            if (bitArray.Get(11))
            {
                writer.Write(HitCounter);
            }
            if (bitArray.Get(12))
            {
                writer.Write(LastHitTime);
            }
            if (bitArray.Get(13))
            {
                Appearance.Write(writer);
            }
            if (bitArray.Get(14))
            {
                writer.Write(Flags1);
                writer.Write(Flags2);
            }
            if (bitArray.Get(15))
            {
                writer.Write(RollTime);
            }
            if (bitArray.Get(16))
            {
                writer.Write(StunTime);
            }
            if (bitArray.Get(17))
            {
                writer.Write(SlowedTime);
            }
            if (bitArray.Get(18))
            {
                writer.Write(MakeBlueTime);
            }
            if (bitArray.Get(19))
            {
                writer.Write(SpeedUpTime);
            }
            if (bitArray.Get(20))
            {
                writer.Write(ShowPatchTime);
            }
            if (bitArray.Get(21))
            {
                writer.Write(ClassType);
            }
            if (bitArray.Get(22))
            {
                writer.Write(Specialization);
            }
            if (bitArray.Get(23))
            {
                writer.Write(ChargedMP);
            }
            if (bitArray.Get(24))
            {
                writer.Write(not_used1);
                writer.Write(not_used2);
                writer.Write(not_used3);
            }
            if (bitArray.Get(25))
            {
                writer.Write(not_used4);
                writer.Write(not_used5);
                writer.Write(not_used6);
            }
            if (bitArray.Get(26))
            {
                RayHit.Write(writer);
            }
            if (bitArray.Get(27))
            {
                writer.Write(HP);
            }
            if (bitArray.Get(28))
            {
                writer.Write(MP);
            }
            if (bitArray.Get(29))
            {
                writer.Write(BlockPower);
            }
            if (bitArray.Get(30))
            {
                writer.Write(MaxHPMultiplier);
                writer.Write(ShootSpeed);
                writer.Write(DamageMultiplier);
                writer.Write(ArmorMultiplier);
                writer.Write(ResistanceMultiplier);
            }
            if (bitArray.Get(31))
            {
                writer.Write(not_used7);
            }
            if (bitArray.Get(32))
            {
                writer.Write(not_used8);
            }
            if (bitArray.Get(33))
            {
                writer.Write(Level);
            }
            if (bitArray.Get(34))
            {
                writer.Write(CurrentXP);
            }
            if (bitArray.Get(35))
            {
                writer.Write(ParentOwner);
            }
            if (bitArray.Get(36))
            {
                writer.Write(unknown_or_not_used1);
                writer.Write(unknown_or_not_used2);
            }
            if (bitArray.Get(37))
            {
                writer.Write(unknown_or_not_used3);
            }
            if (bitArray.Get(38))
            {
                writer.Write(unknown_or_not_used4);
            }
            if (bitArray.Get(39))
            {
                writer.Write(unknown_or_not_used5);
                writer.Write(not_used11);
                writer.Write(not_used12);
            }
            if (bitArray.Get(40))
            {
                writer.Write(not_used13);
                writer.Write(not_used14);
                writer.Write(not_used15);
                writer.Write(not_used16);
                writer.Write(not_used17);
                writer.Write(not_used18);
            }
            if (bitArray.Get(41))
            {
                writer.Write(not_used20);
                writer.Write(not_used21);
                writer.Write(not_used22);
            }
            if (bitArray.Get(42))
            {
                writer.Write(not_used19);
            }
            if (bitArray.Get(43))
            {
                ItemData.Write(writer);
            }
            if (bitArray.Get(44))
            {
                for (int i = 0; i < 13; i++)
                {
                    Equipment[i].Write(writer);
                }
            }
            if (bitArray.Get(45))
            {
                writer.Write(Name);
                writer.Write(new byte[16-Name.Length]);
            }
            if (bitArray.Get(46))
            {
                for (int i = 0; i < 11; i++)
                {
                    writer.Write(Skills[i]);
                }
            }
            if (bitArray.Get(47))
            {
                writer.Write(IceBlockFour);
            }
        }
    }
}
