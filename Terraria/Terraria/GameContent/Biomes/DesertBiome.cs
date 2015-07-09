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
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.World.Generation;
using System.Runtime.InteropServices;

internal class DesertBiome : MicroBiome
{
    // Methods
    private void AddTileVariance(ClusterGroup clusters, Point start, Vector2 scale)
    {
        int num = (int) (scale.X * clusters.Width);
        int num2 = (int) (scale.Y * clusters.Height);
        for (int i = -20; i < (num + 20); i++)
        {
            for (int k = -20; k < (num2 + 20); k++)
            {
                int num5 = i + start.X;
                int num6 = k + start.Y;
                Tile tile = GenBase._tiles[num5, num6];
                Tile testTile = GenBase._tiles[num5, num6 + 1];
                Tile tile3 = GenBase._tiles[num5, num6 + 2];
                if ((tile.type == 0x35) && (!WorldGen.SolidTile(testTile) || !WorldGen.SolidTile(tile3)))
                {
                    tile.type = 0x18d;
                }
            }
        }
        for (int j = -20; j < (num + 20); j++)
        {
            for (int m = -20; m < (num2 + 20); m++)
            {
                int num9 = j + start.X;
                int num10 = m + start.Y;
                Tile tile4 = GenBase._tiles[num9, num10];
                if (tile4.active() && (tile4.type == 0x18c))
                {
                    bool flag = true;
                    for (int n = -1; n >= -3; n--)
                    {
                        if (GenBase._tiles[num9, num10 + n].active())
                        {
                            flag = false;
                            break;
                        }
                    }
                    bool flag2 = true;
                    for (int num12 = 1; num12 <= 3; num12++)
                    {
                        if (GenBase._tiles[num9, num10 + num12].active())
                        {
                            flag2 = false;
                            break;
                        }
                    }
                    if ((flag ^ flag2) && (GenBase._random.Next(5) == 0))
                    {
                        WorldGen.PlaceTile(num9, num10 + (flag ? -1 : 1), 0xa5, true, true, -1, 0);
                    }
                    else if (flag && (GenBase._random.Next(5) == 0))
                    {
                        WorldGen.PlaceTile(num9, num10 - 1, 0xbb, true, true, -1, 0x1d + GenBase._random.Next(6));
                    }
                }
            }
        }
    }

    private bool FindStart(Point origin, Vector2 scale, int xHubCount, int yHubCount, out Point start)
    {
        start = new Point(0, 0);
        int width = (int) (scale.X * xHubCount);
        int height = (int) (scale.Y * yHubCount);
        origin.X -= width >> 1;
        int y = 220;
        for (int i = -20; i < (width + 20); i++)
        {
            for (int j = 220; j < Main.maxTilesY; j++)
            {
                if (WorldGen.SolidTile(i + origin.X, j))
                {
                    switch (GenBase._tiles[i + origin.X, j].type)
                    {
                        case 0x3b:
                        case 60:
                            return false;
                    }
                    if (j > y)
                    {
                        y = j;
                    }
                    break;
                }
            }
        }
        WorldGen.UndergroundDesertLocation = new Rectangle(origin.X, y, width, height);
        start = new Point(origin.X, y);
        return true;
    }

    public override bool Place(Point origin, StructureMap structures)
    {
        Point point;
        float num = ((float) Main.maxTilesX) / 4200f;
        int xHubCount = (int) (80f * num);
        int yHubCount = (int) (((GenBase._random.NextFloat() + 1f) * 80f) * num);
        Vector2 scale = new Vector2(4f, 2f);
        if (!this.FindStart(origin, scale, xHubCount, yHubCount, out point))
        {
            return false;
        }
        ClusterGroup clusters = new ClusterGroup();
        clusters.Generate(xHubCount, yHubCount);
        this.PlaceSand(clusters, point, scale);
        this.PlaceClusters(clusters, point, scale);
        this.AddTileVariance(clusters, point, scale);
        int num4 = (int) (scale.X * clusters.Width);
        int num5 = (int) (scale.Y * clusters.Height);
        for (int i = -20; i < (num4 + 20); i++)
        {
            for (int j = -20; j < (num5 + 20); j++)
            {
                if ((((i + point.X) > 0) && ((i + point.X) < (Main.maxTilesX - 1))) && (((j + point.Y) > 0) && ((j + point.Y) < (Main.maxTilesY - 1))))
                {
                    WorldGen.SquareWallFrame(i + point.X, j + point.Y, true);
                    WorldUtils.TileFrame(i + point.X, j + point.Y, true);
                }
            }
        }
        return true;
    }

