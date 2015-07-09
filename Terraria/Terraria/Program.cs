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
using System.IO;
using System.Windows.Forms;

namespace Terraria
{
    internal static class Program
    {
        public const bool IsServer = false;
        public static Dictionary<string, string> LaunchParameters = new Dictionary<string, string>();
        public static void Main(string[] args)
        {
            try
            {
                using (Main main = new Main())
                {
                    Program.LaunchParameters = Utils.ParseArguements(args);
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].ToLower() == "-port" || args[i].ToLower() == "-p")
                        {
                            i++;
                            try
                            {
                                int listenPort = Convert.ToInt32(args[i]);
                                Netplay.ListenPort = listenPort;
                            }
                            catch { }
                        }
                        if (args[i].ToLower() == "-join" || args[i].ToLower() == "-j")
                        {
                            i++;
                            try
                            {
                                main.AutoJoin(args[i]);
                            }
                            catch { }
                        }
                        if (args[i].ToLower() == "-pass" || args[i].ToLower() == "-password")
                        {
                            i++;
                            Netplay.ServerPassword = args[i];
                            main.AutoPass();
                        }
                        if (args[i].ToLower() == "-host")
                        {
                            main.AutoHost();
                        }
                        if (args[i].ToLower() == "-loadlib")
                        {
                            i++;
                            string path = args[i];
                            main.loadLib(path);
                        }
                    }
                    main.Run();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
                    {
                        streamWriter.WriteLine(DateTime.Now);
                        streamWriter.WriteLine(ex);
                        streamWriter.WriteLine("");
                    }
                    MessageBox.Show(ex.ToString(), "Terraria: Error");
                }
                catch { }
            }
        }
    }
}
