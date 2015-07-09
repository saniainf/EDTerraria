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

namespace Terraria.Utilities
{
    public static class FileOperationAPIWrapper
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

        [Flags]
        private enum FileOperationFlags : ushort
        {
            FOF_ALLOWUNDO = 0x40,
            FOF_NOCONFIRMATION = 0x10,
            FOF_NOERRORUI = 0x400,
            FOF_SILENT = 4,
            FOF_SIMPLEPROGRESS = 0x100,
            FOF_WANTNUKEWARNING = 0x4000
        }

        private enum FileOperationType : uint
        {
            FO_COPY = 2,
            FO_DELETE = 3,
            FO_MOVE = 1,
            FO_RENAME = 4
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public FileOperationAPIWrapper.FileOperationType wFunc;
            public string pFrom;
            public string pTo;
            public FileOperationAPIWrapper.FileOperationFlags fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        private static bool DeleteCompletelySilent(string path)
        {
            return DeleteFile(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI | FileOperationFlags.FOF_SILENT);
        }

        private static bool DeleteFile(string path, FileOperationFlags flags)
        {
            try
            {
                SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT
                {
                    wFunc = FileOperationType.FO_DELETE,
                    pFrom = path + '\0' + '\0',
                    fFlags = flags
                };
                SHFileOperation(ref fileOp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool MoveToRecycleBin(string path)
        {
            return Send(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI | FileOperationFlags.FOF_SILENT);
        }

        private static bool Send(string path)
        {
            return Send(path, FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_WANTNUKEWARNING);
        }

        private static bool Send(string path, FileOperationFlags flags)
        {
            try
            {
                SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT
                {
                    wFunc = FileOperationType.FO_DELETE,
                    pFrom = path + '\0' + '\0',
                    fFlags = (FileOperationFlags)((ushort)(FileOperationFlags.FOF_ALLOWUNDO | flags))
                };
                SHFileOperation(ref fileOp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}