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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.IO
{
    internal class WorldFile
    {
        private static bool? CachedBloodMoon = null;
        private static int? CachedCultistDelay = null;
        private static bool? CachedDayTime = null;
        private static bool? CachedEclipse = null;
        private static int? CachedMoonPhase = null;
        private static double? CachedTime = null;
        private static bool HasCache = false;
        public static bool IsWorldOnCloud = false;
        private static object padlock = new object();
        public static bool tempBloodMoon = Main.bloodMoon;
        public static int tempCultistDelay = CultistRitual.delay;
        public static bool tempDayTime = Main.dayTime;
        public static bool tempEclipse = Main.eclipse;
        public static float tempMaxRain = 0f;
        public static int tempMoonPhase = Main.moonPhase;
        public static bool tempRaining = false;
        public static int tempRainTime = 0;
        public static double tempTime = Main.time;
        public static int versionNumber;

        public static event Action OnWorldLoad;

        public static void CacheSaveTime()
        {
            HasCache = true;
            CachedDayTime = new bool?(Main.dayTime);
            CachedTime = new double?(Main.time);
            CachedMoonPhase = new int?(Main.moonPhase);
            CachedBloodMoon = new bool?(Main.bloodMoon);
            CachedEclipse = new bool?(Main.eclipse);
            CachedCultistDelay = new int?(CultistRitual.delay);
        }

        public static WorldFileData CreateMetadata(string name, bool cloudSave, bool isExpertMode)
        {
            WorldFileData data = new WorldFileData(Main.GetWorldPathFromName(name, cloudSave))
            {
                Name = name,
                IsExpertMode = isExpertMode,
                CreationTime = DateTime.Now,
                Metadata = FileMetadata.FromCurrentSettings(FileType.World)
            };
            data.SetFavorite(false, true);
            return data;
        }

        public static void FixDresserChests()
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile tile = Main.tile[i, j];
                    if ((tile.active() && (tile.type == 0x58)) && (((tile.frameX % 0x36) == 0) && ((tile.frameY % 0x24) == 0)))
                    {
                        Chest.CreateChest(i, j, -1);
                    }
                }
            }
        }

        public static WorldFileData GetAllMetadata(string file)
        {
            if (file != null)
            {
                WorldFileData data = new WorldFileData(file);
                if (!FileUtilities.Exists(file))
                {
                    data.CreationTime = DateTime.Now;
                    data.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
                    return data;
                }
                try
                {
                    using (Stream stream = new FileStream(file, FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            int num = reader.ReadInt32();
                            if (num >= 0x87)
                                data.Metadata = FileMetadata.Read(reader, FileType.World);
                            else
                                data.Metadata = FileMetadata.FromCurrentSettings(FileType.World);

                            if (num <= Main.curRelease)
                            {
                                reader.ReadInt16();
                                stream.Position = reader.ReadInt32();
                                data.Name = reader.ReadString();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                int y = reader.ReadInt32();
                                int x = reader.ReadInt32();
                                data.SetWorldSize(x, y);
                                data.IsExpertMode = (num >= 0x70) && reader.ReadBoolean();
                                if (num >= 0x8d)
                                    data.CreationTime = DateTime.FromBinary(reader.ReadInt64());
                                else
                                    data.CreationTime = DateTime.Now;
                                data.CreationTime = File.GetCreationTime(file);
                                reader.ReadByte();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadDouble();
                                reader.ReadDouble();
                                reader.ReadDouble();
                                reader.ReadBoolean();
                                reader.ReadInt32();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                data.HasCrimson = reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                if (num >= 0x76)
                                {
                                    reader.ReadBoolean();
                                }
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadBoolean();
                                reader.ReadByte();
                                reader.ReadInt32();
                                data.IsHardMode = reader.ReadBoolean();
                                return data;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        public static FileMetadata GetFileMetadata(string file, bool cloudSave)
        {
            if (file != null)
            {
                try
                {
                    using (Stream stream = (new FileStream(file, FileMode.Open)))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            if (reader.ReadInt32() >= 0x87)
                                return FileMetadata.Read(reader, FileType.World);

                            return FileMetadata.FromCurrentSettings(FileType.World);
                        }
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static bool GetWorldDifficulty(string WorldFileName)
        {
            if (WorldFileName != null)
            {
                try
                {
                    using (FileStream stream = new FileStream(WorldFileName, FileMode.Open))
                    {
                        using (BinaryReader reader = new BinaryReader(stream))
                        {
                            int num = reader.ReadInt32();
                            if (num >= 0x87)
                            {
                                Stream baseStream = reader.BaseStream;
                                baseStream.Position += 20L;
                            }
                            if ((num >= 0x70) && (num <= Main.curRelease))
                            {
                                reader.ReadInt16();
                                stream.Position = reader.ReadInt32();
                                reader.ReadString();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                reader.ReadInt32();
                                return reader.ReadBoolean();
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        public static string GetWorldName(string WorldFileName)
        {
            if (WorldFileName == null)
            {
                return string.Empty;
            }
            try
            {
                using (FileStream stream = new FileStream(WorldFileName, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int num = reader.ReadInt32();
                        if ((num > 0) && (num <= Main.curRelease))
                        {
                            string str;
                            if (num <= 0x57)
                            {
                                str = reader.ReadString();
                                reader.Close();
                                return str;
                            }
                            if (num >= 0x87)
                            {
                                Stream baseStream = reader.BaseStream;
                                baseStream.Position += 20L;
                            }
                            reader.ReadInt16();
                            stream.Position = reader.ReadInt32();
                            str = reader.ReadString();
                            reader.Close();
                            return str;
                        }
                    }
                }
            }
            catch
            {
            }
            string[] strArray = WorldFileName.Split(new char[] { Path.DirectorySeparatorChar });
            string str2 = strArray[strArray.Length - 1];
            return str2.Substring(0, str2.Length - 4);
        }

        public static bool IsValidWorld(string file, bool cloudSave)
        {
            return (GetFileMetadata(file, cloudSave) != null);
        }

        private static void LoadChests(BinaryReader reader)
        {
            Chest chest = null;
            int num6;
            int num7;
            int num4 = reader.ReadInt16();
            int num5 = reader.ReadInt16();
            if (num5 < 40)
            {
                num6 = num5;
                num7 = 0;
            }
            else
            {
                num6 = 40;
                num7 = num5 - 40;
            }
            int index = 0;
            while (index < num4)
            {
                chest = new Chest(false)
                {
                    x = reader.ReadInt32(),
                    y = reader.ReadInt32(),
                    name = reader.ReadString()
                };
                int num3 = 0;
                while (num3 < num6)
                {
                    short num = reader.ReadInt16();
                    Item item = new Item();
                    if (num > 0)
                    {
                        item.netDefaults(reader.ReadInt32());
                        item.stack = num;
                        item.Prefix(reader.ReadByte());
                    }
                    else if (num < 0)
                    {
                        item.netDefaults(reader.ReadInt32());
                        item.Prefix(reader.ReadByte());
                        item.stack = 1;
                    }
                    chest.item[num3] = item;
                    num3++;
                }
                for (num3 = 0; num3 < num7; num3++)
                {
                    if (reader.ReadInt16() > 0)
                    {
                        reader.ReadInt32();
                        reader.ReadByte();
                    }
                }
                Main.chest[index] = chest;
                index++;
            }
            while (index < 0x3e8)
            {
                Main.chest[index] = null;
                index++;
            }
            if (versionNumber < 0x73)
            {
                FixDresserChests();
            }
        }

        private static void LoadDummies(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                TargetDummy.dummies[i] = new TargetDummy(reader.ReadInt16(), reader.ReadInt16());
            }
            for (int j = num; j < 0x3e8; j++)
            {
                TargetDummy.dummies[j] = null;
            }
        }

        private static bool LoadFileFormatHeader(BinaryReader reader, out bool[] importance, out int[] positions)
        {
            short num2;
            int num3;
            importance = null;
            positions = null;
            int num4 = reader.ReadInt32();
            versionNumber = num4;
            if (num4 >= 0x87)
            {
                try
                {
                    Main.WorldFileMetadata = FileMetadata.Read(reader, FileType.World);
                    goto Label_004F;
                }
                catch
                {
                    Console.WriteLine("Unable to load world:");
                    return false;
                }
            }
            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
        Label_004F:
            num2 = reader.ReadInt16();
            positions = new int[num2];
            for (num3 = 0; num3 < num2; num3++)
            {
                positions[num3] = reader.ReadInt32();
            }
            short num = reader.ReadInt16();
            importance = new bool[num];
            byte num5 = 0;
            byte num6 = 0x80;
            for (num3 = 0; num3 < num; num3++)
            {
                if (num6 == 0x80)
                {
                    num5 = reader.ReadByte();
                    num6 = 1;
                }
                else
                {
                    num6 = (byte)(num6 << 1);
                }
                if ((num5 & num6) == num6)
                {
                    importance[num3] = true;
                }
            }
            return true;
        }

        private static int LoadFooter(BinaryReader reader)
        {
            if (!reader.ReadBoolean())
            {
                return 6;
            }
            if (reader.ReadString() != Main.worldName)
            {
                return 6;
            }
            if (reader.ReadInt32() != Main.worldID)
            {
                return 6;
            }
            return 0;
        }

        private static void LoadHeader(BinaryReader reader)
        {
            int versionNumber = WorldFile.versionNumber;
            Main.worldName = reader.ReadString();
            Main.worldID = reader.ReadInt32();
            Main.leftWorld = reader.ReadInt32();
            Main.rightWorld = reader.ReadInt32();
            Main.topWorld = reader.ReadInt32();
            Main.bottomWorld = reader.ReadInt32();
            Main.maxTilesY = reader.ReadInt32();
            Main.maxTilesX = reader.ReadInt32();
            WorldGen.clearWorld();
            if (versionNumber >= 0x70)
            {
                Main.expertMode = reader.ReadBoolean();
            }
            else
            {
                Main.expertMode = false;
            }
            if (versionNumber >= 0x8d)
            {
                Main.ActiveWorldFileData.CreationTime = DateTime.FromBinary(reader.ReadInt64());
            }
            Main.moonType = reader.ReadByte();
            Main.treeX[0] = reader.ReadInt32();
            Main.treeX[1] = reader.ReadInt32();
            Main.treeX[2] = reader.ReadInt32();
            Main.treeStyle[0] = reader.ReadInt32();
            Main.treeStyle[1] = reader.ReadInt32();
            Main.treeStyle[2] = reader.ReadInt32();
            Main.treeStyle[3] = reader.ReadInt32();
            Main.caveBackX[0] = reader.ReadInt32();
            Main.caveBackX[1] = reader.ReadInt32();
            Main.caveBackX[2] = reader.ReadInt32();
            Main.caveBackStyle[0] = reader.ReadInt32();
            Main.caveBackStyle[1] = reader.ReadInt32();
            Main.caveBackStyle[2] = reader.ReadInt32();
            Main.caveBackStyle[3] = reader.ReadInt32();
            Main.iceBackStyle = reader.ReadInt32();
            Main.jungleBackStyle = reader.ReadInt32();
            Main.hellBackStyle = reader.ReadInt32();
            Main.spawnTileX = reader.ReadInt32();
            Main.spawnTileY = reader.ReadInt32();
            Main.worldSurface = reader.ReadDouble();
            Main.rockLayer = reader.ReadDouble();
            tempTime = reader.ReadDouble();
            tempDayTime = reader.ReadBoolean();
            tempMoonPhase = reader.ReadInt32();
            tempBloodMoon = reader.ReadBoolean();
            tempEclipse = reader.ReadBoolean();
            Main.eclipse = tempEclipse;
            Main.dungeonX = reader.ReadInt32();
            Main.dungeonY = reader.ReadInt32();
            WorldGen.crimson = reader.ReadBoolean();
            NPC.downedBoss1 = reader.ReadBoolean();
            NPC.downedBoss2 = reader.ReadBoolean();
            NPC.downedBoss3 = reader.ReadBoolean();
            NPC.downedQueenBee = reader.ReadBoolean();
            NPC.downedMechBoss1 = reader.ReadBoolean();
            NPC.downedMechBoss2 = reader.ReadBoolean();
            NPC.downedMechBoss3 = reader.ReadBoolean();
            NPC.downedMechBossAny = reader.ReadBoolean();
            NPC.downedPlantBoss = reader.ReadBoolean();
            NPC.downedGolemBoss = reader.ReadBoolean();
            if (versionNumber >= 0x76)
            {
                NPC.downedSlimeKing = reader.ReadBoolean();
            }
            NPC.savedGoblin = reader.ReadBoolean();
            NPC.savedWizard = reader.ReadBoolean();
            NPC.savedMech = reader.ReadBoolean();
            NPC.downedGoblins = reader.ReadBoolean();
            NPC.downedClown = reader.ReadBoolean();
            NPC.downedFrost = reader.ReadBoolean();
            NPC.downedPirates = reader.ReadBoolean();
            WorldGen.shadowOrbSmashed = reader.ReadBoolean();
            WorldGen.spawnMeteor = reader.ReadBoolean();
            WorldGen.shadowOrbCount = reader.ReadByte();
            WorldGen.altarCount = reader.ReadInt32();
            Main.hardMode = reader.ReadBoolean();
            Main.invasionDelay = reader.ReadInt32();
            Main.invasionSize = reader.ReadInt32();
            Main.invasionType = reader.ReadInt32();
            Main.invasionX = reader.ReadDouble();
            if (versionNumber >= 0x76)
            {
                Main.slimeRainTime = reader.ReadDouble();
            }
            if (versionNumber >= 0x71)
            {
                Main.sundialCooldown = reader.ReadByte();
            }
            tempRaining = reader.ReadBoolean();
            tempRainTime = reader.ReadInt32();
            tempMaxRain = reader.ReadSingle();
            WorldGen.oreTier1 = reader.ReadInt32();
            WorldGen.oreTier2 = reader.ReadInt32();
            WorldGen.oreTier3 = reader.ReadInt32();
            WorldGen.setBG(0, reader.ReadByte());
            WorldGen.setBG(1, reader.ReadByte());
            WorldGen.setBG(2, reader.ReadByte());
            WorldGen.setBG(3, reader.ReadByte());
            WorldGen.setBG(4, reader.ReadByte());
            WorldGen.setBG(5, reader.ReadByte());
            WorldGen.setBG(6, reader.ReadByte());
            WorldGen.setBG(7, reader.ReadByte());
            Main.cloudBGActive = reader.ReadInt32();
            Main.cloudBGAlpha = (Main.cloudBGActive < 1.0) ? 0f : 1f;
            Main.cloudBGActive = -WorldGen.genRand.Next(0x21c0, 0x15180);
            Main.numClouds = reader.ReadInt16();
            Main.windSpeedSet = reader.ReadSingle();
            Main.windSpeed = Main.windSpeedSet;
            if (versionNumber >= 0x5f)
            {
                Main.anglerWhoFinishedToday.Clear();
                for (int i = reader.ReadInt32(); i > 0; i--)
                {
                    Main.anglerWhoFinishedToday.Add(reader.ReadString());
                }
                if (versionNumber >= 0x63)
                {
                    NPC.savedAngler = reader.ReadBoolean();
                    if (versionNumber >= 0x65)
                    {
                        Main.anglerQuest = reader.ReadInt32();
                        if (versionNumber >= 0x68)
                        {
                            NPC.savedStylist = reader.ReadBoolean();
                            if (versionNumber >= 0x81)
                            {
                                NPC.savedTaxCollector = reader.ReadBoolean();
                            }
                            if (versionNumber < 0x6b)
                            {
                                if ((Main.invasionType > 0) && (Main.invasionSize > 0))
                                {
                                    Main.FakeLoadInvasionStart();
                                }
                            }
                            else
                            {
                                Main.invasionSizeStart = reader.ReadInt32();
                            }
                            if (versionNumber < 0x6c)
                            {
                                tempCultistDelay = 0x15180;
                            }
                            else
                            {
                                tempCultistDelay = reader.ReadInt32();
                            }
                            if (versionNumber >= 0x6d)
                            {
                                int num3 = reader.ReadInt16();
                                for (int j = 0; j < num3; j++)
                                {
                                    if (j < 540)
                                    {
                                        NPC.killCount[j] = reader.ReadInt32();
                                    }
                                    else
                                    {
                                        reader.ReadInt32();
                                    }
                                }
                                if (versionNumber >= 0x80)
                                {
                                    Main.fastForwardTime = reader.ReadBoolean();
                                    Main.UpdateSundial();
                                    if (versionNumber >= 0x83)
                                    {
                                        NPC.downedFishron = reader.ReadBoolean();
                                        NPC.downedMartians = reader.ReadBoolean();
                                        NPC.downedAncientCultist = reader.ReadBoolean();
                                        NPC.downedMoonlord = reader.ReadBoolean();
                                        NPC.downedHalloweenKing = reader.ReadBoolean();
                                        NPC.downedHalloweenTree = reader.ReadBoolean();
                                        NPC.downedChristmasIceQueen = reader.ReadBoolean();
                                        NPC.downedChristmasSantank = reader.ReadBoolean();
                                        NPC.downedChristmasTree = reader.ReadBoolean();
                                        if (versionNumber >= 140)
                                        {
                                            NPC.downedTowerSolar = reader.ReadBoolean();
                                            NPC.downedTowerVortex = reader.ReadBoolean();
                                            NPC.downedTowerNebula = reader.ReadBoolean();
                                            NPC.downedTowerStardust = reader.ReadBoolean();
                                            NPC.TowerActiveSolar = reader.ReadBoolean();
                                            NPC.TowerActiveVortex = reader.ReadBoolean();
                                            NPC.TowerActiveNebula = reader.ReadBoolean();
                                            NPC.TowerActiveStardust = reader.ReadBoolean();
                                            NPC.LunarApocalypseIsUp = reader.ReadBoolean();
                                            if (NPC.TowerActiveSolar)
                                            {
                                                NPC.ShieldStrengthTowerSolar = NPC.ShieldStrengthTowerMax;
                                            }
                                            if (NPC.TowerActiveVortex)
                                            {
                                                NPC.ShieldStrengthTowerVortex = NPC.ShieldStrengthTowerMax;
                                            }
                                            if (NPC.TowerActiveNebula)
                                            {
                                                NPC.ShieldStrengthTowerNebula = NPC.ShieldStrengthTowerMax;
                                            }
                                            if (NPC.TowerActiveStardust)
                                            {
                                                NPC.ShieldStrengthTowerStardust = NPC.ShieldStrengthTowerMax;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void LoadNPCs(BinaryReader reader)
        {
            NPC npc;
            int index = 0;
            bool flag = reader.ReadBoolean();
            while (flag)
            {
                npc = Main.npc[index];
                npc.SetDefaults(reader.ReadString());
                npc.displayName = reader.ReadString();
                npc.position.X = reader.ReadSingle();
                npc.position.Y = reader.ReadSingle();
                npc.homeless = reader.ReadBoolean();
                npc.homeTileX = reader.ReadInt32();
                npc.homeTileY = reader.ReadInt32();
                index++;
                flag = reader.ReadBoolean();
            }
            if (versionNumber >= 140)
            {
                for (flag = reader.ReadBoolean(); flag; flag = reader.ReadBoolean())
                {
                    npc = Main.npc[index];
                    npc.SetDefaults(reader.ReadString());
                    npc.position = reader.ReadVector2();
                    index++;
                }
            }
        }

        private static void LoadSigns(BinaryReader reader)
        {
            short num4 = reader.ReadInt16();
            int index = 0;
            while (index < num4)
            {
                Sign sign;
                string str = reader.ReadString();
                int num2 = reader.ReadInt32();
                int num3 = reader.ReadInt32();
                Tile tile = Main.tile[num2, num3];
                if (tile.active() && ((tile.type == 0x37) || (tile.type == 0x55)))
                {
                    sign = new Sign
                    {
                        text = str,
                        x = num2,
                        y = num3
                    };
                }
                else
                {
                    sign = null;
                }
                Main.sign[index] = sign;
                index++;
            }
            while (index < 0x3e8)
            {
                Main.sign[index] = null;
                index++;
            }
        }

        private static void LoadTileEntities(BinaryReader reader)
        {
            TileEntity.ByID.Clear();
            TileEntity.ByPosition.Clear();
            int num = reader.ReadInt32();
            int num2 = 0;
            for (int i = 0; i < num; i++)
            {
                TileEntity entity = TileEntity.Read(reader);
                entity.ID = num2++;
                TileEntity.ByID[entity.ID] = entity;
                TileEntity.ByPosition[entity.Position] = entity;
            }
            TileEntity.TileEntitiesNextID = num;
        }

        public static void loadWorld()
        {
            IsWorldOnCloud = false;
            Main.checkXMas();
            Main.checkHalloween();
            bool cloud = false;
            if (!FileUtilities.Exists(Main.worldPathName) && Main.autoGen)
            {
                if (!cloud)
                {
                    for (int i = Main.worldPathName.Length - 1; i >= 0; i--)
                    {
                        if (Main.worldPathName.Substring(i, 1) == Convert.ToString(Path.DirectorySeparatorChar))
                        {
                            Directory.CreateDirectory(Main.worldPathName.Substring(0, i));
                            break;
                        }
                    }
                }
                WorldGen.clearWorld();
                WorldGen.generateWorld(-1, Main.AutogenProgress);
                saveWorld();
            }
            if (WorldGen.genRand == null)
            {
                WorldGen.genRand = new Random((int)DateTime.Now.Ticks);
            }
            using (MemoryStream stream = new MemoryStream(FileUtilities.ReadAllBytes(Main.worldPathName)))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    try
                    {
                        int num3;
                        WorldGen.loadFailed = false;
                        WorldGen.loadSuccess = false;
                        int num2 = reader.ReadInt32();
                        versionNumber = num2;
                        if (num2 <= 0x57)
                        {
                            num3 = LoadWorld_Version1(reader);
                        }
                        else
                        {
                            num3 = LoadWorld_Version2(reader);
                        }
                        if (num2 < 0x8d)
                            Main.ActiveWorldFileData.CreationTime = File.GetCreationTime(Main.worldPathName);
                        reader.Close();
                        stream.Close();
                        if (num3 != 0)
                        {
                            WorldGen.loadFailed = true;
                        }
                        else
                        {
                            WorldGen.loadSuccess = true;
                        }
                        if (WorldGen.loadFailed || !WorldGen.loadSuccess)
                        {
                            return;
                        }
                        WorldGen.gen = true;
                        WorldGen.waterLine = Main.maxTilesY;
                        Liquid.QuickWater(2, -1, -1);
                        WorldGen.WaterCheck();
                        int num4 = 0;
                        Liquid.quickSettle = true;
                        int num5 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                        float num6 = 0f;
                        while ((Liquid.numLiquid > 0) && (num4 < 0x186a0))
                        {
                            num4++;
                            float num7 = ((float)(num5 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer))) / ((float)num5);
                            if ((Liquid.numLiquid + LiquidBuffer.numLiquidBuffer) > num5)
                            {
                                num5 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                            }
                            if (num7 > num6)
                            {
                                num6 = num7;
                            }
                            else
                            {
                                num7 = num6;
                            }
                            Main.statusText = string.Concat(new object[] { Lang.gen[0x1b], " ", (int)(((num7 * 100f) / 2f) + 50f), "%" });
                            Liquid.UpdateLiquid();
                        }
                        Liquid.quickSettle = false;
                        Main.weatherCounter = WorldGen.genRand.Next(0xe10, 0x4650);
                        Cloud.resetClouds();
                        WorldGen.WaterCheck();
                        WorldGen.gen = false;
                        NPC.setFireFlyChance();
                        Main.InitLifeBytes();
                        if (Main.slimeRainTime > 0.0)
                        {
                            Main.StartSlimeRain(false);
                        }
                        NPC.setWorldMonsters();
                    }
                    catch
                    {
                        WorldGen.loadFailed = true;
                        WorldGen.loadSuccess = false;
                        try
                        {
                            reader.Close();
                            stream.Close();
                        }
                        catch
                        {
                        }
                        return;
                    }
                }
            }
            if (OnWorldLoad != null)
            {
                OnWorldLoad();
            }
        }

        public static int LoadWorld_Version1(BinaryReader fileIO)
        {
            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
            int versionNumber = WorldFile.versionNumber;
            if (versionNumber > Main.curRelease)
            {
                return 1;
            }
            Main.worldName = fileIO.ReadString();
            Main.worldID = fileIO.ReadInt32();
            Main.leftWorld = fileIO.ReadInt32();
            Main.rightWorld = fileIO.ReadInt32();
            Main.topWorld = fileIO.ReadInt32();
            Main.bottomWorld = fileIO.ReadInt32();
            Main.maxTilesY = fileIO.ReadInt32();
            Main.maxTilesX = fileIO.ReadInt32();
            if (versionNumber >= 0x70)
            {
                Main.expertMode = fileIO.ReadBoolean();
            }
            else
            {
                Main.expertMode = false;
            }
            if (versionNumber >= 0x3f)
            {
                Main.moonType = fileIO.ReadByte();
            }
            else
            {
                WorldGen.RandomizeMoonState();
            }
            WorldGen.clearWorld();
            if (versionNumber >= 0x2c)
            {
                Main.treeX[0] = fileIO.ReadInt32();
                Main.treeX[1] = fileIO.ReadInt32();
                Main.treeX[2] = fileIO.ReadInt32();
                Main.treeStyle[0] = fileIO.ReadInt32();
                Main.treeStyle[1] = fileIO.ReadInt32();
                Main.treeStyle[2] = fileIO.ReadInt32();
                Main.treeStyle[3] = fileIO.ReadInt32();
            }
            if (versionNumber >= 60)
            {
                Main.caveBackX[0] = fileIO.ReadInt32();
                Main.caveBackX[1] = fileIO.ReadInt32();
                Main.caveBackX[2] = fileIO.ReadInt32();
                Main.caveBackStyle[0] = fileIO.ReadInt32();
                Main.caveBackStyle[1] = fileIO.ReadInt32();
                Main.caveBackStyle[2] = fileIO.ReadInt32();
                Main.caveBackStyle[3] = fileIO.ReadInt32();
                Main.iceBackStyle = fileIO.ReadInt32();
                if (versionNumber >= 0x3d)
                {
                    Main.jungleBackStyle = fileIO.ReadInt32();
                    Main.hellBackStyle = fileIO.ReadInt32();
                }
            }
            else
            {
                WorldGen.RandomizeCaveBackgrounds();
            }
            Main.spawnTileX = fileIO.ReadInt32();
            Main.spawnTileY = fileIO.ReadInt32();
            Main.worldSurface = fileIO.ReadDouble();
            Main.rockLayer = fileIO.ReadDouble();
            tempTime = fileIO.ReadDouble();
            tempDayTime = fileIO.ReadBoolean();
            tempMoonPhase = fileIO.ReadInt32();
            tempBloodMoon = fileIO.ReadBoolean();
            if (versionNumber >= 0x70)
            {
                tempEclipse = fileIO.ReadBoolean();
                Main.eclipse = tempEclipse;
            }
            Main.dungeonX = fileIO.ReadInt32();
            Main.dungeonY = fileIO.ReadInt32();
            if (versionNumber >= 0x38)
            {
                WorldGen.crimson = fileIO.ReadBoolean();
            }
            else
            {
                WorldGen.crimson = false;
            }
            NPC.downedBoss1 = fileIO.ReadBoolean();
            NPC.downedBoss2 = fileIO.ReadBoolean();
            NPC.downedBoss3 = fileIO.ReadBoolean();
            if (versionNumber >= 0x42)
            {
                NPC.downedQueenBee = fileIO.ReadBoolean();
            }
            if (versionNumber >= 0x2c)
            {
                NPC.downedMechBoss1 = fileIO.ReadBoolean();
                NPC.downedMechBoss2 = fileIO.ReadBoolean();
                NPC.downedMechBoss3 = fileIO.ReadBoolean();
                NPC.downedMechBossAny = fileIO.ReadBoolean();
            }
            if (versionNumber >= 0x40)
            {
                NPC.downedPlantBoss = fileIO.ReadBoolean();
                NPC.downedGolemBoss = fileIO.ReadBoolean();
            }
            if (versionNumber >= 0x1d)
            {
                NPC.savedGoblin = fileIO.ReadBoolean();
                NPC.savedWizard = fileIO.ReadBoolean();
                if (versionNumber >= 0x22)
                {
                    NPC.savedMech = fileIO.ReadBoolean();
                    if (versionNumber >= 80)
                    {
                        NPC.savedStylist = fileIO.ReadBoolean();
                    }
                }
                if (versionNumber >= 0x81)
                {
                    NPC.savedTaxCollector = fileIO.ReadBoolean();
                }
                NPC.downedGoblins = fileIO.ReadBoolean();
            }
            if (versionNumber >= 0x20)
            {
                NPC.downedClown = fileIO.ReadBoolean();
            }
            if (versionNumber >= 0x25)
            {
                NPC.downedFrost = fileIO.ReadBoolean();
            }
            if (versionNumber >= 0x38)
            {
                NPC.downedPirates = fileIO.ReadBoolean();
            }
            WorldGen.shadowOrbSmashed = fileIO.ReadBoolean();
            WorldGen.spawnMeteor = fileIO.ReadBoolean();
            WorldGen.shadowOrbCount = fileIO.ReadByte();
            if (versionNumber >= 0x17)
            {
                WorldGen.altarCount = fileIO.ReadInt32();
                Main.hardMode = fileIO.ReadBoolean();
            }
            Main.invasionDelay = fileIO.ReadInt32();
            Main.invasionSize = fileIO.ReadInt32();
            Main.invasionType = fileIO.ReadInt32();
            Main.invasionX = fileIO.ReadDouble();
            if (versionNumber >= 0x71)
            {
                Main.sundialCooldown = fileIO.ReadByte();
            }
            if (versionNumber >= 0x35)
            {
                tempRaining = fileIO.ReadBoolean();
                tempRainTime = fileIO.ReadInt32();
                tempMaxRain = fileIO.ReadSingle();
            }
            if (versionNumber >= 0x36)
            {
                WorldGen.oreTier1 = fileIO.ReadInt32();
                WorldGen.oreTier2 = fileIO.ReadInt32();
                WorldGen.oreTier3 = fileIO.ReadInt32();
            }
            else if ((versionNumber >= 0x17) && (WorldGen.altarCount == 0))
            {
                WorldGen.oreTier1 = -1;
                WorldGen.oreTier2 = -1;
                WorldGen.oreTier3 = -1;
            }
            else
            {
                WorldGen.oreTier1 = 0x6b;
                WorldGen.oreTier2 = 0x6c;
                WorldGen.oreTier3 = 0x6f;
            }
            int style = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            if (versionNumber >= 0x37)
            {
                style = fileIO.ReadByte();
                num3 = fileIO.ReadByte();
                num4 = fileIO.ReadByte();
            }
            if (versionNumber >= 60)
            {
                num5 = fileIO.ReadByte();
                num6 = fileIO.ReadByte();
                num7 = fileIO.ReadByte();
                num8 = fileIO.ReadByte();
                num9 = fileIO.ReadByte();
            }
            WorldGen.setBG(0, style);
            WorldGen.setBG(1, num3);
            WorldGen.setBG(2, num4);
            WorldGen.setBG(3, num5);
            WorldGen.setBG(4, num6);
            WorldGen.setBG(5, num7);
            WorldGen.setBG(6, num8);
            WorldGen.setBG(7, num9);
            if (versionNumber >= 60)
            {
                Main.cloudBGActive = fileIO.ReadInt32();
                if (Main.cloudBGActive >= 1f)
                {
                    Main.cloudBGAlpha = 1f;
                }
                else
                {
                    Main.cloudBGAlpha = 0f;
                }
            }
            else
            {
                Main.cloudBGActive = -WorldGen.genRand.Next(0x21c0, 0x15180);
            }
            if (versionNumber >= 0x3e)
            {
                Main.numClouds = fileIO.ReadInt16();
                Main.windSpeedSet = fileIO.ReadSingle();
                Main.windSpeed = Main.windSpeedSet;
            }
            else
            {
                WorldGen.RandomizeWeather();
            }
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float num12 = ((float)i) / ((float)Main.maxTilesX);
                Main.statusText = string.Concat(new object[] { Lang.gen[0x33], " ", (int)((num12 * 100f) + 1f), "%" });
                for (int n = 0; n < Main.maxTilesY; n++)
                {
                    Tile tile = Main.tile[i, n];
                    int index = -1;
                    tile.active(fileIO.ReadBoolean());
                    if (tile.active())
                    {
                        if (versionNumber > 0x4d)
                        {
                            index = fileIO.ReadUInt16();
                        }
                        else
                        {
                            index = fileIO.ReadByte();
                        }
                        tile.type = (ushort)index;
                        if (tile.type == 0x7f)
                        {
                            tile.active(false);
                        }
                        if ((versionNumber < 0x48) && (((tile.type == 0x23) || (tile.type == 0x24)) || (((tile.type == 170) || (tile.type == 0xab)) || (tile.type == 0xac))))
                        {
                            tile.frameX = fileIO.ReadInt16();
                            tile.frameY = fileIO.ReadInt16();
                        }
                        else if (Main.tileFrameImportant[index])
                        {
                            if ((versionNumber < 0x1c) && (index == 4))
                            {
                                tile.frameX = 0;
                                tile.frameY = 0;
                            }
                            else if ((versionNumber < 40) && (tile.type == 0x13))
                            {
                                tile.frameX = 0;
                                tile.frameY = 0;
                            }
                            else
                            {
                                tile.frameX = fileIO.ReadInt16();
                                tile.frameY = fileIO.ReadInt16();
                                if (tile.type == 0x90)
                                {
                                    tile.frameY = 0;
                                }
                            }
                        }
                        else
                        {
                            tile.frameX = -1;
                            tile.frameY = -1;
                        }
                        if ((versionNumber >= 0x30) && fileIO.ReadBoolean())
                        {
                            tile.color(fileIO.ReadByte());
                        }
                    }
                    if (versionNumber <= 0x19)
                    {
                        fileIO.ReadBoolean();
                    }
                    if (fileIO.ReadBoolean())
                    {
                        tile.wall = fileIO.ReadByte();
                        if ((versionNumber >= 0x30) && fileIO.ReadBoolean())
                        {
                            tile.wallColor(fileIO.ReadByte());
                        }
                    }
                    if (fileIO.ReadBoolean())
                    {
                        tile.liquid = fileIO.ReadByte();
                        tile.lava(fileIO.ReadBoolean());
                        if (versionNumber >= 0x33)
                        {
                            tile.honey(fileIO.ReadBoolean());
                        }
                    }
                    if (versionNumber >= 0x21)
                    {
                        tile.wire(fileIO.ReadBoolean());
                    }
                    if (versionNumber >= 0x2b)
                    {
                        tile.wire2(fileIO.ReadBoolean());
                        tile.wire3(fileIO.ReadBoolean());
                    }
                    if (versionNumber >= 0x29)
                    {
                        tile.halfBrick(fileIO.ReadBoolean());
                        if (!Main.tileSolid[tile.type])
                        {
                            tile.halfBrick(false);
                        }
                        if (versionNumber >= 0x31)
                        {
                            tile.slope(fileIO.ReadByte());
                            if (!Main.tileSolid[tile.type])
                            {
                                tile.slope(0);
                            }
                        }
                    }
                    if (versionNumber >= 0x2a)
                    {
                        tile.actuator(fileIO.ReadBoolean());
                        tile.inActive(fileIO.ReadBoolean());
                    }
                    int num14 = 0;
                    if (versionNumber >= 0x19)
                    {
                        num14 = fileIO.ReadInt16();
                    }
                    if (index != -1)
                    {
                        if (n <= Main.worldSurface)
                        {
                            if ((n + num14) <= Main.worldSurface)
                            {
                                WorldGen.tileCounts[index] += (num14 + 1) * 5;
                            }
                            else
                            {
                                int num15 = (int)((Main.worldSurface - n) + 1.0);
                                int num16 = (num14 + 1) - num15;
                                WorldGen.tileCounts[index] += (num15 * 5) + num16;
                            }
                        }
                        else
                        {
                            WorldGen.tileCounts[index] += num14 + 1;
                        }
                    }
                    if (num14 > 0)
                    {
                        for (int num17 = n + 1; num17 < ((n + num14) + 1); num17++)
                        {
                            Main.tile[i, num17].CopyFrom(Main.tile[i, n]);
                        }
                        n += num14;
                    }
                }
            }
            WorldGen.AddUpAlignmentCounts(true);
            if (versionNumber < 0x43)
            {
                WorldGen.FixSunflowers();
            }
            if (versionNumber < 0x48)
            {
                WorldGen.FixChands();
            }
            int num18 = 40;
            if (versionNumber < 0x3a)
            {
                num18 = 20;
            }
            for (int j = 0; j < 0x3e8; j++)
            {
                if (fileIO.ReadBoolean())
                {
                    Main.chest[j] = new Chest(false);
                    Main.chest[j].x = fileIO.ReadInt32();
                    Main.chest[j].y = fileIO.ReadInt32();
                    if (versionNumber >= 0x55)
                    {
                        string str = fileIO.ReadString();
                        if (str.Length > 20)
                        {
                            str = str.Substring(0, 20);
                        }
                        Main.chest[j].name = str;
                    }
                    for (int num20 = 0; num20 < 40; num20++)
                    {
                        Main.chest[j].item[num20] = new Item();
                        if (num20 < num18)
                        {
                            int num21 = 0;
                            if (versionNumber >= 0x3b)
                            {
                                num21 = fileIO.ReadInt16();
                            }
                            else
                            {
                                num21 = fileIO.ReadByte();
                            }
                            if (num21 > 0)
                            {
                                if (versionNumber >= 0x26)
                                {
                                    Main.chest[j].item[num20].netDefaults(fileIO.ReadInt32());
                                }
                                else
                                {
                                    string itemName = Item.VersionName(fileIO.ReadString(), versionNumber);
                                    Main.chest[j].item[num20].SetDefaults(itemName);
                                }
                                Main.chest[j].item[num20].stack = num21;
                                if (versionNumber >= 0x24)
                                {
                                    Main.chest[j].item[num20].Prefix(fileIO.ReadByte());
                                }
                            }
                        }
                    }
                }
            }
            for (int k = 0; k < 0x3e8; k++)
            {
                if (fileIO.ReadBoolean())
                {
                    string str3 = fileIO.ReadString();
                    int num23 = fileIO.ReadInt32();
                    int num24 = fileIO.ReadInt32();
                    if (Main.tile[num23, num24].active() && ((Main.tile[num23, num24].type == 0x37) || (Main.tile[num23, num24].type == 0x55)))
                    {
                        Main.sign[k] = new Sign();
                        Main.sign[k].x = num23;
                        Main.sign[k].y = num24;
                        Main.sign[k].text = str3;
                    }
                }
            }
            bool flag = fileIO.ReadBoolean();
            for (int m = 0; flag; m++)
            {
                Main.npc[m].SetDefaults(fileIO.ReadString());
                if (versionNumber >= 0x53)
                {
                    Main.npc[m].displayName = fileIO.ReadString();
                }
                Main.npc[m].position.X = fileIO.ReadSingle();
                Main.npc[m].position.Y = fileIO.ReadSingle();
                Main.npc[m].homeless = fileIO.ReadBoolean();
                Main.npc[m].homeTileX = fileIO.ReadInt32();
                Main.npc[m].homeTileY = fileIO.ReadInt32();
                flag = fileIO.ReadBoolean();
            }
            if ((versionNumber >= 0x1f) && (versionNumber <= 0x53))
            {
                NPC.setNPCName(fileIO.ReadString(), 0x11, true);
                NPC.setNPCName(fileIO.ReadString(), 0x12, true);
                NPC.setNPCName(fileIO.ReadString(), 0x13, true);
                NPC.setNPCName(fileIO.ReadString(), 20, true);
                NPC.setNPCName(fileIO.ReadString(), 0x16, true);
                NPC.setNPCName(fileIO.ReadString(), 0x36, true);
                NPC.setNPCName(fileIO.ReadString(), 0x26, true);
                NPC.setNPCName(fileIO.ReadString(), 0x6b, true);
                NPC.setNPCName(fileIO.ReadString(), 0x6c, true);
                if (versionNumber >= 0x23)
                {
                    NPC.setNPCName(fileIO.ReadString(), 0x7c, true);
                    if (versionNumber >= 0x41)
                    {
                        NPC.setNPCName(fileIO.ReadString(), 160, true);
                        NPC.setNPCName(fileIO.ReadString(), 0xb2, true);
                        NPC.setNPCName(fileIO.ReadString(), 0xcf, true);
                        NPC.setNPCName(fileIO.ReadString(), 0xd0, true);
                        NPC.setNPCName(fileIO.ReadString(), 0xd1, true);
                        NPC.setNPCName(fileIO.ReadString(), 0xe3, true);
                        NPC.setNPCName(fileIO.ReadString(), 0xe4, true);
                        NPC.setNPCName(fileIO.ReadString(), 0xe5, true);
                        if (versionNumber >= 0x4f)
                        {
                            NPC.setNPCName(fileIO.ReadString(), 0x161, true);
                        }
                    }
                }
            }
            if ((Main.invasionType > 0) && (Main.invasionSize > 0))
            {
                Main.FakeLoadInvasionStart();
            }
            if (versionNumber >= 7)
            {
                bool flag2 = fileIO.ReadBoolean();
                string str4 = fileIO.ReadString();
                int num26 = fileIO.ReadInt32();
                if (!flag2 || (!(str4 == Main.worldName) && (num26 != Main.worldID)))
                {
                    return 2;
                }
            }
            return 0;
        }

        public static int LoadWorld_Version2(BinaryReader reader)
        {
            bool[] flagArray;
            int[] numArray;
            reader.BaseStream.Position = 0L;
            if (!LoadFileFormatHeader(reader, out flagArray, out numArray))
            {
                return 5;
            }
            if (reader.BaseStream.Position != numArray[0])
            {
                return 5;
            }
            LoadHeader(reader);
            if (reader.BaseStream.Position != numArray[1])
            {
                return 5;
            }
            LoadWorldTiles(reader, flagArray);
            if (reader.BaseStream.Position != numArray[2])
            {
                return 5;
            }
            LoadChests(reader);
            if (reader.BaseStream.Position != numArray[3])
            {
                return 5;
            }
            LoadSigns(reader);
            if (reader.BaseStream.Position != numArray[4])
            {
                return 5;
            }
            LoadNPCs(reader);
            if (reader.BaseStream.Position != numArray[5])
            {
                return 5;
            }
            if (versionNumber >= 0x74)
            {
                if (versionNumber < 0x7a)
                {
                    LoadDummies(reader);
                    if (reader.BaseStream.Position != numArray[6])
                    {
                        return 5;
                    }
                }
                else
                {
                    LoadTileEntities(reader);
                    if (reader.BaseStream.Position != numArray[6])
                    {
                        return 5;
                    }
                }
            }
            return LoadFooter(reader);
        }

        private static void LoadWorldTiles(BinaryReader reader, bool[] importance)
        {
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float num9 = ((float)i) / ((float)Main.maxTilesX);
                Main.statusText = string.Concat(new object[] { Lang.gen[0x33], " ", (int)((num9 * 100.0) + 1.0), "%" });
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    byte num3;
                    byte num4;
                    int num8;
                    int index = -1;
                    byte num2 = (byte)(num3 = 0);
                    Tile from = Main.tile[i, j];
                    byte num = reader.ReadByte();
                    if ((num & 1) == 1)
                    {
                        num2 = reader.ReadByte();
                        if ((num2 & 1) == 1)
                        {
                            num3 = reader.ReadByte();
                        }
                    }
                    if ((num & 2) == 2)
                    {
                        from.active(true);
                        if ((num & 0x20) == 0x20)
                        {
                            num4 = reader.ReadByte();
                            index = (reader.ReadByte() << 8) | num4;
                        }
                        else
                        {
                            index = reader.ReadByte();
                        }
                        from.type = (ushort)index;
                        if (importance[index])
                        {
                            from.frameX = reader.ReadInt16();
                            from.frameY = reader.ReadInt16();
                            if (from.type == 0x90)
                            {
                                from.frameY = 0;
                            }
                        }
                        else
                        {
                            from.frameX = -1;
                            from.frameY = -1;
                        }
                        if ((num3 & 8) == 8)
                        {
                            from.color(reader.ReadByte());
                        }
                    }
                    if ((num & 4) == 4)
                    {
                        from.wall = reader.ReadByte();
                        if ((num3 & 0x10) == 0x10)
                        {
                            from.wallColor(reader.ReadByte());
                        }
                    }
                    num4 = (byte)((num & 0x18) >> 3);
                    if (num4 != 0)
                    {
                        from.liquid = reader.ReadByte();
                        if (num4 > 1)
                        {
                            if (num4 == 2)
                            {
                                from.lava(true);
                            }
                            else
                            {
                                from.honey(true);
                            }
                        }
                    }
                    if (num2 > 1)
                    {
                        if ((num2 & 2) == 2)
                        {
                            from.wire(true);
                        }
                        if ((num2 & 4) == 4)
                        {
                            from.wire2(true);
                        }
                        if ((num2 & 8) == 8)
                        {
                            from.wire3(true);
                        }
                        num4 = (byte)((num2 & 0x70) >> 4);
                        if ((num4 != 0) && Main.tileSolid[from.type])
                        {
                            if (num4 == 1)
                            {
                                from.halfBrick(true);
                            }
                            else
                            {
                                from.slope((byte)(num4 - 1));
                            }
                        }
                    }
                    if (num3 > 0)
                    {
                        if ((num3 & 2) == 2)
                        {
                            from.actuator(true);
                        }
                        if ((num3 & 4) == 4)
                        {
                            from.inActive(true);
                        }
                    }
                    switch (((byte)((num & 0xc0) >> 6)))
                    {
                        case 0:
                            num8 = 0;
                            break;

                        case 1:
                            num8 = reader.ReadByte();
                            break;

                        default:
                            num8 = reader.ReadInt16();
                            break;
                    }
                    if (index != -1)
                    {
                        if (j <= Main.worldSurface)
                        {
                            if ((j + num8) <= Main.worldSurface)
                            {
                                WorldGen.tileCounts[index] += (num8 + 1) * 5;
                            }
                            else
                            {
                                int num10 = (int)((Main.worldSurface - j) + 1.0);
                                int num11 = (num8 + 1) - num10;
                                WorldGen.tileCounts[index] += (num10 * 5) + num11;
                            }
                        }
                        else
                        {
                            WorldGen.tileCounts[index] += num8 + 1;
                        }
                    }
                    while (num8 > 0)
                    {
                        j++;
                        Main.tile[i, j].CopyFrom(from);
                        num8--;
                    }
                }
            }
            WorldGen.AddUpAlignmentCounts(true);
            if (versionNumber < 0x69)
            {
                WorldGen.FixHearts();
            }
        }

        public static void ResetTemps()
        {
            tempRaining = false;
            tempMaxRain = 0f;
            tempRainTime = 0;
            tempDayTime = true;
            tempBloodMoon = false;
            tempEclipse = false;
            tempMoonPhase = 0;
            Main.anglerWhoFinishedToday.Clear();
            Main.anglerQuestFinished = false;
        }

        private static int SaveChests(BinaryWriter writer)
        {
            Chest chest;
            int num;
            short num3 = 0;
            for (num = 0; num < 0x3e8; num++)
            {
                chest = Main.chest[num];
                if (chest != null)
                {
                    bool flag = false;
                    for (int i = chest.x; i <= (chest.x + 1); i++)
                    {
                        for (int j = chest.y; j <= (chest.y + 1); j++)
                        {
                            if (((i < 0) || (j < 0)) || ((i >= Main.maxTilesX) || (j >= Main.maxTilesY)))
                            {
                                flag = true;
                                break;
                            }
                            Tile tile = Main.tile[i, j];
                            if (!tile.active() || !Main.tileContainer[tile.type])
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        Main.chest[num] = null;
                    }
                    else
                    {
                        num3 = (short)(num3 + 1);
                    }
                }
            }
            writer.Write(num3);
            writer.Write((short)40);
            for (num = 0; num < 0x3e8; num++)
            {
                chest = Main.chest[num];
                if (chest != null)
                {
                    writer.Write(chest.x);
                    writer.Write(chest.y);
                    writer.Write(chest.name);
                    for (int k = 0; k < 40; k++)
                    {
                        Item item = chest.item[k];
                        if (item == null)
                        {
                            writer.Write((short)0);
                        }
                        else
                        {
                            if (item.stack > item.maxStack)
                            {
                                item.stack = item.maxStack;
                            }
                            if (item.stack < 0)
                            {
                                item.stack = 1;
                            }
                            writer.Write((short)item.stack);
                            if (item.stack > 0)
                            {
                                writer.Write(item.netID);
                                writer.Write(item.prefix);
                            }
                        }
                    }
                }
            }
            return (int)writer.BaseStream.Position;
        }

        private static int SaveDummies(BinaryWriter writer)
        {
            int num = 0;
            for (int i = 0; i < 0x3e8; i++)
            {
                if (TargetDummy.dummies[i] != null)
                {
                    num++;
                }
            }
            writer.Write(num);
            for (int j = 0; j < 0x3e8; j++)
            {
                TargetDummy dummy = TargetDummy.dummies[j];
                if (dummy != null)
                {
                    writer.Write(dummy.x);
                    writer.Write(dummy.y);
                }
            }
            return (int)writer.BaseStream.Position;
        }

        private static int SaveFileFormatHeader(BinaryWriter writer)
        {
            int num;
            short num2 = 0x1a3;
            short num3 = 10;
            writer.Write(Main.curRelease);
            Main.WorldFileMetadata.IncrementAndWrite(writer);
            writer.Write(num3);
            for (num = 0; num < num3; num++)
            {
                writer.Write(0);
            }
            writer.Write(num2);
            byte num4 = 0;
            byte num5 = 1;
            for (num = 0; num < num2; num++)
            {
                if (Main.tileFrameImportant[num])
                {
                    num4 = (byte)(num4 | num5);
                }
                if (num5 == 0x80)
                {
                    writer.Write(num4);
                    num4 = 0;
                    num5 = 1;
                }
                else
                {
                    num5 = (byte)(num5 << 1);
                }
            }
            if (num5 != 1)
            {
                writer.Write(num4);
            }
            return (int)writer.BaseStream.Position;
        }

        private static int SaveFooter(BinaryWriter writer)
        {
            writer.Write(true);
            writer.Write(Main.worldName);
            writer.Write(Main.worldID);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveHeaderPointers(BinaryWriter writer, int[] pointers)
        {
            writer.BaseStream.Position = 0L;
            writer.Write(Main.curRelease);
            Stream baseStream = writer.BaseStream;
            baseStream.Position += 20L;
            writer.Write((short)pointers.Length);
            for (int i = 0; i < pointers.Length; i++)
            {
                writer.Write(pointers[i]);
            }
            return (int)writer.BaseStream.Position;
        }

        private static int SaveNPCs(BinaryWriter writer)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc = Main.npc[i];
                if ((npc.active && npc.townNPC) && (npc.type != 0x170))
                {
                    writer.Write(npc.active);
                    writer.Write(npc.name);
                    writer.Write(npc.displayName);
                    writer.Write(npc.position.X);
                    writer.Write(npc.position.Y);
                    writer.Write(npc.homeless);
                    writer.Write(npc.homeTileX);
                    writer.Write(npc.homeTileY);
                }
            }
            writer.Write(false);
            for (int j = 0; j < Main.npc.Length; j++)
            {
                NPC npc2 = Main.npc[j];
                if (npc2.active && NPCID.Sets.SavesAndLoads[npc2.type])
                {
                    writer.Write(npc2.active);
                    writer.Write(npc2.name);
                    writer.WriteVector2(npc2.position);
                }
            }
            writer.Write(false);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveSigns(BinaryWriter writer)
        {
            Sign sign;
            short num = 0;
            for (int i = 0; i < 0x3e8; i++)
            {
                sign = Main.sign[i];
                if ((sign != null) && (sign.text != null))
                {
                    num = (short)(num + 1);
                }
            }
            writer.Write(num);
            for (int j = 0; j < 0x3e8; j++)
            {
                sign = Main.sign[j];
                if ((sign != null) && (sign.text != null))
                {
                    writer.Write(sign.text);
                    writer.Write(sign.x);
                    writer.Write(sign.y);
                }
            }
            return (int)writer.BaseStream.Position;
        }

        private static int SaveTileEntities(BinaryWriter writer)
        {
            writer.Write(TileEntity.ByID.Count);
            foreach (KeyValuePair<int, TileEntity> pair in TileEntity.ByID)
            {
                TileEntity.Write(writer, pair.Value);
            }
            return (int)writer.BaseStream.Position;
        }

        public static void saveWorld()
        {
            saveWorld(IsWorldOnCloud, false);
        }

        public static void saveWorld(bool useCloudSaving, bool resetTime = false)
        {
            if (!useCloudSaving)
            {
                if (Main.worldName == "")
                {
                    Main.worldName = "World";
                }
                if (!WorldGen.saveLock)
                {
                    WorldGen.saveLock = true;
                    while (WorldGen.IsGeneratingHardMode)
                    {
                        Main.statusText = Lang.gen[0x30];
                    }
                    lock (padlock)
                    {
                        try
                        {
                            Directory.CreateDirectory(Main.WorldPath);
                        }
                        catch
                        {
                        }
                        if (Main.skipMenu)
                        {
                            return;
                        }
                        if (HasCache)
                        {
                            HasCache = false;
                            tempDayTime = CachedDayTime.Value;
                            tempTime = CachedTime.Value;
                            tempMoonPhase = CachedMoonPhase.Value;
                            tempBloodMoon = CachedBloodMoon.Value;
                            tempEclipse = CachedEclipse.Value;
                            tempCultistDelay = CachedCultistDelay.Value;
                        }
                        else
                        {
                            tempDayTime = Main.dayTime;
                            tempTime = Main.time;
                            tempMoonPhase = Main.moonPhase;
                            tempBloodMoon = Main.bloodMoon;
                            tempEclipse = Main.eclipse;
                            tempCultistDelay = CultistRitual.delay;
                        }
                        if (resetTime)
                        {
                            tempDayTime = true;
                            tempTime = 13500.0;
                            tempMoonPhase = 0;
                            tempBloodMoon = false;
                            tempEclipse = false;
                            tempCultistDelay = 0x15180;
                        }
                        if (Main.worldPathName == null)
                        {
                            return;
                        }
                        new Stopwatch().Start();
                        byte[] data = null;
                        int length = 0;
                        using (MemoryStream stream = new MemoryStream(0x6acfc0))
                        {
                            using (BinaryWriter writer = new BinaryWriter(stream))
                            {
                                SaveWorld_Version2(writer);
                                writer.Flush();
                                data = stream.GetBuffer();
                                length = (int)stream.Length;
                            }
                        }
                        if (data == null)
                        {
                            return;
                        }
                        byte[] buffer2 = null;
                        if (FileUtilities.Exists(Main.worldPathName))
                        {
                            buffer2 = FileUtilities.ReadAllBytes(Main.worldPathName);
                        }
                        FileUtilities.Write(Main.worldPathName, data, length);
                        data = FileUtilities.ReadAllBytes(Main.worldPathName);
                        string path = null;
                        using (MemoryStream stream2 = new MemoryStream(data, 0, length, false))
                        {
                            using (BinaryReader reader = new BinaryReader(stream2))
                            {
                                if (!Main.validateSaves || validateWorld(reader))
                                {
                                    if (buffer2 != null)
                                    {
                                        path = Main.worldPathName + ".bak";
                                        Main.statusText = Lang.gen[50];
                                    }
                                }
                                else
                                {
                                    path = Main.worldPathName;
                                }
                            }
                        }
                        if (path != null)
                        {
                            FileUtilities.WriteAllBytes(path, buffer2);
                        }
                        WorldGen.saveLock = false;
                    }
                    Main.serverGenLock = false;
                }
            }
        }

        public static void SaveWorld_Version2(BinaryWriter writer)
        {
            int[] pointers = new int[10];
            pointers[0] = SaveFileFormatHeader(writer);
            pointers[1] = SaveWorldHeader(writer);
            pointers[2] = SaveWorldTiles(writer);
            pointers[3] = SaveChests(writer);
            pointers[4] = SaveSigns(writer);
            pointers[5] = SaveNPCs(writer);
            pointers[6] = SaveTileEntities(writer);
            SaveFooter(writer);
            SaveHeaderPointers(writer, pointers);
        }

        private static int SaveWorldHeader(BinaryWriter writer)
        {
            writer.Write(Main.worldName);
            writer.Write(Main.worldID);
            writer.Write((int)Main.leftWorld);
            writer.Write((int)Main.rightWorld);
            writer.Write((int)Main.topWorld);
            writer.Write((int)Main.bottomWorld);
            writer.Write(Main.maxTilesY);
            writer.Write(Main.maxTilesX);
            writer.Write(Main.expertMode);
            writer.Write(Main.ActiveWorldFileData.CreationTime.ToBinary());
            writer.Write((byte)Main.moonType);
            writer.Write(Main.treeX[0]);
            writer.Write(Main.treeX[1]);
            writer.Write(Main.treeX[2]);
            writer.Write(Main.treeStyle[0]);
            writer.Write(Main.treeStyle[1]);
            writer.Write(Main.treeStyle[2]);
            writer.Write(Main.treeStyle[3]);
            writer.Write(Main.caveBackX[0]);
            writer.Write(Main.caveBackX[1]);
            writer.Write(Main.caveBackX[2]);
            writer.Write(Main.caveBackStyle[0]);
            writer.Write(Main.caveBackStyle[1]);
            writer.Write(Main.caveBackStyle[2]);
            writer.Write(Main.caveBackStyle[3]);
            writer.Write(Main.iceBackStyle);
            writer.Write(Main.jungleBackStyle);
            writer.Write(Main.hellBackStyle);
            writer.Write(Main.spawnTileX);
            writer.Write(Main.spawnTileY);
            writer.Write(Main.worldSurface);
            writer.Write(Main.rockLayer);
            writer.Write(tempTime);
            writer.Write(tempDayTime);
            writer.Write(tempMoonPhase);
            writer.Write(tempBloodMoon);
            writer.Write(tempEclipse);
            writer.Write(Main.dungeonX);
            writer.Write(Main.dungeonY);
            writer.Write(WorldGen.crimson);
            writer.Write(NPC.downedBoss1);
            writer.Write(NPC.downedBoss2);
            writer.Write(NPC.downedBoss3);
            writer.Write(NPC.downedQueenBee);
            writer.Write(NPC.downedMechBoss1);
            writer.Write(NPC.downedMechBoss2);
            writer.Write(NPC.downedMechBoss3);
            writer.Write(NPC.downedMechBossAny);
            writer.Write(NPC.downedPlantBoss);
            writer.Write(NPC.downedGolemBoss);
            writer.Write(NPC.downedSlimeKing);
            writer.Write(NPC.savedGoblin);
            writer.Write(NPC.savedWizard);
            writer.Write(NPC.savedMech);
            writer.Write(NPC.downedGoblins);
            writer.Write(NPC.downedClown);
            writer.Write(NPC.downedFrost);
            writer.Write(NPC.downedPirates);
            writer.Write(WorldGen.shadowOrbSmashed);
            writer.Write(WorldGen.spawnMeteor);
            writer.Write((byte)WorldGen.shadowOrbCount);
            writer.Write(WorldGen.altarCount);
            writer.Write(Main.hardMode);
            writer.Write(Main.invasionDelay);
            writer.Write(Main.invasionSize);
            writer.Write(Main.invasionType);
            writer.Write(Main.invasionX);
            writer.Write(Main.slimeRainTime);
            writer.Write((byte)Main.sundialCooldown);
            writer.Write(tempRaining);
            writer.Write(tempRainTime);
            writer.Write(tempMaxRain);
            writer.Write(WorldGen.oreTier1);
            writer.Write(WorldGen.oreTier2);
            writer.Write(WorldGen.oreTier3);
            writer.Write((byte)WorldGen.treeBG);
            writer.Write((byte)WorldGen.corruptBG);
            writer.Write((byte)WorldGen.jungleBG);
            writer.Write((byte)WorldGen.snowBG);
            writer.Write((byte)WorldGen.hallowBG);
            writer.Write((byte)WorldGen.crimsonBG);
            writer.Write((byte)WorldGen.desertBG);
            writer.Write((byte)WorldGen.oceanBG);
            writer.Write((int)Main.cloudBGActive);
            writer.Write((short)Main.numClouds);
            writer.Write(Main.windSpeedSet);
            writer.Write(Main.anglerWhoFinishedToday.Count);
            for (int i = 0; i < Main.anglerWhoFinishedToday.Count; i++)
            {
                writer.Write(Main.anglerWhoFinishedToday[i]);
            }
            writer.Write(NPC.savedAngler);
            writer.Write(Main.anglerQuest);
            writer.Write(NPC.savedStylist);
            writer.Write(NPC.savedTaxCollector);
            writer.Write(Main.invasionSizeStart);
            writer.Write(tempCultistDelay);
            writer.Write((short)540);
            for (int j = 0; j < 540; j++)
            {
                writer.Write(NPC.killCount[j]);
            }
            writer.Write(Main.fastForwardTime);
            writer.Write(NPC.downedFishron);
            writer.Write(NPC.downedMartians);
            writer.Write(NPC.downedAncientCultist);
            writer.Write(NPC.downedMoonlord);
            writer.Write(NPC.downedHalloweenKing);
            writer.Write(NPC.downedHalloweenTree);
            writer.Write(NPC.downedChristmasIceQueen);
            writer.Write(NPC.downedChristmasSantank);
            writer.Write(NPC.downedChristmasTree);
            writer.Write(NPC.downedTowerSolar);
            writer.Write(NPC.downedTowerVortex);
            writer.Write(NPC.downedTowerNebula);
            writer.Write(NPC.downedTowerStardust);
            writer.Write(NPC.TowerActiveSolar);
            writer.Write(NPC.TowerActiveVortex);
            writer.Write(NPC.TowerActiveNebula);
            writer.Write(NPC.TowerActiveStardust);
            writer.Write(NPC.LunarApocalypseIsUp);
            return (int)writer.BaseStream.Position;
        }

        private static int SaveWorldTiles(BinaryWriter writer)
        {
            byte[] buffer = new byte[13];
            for (int i = 0; i < Main.maxTilesX; i++)
            {
                float num12 = ((float)i) / ((float)Main.maxTilesX);
                Main.statusText = string.Concat(new object[] { Lang.gen[0x31], " ", (int)((num12 * 100f) + 1f), "%" });
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    byte num9;
                    byte num10;
                    int num11;
                    Tile tile = Main.tile[i, j];
                    int index = 3;
                    byte num8 = num9 = (byte)(num10 = 0);
                    bool flag = false;
                    if (tile.active())
                    {
                        flag = true;
                        if (tile.type == 0x7f)
                        {
                            WorldGen.KillTile(i, j, false, false, false);
                            if (!tile.active())
                            {
                                flag = false;
                                if (Main.netMode != 0)
                                {
                                    NetMessage.SendData(0x11, -1, -1, "", 0, (float)i, (float)j, 0f, 0, 0, 0);
                                }
                            }
                        }
                    }
                    if (flag)
                    {
                        num8 = (byte)(num8 | 2);
                        if (tile.type == 0x7f)
                        {
                            WorldGen.KillTile(i, j, false, false, false);
                            if (!tile.active() && (Main.netMode != 0))
                            {
                                NetMessage.SendData(0x11, -1, -1, "", 0, (float)i, (float)j, 0f, 0, 0, 0);
                            }
                        }
                        buffer[index] = (byte)tile.type;
                        index++;
                        if (tile.type > 0xff)
                        {
                            buffer[index] = (byte)(tile.type >> 8);
                            index++;
                            num8 = (byte)(num8 | 0x20);
                        }
                        if (Main.tileFrameImportant[tile.type])
                        {
                            buffer[index] = (byte)(tile.frameX & 0xff);
                            index++;
                            buffer[index] = (byte)((tile.frameX & 0xff00) >> 8);
                            index++;
                            buffer[index] = (byte)(tile.frameY & 0xff);
                            index++;
                            buffer[index] = (byte)((tile.frameY & 0xff00) >> 8);
                            index++;
                        }
                        if (tile.color() != 0)
                        {
                            num10 = (byte)(num10 | 8);
                            buffer[index] = tile.color();
                            index++;
                        }
                    }
                    if (tile.wall != 0)
                    {
                        num8 = (byte)(num8 | 4);
                        buffer[index] = tile.wall;
                        index++;
                        if (tile.wallColor() != 0)
                        {
                            num10 = (byte)(num10 | 0x10);
                            buffer[index] = tile.wallColor();
                            index++;
                        }
                    }
                    if (tile.liquid != 0)
                    {
                        if (tile.lava())
                        {
                            num8 = (byte)(num8 | 0x10);
                        }
                        else if (tile.honey())
                        {
                            num8 = (byte)(num8 | 0x18);
                        }
                        else
                        {
                            num8 = (byte)(num8 | 8);
                        }
                        buffer[index] = tile.liquid;
                        index++;
                    }
                    if (tile.wire())
                    {
                        num9 = (byte)(num9 | 2);
                    }
                    if (tile.wire2())
                    {
                        num9 = (byte)(num9 | 4);
                    }
                    if (tile.wire3())
                    {
                        num9 = (byte)(num9 | 8);
                    }
                    if (tile.halfBrick())
                    {
                        num11 = 0x10;
                    }
                    else if (tile.slope() != 0)
                    {
                        num11 = (tile.slope() + 1) << 4;
                    }
                    else
                    {
                        num11 = 0;
                    }
                    num9 = (byte)(num9 | ((byte)num11));
                    if (tile.actuator())
                    {
                        num10 = (byte)(num10 | 2);
                    }
                    if (tile.inActive())
                    {
                        num10 = (byte)(num10 | 4);
                    }
                    int num7 = 2;
                    if (num10 != 0)
                    {
                        num9 = (byte)(num9 | 1);
                        buffer[num7] = num10;
                        num7--;
                    }
                    if (num9 != 0)
                    {
                        num8 = (byte)(num8 | 1);
                        buffer[num7] = num9;
                        num7--;
                    }
                    short num4 = 0;
                    int num3 = j + 1;
                    int num5 = (Main.maxTilesY - j) - 1;
                    while (num5 > 0)
                    {
                        if (!tile.isTheSameAs(Main.tile[i, num3]))
                        {
                            break;
                        }
                        num4 = (short)(num4 + 1);
                        num5--;
                        num3++;
                    }
                    j += num4;
                    if (num4 > 0)
                    {
                        buffer[index] = (byte)(num4 & 0xff);
                        index++;
                        if (num4 > 0xff)
                        {
                            num8 = (byte)(num8 | 0x80);
                            buffer[index] = (byte)((num4 & 0xff00) >> 8);
                            index++;
                        }
                        else
                        {
                            num8 = (byte)(num8 | 0x40);
                        }
                    }
                    buffer[num7] = num8;
                    writer.Write(buffer, num7, index - num7);
                }
            }
            return (int)writer.BaseStream.Position;
        }

        public static bool validateWorld(BinaryReader fileIO)
        {
            new Stopwatch().Start();
            if (WorldGen.genRand == null)
            {
                WorldGen.genRand = new Random((int)DateTime.Now.Ticks);
            }
            try
            {
                bool[] flagArray;
                int[] numArray;
                bool flag;
                Stream baseStream = fileIO.BaseStream;
                int num = fileIO.ReadInt32();
                if ((num == 0) || (num > Main.curRelease))
                {
                    return false;
                }
                baseStream.Position = 0L;
                if (!LoadFileFormatHeader(fileIO, out flagArray, out numArray))
                {
                    return false;
                }
                string str = fileIO.ReadString();
                int num2 = fileIO.ReadInt32();
                fileIO.ReadInt32();
                fileIO.ReadInt32();
                fileIO.ReadInt32();
                fileIO.ReadInt32();
                int num3 = fileIO.ReadInt32();
                int num4 = fileIO.ReadInt32();
                baseStream.Position = numArray[1];
                for (int i = 0; i < num4; i++)
                {
                    float num6 = ((float)i) / ((float)Main.maxTilesX);
                    Main.statusText = string.Concat(new object[] { Lang.gen[0x49], " ", (int)((num6 * 100f) + 1f), "%" });
                    for (int m = 0; m < num3; m++)
                    {
                        byte num10;
                        int num13;
                        byte num9 = (byte)(num10 = 0);
                        byte num8 = fileIO.ReadByte();
                        if (((num8 & 1) == 1) && ((fileIO.ReadByte() & 1) == 1))
                        {
                            num10 = fileIO.ReadByte();
                        }
                        if ((num8 & 2) == 2)
                        {
                            int num12;
                            if ((num8 & 0x20) == 0x20)
                            {
                                byte num11 = fileIO.ReadByte();
                                num12 = (fileIO.ReadByte() << 8) | num11;
                            }
                            else
                            {
                                num12 = fileIO.ReadByte();
                            }
                            if (flagArray[num12])
                            {
                                fileIO.ReadInt16();
                                fileIO.ReadInt16();
                            }
                            if ((num10 & 8) == 8)
                            {
                                fileIO.ReadByte();
                            }
                        }
                        if ((num8 & 4) == 4)
                        {
                            fileIO.ReadByte();
                            if ((num10 & 0x10) == 0x10)
                            {
                                fileIO.ReadByte();
                            }
                        }
                        if (((num8 & 0x18) >> 3) != 0)
                        {
                            fileIO.ReadByte();
                        }
                        switch (((byte)((num8 & 0xc0) >> 6)))
                        {
                            case 0:
                                num13 = 0;
                                break;

                            case 1:
                                num13 = fileIO.ReadByte();
                                break;

                            default:
                                num13 = fileIO.ReadInt16();
                                break;
                        }
                        m += num13;
                    }
                }
                if (baseStream.Position != numArray[2])
                {
                    return false;
                }
                int num14 = fileIO.ReadInt16();
                int num15 = fileIO.ReadInt16();
                for (int j = 0; j < num14; j++)
                {
                    fileIO.ReadInt32();
                    fileIO.ReadInt32();
                    fileIO.ReadString();
                    for (int n = 0; n < num15; n++)
                    {
                        if (fileIO.ReadInt16() > 0)
                        {
                            fileIO.ReadInt32();
                            fileIO.ReadByte();
                        }
                    }
                }
                if (baseStream.Position != numArray[3])
                {
                    return false;
                }
                int num19 = fileIO.ReadInt16();
                for (int k = 0; k < num19; k++)
                {
                    fileIO.ReadString();
                    fileIO.ReadInt32();
                    fileIO.ReadInt32();
                }
                if (baseStream.Position != numArray[4])
                {
                    return false;
                }
                for (flag = fileIO.ReadBoolean(); flag; flag = fileIO.ReadBoolean())
                {
                    fileIO.ReadString();
                    fileIO.ReadString();
                    fileIO.ReadSingle();
                    fileIO.ReadSingle();
                    fileIO.ReadBoolean();
                    fileIO.ReadInt32();
                    fileIO.ReadInt32();
                }
                for (flag = fileIO.ReadBoolean(); flag; flag = fileIO.ReadBoolean())
                {
                    fileIO.ReadString();
                    fileIO.ReadSingle();
                    fileIO.ReadSingle();
                }
                if (baseStream.Position != numArray[5])
                {
                    return false;
                }
                if ((versionNumber >= 0x74) && (versionNumber <= 0x79))
                {
                    int num21 = fileIO.ReadInt32();
                    for (int num22 = 0; num22 < num21; num22++)
                    {
                        fileIO.ReadInt16();
                        fileIO.ReadInt16();
                    }
                    if (baseStream.Position != numArray[6])
                    {
                        return false;
                    }
                }
                if (versionNumber >= 0x7a)
                {
                    int num23 = fileIO.ReadInt32();
                    for (int num24 = 0; num24 < num23; num24++)
                    {
                        TileEntity.Read(fileIO);
                    }
                }
                bool flag2 = fileIO.ReadBoolean();
                string str2 = fileIO.ReadString();
                int num25 = fileIO.ReadInt32();
                bool flag3 = false;
                if (flag2 && ((str2 == str) || (num25 == num2)))
                {
                    flag3 = true;
                }
                return flag3;
            }
            catch (Exception exception)
            {
                using (StreamWriter writer = new StreamWriter("client-crashlog.txt", true))
                {
                    writer.WriteLine(DateTime.Now);
                    writer.WriteLine(exception);
                    writer.WriteLine("");
                }
                return false;
            }
        }
    }
}