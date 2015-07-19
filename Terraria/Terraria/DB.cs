/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System.Data.SQLite;
using System.Collections.Generic;

namespace Terraria
{
    public class DB
    {
        private SQLiteConnection connection;
        private SQLiteDataReader reader;
        private const string folderName = "db";

        public static Dictionary<string, int> Tiles = new Dictionary<string, int>();
        public static Dictionary<string, int> Buffs = new Dictionary<string, int>();

        public DB()
        {
            SQLiteDataReader tempReader = Load("terraria", "SELECT name, id FROM tiles");
            while (tempReader.Read())
                Tiles.Add(tempReader.GetString(0), tempReader.GetInt32(1));

            tempReader = Load("terraria", "SELECT name, id FROM buffs");
            while (tempReader.Read())
                Buffs.Add(tempReader.GetString(0), tempReader.GetInt32(1));

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

        private void CloseConnections()
        {
            if (!reader.IsClosed)
                reader.Close();
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }
}