    private void PlaceClusters(ClusterGroup clusters, Point start, Vector2 scale)
    {
        int num = (int) (scale.X * clusters.Width);
        int num2 = (int) (scale.Y * clusters.Height);
        Vector2 vector = new Vector2((float) num, (float) num2);
        Vector2 vector2 = new Vector2((float) clusters.Width, (float) clusters.Height);
        for (int i = -20; i < (num + 20); i++)
        {
            for (int j = -20; j < (num2 + 20); j++)
            {
                float num5 = 0f;
                int num6 = -1;
                float num7 = 0f;
                int x = i + start.X;
                int y = j + start.Y;
                Vector2 vector3 = (new Vector2((float) i, (float) j) / vector) * vector2;
                float num10 = (((Vector2) ((new Vector2((float) i, (float) j) / vector) * 2f)) - Vector2.One).Length();
                for (int k = 0; k < clusters.Count; k++)
                {
                    Cluster cluster = clusters[k];
                    if ((Math.Abs((float) (cluster[0].Position.X - vector3.X)) <= 10f) && (Math.Abs((float) (cluster[0].Position.Y - vector3.Y)) <= 10f))
                    {
                        float num12 = 0f;
                        foreach (Hub hub in cluster)
                        {
                            num12 += 1f / Vector2.DistanceSquared(hub.Position, vector3);
                        }
                        if (num12 > num5)
                        {
                            if (num5 > num7)
                            {
                                num7 = num5;
                            }
                            num5 = num12;
                            num6 = k;
                        }
                        else if (num12 > num7)
                        {
                            num7 = num12;
                        }
                    }
                }
                float num13 = num5 + num7;
                Tile tile = GenBase._tiles[x, y];
                bool flag = num10 >= 0.8f;
                if (num13 > 3.5f)
                {
                    tile.ClearEverything();
                    tile.wall = 0xbb;
                    tile.liquid = 0;
                    if ((num6 % 15) == 2)
                    {
                        tile.ResetToType(0x194);
                        tile.wall = 0xbb;
                        tile.active(true);
                    }
                    Tile.SmoothSlope(x, y, true);
                }
                else if (num13 > 1.8f)
                {
                    tile.wall = 0xbb;
                    if (!flag || tile.active())
                    {
                        tile.ResetToType(0x18c);
                        tile.wall = 0xbb;
                        tile.active(true);
                        Tile.SmoothSlope(x, y, true);
                    }
                    tile.liquid = 0;
                }
                else if ((num13 > 0.7f) || !flag)
                {
                    if (!flag || tile.active())
                    {
                        tile.ResetToType(0x18d);
                        tile.active(true);
                        Tile.SmoothSlope(x, y, true);
                    }
                    tile.liquid = 0;
                    tile.wall = 0xd8;
                }
                else if (num13 > 0.25f)
                {
                    float num14 = (num13 - 0.25f) / 0.45f;
                    if (GenBase._random.NextFloat() < num14)
                    {
                        if (tile.active())
                        {
                            tile.ResetToType(0x18d);
                            tile.active(true);
                            Tile.SmoothSlope(x, y, true);
                            tile.wall = 0xd8;
                        }
                        tile.liquid = 0;
                        tile.wall = 0xbb;
                    }
                }
            }
        }
    }

