/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria;

namespace Terraria
{
    public struct TileObject
    {
        public static TileObject Empty = new TileObject();
        public static TileObjectPreviewData objectPreview = new TileObjectPreviewData();
        public int xCoord;
        public int yCoord;
        public int type;
        public int style;
        public int alternate;
        public int random;

        public static bool Place(TileObject toBePlaced)
        {
            TileObjectData tileData = TileObjectData.GetTileData(toBePlaced.type, toBePlaced.style, toBePlaced.alternate);
            if (tileData == null)
                return false;
            if (tileData.HookPlaceOverride.hook != null)
            {
                int num1;
                int num2;
                if (tileData.HookPlaceOverride.processedCoordinates)
                {
                    num1 = toBePlaced.xCoord;
                    num2 = toBePlaced.yCoord;
                }
                else
                {
                    num1 = toBePlaced.xCoord + (int)tileData.Origin.X;
                    num2 = toBePlaced.yCoord + (int)tileData.Origin.Y;
                }
                if (tileData.HookPlaceOverride.hook(num1, num2, toBePlaced.type, toBePlaced.style, 1) == tileData.HookPlaceOverride.badReturn)
                    return false;
            }
            else
            {
                ushort num1 = (ushort)toBePlaced.type;
                int num2 = tileData.CalculatePlacementStyle(toBePlaced.style, toBePlaced.alternate, toBePlaced.random);
                int num3 = 0;
                if (tileData.StyleWrapLimit > 0)
                {
                    num3 = num2 / tileData.StyleWrapLimit;
                    num2 %= tileData.StyleWrapLimit;
                }
                int num4;
                int num5;
                if (tileData.StyleHorizontal)
                {
                    num4 = tileData.CoordinateFullWidth * num2;
                    num5 = tileData.CoordinateFullHeight * num3;
                }
                else
                {
                    num4 = tileData.CoordinateFullWidth * num3;
                    num5 = tileData.CoordinateFullHeight * num2;
                }
                int num6 = toBePlaced.xCoord;
                int num7 = toBePlaced.yCoord;
                for (int index1 = 0; index1 < tileData.Width; ++index1)
                {
                    for (int index2 = 0; index2 < tileData.Height; ++index2)
                    {
                        Tile tileSafely = Framing.GetTileSafely(num6 + index1, num7 + index2);
                        if (tileSafely.active() && Main.tileCut[(int)tileSafely.type])
                            WorldGen.KillTile(num6 + index1, num7 + index2, false, false, false);
                    }
                }
                for (int index1 = 0; index1 < tileData.Width; ++index1)
                {
                    int num8 = num4 + index1 * (tileData.CoordinateWidth + tileData.CoordinatePadding);
                    int num9 = num5;
                    for (int index2 = 0; index2 < tileData.Height; ++index2)
                    {
                        Tile tileSafely = Framing.GetTileSafely(num6 + index1, num7 + index2);
                        if (!tileSafely.active())
                        {
                            tileSafely.active(true);
                            tileSafely.frameX = (short)num8;
                            tileSafely.frameY = (short)num9;
                            tileSafely.type = num1;
                        }
                        num9 += tileData.CoordinateHeights[index2] + tileData.CoordinatePadding;
                    }
                }
            }
            if (tileData.FlattenAnchors)
            {
                AnchorData anchorData = tileData.AnchorBottom;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    int num = toBePlaced.xCoord + anchorData.checkStart;
                    int j = toBePlaced.yCoord + tileData.Height;
                    for (int index = 0; index < anchorData.tileCount; ++index)
                    {
                        Tile tileSafely = Framing.GetTileSafely(num + index, j);
                        if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(num + index, j, 0);
                    }
                }
                anchorData = tileData.AnchorTop;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    int num = toBePlaced.xCoord + anchorData.checkStart;
                    int j = toBePlaced.yCoord - 1;
                    for (int index = 0; index < anchorData.tileCount; ++index)
                    {
                        Tile tileSafely = Framing.GetTileSafely(num + index, j);
                        if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(num + index, j, 0);
                    }
                }
                anchorData = tileData.AnchorRight;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    int i = toBePlaced.xCoord + tileData.Width;
                    int num = toBePlaced.yCoord + anchorData.checkStart;
                    for (int index = 0; index < anchorData.tileCount; ++index)
                    {
                        Tile tileSafely = Framing.GetTileSafely(i, num + index);
                        if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(i, num + index, 0);
                    }
                }
                anchorData = tileData.AnchorLeft;
                if (anchorData.tileCount != 0 && (anchorData.type & AnchorType.SolidTile) == AnchorType.SolidTile)
                {
                    int i = toBePlaced.xCoord - 1;
                    int num = toBePlaced.yCoord + anchorData.checkStart;
                    for (int index = 0; index < anchorData.tileCount; ++index)
                    {
                        Tile tileSafely = Framing.GetTileSafely(i, num + index);
                        if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && tileSafely.blockType() != 0)
                            WorldGen.SlopeTile(i, num + index, 0);
                    }
                }
            }
            return true;
        }

        public static bool CanPlace(int x, int y, int type, int style, int dir, out TileObject objectData, bool onlyCheck = false)
        {
            TileObjectData tileData1 = TileObjectData.GetTileData(type, style, 0);
            objectData = TileObject.Empty;
            if (tileData1 == null)
                return false;
            int num1 = x - (int)tileData1.Origin.X;
            int num2 = y - (int)tileData1.Origin.Y;
            if (num1 < 0 || num1 + tileData1.Width >= Main.maxTilesX || (num2 < 0 || num2 + tileData1.Height >= Main.maxTilesY))
                return false;
            bool flag1 = tileData1.RandomStyleRange > 0;
            if (TileObjectPreviewData.placementCache == null)
                TileObjectPreviewData.placementCache = new TileObjectPreviewData();
            TileObjectPreviewData.placementCache.Reset();
            int num3 = 0;
            int num4 = 0;
            if (tileData1.AlternatesCount != 0)
                num4 = tileData1.AlternatesCount;
            float num5 = -1f;
            float num6 = -1f;
            int num7 = 0;
            TileObjectData tileObjectData = (TileObjectData)null;
            int alternate = num3 - 1;
            while (alternate < num4)
            {
                ++alternate;
                TileObjectData tileData2 = TileObjectData.GetTileData(type, style, alternate);
                if (tileData2.Direction == TileObjectDirection.None || (tileData2.Direction != TileObjectDirection.PlaceLeft || dir != 1) && (tileData2.Direction != TileObjectDirection.PlaceRight || dir != -1))
                {
                    int num8 = x - (int)tileData2.Origin.X;
                    int num9 = y - (int)tileData2.Origin.Y;
                    if (num8 < 5 || num8 + tileData2.Width > Main.maxTilesX - 5 || (num9 < 5 || num9 + tileData2.Height > Main.maxTilesY - 5))
                        return false;
                    Rectangle rectangle = new Rectangle(0, 0, tileData2.Width, tileData2.Height);
                    int X = 0;
                    int Y = 0;
                    if (tileData2.AnchorTop.tileCount != 0)
                    {
                        if (rectangle.Y == 0)
                        {
                            rectangle.Y = -1;
                            ++rectangle.Height;
                            ++Y;
                        }
                        int num10 = tileData2.AnchorTop.checkStart;
                        if (num10 < rectangle.X)
                        {
                            rectangle.Width += rectangle.X - num10;
                            X += rectangle.X - num10;
                            rectangle.X = num10;
                        }
                        int num11 = num10 + tileData2.AnchorTop.tileCount - 1;
                        int num12 = rectangle.X + rectangle.Width - 1;
                        if (num11 > num12)
                            rectangle.Width += num11 - num12;
                    }
                    if (tileData2.AnchorBottom.tileCount != 0)
                    {
                        if (rectangle.Y + rectangle.Height == tileData2.Height)
                            ++rectangle.Height;
                        int num10 = tileData2.AnchorBottom.checkStart;
                        if (num10 < rectangle.X)
                        {
                            rectangle.Width += rectangle.X - num10;
                            X += rectangle.X - num10;
                            rectangle.X = num10;
                        }
                        int num11 = num10 + tileData2.AnchorBottom.tileCount - 1;
                        int num12 = rectangle.X + rectangle.Width - 1;
                        if (num11 > num12)
                            rectangle.Width += num11 - num12;
                    }
                    if (tileData2.AnchorLeft.tileCount != 0)
                    {
                        if (rectangle.X == 0)
                        {
                            rectangle.X = -1;
                            ++rectangle.Width;
                            ++X;
                        }
                        int num10 = tileData2.AnchorLeft.checkStart;
                        if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
                            --num10;
                        if (num10 < rectangle.Y)
                        {
                            rectangle.Width += rectangle.Y - num10;
                            Y += rectangle.Y - num10;
                            rectangle.Y = num10;
                        }
                        int num11 = num10 + tileData2.AnchorLeft.tileCount - 1;
                        if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
                            num11 += 2;
                        int num12 = rectangle.Y + rectangle.Height - 1;
                        if (num11 > num12)
                            rectangle.Height += num11 - num12;
                    }
                    if (tileData2.AnchorRight.tileCount != 0)
                    {
                        if (rectangle.X + rectangle.Width == tileData2.Width)
                            ++rectangle.Width;
                        int num10 = tileData2.AnchorLeft.checkStart;
                        if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
                            --num10;
                        if (num10 < rectangle.Y)
                        {
                            rectangle.Width += rectangle.Y - num10;
                            Y += rectangle.Y - num10;
                            rectangle.Y = num10;
                        }
                        int num11 = num10 + tileData2.AnchorRight.tileCount - 1;
                        if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
                            num11 += 2;
                        int num12 = rectangle.Y + rectangle.Height - 1;
                        if (num11 > num12)
                            rectangle.Height += num11 - num12;
                    }
                    if (onlyCheck)
                    {
                        TileObject.objectPreview.Reset();
                        TileObject.objectPreview.Active = true;
                        TileObject.objectPreview.Type = (ushort)type;
                        TileObject.objectPreview.Style = (short)style;
                        TileObject.objectPreview.Alternate = alternate;
                        TileObject.objectPreview.Size = new Point16(rectangle.Width, rectangle.Height);
                        TileObject.objectPreview.ObjectStart = new Point16(X, Y);
                        TileObject.objectPreview.Coordinates = new Point16(num8 - X, num9 - Y);
                    }
                    float num13 = 0.0f;
                    float num14 = (float)(tileData2.Width * tileData2.Height);
                    float num15 = 0.0f;
                    float num16 = 0.0f;
                    for (int index1 = 0; index1 < tileData2.Width; ++index1)
                    {
                        for (int index2 = 0; index2 < tileData2.Height; ++index2)
                        {
                            Tile tileSafely = Framing.GetTileSafely(num8 + index1, num9 + index2);
                            bool flag2 = !tileData2.LiquidPlace(tileSafely);
                            bool flag3 = false;
                            if (tileData2.AnchorWall)
                            {
                                ++num16;
                                if (!tileData2.isValidWallAnchor((int)tileSafely.wall))
                                    flag3 = true;
                                else
                                    ++num15;
                            }
                            bool flag4 = false;
                            if (tileSafely.active() && !Main.tileCut[(int)tileSafely.type])
                                flag4 = true;
                            if (flag4 || flag2 || flag3)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[index1 + X, index2 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[index1 + X, index2 + Y] = 1;
                                ++num13;
                            }
                        }
                    }
                    AnchorData anchorBottom = tileData2.AnchorBottom;
                    if (anchorBottom.tileCount != 0)
                    {
                        num16 += (float)anchorBottom.tileCount;
                        int height = tileData2.Height;
                        for (int index = 0; index < anchorBottom.tileCount; ++index)
                        {
                            int num10 = anchorBottom.checkStart + index;
                            Tile tileSafely = Framing.GetTileSafely(num8 + num10, num9 + height);
                            bool flag2 = false;
                            if (tileSafely.nactive())
                            {
                                if ((anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile && Main.tileSolid[(int)tileSafely.type] && (!Main.tileSolidTop[(int)tileSafely.type] && !Main.tileNoAttach[(int)tileSafely.type]) && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int)tileSafely.type);
                                if (!flag2 && ((anchorBottom.type & AnchorType.SolidWithTop) == AnchorType.SolidWithTop || (anchorBottom.type & AnchorType.Table) == AnchorType.Table))
                                {
                                    if ((int)tileSafely.type == 19)
                                    {
                                        int num11 = (int)tileSafely.frameX / TileObjectData.PlatformFrameWidth();
                                        if (!tileSafely.halfBrick() && num11 >= 0 && num11 <= 7 || (num11 >= 12 && num11 <= 16 || num11 >= 25 && num11 <= 26))
                                            flag2 = true;
                                    }
                                    else if (Main.tileSolid[(int)tileSafely.type] && Main.tileSolidTop[(int)tileSafely.type])
                                        flag2 = true;
                                }
                                if (!flag2 && (anchorBottom.type & AnchorType.Table) == AnchorType.Table && ((int)tileSafely.type != 19 && Main.tileTable[(int)tileSafely.type]) && tileSafely.blockType() == 0)
                                    flag2 = true;
                                if (!flag2 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type]))
                                {
                                    switch (tileSafely.blockType())
                                    {
                                        case 4:
                                        case 5:
                                            flag2 = tileData2.isValidTileAnchor((int)tileSafely.type);
                                            break;
                                    }
                                }
                                if (!flag2 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;
                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, height + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, height + Y] = 1;
                                ++num15;
                            }
                        }
                    }
                    AnchorData anchorTop = tileData2.AnchorTop;
                    if (anchorTop.tileCount != 0)
                    {
                        num16 += (float)anchorTop.tileCount;
                        int num10 = -1;
                        for (int index = 0; index < anchorTop.tileCount; ++index)
                        {
                            int num11 = anchorTop.checkStart + index;
                            Tile tileSafely = Framing.GetTileSafely(num8 + num11, num9 + num10);
                            bool flag2 = false;
                            if (tileSafely.nactive())
                            {
                                if (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && !Main.tileNoAttach[(int)tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int)tileSafely.type);
                                if (!flag2 && (anchorTop.type & AnchorType.SolidBottom) == AnchorType.SolidBottom && (Main.tileSolid[(int)tileSafely.type] && (!Main.tileSolidTop[(int)tileSafely.type] || (int)tileSafely.type == 19 && (tileSafely.halfBrick() || tileSafely.topSlope())) || (tileSafely.halfBrick() || tileSafely.topSlope())) && (!TileID.Sets.NotReallySolid[(int)tileSafely.type] && !tileSafely.bottomSlope()))
                                    flag2 = tileData2.isValidTileAnchor((int)tileSafely.type);
                                if (!flag2 && (anchorTop.type & AnchorType.SolidSide) == AnchorType.SolidSide && (Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type]))
                                {
                                    switch (tileSafely.blockType())
                                    {
                                        case 2:
                                        case 3:
                                            flag2 = tileData2.isValidTileAnchor((int)tileSafely.type);
                                            break;
                                    }
                                }
                                if (!flag2 && (anchorTop.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorTop.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;
                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num11 + X, num10 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num11 + X, num10 + Y] = 1;
                                ++num15;
                            }
                        }
                    }
                    AnchorData anchorRight = tileData2.AnchorRight;
                    if (anchorRight.tileCount != 0)
                    {
                        num16 += (float)anchorRight.tileCount;
                        int width = tileData2.Width;
                        for (int index = 0; index < anchorRight.tileCount; ++index)
                        {
                            int num10 = anchorRight.checkStart + index;
                            Tile tileSafely1 = Framing.GetTileSafely(num8 + width, num9 + num10);
                            bool flag2 = false;
                            if (tileSafely1.nactive())
                            {
                                if (Main.tileSolid[(int)tileSafely1.type] && !Main.tileSolidTop[(int)tileSafely1.type] && !Main.tileNoAttach[(int)tileSafely1.type] && (tileData2.FlattenAnchors || tileSafely1.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int)tileSafely1.type);
                                if (!flag2 && (anchorRight.type & AnchorType.SolidSide) == AnchorType.SolidSide && (Main.tileSolid[(int)tileSafely1.type] && !Main.tileSolidTop[(int)tileSafely1.type]))
                                {
                                    switch (tileSafely1.blockType())
                                    {
                                        case 2:
                                        case 4:
                                            flag2 = tileData2.isValidTileAnchor((int)tileSafely1.type);
                                            break;
                                    }
                                }
                                if (!flag2 && (anchorRight.type & AnchorType.Tree) == AnchorType.Tree && (int)tileSafely1.type == 5)
                                {
                                    flag2 = true;
                                    if (index == 0)
                                    {
                                        ++num16;
                                        Tile tileSafely2 = Framing.GetTileSafely(num8 + width, num9 + num10 - 1);
                                        if (tileSafely2.nactive() && (int)tileSafely2.type == 5)
                                        {
                                            ++num15;
                                            if (onlyCheck)
                                                TileObject.objectPreview[width + X, num10 + Y - 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[width + X, num10 + Y - 1] = 2;
                                    }
                                    if (index == anchorRight.tileCount - 1)
                                    {
                                        ++num16;
                                        Tile tileSafely2 = Framing.GetTileSafely(num8 + width, num9 + num10 + 1);
                                        if (tileSafely2.nactive() && (int)tileSafely2.type == 5)
                                        {
                                            ++num15;
                                            if (onlyCheck)
                                                TileObject.objectPreview[width + X, num10 + Y + 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[width + X, num10 + Y + 1] = 2;
                                    }
                                }
                                if (!flag2 && (anchorRight.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely1.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorRight.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;
                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[width + X, num10 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[width + X, num10 + Y] = 1;
                                ++num15;
                            }
                        }
                    }
                    AnchorData anchorLeft = tileData2.AnchorLeft;
                    if (anchorLeft.tileCount != 0)
                    {
                        num16 += (float)anchorLeft.tileCount;
                        int num10 = -1;
                        for (int index = 0; index < anchorLeft.tileCount; ++index)
                        {
                            int num11 = anchorLeft.checkStart + index;
                            Tile tileSafely1 = Framing.GetTileSafely(num8 + num10, num9 + num11);
                            bool flag2 = false;
                            if (tileSafely1.nactive())
                            {
                                if (Main.tileSolid[(int)tileSafely1.type] && !Main.tileSolidTop[(int)tileSafely1.type] && !Main.tileNoAttach[(int)tileSafely1.type] && (tileData2.FlattenAnchors || tileSafely1.blockType() == 0))
                                    flag2 = tileData2.isValidTileAnchor((int)tileSafely1.type);
                                if (!flag2 && (anchorLeft.type & AnchorType.SolidSide) == AnchorType.SolidSide && (Main.tileSolid[(int)tileSafely1.type] && !Main.tileSolidTop[(int)tileSafely1.type]))
                                {
                                    switch (tileSafely1.blockType())
                                    {
                                        case 3:
                                        case 5:
                                            flag2 = tileData2.isValidTileAnchor((int)tileSafely1.type);
                                            break;
                                    }
                                }
                                if (!flag2 && (anchorLeft.type & AnchorType.Tree) == AnchorType.Tree && (int)tileSafely1.type == 5)
                                {
                                    flag2 = true;
                                    if (index == 0)
                                    {
                                        ++num16;
                                        Tile tileSafely2 = Framing.GetTileSafely(num8 + num10, num9 + num11 - 1);
                                        if (tileSafely2.nactive() && (int)tileSafely2.type == 5)
                                        {
                                            ++num15;
                                            if (onlyCheck)
                                                TileObject.objectPreview[num10 + X, num11 + Y - 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[num10 + X, num11 + Y - 1] = 2;
                                    }
                                    if (index == anchorLeft.tileCount - 1)
                                    {
                                        ++num16;
                                        Tile tileSafely2 = Framing.GetTileSafely(num8 + num10, num9 + num11 + 1);
                                        if (tileSafely2.nactive() && (int)tileSafely2.type == 5)
                                        {
                                            ++num15;
                                            if (onlyCheck)
                                                TileObject.objectPreview[num10 + X, num11 + Y + 1] = 1;
                                        }
                                        else if (onlyCheck)
                                            TileObject.objectPreview[num10 + X, num11 + Y + 1] = 2;
                                    }
                                }
                                if (!flag2 && (anchorLeft.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor((int)tileSafely1.type))
                                    flag2 = true;
                            }
                            else if (!flag2 && (anchorLeft.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
                                flag2 = true;
                            if (!flag2)
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, num11 + Y] = 2;
                            }
                            else
                            {
                                if (onlyCheck)
                                    TileObject.objectPreview[num10 + X, num11 + Y] = 1;
                                ++num15;
                            }
                        }
                    }
                    if (tileData2.HookCheck.hook != null)
                    {
                        if (tileData2.HookCheck.processedCoordinates)
                        {
                            int num10 = (int)tileData2.Origin.X;
                            int num11 = (int)tileData2.Origin.Y;
                        }
                        if (tileData2.HookCheck.hook(x, y, type, style, dir) == tileData2.HookCheck.badReturn && tileData2.HookCheck.badResponse == 0)
                        {
                            num15 = 0.0f;
                            num13 = 0.0f;
                            TileObject.objectPreview.AllInvalid();
                        }
                    }
                    float num17 = num15 / num16;
                    float num18 = num13 / num14;
                    if ((double)num17 == 1.0 && (double)num18 == 1.0)
                    {
                        num5 = 1f;
                        num6 = 1f;
                        num7 = alternate;
                        tileObjectData = tileData2;
                        break;
                    }
                    if ((double)num17 > (double)num5 || (double)num17 == (double)num5 && (double)num18 > (double)num6)
                    {
                        TileObjectPreviewData.placementCache.CopyFrom(TileObject.objectPreview);
                        num5 = num17;
                        num6 = num18;
                        tileObjectData = tileData2;
                        num7 = alternate;
                    }
                }
            }
            int num19 = -1;
            if (flag1)
            {
                if (TileObjectPreviewData.randomCache == null)
                    TileObjectPreviewData.randomCache = new TileObjectPreviewData();
                bool flag2 = false;
                if ((int)TileObjectPreviewData.randomCache.Type == type)
                {
                    Point16 coordinates = TileObjectPreviewData.randomCache.Coordinates;
                    Point16 objectStart = TileObjectPreviewData.randomCache.ObjectStart;
                    int num8 = (int)coordinates.X + (int)objectStart.X;
                    int num9 = (int)coordinates.Y + (int)objectStart.Y;
                    int num10 = x - (int)tileData1.Origin.X;
                    int num11 = y - (int)tileData1.Origin.Y;
                    if (num8 != num10 || num9 != num11)
                        flag2 = true;
                }
                else
                    flag2 = true;
                num19 = !flag2 ? TileObjectPreviewData.randomCache.Random : Main.rand.Next(tileData1.RandomStyleRange);
            }
            if (onlyCheck)
            {
                if ((double)num5 != 1.0 || (double)num6 != 1.0)
                {
                    TileObject.objectPreview.CopyFrom(TileObjectPreviewData.placementCache);
                    alternate = num7;
                }
                TileObject.objectPreview.Random = num19;
                if (tileData1.RandomStyleRange > 0)
                    TileObjectPreviewData.randomCache.CopyFrom(TileObject.objectPreview);
            }
            if (!onlyCheck)
            {
                objectData.xCoord = x - (int)tileObjectData.Origin.X;
                objectData.yCoord = y - (int)tileObjectData.Origin.Y;
                objectData.type = type;
                objectData.style = style;
                objectData.alternate = alternate;
                objectData.random = num19;
            }
            if ((double)num5 == 1.0)
                return (double)num6 == 1.0;
            return false;
        }

        public static void DrawPreview(SpriteBatch sb, TileObjectPreviewData op, Vector2 position)
        {
            Point16 coordinates = op.Coordinates;
            Texture2D texture = Main.tileTexture[(int)op.Type];
            TileObjectData tileData = TileObjectData.GetTileData((int)op.Type, (int)op.Style, op.Alternate);
            int num1 = tileData.CalculatePlacementStyle((int)op.Style, op.Alternate, op.Random);
            int num2 = 0;
            int drawYoffset = tileData.DrawYOffset;
            if (tileData.StyleWrapLimit > 0)
            {
                num2 = num1 / tileData.StyleWrapLimit;
                num1 %= tileData.StyleWrapLimit;
            }
            int num3;
            int num4;
            if (tileData.StyleHorizontal)
            {
                num3 = tileData.CoordinateFullWidth * num1;
                num4 = tileData.CoordinateFullHeight * num2;
            }
            else
            {
                num3 = tileData.CoordinateFullWidth * num2;
                num4 = tileData.CoordinateFullHeight * num1;
            }
            for (int index1 = 0; index1 < (int)op.Size.X; ++index1)
            {
                int x = num3 + (index1 - (int)op.ObjectStart.X) * (tileData.CoordinateWidth + tileData.CoordinatePadding);
                int y = num4;
                for (int index2 = 0; index2 < (int)op.Size.Y; ++index2)
                {
                    int i = (int)coordinates.X + index1;
                    int num5 = (int)coordinates.Y + index2;
                    if (index2 == 0 && tileData.DrawStepDown != 0 && WorldGen.SolidTile(Framing.GetTileSafely(i, num5 - 1)))
                        drawYoffset += tileData.DrawStepDown;
                    Color color1;
                    switch (op[index1, index2])
                    {
                        case 1:
                            color1 = Color.White;
                            break;
                        case 2:
                            color1 = Color.Red * 0.7f;
                            break;
                        default:
                            goto default;
                    }
                    Color color2 = color1 * 0.5f;
                    if (index1 >= (int)op.ObjectStart.X && index1 < (int)op.ObjectStart.X + tileData.Width && (index2 >= (int)op.ObjectStart.Y && index2 < (int)op.ObjectStart.Y + tileData.Height))
                    {
                        SpriteEffects effects = SpriteEffects.None;
                        if (tileData.DrawFlipHorizontal && index1 % 2 == 1)
                            effects |= SpriteEffects.FlipHorizontally;
                        if (tileData.DrawFlipVertical && index2 % 2 == 1)
                            effects |= SpriteEffects.FlipVertically;
                        Rectangle rectangle = new Rectangle(x, y, tileData.CoordinateWidth, tileData.CoordinateHeights[index2 - (int)op.ObjectStart.Y]);
                        sb.Draw(texture, new Vector2((float)(i * 16 - (int)((double)position.X + (double)(tileData.CoordinateWidth - 16) / 2.0)), (float)(num5 * 16 - (int)position.Y + drawYoffset)), new Rectangle?(rectangle), color2, 0.0f, Vector2.Zero, 1f, effects, 0.0f);
                        y += tileData.CoordinateHeights[index2 - (int)op.ObjectStart.Y] + tileData.CoordinatePadding;
                    }
                }
            }
        }
    }
}
