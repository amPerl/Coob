using System.IO;

namespace Coob.Structures
{
    /* Structure info mostly stolen from mat^2 ;) */

    public class Item
    {
        public byte Type, SubType;
        public uint Modifier;
        public uint MinusModifier;
        public byte Rarity, Material, Flags;
        public short Level;
        public ItemUpgrade[] Upgrades;
        public uint UpgradeCount;

        public Item()
        {
            Upgrades = new ItemUpgrade[32];
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

        public void Write(BinaryWriter writer)
        {
            writer.Write(Type);
            writer.Write(SubType);

            writer.Write((short)0); // skip 2

            writer.Write(Modifier);
            writer.Write(MinusModifier);
            writer.Write(Rarity);
            writer.Write(Material);
            writer.Write(Flags);

            writer.Write((byte)0); // skip 1

            writer.Write(Level);

            writer.Write((short)0); // skip 2

            foreach (ItemUpgrade t in Upgrades)
            {
                t.Write(writer);
            }
            
            writer.Write(UpgradeCount);
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
            Level = reader.ReadInt16();
            reader.ReadInt16(); // skip 2
            
            for (int i = 0; i < Upgrades.Length; ++i)
            {
                Upgrades[i] = new ItemUpgrade();
                Upgrades[i].Read(reader);
            }

            UpgradeCount = reader.ReadUInt32();
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

        public void CopyFrom(ItemUpgrade from)
        {
            if (from == null)
                return;

            X = from.X;
            Y = from.Y;
            Z = from.Z;
            Material = from.Material;
            Level = from.Level;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Material);
            writer.Write(Level);
        }
    }
}
