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
using System;
using Terraria;
using Terraria.Graphics;

namespace Terraria.GameContent.Liquid
{
    internal class LiquidRenderer
    {
        private static int[] WATERFALL_LENGTH = new int[3]
    {
      10,
      3,
      2
    };
        private static readonly float[] DEFAULT_OPACITY = new float[3]
    {
      0.6f,
      0.95f,
      0.95f
    };
        private static readonly Tile EMPTY_TILE = new Tile();
        public static LiquidRenderer Instance = new LiquidRenderer();
        private Tile[,] _tiles = Main.tile;
        private Texture2D[] _liquidTextures = new Texture2D[12];
        private LiquidRenderer.LiquidCache[] _cache = new LiquidRenderer.LiquidCache[41616];
        private LiquidRenderer.LiquidDrawCache[] _drawCache = new LiquidRenderer.LiquidDrawCache[40000];
        private Random _random = new Random();
        private const int ANIMATION_FRAME_COUNT = 16;
        private const int CACHE_PADDING = 2;
        private const int CACHE_PADDING_2 = 4;
        public const float MIN_LIQUID_SIZE = 0.25f;
        private int _animationFrame;
        private Rectangle _drawArea;

        public LiquidRenderer()
        {
            for (int index = 0; index < this._liquidTextures.Length; ++index)
                this._liquidTextures[index] = TextureManager.Load("Images/Misc/water_" + (object)index);
        }

