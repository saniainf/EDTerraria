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
using Terraria.Utilities;

namespace Terraria.IO
{
    public class WorldFileData : FileData
    {
        public bool IsValid = true;
        public bool HasCorruption = true;
        public DateTime CreationTime;
        public int WorldSizeX;
        public int WorldSizeY;
        public string _worldSizeName;
        public bool IsExpertMode;
        public bool IsHardMode;

        public string WorldSizeName
        {
            get
            {
                return this._worldSizeName;
            }
        }

        public bool HasCrimson
        {
            get
            {
                return !this.HasCorruption;
            }
            set
            {
                this.HasCorruption = !value;
            }
        }

        public WorldFileData()
            : base("World")
        {
        }

        public WorldFileData(string path, bool cloudSave)
            : base("World", path, cloudSave)
        {
        }

        public override void SetAsActive()
        {
            Main.ActiveWorldFileData = this;
        }

        public void SetWorldSize(int x, int y)
        {
            this.WorldSizeX = x;
            this.WorldSizeY = y;
            switch (x)
            {
                case 4200:
                    this._worldSizeName = "Small";
                    break;
                case 6400:
                    this._worldSizeName = "Medium";
                    break;
                case 8400:
                    this._worldSizeName = "Large";
                    break;
                default:
                    this._worldSizeName = "Unknown";
                    break;
            }
        }

        public static WorldFileData FromInvalidWorld(string path, bool cloudSave)
        {
            WorldFileData worldFileData = new WorldFileData(path, cloudSave);
            worldFileData.IsExpertMode = false;
            worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
            worldFileData.SetWorldSize(1, 1);
            worldFileData.HasCorruption = true;
            worldFileData.IsHardMode = false;
            worldFileData.IsValid = false;
            worldFileData.Name = FileUtilities.GetFileName(path, false);
            worldFileData.CreationTime = cloudSave ? DateTime.Now : File.GetCreationTime(path);
            return worldFileData;
        }
    }
}