    private void PlaceSand(ClusterGroup clusters, Point start, Vector2 scale)
    {
        int num = (int) (scale.X * clusters.Width);
        int num2 = (int) (scale.Y * clusters.Height);
        int num3 = 5;
        int num4 = start.Y + (num2 >> 1);
        float num5 = 0f;
        short[] numArray = new short[num + (num3 * 2)];
        for (int i = -num3; i < (num + num3); i++)
        {
            for (int k = 150; k < num4; k++)
            {
                if (WorldGen.SolidOrSlopedTile(i + start.X, k))
                {
                    num5 += k - 1;
                    numArray[i + num3] = (short) (k - 1);
                    break;
                }
            }
        }
        float num8 = num5 / ((float) (num + (num3 * 2)));
        int num9 = 0;
        for (int j = -num3; j < (num + num3); j++)
        {
            float num11 = (Math.Abs((float) (((float) (j + num3)) / ((float) (num + (num3 * 2))))) * 2f) - 1f;
            num11 = MathHelper.Clamp(num11, -1f, 1f);
            if ((j % 3) == 0)
            {
                num9 = Utils.Clamp<int>(num9 + GenBase._random.Next(-1, 2), -10, 10);
            }
            float num12 = (float) Math.Sqrt((double) (1f - (((num11 * num11) * num11) * num11)));
            int min = (num4 - ((int) (num12 * (num4 - num8)))) + num9;
            int num14 = num4 - ((int) ((num4 - num8) * ((num12 - (0.15f / ((float) Math.Sqrt(Math.Max((double) 0.01, (double) (Math.Abs((float) (8f * num11)) - 0.1)))))) + 0.25f)));
            num14 = Math.Min(num4, num14);
            int num15 = (int) (70f - (Utils.SmoothStep(0.5f, 0.8f, Math.Abs(num11)) * 70f));
            if ((min - numArray[j + num3]) < num15)
            {
                for (int n = 0; n < num15; n++)
                {
                    int num17 = j + start.X;
                    int num18 = (n + min) - 70;
                    GenBase._tiles[num17, num18].active(false);
                    GenBase._tiles[num17, num18].wall = 0;
                }
                numArray[j + num3] = (short) Utils.Clamp<int>((num15 + min) - 70, min, numArray[j + num3]);
            }
            for (int m = num4 - 1; m >= min; m--)
            {
                int num20 = j + start.X;
                int num21 = m;
                Tile tile = GenBase._tiles[num20, num21];
                tile.liquid = 0;
                Tile testTile = GenBase._tiles[num20, num21 + 1];
                Tile tile3 = GenBase._tiles[num20, num21 + 2];
                tile.type = (WorldGen.SolidTile(testTile) && WorldGen.SolidTile(tile3)) ? ((ushort) 0x35) : ((ushort) 0x18d);
                if (m > (min + 5))
                {
                    tile.wall = 0xbb;
                }
                tile.active(true);
                if (tile.wall != 0xbb)
                {
                    tile.wall = 0;
                }
                if (m < num14)
                {
                    if (m > (min + 5))
                    {
                        tile.wall = 0xbb;
                    }
                    tile.active(false);
                }
                WorldGen.SquareWallFrame(num20, num21, true);
            }
        }
    }

    // Nested Types
    private class Cluster : List<DesertBiome.Hub>
    {
    }

    private class ClusterGroup : List<DesertBiome.Cluster>
    {
        // Fields
        public int Height;
        public int Width;

        // Methods
        private void AttemptClaim(int x, int y, int[,] clusterIndexMap, List<List<Point>> pointClusters, int index)
        {
            int num = clusterIndexMap[x, y];
            if ((num != -1) && (num != index))
            {
                int num2 = (WorldGen.genRand.Next(2) == 0) ? -1 : index;
                List<Point> list = pointClusters[num];
                foreach (Point point in list)
                {
                    clusterIndexMap[point.X, point.Y] = num2;
                }
            }
        }

