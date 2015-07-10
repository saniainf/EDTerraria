/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Terraria.Utilities;

namespace Terraria.IO
{
    public class FavoritesFile
    {
        private Dictionary<string, Dictionary<string, bool>> _data = new Dictionary<string, Dictionary<string, bool>>();
        public readonly string Path;
        public readonly bool IsCloudSave;

        public FavoritesFile(string path)
        {
            Path = path;
        }

        public void SaveFavorite(FileData fileData)
        {
            if (!_data.ContainsKey(fileData.Type))
                _data.Add(fileData.Type, new Dictionary<string, bool>());

            _data[fileData.Type][fileData.GetFileName(true)] = fileData.IsFavorite;
            Save();
        }

        public void ClearEntry(FileData fileData)
        {
            if (!_data.ContainsKey(fileData.Type))
                return;

            _data[fileData.Type].Remove(fileData.GetFileName(true));
            Save();
        }

        public bool IsFavorite(FileData fileData)
        {
            if (!_data.ContainsKey(fileData.Type))
                return false;

            string fileName = fileData.GetFileName(true);
            bool flag;
            if (_data[fileData.Type].TryGetValue(fileName, out flag))
                return flag;

            return false;
        }

        public void Save()
        {
            FileUtilities.WriteAllBytes(Path, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(_data, (Formatting)1)));
        }

        public void Load()
        {
            if (!FileUtilities.Exists(Path))
                _data.Clear();
            else
            {
                _data = (Dictionary<string, Dictionary<string, bool>>)JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, bool>>>(Encoding.ASCII.GetString(FileUtilities.ReadAllBytes(Path)));
                if (_data != null)
                    return;

                _data = new Dictionary<string, Dictionary<string, bool>>();
            }
        }
    }
}
