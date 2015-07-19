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
using System.Data.SQLite;
using System.Collections.Generic;

namespace Terraria
{
    public class ItemPrefix
    {
        /// <summary>
        /// name[0] = en;
        /// name[1] = de;
        /// name[2] = it;
        /// name[3] = fr;
        /// name[4] = es;
        /// </summary>
        public List<string> name = new List<string>();
        public float damageMod;
        public float knockBackMod;
        public float speedMod;
        public float sizeMod;
        public float velocityMod;
        public float manaCostMod;
        public int critStrikeMod;
        public float sellValueMod;
    }

    public class DB
    {
        private SQLiteConnection connection;
        private SQLiteDataReader reader;
        private const string folderName = "db";

        public static Dictionary<string, int> Tiles = new Dictionary<string, int>();
        public static Dictionary<string, int> Buffs = new Dictionary<string, int>();
        public static Dictionary<int, ItemPrefix> ItemPrefixes = new Dictionary<int, ItemPrefix>();

        public DB()
        {
            SQLiteDataReader tempReader = Load("terraria", "SELECT name, id FROM tiles");
            while (tempReader.Read())
                Tiles.Add(GetString(tempReader, 0), GetInt32(tempReader, 1));

            tempReader = Load("terraria", "SELECT name, id FROM buffs");
            while (tempReader.Read())
                Buffs.Add(GetString(tempReader, 0), GetInt32(tempReader, 1));

            tempReader = Load("terraria", "SELECT id, name_en, name_de, name_it, name_fr, name_es, damageMod, knockbackMod, speedMod, " + 
                "sizeMod, velocityMod, manaCostMod, critStrikeMod, sellValueMod FROM item_prefix");
            while(tempReader.Read())
            {
                ItemPrefix prefix = new ItemPrefix();
                prefix.name.Add(GetString(tempReader, 1));
                prefix.name.Add(GetString(tempReader, 2));
                prefix.name.Add(GetString(tempReader, 3));
                prefix.name.Add(GetString(tempReader, 4));
                prefix.name.Add(GetString(tempReader, 5));
                prefix.damageMod = GetFloat(tempReader, 6);
                prefix.knockBackMod = GetFloat(tempReader, 7);
                prefix.speedMod = GetFloat(tempReader, 8);
                prefix.sizeMod = GetFloat(tempReader, 9);
                prefix.velocityMod = GetFloat(tempReader, 10);
                prefix.manaCostMod = GetFloat(tempReader, 11);
                prefix.critStrikeMod = GetInt32(tempReader, 12);
                prefix.sellValueMod = GetFloat(tempReader, 13);

                ItemPrefixes.Add(GetInt32(tempReader, 0), prefix);
            }

            if (!tempReader.IsClosed)
                tempReader.Close();
            CloseConnections();
        }

        private SQLiteDataReader Load(string file, string query)
        {
            string str = string.Format(@"Data Source={0}\{1}.sqlite;", folderName, file);
            connection = new SQLiteConnection(str);
            try
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    reader = command.ExecuteReader();
                    return reader;
                }
            }
            catch
            {
                reader.Close();
                connection.Close();
                return null;
            }
        }

        private float GetFloat(SQLiteDataReader reader, int col)
        {
            return reader.IsDBNull(col) ? 0 : reader.GetFloat(col);
        }

        private string GetString(SQLiteDataReader reader, int col)
        {
            return reader.IsDBNull(col) ? string.Empty : reader.GetString(col);
        }

        private Int32 GetInt32(SQLiteDataReader reader, int col)
        {
            return reader.IsDBNull(col) ? 0 : reader.GetInt32(col);
        }

        private void CloseConnections()
        {
            if (!reader.IsClosed)
                reader.Close();
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }
}
