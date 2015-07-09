/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Terraria.World.Generation
{
    public class StructureMap
    {
        private List<Microsoft.Xna.Framework.Rectangle> _structures = new List<Microsoft.Xna.Framework.Rectangle>(2048);

        public bool CanPlace(Microsoft.Xna.Framework.Rectangle area, int padding = 0)
        {
            return this.CanPlace(area, TileID.Sets.GeneralPlacementTiles, padding);
        }

        public bool CanPlace(Microsoft.Xna.Framework.Rectangle area, bool[] validTiles, int padding = 0)
        {
            if (area.X < 0 || area.Y < 0 || (area.X + area.Width > Main.maxTilesX - 1 || area.Y + area.Height > Main.maxTilesY - 1))
                return false;
            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(area.X - padding, area.Y - padding, area.Width + padding * 2, area.Height + padding * 2);
            for (int index = 0; index < this._structures.Count; ++index)
            {
                if (rectangle.Intersects(this._structures[index]))
                    return false;
            }
            for (int index1 = rectangle.X; index1 < rectangle.X + rectangle.Width; ++index1)
            {
                for (int index2 = rectangle.Y; index2 < rectangle.Y + rectangle.Height; ++index2)
                {
                    if (Main.tile[index1, index2].active())
                    {
                        ushort num = Main.tile[index1, index2].type;
                        if (!validTiles[(int)num])
                            return false;
                    }
                }
            }
            return true;
        }

        public void AddStructure(Microsoft.Xna.Framework.Rectangle area, int padding = 0)
        {
            this._structures.Add(new Microsoft.Xna.Framework.Rectangle(area.X - padding, area.Y - padding, area.Width + padding * 2, area.Height + padding * 2));
        }

        public void Reset()
        {
            this._structures.Clear();
        }
    }
}
