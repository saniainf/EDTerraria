/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Terraria;
using Terraria.Utilities;

namespace Terraria.IO
{
    public abstract class FileData
    {
        protected string _path;
        public FileMetadata Metadata;
        public string Name;
        public readonly string Type;
        protected bool _isFavorite;

        public string Path
        {
            get { return _path; }
        }

        public bool IsFavorite
        {
            get { return _isFavorite; }
        }

        protected FileData(string type)
        {
            Type = type;
        }

        protected FileData(string type, string path)
        {
            Type = type;
            _path = path;
            _isFavorite = Main.LocalFavoriteData.IsFavorite(this);
        }

        public void ToggleFavorite()
        {
            SetFavorite(!IsFavorite, true);
        }

        public string GetFileName(bool includeExtension = true)
        {
            return FileUtilities.GetFileName(Path, includeExtension);
        }

        public void SetFavorite(bool favorite, bool saveChanges = true)
        {
            _isFavorite = favorite;
            if (!saveChanges)
                return;

            Main.LocalFavoriteData.SaveFavorite(this);
        }

        public abstract void SetAsActive();
    }
}
