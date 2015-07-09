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
                this.tileValid = (int[])null;
                this.tileInvalid = (int[])null;
                this.tileAlternates = (int[])null;
                this.wallValid = (int[])null;
            }
            else
            {
                if (copyFrom.tileValid == null)
                {
                    this.tileValid = (int[])null;
                }
                else
                {
                    this.tileValid = new int[copyFrom.tileValid.Length];
                    Array.Copy((Array)copyFrom.tileValid, (Array)this.tileValid, this.tileValid.Length);
                }
                if (copyFrom.tileInvalid == null)
                {
                    this.tileInvalid = (int[])null;
                }
                else
                {
                    this.tileInvalid = new int[copyFrom.tileInvalid.Length];
                    Array.Copy((Array)copyFrom.tileInvalid, (Array)this.tileInvalid, this.tileInvalid.Length);
                }
                if (copyFrom.tileAlternates == null)
                {
                    this.tileAlternates = (int[])null;
                }
                else
                {
                    this.tileAlternates = new int[copyFrom.tileAlternates.Length];
                    Array.Copy((Array)copyFrom.tileAlternates, (Array)this.tileAlternates, this.tileAlternates.Length);
                }
                if (copyFrom.wallValid == null)
                {
                    this.wallValid = (int[])null;
                }
                else
                {
                    this.wallValid = new int[copyFrom.wallValid.Length];
                    Array.Copy((Array)copyFrom.wallValid, (Array)this.wallValid, this.wallValid.Length);
                }
            }
        }
    }
}