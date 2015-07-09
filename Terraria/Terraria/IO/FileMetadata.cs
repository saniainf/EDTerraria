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
using System.IO;
using Terraria;

namespace Terraria.IO
{
    public class FileMetadata
    {
        public const ulong MAGIC_NUMBER = 27981915666277746UL;
        public const int SIZE = 20;
        public FileType Type;
        public uint Revision;
        public bool IsFavorite;

        private FileMetadata()
        {
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((ulong)(27981915666277746L | (long)this.Type << 56));
            writer.Write(this.Revision);
            writer.Write((ulong)(Utils.ToInt(this.IsFavorite) & 1));
        }

        public void IncrementAndWrite(BinaryWriter writer)
        {
            ++this.Revision;
            this.Write(writer);
        }

        public static FileMetadata FromCurrentSettings(FileType type)
        {
            return new FileMetadata()
            {
                Type = type,
                Revision = 0U,
                IsFavorite = false
            };
        }

        public static FileMetadata Read(BinaryReader reader, FileType expectedType)
        {
            FileMetadata fileMetadata = new FileMetadata();
            fileMetadata.Read(reader);
            if (fileMetadata.Type != expectedType)
                throw new Exception("Expected type \"" + Enum.GetName(typeof(FileType), (object)expectedType) + "\" but found \"" + Enum.GetName(typeof(FileType), (object)fileMetadata.Type) + "\".");
            return fileMetadata;
        }

        private void Read(BinaryReader reader)
        {
            ulong num1 = reader.ReadUInt64();
            if (((long)num1 & 72057594037927935L) != 27981915666277746L)
                throw new Exception("Expected Re-Logic file format.");
            byte num2 = (byte)(num1 >> 56 & (ulong)byte.MaxValue);
            FileType fileType = FileType.None;
            FileType[] fileTypeArray = (FileType[])Enum.GetValues(typeof(FileType));
            for (int index = 0; index < fileTypeArray.Length; ++index)
            {
                if (fileTypeArray[index] == (FileType)num2)
                {
                    fileType = fileTypeArray[index];
                    break;
                }
            }
            if (fileType == FileType.None)
                throw new Exception("Found invalid file type.");
            this.Type = fileType;
            this.Revision = reader.ReadUInt32();
            this.IsFavorite = ((long)reader.ReadUInt64() & 1L) == 1L;
        }
    }
}
