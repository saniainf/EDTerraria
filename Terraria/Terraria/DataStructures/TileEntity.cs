/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;
using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.Tile_Entities;

namespace Terraria.DataStructures
{
    public abstract class TileEntity
    {
        public static Dictionary<int, TileEntity> ByID = new Dictionary<int, TileEntity>();
        public static Dictionary<Point16, TileEntity> ByPosition = new Dictionary<Point16, TileEntity>();
        public static int TileEntitiesNextID = 0;
        public const int MaxEntitiesPerChunk = 1000;
        public int ID;
        public Point16 Position;
        public byte type;

        public static event Action _UpdateStart;
        public static event Action _UpdateEnd;

        public static int AssignNewID()
        {
            return TileEntitiesNextID++;
        }

        public static void UpdateStart()
        {
            if (_UpdateStart == null)
                return;
            _UpdateStart();
        }

        public static void UpdateEnd()
        {
            if (_UpdateEnd == null)
                return;
            _UpdateEnd();
        }

        public static void InitializeAll()
        {
            TETrainingDummy.Initialize();
        }

        public virtual void Update() { }

        public static void Write(BinaryWriter writer, TileEntity ent)
        {
            writer.Write(ent.type);
            ent.WriteInner(writer);
        }

        public static TileEntity Read(BinaryReader reader)
        {
            TileEntity tileEntity = null;
            byte num = reader.ReadByte();
            switch (num)
            {
                case (byte)0:
                    tileEntity = (TileEntity)new TETrainingDummy();
                    break;
                case (byte)1:
                    tileEntity = (TileEntity)new TEItemFrame();
                    break;
            }

            tileEntity.type = num;
            tileEntity.ReadInner(reader);
            return tileEntity;
        }

        private void WriteInner(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            WriteExtraData(writer);
        }

        private void ReadInner(BinaryReader reader)
        {
            ID = reader.ReadInt32();
            Position = new Point16(reader.ReadInt16(), reader.ReadInt16());
            ReadExtraData(reader);
        }

        public virtual void WriteExtraData(BinaryWriter writer) { }

        public virtual void ReadExtraData(BinaryReader reader) { }
    }
}