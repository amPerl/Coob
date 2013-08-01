using System.IO;

namespace Coob.Structures
{
    /* Structure info mostly stolen from mat^2 ;) */

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
            BodyOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            HeadOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            HandOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            FootOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            BackOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            WingOffset = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public void CopyFrom(Appearance from)
        {
            if (from == null)
                return;

            NotUsed1 = from.NotUsed1;
            NotUsed2 = from.NotUsed2;
            HairR = from.HairR;
            HairG = from.HairG;
            HairB = from.HairB;
            MovementFlags = from.MovementFlags;
            EntityFlags = from.EntityFlags;
            Scale = from.Scale;
            BoundingRadius = from.BoundingRadius;
            BoundingHeight = from.BoundingHeight;
            HeadModel = from.HeadModel;
            HairModel = from.HairModel;
            HandModel = from.HandModel;
            FootModel = from.FootModel;
            BodyModel = from.BodyModel;
            BackModel = from.BackModel;
            ShoulderModel = from.ShoulderModel;
            WingModel = from.WingModel;
            HeadScale = from.HeadScale;
            BodyScale = from.BodyScale;
            HandScale = from.HandScale;
            FootScale = from.FootScale;
            ShoulderScale = from.ShoulderScale;
            WeaponScale = from.WeaponScale;
            BackScale = from.BackScale;
            Unknown = from.Unknown;
            WingScale = from.WingScale;
            BodyPitch = from.BodyPitch;
            ArmPitch = from.ArmPitch;
            ArmRoll = from.ArmRoll;
            ArmYaw = from.ArmYaw;
            FeetPitch = from.FeetPitch;
            WingPitch = from.WingPitch;
            BackPitch = from.BackPitch;
            BodyOffset = from.BodyOffset.Clone();
            HeadOffset = from.HeadOffset.Clone();
            HandOffset = from.HandOffset.Clone();
            FootOffset = from.FootOffset.Clone();
            BackOffset = from.BackOffset.Clone();
            WingOffset = from.WingOffset.Clone();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(NotUsed1);
            writer.Write(NotUsed2);
            writer.Write(HairR);
            writer.Write(HairG);
            writer.Write(HairB);
            writer.Write((byte)0);
            writer.Write(MovementFlags);
            writer.Write(EntityFlags);
            writer.Write(Scale);
            writer.Write(BoundingRadius);
            writer.Write(BoundingHeight);
            writer.Write(HeadModel);
            writer.Write(HairModel);
            writer.Write(HandModel);
            writer.Write(FootModel);
            writer.Write(BodyModel);
            writer.Write(BackModel);
            writer.Write(ShoulderModel);
            writer.Write(WingModel);
            writer.Write(HeadScale);
            writer.Write(BodyScale);
            writer.Write(HandScale);
            writer.Write(FootScale);
            writer.Write(ShoulderScale);
            writer.Write(WeaponScale);
            writer.Write(BackScale);
            writer.Write(Unknown);
            writer.Write(WingScale);
            writer.Write(BodyPitch);
            writer.Write(ArmPitch);
            writer.Write(ArmRoll);
            writer.Write(ArmYaw);
            writer.Write(FeetPitch);
            writer.Write(WingPitch);
            writer.Write(BackPitch);
            BodyOffset.Write(writer);
            HeadOffset.Write(writer);
            HandOffset.Write(writer);
            FootOffset.Write(writer);
            BackOffset.Write(writer);
            WingOffset.Write(writer);
        }
    }
}
