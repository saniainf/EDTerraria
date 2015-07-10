/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace Terraria.ID
{
    public static class MountID
    {
        public static int Count = 14;

        public static class Sets
        {
            public static SetFactory Factory = new SetFactory(MountID.Count);
            public static bool[] Cart = Factory.CreateBoolSet(6, 11, 13);
        }
    }
}