        public void Generate(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            base.Clear();
            bool[,] hubMap = new bool[width, height];
            int x = (width >> 1) - 1;
            int y = (height >> 1) - 1;
            int num3 = (x + 1) * (x + 1);
            Point point = new Point(x, y);
            for (int i = point.Y - y; i <= (point.Y + y); i++)
            {
                float num5 = (((float) x) / ((float) y)) * (i - point.Y);
                int num6 = Math.Min(x, (int) Math.Sqrt((double) (num3 - (num5 * num5))));
                for (int num7 = point.X - num6; num7 <= (point.X + num6); num7++)
                {
                    hubMap[num7, i] = WorldGen.genRand.Next(2) == 0;
                }
            }
            List<List<Point>> pointClusters = new List<List<Point>>();
            for (int j = 0; j < hubMap.GetLength(0); j++)
            {
                for (int num9 = 0; num9 < hubMap.GetLength(1); num9++)
                {
                    if (hubMap[j, num9] && (WorldGen.genRand.Next(2) == 0))
                    {
                        List<Point> pointCluster = new List<Point>();
                        this.SearchForCluster(hubMap, pointCluster, j, num9, 2);
                        if (pointCluster.Count > 2)
                        {
                            pointClusters.Add(pointCluster);
                        }
                    }
                }
            }
            int[,] clusterIndexMap = new int[hubMap.GetLength(0), hubMap.GetLength(1)];
            for (int k = 0; k < clusterIndexMap.GetLength(0); k++)
            {
                for (int num11 = 0; num11 < clusterIndexMap.GetLength(1); num11++)
                {
                    clusterIndexMap[k, num11] = -1;
                }
            }
            for (int m = 0; m < pointClusters.Count; m++)
            {
                foreach (Point point2 in pointClusters[m])
                {
                    clusterIndexMap[point2.X, point2.Y] = m;
                }
            }
            for (int n = 0; n < pointClusters.Count; n++)
            {
                List<Point> list3 = pointClusters[n];
                foreach (Point point3 in list3)
                {
                    int num14 = point3.X;
                    int num15 = point3.Y;
                    if (clusterIndexMap[num14, num15] == -1)
                    {
                        break;
                    }
                    int index = clusterIndexMap[num14, num15];
                    if (num14 > 0)
                    {
                        this.AttemptClaim(num14 - 1, num15, clusterIndexMap, pointClusters, index);
                    }
                    if (num14 < (clusterIndexMap.GetLength(0) - 1))
                    {
                        this.AttemptClaim(num14 + 1, num15, clusterIndexMap, pointClusters, index);
                    }
                    if (num15 > 0)
                    {
                        this.AttemptClaim(num14, num15 - 1, clusterIndexMap, pointClusters, index);
                    }
                    if (num15 < (clusterIndexMap.GetLength(1) - 1))
                    {
                        this.AttemptClaim(num14, num15 + 1, clusterIndexMap, pointClusters, index);
                    }
                }
            }
            foreach (List<Point> list4 in pointClusters)
            {
                list4.Clear();
            }
            for (int num17 = 0; num17 < clusterIndexMap.GetLength(0); num17++)
            {
                for (int num18 = 0; num18 < clusterIndexMap.GetLength(1); num18++)
                {
                    if (clusterIndexMap[num17, num18] != -1)
                    {
                        pointClusters[clusterIndexMap[num17, num18]].Add(new Point(num17, num18));
                    }
                }
            }
            foreach (List<Point> list5 in pointClusters)
            {
                if (list5.Count < 4)
                {
                    list5.Clear();
                }
            }
            foreach (List<Point> list6 in pointClusters)
            {
                DesertBiome.Cluster item = new DesertBiome.Cluster();
                if (list6.Count > 0)
                {
                    foreach (Point point4 in list6)
                    {
                        item.Add(new DesertBiome.Hub(point4.X + ((WorldGen.genRand.NextFloat() - 0.5f) * 0.5f), point4.Y + ((WorldGen.genRand.NextFloat() - 0.5f) * 0.5f)));
                    }
                    base.Add(item);
                }
            }
        }

        private void SearchForCluster(bool[,] hubMap, List<Point> pointCluster, int x, int y, int level = 2)
        {
            pointCluster.Add(new Point(x, y));
            hubMap[x, y] = false;
            level--;
            if (level != -1)
            {
                if ((x > 0) && hubMap[x - 1, y])
                {
                    this.SearchForCluster(hubMap, pointCluster, x - 1, y, level);
                }
                if ((x < (hubMap.GetLength(0) - 1)) && hubMap[x + 1, y])
                {
                    this.SearchForCluster(hubMap, pointCluster, x + 1, y, level);
                }
                if ((y > 0) && hubMap[x, y - 1])
                {
                    this.SearchForCluster(hubMap, pointCluster, x, y - 1, level);
                }
                if ((y < (hubMap.GetLength(1) - 1)) && hubMap[x, y + 1])
                {
                    this.SearchForCluster(hubMap, pointCluster, x, y + 1, level);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Hub
    {
        public Vector2 Position;
        public Hub(Vector2 position)
        {
            this.Position = position;
        }

        public Hub(float x, float y)
        {
            this.Position = new Vector2(x, y);
        }
    }
}
