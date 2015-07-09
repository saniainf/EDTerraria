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
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Terraria;

namespace Terraria.Utilities
{
    public class CrashDump
    {
        [DllImport("dbghelp.dll")]
        private static extern bool MiniDumpWriteDump(IntPtr hProcess, int ProcessId, IntPtr hFile, CrashDump.MINIDUMP_TYPE DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);

        public static void Create()
        {
            DateTime dateTime = DateTime.Now.ToLocalTime();
            CrashDump.Create("Terraria " + Main.versionNumber + " " + dateTime.Year.ToString("D4") + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2") + " " + dateTime.Hour.ToString("D2") + "_" + dateTime.Minute.ToString("D2") + "_" + dateTime.Second.ToString("D2") + ".dmp");
        }

        public static void CreateFull()
        {
            DateTime dateTime = DateTime.Now.ToLocalTime();
            using (FileStream fileStream = File.Create("DMP-FULL Terraria " + Main.versionNumber + " " + dateTime.Year.ToString("D4") + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2") + " " + dateTime.Hour.ToString("D2") + "_" + dateTime.Minute.ToString("D2") + "_" + dateTime.Second.ToString("D2") + ".dmp"))
            {
                Process currentProcess = Process.GetCurrentProcess();
                CrashDump.MiniDumpWriteDump(currentProcess.Handle, currentProcess.Id, fileStream.SafeFileHandle.DangerousGetHandle(), CrashDump.MINIDUMP_TYPE.MiniDumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void Create(string path)
        {
            bool flag = Program.LaunchParameters.ContainsKey("-fulldump");
            using (FileStream fileStream = File.Create(path))
            {
                Process currentProcess = Process.GetCurrentProcess();
                CrashDump.MiniDumpWriteDump(currentProcess.Handle, currentProcess.Id, fileStream.SafeFileHandle.DangerousGetHandle(), flag ? CrashDump.MINIDUMP_TYPE.MiniDumpWithFullMemory : CrashDump.MINIDUMP_TYPE.MiniDumpWithIndirectlyReferencedMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        internal enum MINIDUMP_TYPE
        {
            MiniDumpNormal = 0,
            MiniDumpWithDataSegs = 1,
            MiniDumpWithFullMemory = 2,
            MiniDumpWithHandleData = 4,
            MiniDumpFilterMemory = 8,
            MiniDumpScanMemory = 16,
            MiniDumpWithUnloadedModules = 32,
            MiniDumpWithIndirectlyReferencedMemory = 64,
            MiniDumpFilterModulePaths = 128,
            MiniDumpWithProcessThreadData = 256,
            MiniDumpWithPrivateReadWriteMemory = 512,
            MiniDumpWithoutOptionalData = 1024,
            MiniDumpWithFullMemoryInfo = 2048,
            MiniDumpWithThreadInfo = 4096,
            MiniDumpWithCodeSegs = 8192
        }
    }
}
