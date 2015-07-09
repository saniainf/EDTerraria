/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.IO;
using System.Text.RegularExpressions;

namespace Terraria.Utilities
{
    public static class FileUtilities
    {
        private static Regex FileNameRegex = new Regex("^(?<path>.*[\\\\\\/])?(?:$|(?<fileName>.+?)(?:(?<extension>\\.[^.]*$)|$))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void Delete(string path)
        {
            FileOperationAPIWrapper.MoveToRecycleBin(path);
        }

        public static string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public static void Copy(string source, string destination, bool overwrite = true)
        {
            File.Copy(source, destination, overwrite);
        }

        public static void Move(string source, string destination, bool overwrite = true)
        {
            FileUtilities.Copy(source, destination, overwrite);
            FileUtilities.Delete(source);
        }

        public static byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static void WriteAllBytes(string path, byte[] data)
        {
            if (data == null)
                return;

            FileUtilities.Write(path, data, data.Length);
        }

        public static void Write(string path, byte[] data, int length)
        {
            string parentFolderPath = FileUtilities.GetParentFolderPath(path, true);
            if (parentFolderPath != "")
                Directory.CreateDirectory(parentFolderPath);

            using (FileStream fileStream = File.Open(path, FileMode.Create))
                fileStream.Write(data, 0, length);
        }

        public static string GetFileName(string path, bool includeExtension = true)
        {
            Match match = FileUtilities.FileNameRegex.Match(path);
            if (match == null || match.Groups["fileName"] == null)
                return "";

            includeExtension &= match.Groups["extension"] != null;
            return match.Groups["fileName"].Value + (includeExtension ? "." + match.Groups["extension"].Value : "");
        }

        public static string GetParentFolderPath(string path, bool includeExtension = true)
        {
            Match match = FileUtilities.FileNameRegex.Match(path);
            if (match == null || match.Groups["path"] == null)
                return "";

            return match.Groups["path"].Value;
        }
    }
}