        private unsafe void InternalUpdate(Rectangle drawArea)
        {
            Rectangle rectangle = new Rectangle(drawArea.X - 2, drawArea.Y - 2, drawArea.Width + 4, drawArea.Height + 4);
            this._drawArea = drawArea;
            fixed (LiquidRenderer.LiquidCache* liquidCachePtr1 = &this._cache[0])
            {
                int num1 = rectangle.Height * 2 + 2;
                LiquidRenderer.LiquidCache* liquidCachePtr2 = liquidCachePtr1;
                for (int index1 = rectangle.X; index1 < rectangle.X + rectangle.Width; ++index1)
                {
                    for (int index2 = rectangle.Y; index2 < rectangle.Y + rectangle.Height; ++index2)
                    {
                        Tile tile = this._tiles[index1, index2] ?? LiquidRenderer.EMPTY_TILE;
                        liquidCachePtr2->LiquidLevel = (float)tile.liquid / (float)byte.MaxValue;
                        liquidCachePtr2->IsHalfBrick = tile.halfBrick() && liquidCachePtr2[-1].HasLiquid;
                        liquidCachePtr2->IsSolid = WorldGen.SolidOrSlopedTile(tile) && !liquidCachePtr2->IsHalfBrick;
                        liquidCachePtr2->HasLiquid = (int)tile.liquid != 0;
                        liquidCachePtr2->VisibleLiquidLevel = 0.0f;
                        liquidCachePtr2->HasWall = (int)tile.wall != 0;
                        liquidCachePtr2->Type = tile.liquidType();
                        if (liquidCachePtr2->IsHalfBrick && !liquidCachePtr2->HasLiquid)
                            liquidCachePtr2->Type = liquidCachePtr2[-1].Type;
                        ++liquidCachePtr2;
                    }
                }
                LiquidRenderer.LiquidCache* liquidCachePtr3 = liquidCachePtr1 + num1;
                LiquidRenderer.LiquidCache liquidCache1;
                LiquidRenderer.LiquidCache liquidCache2;
                LiquidRenderer.LiquidCache liquidCache3;
                LiquidRenderer.LiquidCache liquidCache4;
                for (int index1 = 2; index1 < rectangle.Width - 2; ++index1)
                {
                    for (int index2 = 2; index2 < rectangle.Height - 2; ++index2)
                    {
                        float val1 = 0.0f;
                        float num2;
                        if (liquidCachePtr3->IsHalfBrick && liquidCachePtr3[-1].HasLiquid)
                            num2 = 1f;
                        else if (!liquidCachePtr3->HasLiquid)
                        {
                            liquidCache1 = liquidCachePtr3[-rectangle.Height];
                            liquidCache2 = liquidCachePtr3[rectangle.Height];
                            liquidCache3 = liquidCachePtr3[-1];
                            liquidCache4 = liquidCachePtr3[1];
                            if (liquidCache1.HasLiquid && liquidCache2.HasLiquid && (int)liquidCache1.Type == (int)liquidCache2.Type)
                            {
                                val1 = liquidCache1.LiquidLevel + liquidCache2.LiquidLevel;
                                liquidCachePtr3->Type = liquidCache1.Type;
                            }
                            if (liquidCache3.HasLiquid && liquidCache4.HasLiquid && (int)liquidCache3.Type == (int)liquidCache4.Type)
                            {
                                val1 = Math.Max(val1, liquidCache3.LiquidLevel + liquidCache4.LiquidLevel);
                                liquidCachePtr3->Type = liquidCache3.Type;
                            }
                            num2 = val1 * 0.5f;
                        }
                        else
                            num2 = liquidCachePtr3->LiquidLevel;
                        liquidCachePtr3->VisibleLiquidLevel = num2;
                        liquidCachePtr3->HasVisibleLiquid = (double)num2 != 0.0;
                        ++liquidCachePtr3;
                    }
                    liquidCachePtr3 += 4;
                }
                LiquidRenderer.LiquidCache* liquidCachePtr4 = liquidCachePtr1;
                for (int index1 = 0; index1 < rectangle.Width; ++index1)
                {
                    for (int index2 = 0; index2 < rectangle.Height - 10; ++index2)
                    {
                        if (liquidCachePtr4->HasVisibleLiquid && !liquidCachePtr4->IsSolid)
                        {
                            liquidCachePtr4->Opacity = 1f;
                            liquidCachePtr4->VisibleType = liquidCachePtr4->Type;
                            float num2 = 1f / (float)(LiquidRenderer.WATERFALL_LENGTH[(int)liquidCachePtr4->Type] + 1);
                            float num3 = 1f;
                            for (int index3 = 1; index3 <= LiquidRenderer.WATERFALL_LENGTH[(int)liquidCachePtr4->Type]; ++index3)
                            {
                                num3 -= num2;
                                if (!liquidCachePtr4[index3].IsSolid)
                                {
                                    liquidCachePtr4[index3].VisibleLiquidLevel = Math.Max(liquidCachePtr4[index3].VisibleLiquidLevel, liquidCachePtr4->VisibleLiquidLevel * num3);
                                    liquidCachePtr4[index3].Opacity = num3;
                                    liquidCachePtr4[index3].VisibleType = liquidCachePtr4->Type;
                                }
                                else
                                    break;
                            }
                        }
                        if (liquidCachePtr4->IsSolid)
                        {
                            liquidCachePtr4->VisibleLiquidLevel = 1f;
                            liquidCachePtr4->HasVisibleLiquid = false;
                        }
                        else
                            liquidCachePtr4->HasVisibleLiquid = (double)liquidCachePtr4->VisibleLiquidLevel != 0.0;
                        ++liquidCachePtr4;
                    }
                    liquidCachePtr4 += 10;
                }
                LiquidRenderer.LiquidCache* liquidCachePtr5 = liquidCachePtr1 + num1;
                for (int index1 = 2; index1 < rectangle.Width - 2; ++index1)
                {
                    for (int index2 = 2; index2 < rectangle.Height - 2; ++index2)
                    {
                        if (!liquidCachePtr5->HasVisibleLiquid || liquidCachePtr5->IsSolid)
                        {
                            liquidCachePtr5->HasLeftEdge = false;
                            liquidCachePtr5->HasTopEdge = false;
                            liquidCachePtr5->HasRightEdge = false;
                            liquidCachePtr5->HasBottomEdge = false;
                        }
                        else
                        {
                            liquidCache1 = liquidCachePtr5[-1];
                            liquidCache2 = liquidCachePtr5[1];
                            liquidCache3 = liquidCachePtr5[-rectangle.Height];
                            liquidCache4 = liquidCachePtr5[rectangle.Height];
                            float num2 = 0.0f;
                            float num3 = 1f;
                            float num4 = 0.0f;
                            float num5 = 1f;
                            float num6 = liquidCachePtr5->VisibleLiquidLevel;
                            if (!liquidCache1.HasVisibleLiquid)
                                num4 += liquidCache2.VisibleLiquidLevel * (1f - num6);
                            if (!liquidCache2.HasVisibleLiquid && !liquidCache2.IsSolid && !liquidCache2.IsHalfBrick)
                                num5 -= liquidCache1.VisibleLiquidLevel * (1f - num6);
                            if (!liquidCache3.HasVisibleLiquid && !liquidCache3.IsSolid && !liquidCache3.IsHalfBrick)
                                num2 += liquidCache4.VisibleLiquidLevel * (1f - num6);
                            if (!liquidCache4.HasVisibleLiquid && !liquidCache4.IsSolid && !liquidCache4.IsHalfBrick)
                                num3 -= liquidCache3.VisibleLiquidLevel * (1f - num6);
                            liquidCachePtr5->LeftWall = num2;
                            liquidCachePtr5->RightWall = num3;
                            liquidCachePtr5->BottomWall = num5;
                            liquidCachePtr5->TopWall = num4;
                            Point zero = Point.Zero;
                            liquidCachePtr5->HasTopEdge = !liquidCache1.HasVisibleLiquid && !liquidCache1.IsSolid || (double)num4 != 0.0;
                            liquidCachePtr5->HasBottomEdge = !liquidCache2.HasVisibleLiquid && !liquidCache2.IsSolid || (double)num5 != 1.0;
                            liquidCachePtr5->HasLeftEdge = !liquidCache3.HasVisibleLiquid && !liquidCache3.IsSolid || (double)num2 != 0.0;
                            liquidCachePtr5->HasRightEdge = !liquidCache4.HasVisibleLiquid && !liquidCache4.IsSolid || (double)num3 != 1.0;
                            if (!liquidCachePtr5->HasLeftEdge)
                            {
                                if (liquidCachePtr5->HasRightEdge)
                                    zero.X += 32;
                                else
                                    zero.X += 16;
                            }
                            if (liquidCachePtr5->HasLeftEdge && liquidCachePtr5->HasRightEdge)
                            {
                                zero.X = 16;
                                zero.Y += 32;
                                if (liquidCachePtr5->HasTopEdge)
                                    zero.Y = 16;
                            }
                            else if (!liquidCachePtr5->HasTopEdge)
                            {
                                if (!liquidCachePtr5->HasLeftEdge && !liquidCachePtr5->HasRightEdge)
                                    zero.Y += 48;
                                else
                                    zero.Y += 16;
                            }
                            if (zero.Y == 16 && liquidCachePtr5->HasLeftEdge ^ liquidCachePtr5->HasRightEdge && index2 + rectangle.Y % 2 == 0)
                                zero.Y += 16;
                            liquidCachePtr5->FrameOffset = zero;
                        }
                        ++liquidCachePtr5;
                    }
                    liquidCachePtr5 += 4;
                }
                LiquidRenderer.LiquidCache* liquidCachePtr6 = liquidCachePtr1 + num1;
                for (int index1 = 2; index1 < rectangle.Width - 2; ++index1)
                {
                    for (int index2 = 2; index2 < rectangle.Height - 2; ++index2)
                    {
                        if (liquidCachePtr6->HasVisibleLiquid)
                        {
                            liquidCache1 = liquidCachePtr6[-1];
                            liquidCache2 = liquidCachePtr6[1];
                            liquidCache3 = liquidCachePtr6[-rectangle.Height];
                            liquidCache4 = liquidCachePtr6[rectangle.Height];
                            liquidCachePtr6->VisibleLeftWall = liquidCachePtr6->LeftWall;
                            liquidCachePtr6->VisibleRightWall = liquidCachePtr6->RightWall;
                            liquidCachePtr6->VisibleTopWall = liquidCachePtr6->TopWall;
                            liquidCachePtr6->VisibleBottomWall = liquidCachePtr6->BottomWall;
                            if (liquidCache1.HasVisibleLiquid && liquidCache2.HasVisibleLiquid)
                            {
                                if (liquidCachePtr6->HasLeftEdge)
                                    liquidCachePtr6->VisibleLeftWall = (float)(((double)liquidCachePtr6->LeftWall * 2.0 + (double)liquidCache1.LeftWall + (double)liquidCache2.LeftWall) * 0.25);
                                if (liquidCachePtr6->HasRightEdge)
                                    liquidCachePtr6->VisibleRightWall = (float)(((double)liquidCachePtr6->RightWall * 2.0 + (double)liquidCache1.RightWall + (double)liquidCache2.RightWall) * 0.25);
                            }
                            if (liquidCache3.HasVisibleLiquid && liquidCache4.HasVisibleLiquid)
                            {
                                if (liquidCachePtr6->HasTopEdge)
                                    liquidCachePtr6->VisibleTopWall = (float)(((double)liquidCachePtr6->TopWall * 2.0 + (double)liquidCache3.TopWall + (double)liquidCache4.TopWall) * 0.25);
                                if (liquidCachePtr6->HasBottomEdge)
                                    liquidCachePtr6->VisibleBottomWall = (float)(((double)liquidCachePtr6->BottomWall * 2.0 + (double)liquidCache3.BottomWall + (double)liquidCache4.BottomWall) * 0.25);
                            }
                        }
                        ++liquidCachePtr6;
                    }
                    liquidCachePtr6 += 4;
                }
                LiquidRenderer.LiquidCache* liquidCachePtr7 = liquidCachePtr1 + num1;
                for (int index1 = 2; index1 < rectangle.Width - 2; ++index1)
                {
                    for (int index2 = 2; index2 < rectangle.Height - 2; ++index2)
                    {
                        if (liquidCachePtr7->HasLiquid)
                        {
                            liquidCache1 = liquidCachePtr7[-1];
                            liquidCache2 = liquidCachePtr7[1];
                            liquidCache3 = liquidCachePtr7[-rectangle.Height];
                            liquidCache4 = liquidCachePtr7[rectangle.Height];
                            if (liquidCachePtr7->HasTopEdge && !liquidCachePtr7->HasBottomEdge && liquidCachePtr7->HasLeftEdge ^ liquidCachePtr7->HasRightEdge)
                            {
                                if (liquidCachePtr7->HasRightEdge)
                                {
                                    liquidCachePtr7->VisibleRightWall = liquidCache2.VisibleRightWall;
                                    liquidCachePtr7->VisibleTopWall = liquidCache3.VisibleTopWall;
                                }
                                else
                                {
                                    liquidCachePtr7->VisibleLeftWall = liquidCache2.VisibleLeftWall;
                                    liquidCachePtr7->VisibleTopWall = liquidCache4.VisibleTopWall;
                                }
                            }
                            else if (liquidCache2.FrameOffset.X == 16 && liquidCache2.FrameOffset.Y == 32)
                            {
                                if ((double)liquidCachePtr7->VisibleLeftWall > 0.5)
                                {
                                    liquidCachePtr7->VisibleLeftWall = 0.0f;
                                    liquidCachePtr7->FrameOffset = new Point(0, 0);
                                }
                                else if ((double)liquidCachePtr7->VisibleRightWall < 0.5)
                                {
                                    liquidCachePtr7->VisibleRightWall = 1f;
                                    liquidCachePtr7->FrameOffset = new Point(32, 0);
                                }
                            }
                        }
                        ++liquidCachePtr7;
                    }
                    liquidCachePtr7 += 4;
                }
                LiquidRenderer.LiquidCache* liquidCachePtr8 = liquidCachePtr1 + num1;
                for (int index1 = 2; index1 < rectangle.Width - 2; ++index1)
                {
                    for (int index2 = 2; index2 < rectangle.Height - 2; ++index2)
                    {
                        if (liquidCachePtr8->HasLiquid)
                        {
                            liquidCache1 = liquidCachePtr8[-1];
                            liquidCache2 = liquidCachePtr8[1];
                            liquidCache3 = liquidCachePtr8[-rectangle.Height];
                            liquidCache4 = liquidCachePtr8[rectangle.Height];
                            if (!liquidCachePtr8->HasBottomEdge && !liquidCachePtr8->HasLeftEdge && (!liquidCachePtr8->HasTopEdge && !liquidCachePtr8->HasRightEdge))
                            {
                                if (liquidCache3.HasTopEdge && liquidCache1.HasLeftEdge)
                                {
                                    liquidCachePtr8->FrameOffset.X = Math.Max(4, (int)(16.0 - (double)liquidCache1.VisibleLeftWall * 16.0)) - 4;
                                    liquidCachePtr8->FrameOffset.Y = 48 + Math.Max(4, (int)(16.0 - (double)liquidCache3.VisibleTopWall * 16.0)) - 4;
                                    liquidCachePtr8->VisibleLeftWall = 0.0f;
                                    liquidCachePtr8->VisibleTopWall = 0.0f;
                                    liquidCachePtr8->VisibleRightWall = 1f;
                                    liquidCachePtr8->VisibleBottomWall = 1f;
                                }
                                else if (liquidCache4.HasTopEdge && liquidCache1.HasRightEdge)
                                {
                                    liquidCachePtr8->FrameOffset.X = 32 - Math.Min(16, (int)((double)liquidCache1.VisibleRightWall * 16.0) - 4);
                                    liquidCachePtr8->FrameOffset.Y = 48 + Math.Max(4, (int)(16.0 - (double)liquidCache4.VisibleTopWall * 16.0)) - 4;
                                    liquidCachePtr8->VisibleLeftWall = 0.0f;
                                    liquidCachePtr8->VisibleTopWall = 0.0f;
                                    liquidCachePtr8->VisibleRightWall = 1f;
                                    liquidCachePtr8->VisibleBottomWall = 1f;
                                }
                            }
                        }
                        ++liquidCachePtr8;
                    }
                    liquidCachePtr8 += 4;
                }
                LiquidRenderer.LiquidCache* liquidCachePtr9 = liquidCachePtr1 + num1;
                fixed (LiquidRenderer.LiquidDrawCache* liquidDrawCachePtr1 = &this._drawCache[0])
                {
                    LiquidRenderer.LiquidDrawCache* liquidDrawCachePtr2 = liquidDrawCachePtr1;
                    for (int index1 = 2; index1 < rectangle.Width - 2; ++index1)
                    {
                        for (int index2 = 2; index2 < rectangle.Height - 2; ++index2)
                        {
                            if (liquidCachePtr9->HasVisibleLiquid)
                            {
                                float num2 = Math.Min(0.75f, liquidCachePtr9->VisibleLeftWall);
                                float num3 = Math.Max(0.25f, liquidCachePtr9->VisibleRightWall);
                                float num4 = Math.Min(0.75f, liquidCachePtr9->VisibleTopWall);
                                float num5 = Math.Max(0.25f, liquidCachePtr9->VisibleBottomWall);
                                if (liquidCachePtr9->IsHalfBrick && (double)num5 > 0.5)
                                    num5 = 0.5f;
                                liquidDrawCachePtr2->IsVisible = liquidCachePtr9->HasWall || (!liquidCachePtr9->IsHalfBrick || !liquidCachePtr9->HasLiquid);
                                liquidDrawCachePtr2->SourceRectangle = new Rectangle((int)(16.0 - (double)num3 * 16.0) + liquidCachePtr9->FrameOffset.X, (int)(16.0 - (double)num5 * 16.0) + liquidCachePtr9->FrameOffset.Y, (int)Math.Ceiling(((double)num3 - (double)num2) * 16.0), (int)Math.Ceiling(((double)num5 - (double)num4) * 16.0));
                                liquidDrawCachePtr2->IsSurfaceLiquid = liquidCachePtr9->FrameOffset.X == 16 && liquidCachePtr9->FrameOffset.Y == 0 && (double)(index2 + rectangle.Y) > Main.worldSurface - 40.0;
                                liquidDrawCachePtr2->Opacity = liquidCachePtr9->Opacity;
                                liquidDrawCachePtr2->LiquidOffset = new Vector2((float)Math.Floor((double)num2 * 16.0), (float)Math.Floor((double)num4 * 16.0));
                                liquidDrawCachePtr2->Type = liquidCachePtr9->VisibleType;
                                liquidDrawCachePtr2->HasWall = liquidCachePtr9->HasWall;
                            }
                            else
                                liquidDrawCachePtr2->IsVisible = false;
                            ++liquidCachePtr9;
                            ++liquidDrawCachePtr2;
                        }
                        liquidCachePtr9 += 4;
                    }
                }
                LiquidRenderer.LiquidCache* liquidCachePtr10 = liquidCachePtr1;
                for (int index1 = rectangle.X; index1 < rectangle.X + rectangle.Width; ++index1)
                {
                    for (int index2 = rectangle.Y; index2 < rectangle.Y + rectangle.Height; ++index2)
                    {
                        if ((int)liquidCachePtr10->VisibleType == 1 && liquidCachePtr10->HasVisibleLiquid && Dust.lavaBubbles < 200)
                        {
                            if (this._random.Next(700) == 0)
                                Dust.NewDust(new Vector2((float)(index1 * 16), (float)(index2 * 16)), 16, 16, 35, 0.0f, 0.0f, 0, Color.White, 1f);
                            if (this._random.Next(350) == 0)
                            {
                                int index3 = Dust.NewDust(new Vector2((float)(index1 * 16), (float)(index2 * 16)), 16, 8, 35, 0.0f, 0.0f, 50, Color.White, 1.5f);
                                Main.dust[index3].velocity *= 0.8f;
                                Main.dust[index3].velocity.X *= 2f;
                                Main.dust[index3].velocity.Y -= (float)this._random.Next(1, 7) * 0.1f;
                                if (this._random.Next(10) == 0)
                                    Main.dust[index3].velocity.Y *= (float)this._random.Next(2, 5);
                                Main.dust[index3].noGravity = true;
                            }
                        }
                        ++liquidCachePtr10;
                    }
                }
            }
        }

