using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coob.Structures
{
    public class Entity
    {
        #region Fields
        public long ID;
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

        public object unknown_or_not_used1;
        public object unknown_or_not_used2;
        public object unknown_or_not_used3;
        public object unknown_or_not_used4;
        public object unknown_or_not_used5;
        public object not_used1;
        public object not_used2;
        public object not_used3;
        public object not_used4;
        public object not_used5;
        public object not_used6;
        public object not_used7;
        public object not_used8;
        public object not_used9;
        public object not_used10;
        public object not_used11;
        public object not_used12;
        public object not_used13;
        public object not_used14;
        public object not_used15;
        public object not_used16;
        public object not_used17;
        public object not_used18;
        public object not_used20;
        public object not_used21;
        public object not_used22;
        public object not_used19;

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
            for(int i = 0; i < 13; i++)
                Equipment[i] = new Item();

            Skills = new uint[11];
        }

        public void ReadMaskedValues(BinaryReader reader)
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
                not_used9 = reader.ReadUInt32();
                not_used10 = reader.ReadUInt32();
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

        public void CopyValuesByMask(Entity from)
        {
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
                not_used9 = from.not_used9;
                not_used10 = from.not_used10;
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
    }

    public class Appearance
    {
        public byte NotUsed1, NotUsed2;
        public byte HairR, HairG, HairB;
        public byte MovementFlags, EntityFlags;
        public float Scale;
        public float BoundingRadius;
        public float BoundingHeight;
        public ushort HeadModel, HairModel, HandModel, FootModel, BodyModel, BackModel, ShoulderModel, WingModel;
        public float HeadScale, BodyScale, HandScale, FootScale, ShoulderScale, WeaponScale, BackScale, Unknown, WingScale;
        public float BodyPitch, ArmPitch, ArmRoll, ArmYaw;
        public float FeetPitch, WingPitch, BackPitch;
        public Vector3 BodyOffset, HeadOffset, HandOffset, FootOffset, BackOffset, WingOffset;

        public Appearance()
        {
        }

        public void Read(BinaryReader reader)
        {
            NotUsed1 = reader.ReadByte();
            NotUsed2 = reader.ReadByte();
            HairR = reader.ReadByte();
            HairG = reader.ReadByte();
            HairB = reader.ReadByte();
            reader.ReadByte(); // skip 1
            MovementFlags = reader.ReadByte();
            EntityFlags = reader.ReadByte();
            Scale = reader.ReadSingle();
            BoundingRadius = reader.ReadSingle();
            BoundingHeight = reader.ReadSingle();
            HeadModel = reader.ReadUInt16();
            HairModel = reader.ReadUInt16();
            HandModel = reader.ReadUInt16();
            FootModel = reader.ReadUInt16();
            BodyModel = reader.ReadUInt16();
            BackModel = reader.ReadUInt16();
            ShoulderModel = reader.ReadUInt16();
            WingModel = reader.ReadUInt16();
            HeadScale = reader.ReadSingle();
            BodyScale = reader.ReadSingle();
            HandScale = reader.ReadSingle();
            FootScale = reader.ReadSingle();
            ShoulderScale = reader.ReadSingle();
            WeaponScale = reader.ReadSingle();
            BackScale = reader.ReadSingle();
            Unknown = reader.ReadSingle();
            WingScale = reader.ReadSingle();
            BodyPitch = reader.ReadSingle();
            ArmPitch = reader.ReadSingle();
            ArmRoll = reader.ReadSingle();
            ArmYaw = reader.ReadSingle();
            FeetPitch = reader.ReadSingle();
            WingPitch = reader.ReadSingle();
            BackPitch = reader.ReadSingle();
            BodyOffset = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            HeadOffset = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            HandOffset = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            FootOffset = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            BackOffset = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
            WingOffset = new Vector3 { X = reader.ReadSingle(), Y = reader.ReadSingle(), Z = reader.ReadSingle() };
        }

        public void ReadNet(NetReader reader)
        {
            NotUsed1 = reader.ReadByte();
            NotUsed2 = reader.ReadByte();
            HairR = reader.ReadByte();
            HairG = reader.ReadByte();
            HairB = reader.ReadByte();
            reader.ReadByte(); // skip 1
            MovementFlags = reader.ReadByte();
            EntityFlags = reader.ReadByte();
            Scale = reader.ReadFloat();
            BoundingRadius = reader.ReadFloat();
            BoundingHeight = reader.ReadFloat();
            HeadModel = reader.ReadUShort();
            HairModel = reader.ReadUShort();
            HandModel = reader.ReadUShort();
            FootModel = reader.ReadUShort();
            BodyModel = reader.ReadUShort();
            BackModel = reader.ReadUShort();
            ShoulderModel = reader.ReadUShort();
            WingModel = reader.ReadUShort();
            HeadScale = reader.ReadFloat();
            BodyScale = reader.ReadFloat();
            HandScale = reader.ReadFloat();
            FootScale = reader.ReadFloat();
            ShoulderScale = reader.ReadFloat();
            WeaponScale = reader.ReadFloat();
            BackScale = reader.ReadFloat();
            Unknown = reader.ReadFloat();
            WingScale = reader.ReadFloat();
            BodyPitch = reader.ReadFloat();
            ArmPitch = reader.ReadFloat();
            ArmRoll = reader.ReadFloat();
            ArmYaw = reader.ReadFloat();
            FeetPitch = reader.ReadFloat();
            WingPitch = reader.ReadFloat();
            BackPitch = reader.ReadFloat();
            BodyOffset = new Vector3 { X = reader.ReadFloat(), Y = reader.ReadFloat(), Z = reader.ReadFloat() };
            HeadOffset = new Vector3 { X = reader.ReadFloat(), Y = reader.ReadFloat(), Z = reader.ReadFloat() };
            HandOffset = new Vector3 { X = reader.ReadFloat(), Y = reader.ReadFloat(), Z = reader.ReadFloat() };
            FootOffset = new Vector3 { X = reader.ReadFloat(), Y = reader.ReadFloat(), Z = reader.ReadFloat() };
            BackOffset = new Vector3 { X = reader.ReadFloat(), Y = reader.ReadFloat(), Z = reader.ReadFloat() };
            WingOffset = new Vector3 { X = reader.ReadFloat(), Y = reader.ReadFloat(), Z = reader.ReadFloat() };
        }

        public void CopyFrom(Appearance from)
        {
            if (from == null) return;

            NotUsed1 =      from.NotUsed1;
            NotUsed2 =      from.NotUsed2;
            HairR =         from.HairR;
            HairG =         from.HairG;
            HairB =         from.HairB;
            MovementFlags = from.MovementFlags;
            EntityFlags =   from.EntityFlags;
            Scale =         from.Scale;
            BoundingRadius = from.BoundingRadius;
            BoundingHeight = from.BoundingHeight;
            HeadModel =     from.HeadModel;
            HairModel =     from.HairModel;
            HandModel =     from.HandModel;
            FootModel =     from.FootModel;
            BodyModel =     from.BodyModel;
            BackModel =     from.BackModel;
            ShoulderModel = from.ShoulderModel;
            WingModel =     from.WingModel;
            HeadScale =     from.HeadScale;
            BodyScale =     from.BodyScale;
            HandScale =     from.HandScale;
            FootScale =     from.FootScale;
            ShoulderScale = from.ShoulderScale;
            WeaponScale =   from.WeaponScale;
            BackScale =     from.BackScale;
            Unknown =       from.Unknown;
            WingScale =     from.WingScale;
            BodyPitch =     from.BodyPitch;
            ArmPitch =      from.ArmPitch;
            ArmRoll =       from.ArmRoll;
            ArmYaw =        from.ArmYaw;
            FeetPitch =     from.FeetPitch;
            WingPitch =     from.WingPitch;
            BackPitch =     from.BackPitch;
            BodyOffset =    from.BodyOffset.Clone();
            HeadOffset =    from.HeadOffset.Clone();
            HandOffset =    from.HandOffset.Clone();
            FootOffset =    from.FootOffset.Clone();
            BackOffset =    from.BackOffset.Clone();
            WingOffset =    from.WingOffset.Clone();
        }
    }

    public class Item
    {
        public byte Type, SubType;
        public uint Modifier;
        public uint MinusModifier;
        public byte Rarity, Material, Flags;
        public ushort Level;
        public ItemUpgrade[] Upgrades;
        public uint UpgradeCount;

        public Item()
        {
            Upgrades = new ItemUpgrade[32];
        }

        public void Read(BinaryReader reader)
        {
            Type = reader.ReadByte();
            SubType = reader.ReadByte();
            reader.ReadInt16(); // skip 2
            Modifier = reader.ReadUInt32();
            MinusModifier = reader.ReadUInt32();
            Rarity = reader.ReadByte();
            Material = reader.ReadByte();
            Flags = reader.ReadByte();
            reader.ReadByte(); // skip 1
            Level = reader.ReadUInt16();
            reader.ReadInt16(); // skip 2
            for (int i = 0; i < 32; i++)
            {
                Upgrades[i] = new ItemUpgrade();
                Upgrades[i].Read(reader);
            }
            UpgradeCount = reader.ReadUInt32();
            
        }

        public void ReadNet(NetReader reader)
        {
            Type = reader.ReadByte();
            SubType = reader.ReadByte();
            reader.ReadShort(); // skip 2
            Modifier = reader.ReadUInt();
            MinusModifier = reader.ReadUInt();
            Rarity = reader.ReadByte();
            Material = reader.ReadByte();
            Flags = reader.ReadByte();
            reader.ReadByte(); // skip 1
            Level = reader.ReadUShort();
            reader.ReadShort(); // skip 2
            for (int i = 0; i < 32; i++)
            {
                Upgrades[i] = new ItemUpgrade();
                Upgrades[i].ReadNet(reader);
            }
            UpgradeCount = reader.ReadUInt();
        }

        public void CopyFrom(Item from)
        {
            if (from == null) return;

            Type = from.Type;
            SubType = from.SubType;
            Modifier = from.Modifier;
            MinusModifier = from.MinusModifier;
            Rarity = from.Rarity;
            Material = from.Material;
            Flags = from.Flags;
            Level = from.Level;
            UpgradeCount = from.UpgradeCount;
            for (int i = 0; i < 32; i++)
            {
                Upgrades[i] = new ItemUpgrade();
                Upgrades[i].CopyFrom(from.Upgrades[i]);
            }
        }
    }

    public class ItemUpgrade
    {
        public byte X, Y, Z, Material;
        public uint Level;

        public void Read(BinaryReader reader)
        {
            X = reader.ReadByte();
            Y = reader.ReadByte();
            Z = reader.ReadByte();
            Material = reader.ReadByte();
            Level = reader.ReadUInt32();
        }

        public void ReadNet(NetReader reader)
        {
            X = reader.ReadByte();
            Y = reader.ReadByte();
            Z = reader.ReadByte();
            Material = reader.ReadByte();
            Level = reader.ReadUInt();
        }

        public void CopyFrom(ItemUpgrade from)
        {
            if (from == null) return;

            X = from.X;
            Y = from.Y;
            Z = from.Z;
            Material = from.Material;
            Level = from.Level;
        }
    }
}