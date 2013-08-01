using System.Collections;
using System.IO;
using System.Text;

namespace Coob.Structures
{
    /* Structure info mostly stolen from mat^2 ;) */

    public class Entity
    {
        #region Fields
        public ulong Id;
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
        public float ChargedMp;
        public Vector3 RayHit;
        public float Hp;
        public float Mp;
        public float BlockPower;
        public float MaxHpMultiplier;
        public float ShootSpeed;
        public float DamageMultiplier;
        public float ArmorMultiplier;
        public float ResistanceMultiplier;
        public uint Level;
        public uint CurrentXp;
        public Item ItemData;
        public Item[] Equipment;
        public uint IceBlockFour;
        public uint[] Skills;
        public string Name;

        private uint unknownOrNotUsed1;
        private uint unknownOrNotUsed2;
        private byte unknownOrNotUsed3;
        private uint unknownOrNotUsed4;
        private uint unknownOrNotUsed5;
        private uint notUsed1;
        private uint notUsed2;
        private uint notUsed3;
        private uint notUsed4;
        private uint notUsed5;
        private uint notUsed6;
        private byte notUsed7;
        private byte notUsed8;
        public ulong ParentOwner;
        private uint notUsed11;
        private uint notUsed12;
        private uint notUsed13;
        private uint notUsed14;
        private uint notUsed15;
        private uint notUsed16;
        private uint notUsed17;
        private uint notUsed18;
        private uint notUsed20;
        private uint notUsed21;
        private uint notUsed22;
        private byte notUsed19;

        #endregion

        public Coob Coob;
        public Client Client;

        // TODO Find a better way to do this