        public unsafe void InternalDraw(SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float globalAlpha, bool isBackgroundDraw)
        {
            Rectangle rectangle1 = this._drawArea;
            fixed (LiquidRenderer.LiquidDrawCache* liquidDrawCachePtr1 = &this._drawCache[0])
            {
                LiquidRenderer.LiquidDrawCache* liquidDrawCachePtr2 = liquidDrawCachePtr1;
                for (int centerX = rectangle1.X; centerX < rectangle1.X + rectangle1.Width; ++centerX)
                {
                    for (int centerY = rectangle1.Y; centerY < rectangle1.Y + rectangle1.Height; ++centerY)
                    {
                        if (liquidDrawCachePtr2->IsVisible)
                        {
                            Rectangle rectangle2 = liquidDrawCachePtr2->SourceRectangle;
                            if (liquidDrawCachePtr2->IsSurfaceLiquid)
                                rectangle2.Y = 1280;
                            else
                                rectangle2.Y += this._animationFrame * 80;
                            Vector2 vector2 = liquidDrawCachePtr2->LiquidOffset;
                            int index = (int)liquidDrawCachePtr2->Type;
                            switch (index)
                            {
                                case 0:
                                    index = waterStyle;
                                    break;
                                case 2:
                                    index = 11;
                                    break;
                            }
                            VertexColors vertices;
                            Lighting.GetColor4Slice_New(centerX, centerY, out vertices, 1f);
                            float num = liquidDrawCachePtr2->Opacity * Math.Min(1f, isBackgroundDraw ? 1f : globalAlpha * LiquidRenderer.DEFAULT_OPACITY[(int)liquidDrawCachePtr2->Type]);
                            vertices.BottomLeftColor *= num;
                            vertices.BottomRightColor *= num;
                            vertices.TopLeftColor *= num;
                            vertices.TopRightColor *= num;
                            Main.tileBatch.Draw(this._liquidTextures[index], new Vector2((float)(centerX << 4), (float)(centerY << 4)) + drawOffset + vector2, new Rectangle?(rectangle2), vertices, Vector2.Zero, 1f, SpriteEffects.None);
                        }
                        ++liquidDrawCachePtr2;
                    }
                }
            }
        }

