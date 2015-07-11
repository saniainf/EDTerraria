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
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Terraria;
using Terraria.Utilities;

namespace Terraria.Achievements
{
    public class AchievementManager
    {
        private static object _ioLock = new object();
        private Dictionary<string, Achievement> _achievements = new Dictionary<string, Achievement>();
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings();
        private Dictionary<string, int> _achievementIconIndexes = new Dictionary<string, int>();
        private string _savePath;
        private ICryptoTransform _encryptor;
        private ICryptoTransform _decryptor;

        public event Achievement.AchievementCompleted OnAchievementCompleted;

        public AchievementManager()
        {
            _savePath = "Data\\achievements.dat";
            byte[] numArray = Encoding.ASCII.GetBytes("RELOGIC-TERRARIA");
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            _encryptor = rijndaelManaged.CreateEncryptor(numArray, numArray);
            rijndaelManaged.Padding = PaddingMode.None;
            _decryptor = rijndaelManaged.CreateDecryptor(numArray, numArray);
        }

        public void Save()
        {
            lock (AchievementManager._ioLock)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, _encryptor, CryptoStreamMode.Write))
                        {
                            using (BsonWriter writer = new BsonWriter(cs))
                            {
                                JsonSerializer.Create(_serializerSettings).Serialize((JsonWriter)writer, _achievements);
                                ((JsonWriter)writer).Flush();
                                cs.FlushFinalBlock();
                                FileUtilities.Write(_savePath, ms.GetBuffer(), (int)ms.Length);
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public List<Achievement> CreateAchievementsList()
        {
            return Enumerable.ToList<Achievement>((IEnumerable<Achievement>)_achievements.Values);
        }

        public void Load()
        {
            lock (_ioLock)
            {
                if (!FileUtilities.Exists(_savePath))
                    return;

                byte[] bytes = FileUtilities.ReadAllBytes(_savePath);
                Dictionary<string, StoredAchievement> tempStoredList = (Dictionary<string, StoredAchievement>)null;
                try
                {
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, _decryptor, CryptoStreamMode.Read))
                        {
                            using (BsonReader reader = new BsonReader(cs))
                                tempStoredList = (Dictionary<string, StoredAchievement>)JsonSerializer.Create(_serializerSettings).Deserialize<Dictionary<string, StoredAchievement>>((JsonReader)reader);
                        }
                    }
                }
                catch
                {
                    FileUtilities.Delete(_savePath);
                    return;
                }

                if (tempStoredList != null)
                {
                    foreach (KeyValuePair<string, StoredAchievement> achievements in tempStoredList)
                    {
                        if (_achievements.ContainsKey(achievements.Key))
                            _achievements[achievements.Key].Load(achievements.Value.Conditions);
                    }
                }
            }
        }

        private void AchievementCompleted(Achievement achievement)
        {
            Save();
            if (OnAchievementCompleted != null)
                OnAchievementCompleted(achievement);
        }

        public void Register(Achievement achievement)
        {
            _achievements.Add(achievement.Name, achievement);
            achievement.OnCompleted += new Achievement.AchievementCompleted(AchievementCompleted);
        }

        public void RegisterIconIndex(string achievementName, int iconIndex)
        {
            _achievementIconIndexes.Add(achievementName, iconIndex);
        }

        public void RegisterAchievementCategory(string achievementName, AchievementCategory category)
        {
            _achievements[achievementName].SetCategory(category);
        }

        public Achievement GetAchievement(string achievementName)
        {
            Achievement achievement;
            if (_achievements.TryGetValue(achievementName, out achievement))
                return achievement;

            return null;
        }

        public T GetCondition<T>(string achievementName, string conditionName) where T : AchievementCondition
        {
            return GetCondition(achievementName, conditionName) as T;
        }

        public AchievementCondition GetCondition(string achievementName, string conditionName)
        {
            Achievement achievement;
            if (_achievements.TryGetValue(achievementName, out achievement))
                return achievement.GetCondition(conditionName);

            return null;
        }

        public int GetIconIndex(string achievementName)
        {
            int num;
            if (_achievementIconIndexes.TryGetValue(achievementName, out num))
                return num;

            return 0;
        }

        private class StoredAchievement
        {
            public Dictionary<string, JObject> Conditions = new Dictionary<string, JObject>();
        }
    }
}
