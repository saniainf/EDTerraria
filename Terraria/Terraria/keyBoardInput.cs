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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Terraria
{
    public class keyBoardInput
    {
        public static bool slashToggle = true;

        public static event Action<char> newKeyEvent;

        static keyBoardInput()
        {
            Application.AddMessageFilter((IMessageFilter)new keyBoardInput.inKey());
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool TranslateMessage(IntPtr message);

        public class inKey : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 258)
                {
                    char ch = (char)(int)m.WParam;
                    Console.WriteLine(ch);
                    if (keyBoardInput.newKeyEvent != null)
                        keyBoardInput.newKeyEvent(ch);
                }
                else if (m.Msg == 256)
                {
                    IntPtr num = Marshal.AllocHGlobal(Marshal.SizeOf((object)m));
                    Marshal.StructureToPtr((object)m, num, true);
                    keyBoardInput.TranslateMessage(num);
                }
                return false;
            }
        }
    }
}