        public bool HasFullWater(int x, int y)
        {
            x -= this._drawArea.X;
            y -= this._drawArea.Y;
            int index = x * this._drawArea.Height + y;
            if (index < 0 || index >= this._drawCache.Length)
                return true;
            if (this._drawCache[index].IsVisible)
                return !this._drawCache[index].IsSurfaceLiquid;
            return false;
        }

        public void Update(Rectangle drawArea)
        {
            this.InternalUpdate(drawArea);
            this._animationFrame = (this._animationFrame + 1) % 16;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float alpha, bool isBackgroundDraw)
        {
            this.InternalDraw(spriteBatch, drawOffset, waterStyle, alpha, isBackgroundDraw);
        }

        private struct LiquidCache
        {
            public float LiquidLevel;
            public float VisibleLiquidLevel;
            public float Opacity;
            public bool IsSolid;
            public bool IsHalfBrick;
            public bool HasLiquid;
            public bool HasVisibleLiquid;
            public bool HasWall;
            public Point FrameOffset;
            public bool HasLeftEdge;
            public bool HasRightEdge;
            public bool HasTopEdge;
            public bool HasBottomEdge;
            public float LeftWall;
            public float RightWall;
            public float BottomWall;
            public float TopWall;
            public float VisibleLeftWall;
            public float VisibleRightWall;
            public float VisibleBottomWall;
            public float VisibleTopWall;
            public byte Type;
            public byte VisibleType;
        }

        private struct LiquidDrawCache
        {
            public Rectangle SourceRectangle;
            public Vector2 LiquidOffset;
            public bool IsVisible;
            public float Opacity;
            public byte Type;
            public bool IsSurfaceLiquid;
            public bool HasWall;
        }
    }
}
