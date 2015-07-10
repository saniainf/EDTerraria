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

namespace Terraria.Modules
{
    public class AnchorTypesModule
    {
        public int[] tileValid;
        public int[] tileInvalid;
        public int[] tileAlternates;
        public int[] wallValid;

        public AnchorTypesModule(AnchorTypesModule copyFrom = null)
        {
            if (copyFrom == null)
            {
                tileValid = null;
                tileInvalid = null;
                tileAlternates = null;
                wallValid = null;
            }
            else
            {
                if (copyFrom.tileValid == null)
                    tileValid = null;
                else
                {
                    tileValid = new int[copyFrom.tileValid.Length];
                    Array.Copy(copyFrom.tileValid, tileValid, tileValid.Length);
                }

                if (copyFrom.tileInvalid == null)
                    tileInvalid = null;
                else
                {
                    tileInvalid = new int[copyFrom.tileInvalid.Length];
                    Array.Copy(copyFrom.tileInvalid, tileInvalid, tileInvalid.Length);
                }

                if (copyFrom.tileAlternates == null)
                    tileAlternates = null;
                else
                {
                    this.tileAlternates = new int[copyFrom.tileAlternates.Length];
                    Array.Copy(copyFrom.tileAlternates, tileAlternates, tileAlternates.Length);
                }

                if (copyFrom.wallValid == null)
                    wallValid = null;
                else
                {
                    wallValid = new int[copyFrom.wallValid.Length];
                    Array.Copy(copyFrom.wallValid, wallValid, wallValid.Length);
                }
            }
        }
    }
}