        public Entity(Coob coob, Client owner)
        {
            Client = owner;
            Coob = coob;
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
                    Pitch = reader.ReadSingle(),
                    Roll = reader.ReadSingle(),
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

                // Not sure how necessary this is.
                if (Client != null && Client.Pvp)
                {
                    if (Flags1 == 0x40) // Non-hostile
                        Flags1 = 0x20;
                }
                else if (Client != null && !Client.Pvp)
                {
                    if (Flags1 == 0x20) // Hostile
                        Flags1 = 0x40;
                }

                Log.Info("Flags1: " + Flags1);

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
                ChargedMp = reader.ReadSingle();
            }
            if (bitArray.Get(24))
            {
                notUsed1 = reader.ReadUInt32();
                notUsed2 = reader.ReadUInt32();
                notUsed3 = reader.ReadUInt32();
            }
            if (bitArray.Get(25))
            {
                notUsed4 = reader.ReadUInt32();
                notUsed5 = reader.ReadUInt32();
                notUsed6 = reader.ReadUInt32();
            }
            if (bitArray.Get(26))
            {
                RayHit = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            }
            if (bitArray.Get(27))
            {
                Hp = reader.ReadSingle();
            }
            if (bitArray.Get(28))
            {
                Mp = reader.ReadSingle();
            }
            if (bitArray.Get(29))
            {
                BlockPower = reader.ReadSingle();
            }
            if (bitArray.Get(30))
            {
                MaxHpMultiplier = reader.ReadSingle();
                ShootSpeed = reader.ReadSingle();
                DamageMultiplier = reader.ReadSingle();
                ArmorMultiplier = reader.ReadSingle();
                ResistanceMultiplier = reader.ReadSingle();
            }
            if (bitArray.Get(31))
            {
                notUsed7 = reader.ReadByte();
            }
            if (bitArray.Get(32))
            {
                notUsed8 = reader.ReadByte();
            }
            if (bitArray.Get(33))
            {
                Level = reader.ReadUInt32();
            }
            if (bitArray.Get(34))
            {
                CurrentXp = reader.ReadUInt32();
            }
            if (bitArray.Get(35))
            {
                ParentOwner = reader.ReadUInt64();
            }
            if (bitArray.Get(36))
            {
                unknownOrNotUsed1 = reader.ReadUInt32();
                unknownOrNotUsed2 = reader.ReadUInt32();
            }
            if (bitArray.Get(37))
            {
                unknownOrNotUsed3 = reader.ReadByte();
            }
            if (bitArray.Get(38))
            {
                unknownOrNotUsed4 = reader.ReadUInt32();
            }
            if (bitArray.Get(39))
            {
                unknownOrNotUsed5 = reader.ReadUInt32();
                notUsed11 = reader.ReadUInt32();
                notUsed12 = reader.ReadUInt32();
            }
            if (bitArray.Get(40))
            {
                notUsed13 = reader.ReadUInt32();
                notUsed14 = reader.ReadUInt32();
                notUsed15 = reader.ReadUInt32();
                notUsed16 = reader.ReadUInt32();
                notUsed17 = reader.ReadUInt32();
                notUsed18 = reader.ReadUInt32();
            }
            if (bitArray.Get(41))
            {
                notUsed20 = reader.ReadUInt32();
                notUsed21 = reader.ReadUInt32();
                notUsed22 = reader.ReadUInt32();
            }
            if (bitArray.Get(42))
            {
                notUsed19 = reader.ReadByte();
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
                Rotation = from.Rotation.Clone();
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
                ChargedMp = from.ChargedMp;
            }
            if (bitArray.Get(24))
            {
                notUsed1 = from.notUsed1;
                notUsed2 = from.notUsed2;
                notUsed3 = from.notUsed3;
            }
            if (bitArray.Get(25))
            {
                notUsed4 = from.notUsed4;
                notUsed5 = from.notUsed5;
                notUsed6 = from.notUsed6;
            }
            if (bitArray.Get(26))
            {
                RayHit = from.RayHit.Clone();
            }
            if (bitArray.Get(27))
            {
                Hp = from.Hp;
            }
            if (bitArray.Get(28))
            {
                Mp = from.Mp;
            }
            if (bitArray.Get(29))
            {
                BlockPower = from.BlockPower;
            }
            if (bitArray.Get(30))
            {
                MaxHpMultiplier = from.MaxHpMultiplier;
                ShootSpeed = from.ShootSpeed;
                DamageMultiplier = from.DamageMultiplier;
                ArmorMultiplier = from.ArmorMultiplier;
                ResistanceMultiplier = from.ResistanceMultiplier;
            }
            if (bitArray.Get(31))
            {
                notUsed7 = from.notUsed7;
            }
            if (bitArray.Get(32))
            {
                notUsed8 = from.notUsed8;
            }
            if (bitArray.Get(33))
            {
                Level = from.Level;
            }
            if (bitArray.Get(34))
            {
                CurrentXp = from.CurrentXp;
            }
            if (bitArray.Get(35))
            {
                ParentOwner = from.ParentOwner;
            }
            if (bitArray.Get(36))
            {
                unknownOrNotUsed1 = from.unknownOrNotUsed1;
                unknownOrNotUsed2 = from.unknownOrNotUsed2;
            }
            if (bitArray.Get(37))
            {
                unknownOrNotUsed3 = from.unknownOrNotUsed3;
            }
            if (bitArray.Get(38))
            {
                unknownOrNotUsed4 = from.unknownOrNotUsed4;
            }
            if (bitArray.Get(39))
            {
                unknownOrNotUsed5 = from.unknownOrNotUsed5;
                notUsed11 = from.notUsed11;
                notUsed12 = from.notUsed12;
            }
            if (bitArray.Get(40))
            {
                notUsed13 = from.notUsed13;
                notUsed14 = from.notUsed14;
                notUsed15 = from.notUsed15;
                notUsed16 = from.notUsed16;
                notUsed17 = from.notUsed17;
                notUsed18 = from.notUsed18;
            }
            if (bitArray.Get(41))
            {
                notUsed20 = from.notUsed20;
                notUsed21 = from.notUsed21;
                notUsed22 = from.notUsed22;
            }
            if (bitArray.Get(42))
            {
                notUsed19 = from.notUsed19;
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
                writer.Write(ChargedMp);
            }
            if (bitArray.Get(24))
            {
                writer.Write(notUsed1);
                writer.Write(notUsed2);
                writer.Write(notUsed3);
            }
            if (bitArray.Get(25))
            {
                writer.Write(notUsed4);
                writer.Write(notUsed5);
                writer.Write(notUsed6);
            }
            if (bitArray.Get(26))
            {
                RayHit.Write(writer);
            }
            if (bitArray.Get(27))
            {
                writer.Write(Hp);
            }
            if (bitArray.Get(28))
            {
                writer.Write(Mp);
            }
            if (bitArray.Get(29))
            {
                writer.Write(BlockPower);
            }
            if (bitArray.Get(30))
            {
                writer.Write(MaxHpMultiplier);
                writer.Write(ShootSpeed);
                writer.Write(DamageMultiplier);
                writer.Write(ArmorMultiplier);
                writer.Write(ResistanceMultiplier);
            }
            if (bitArray.Get(31))
            {
                writer.Write(notUsed7);
            }
            if (bitArray.Get(32))
            {
                writer.Write(notUsed8);
            }
            if (bitArray.Get(33))
            {
                writer.Write(Level);
            }
            if (bitArray.Get(34))
            {
                writer.Write(CurrentXp);
            }
            if (bitArray.Get(35))
            {
                writer.Write(ParentOwner);
            }
            if (bitArray.Get(36))
            {
                writer.Write(unknownOrNotUsed1);
                writer.Write(unknownOrNotUsed2);
            }
            if (bitArray.Get(37))
            {
                writer.Write(unknownOrNotUsed3);
            }
            if (bitArray.Get(38))
            {
                writer.Write(unknownOrNotUsed4);
            }
            if (bitArray.Get(39))
            {
                writer.Write(unknownOrNotUsed5);
                writer.Write(notUsed11);
                writer.Write(notUsed12);
            }
            if (bitArray.Get(40))
            {
                writer.Write(notUsed13);
                writer.Write(notUsed14);
                writer.Write(notUsed15);
                writer.Write(notUsed16);
                writer.Write(notUsed17);
                writer.Write(notUsed18);
            }
            if (bitArray.Get(41))
            {
                writer.Write(notUsed20);
                writer.Write(notUsed21);
                writer.Write(notUsed22);
            }
            if (bitArray.Get(42))
            {
                writer.Write(notUsed19);
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
                writer.Write(Encoding.UTF8.GetBytes(Name));
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
