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
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Terraria.IO
{
    public class Preferences
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();
        private readonly string _path;
        private readonly JsonSerializerSettings _serializerSettings;
        public readonly bool UseBson;
        private readonly object _lock = new object();
        public bool AutoSave;
        public event Action<Preferences> OnSave;
        public event Action<Preferences> OnLoad;

        public Preferences(string path, bool parseAllTypes = false, bool useBson = false)
        {
            _path = path;
            UseBson = useBson;
            if (parseAllTypes)
            {
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.TypeNameHandling = TypeNameHandling.None;
                jsonSerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Default;
                _serializerSettings = jsonSerializerSettings;
                return;
            }
            _serializerSettings = new JsonSerializerSettings();
        }

        public bool Load()
        {
            bool result;
            lock (_lock)
            {
                if (!File.Exists(_path))
                    result = false;
                else
                {
                    try
                    {
                        if (!UseBson)
                        {
                            string text = File.ReadAllText(_path);
                            this._data = JsonConvert.DeserializeObject<Dictionary<string, object>>(text, _serializerSettings);
                        }
                        else
                        {
                            using (FileStream fileStream = File.OpenRead(_path))
                            {
                                using (BsonReader bsonReader = new BsonReader(fileStream))
                                {
                                    JsonSerializer jsonSerializer = JsonSerializer.Create(_serializerSettings);
                                    _data = jsonSerializer.Deserialize<Dictionary<string, object>>(bsonReader);
                                }
                            }
                        }

                        if (_data == null)
                            _data = new Dictionary<string, object>();
                        if (OnLoad != null)
                            OnLoad(this);
                        result = true;
                    }
                    catch (Exception)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool Save(bool createFile = true)
        {
            bool result;
            lock (_lock)
            {
                try
                {
                    if (OnSave != null)
                        OnSave(this);
                    if (!createFile && !File.Exists(_path))
                    {
                        result = false;
                        return result;
                    }

                    Directory.GetParent(_path).Create();
                    if (!createFile)
                        File.SetAttributes(_path, FileAttributes.Normal);
                    if (!UseBson)
                    {
                        File.WriteAllText(_path, JsonConvert.SerializeObject(_data, null, _serializerSettings));
                        File.SetAttributes(_path, FileAttributes.Normal);
                    }
                    else
                    {
                        using (FileStream fileStream = File.Create(_path))
                        {
                            using (BsonWriter bsonWriter = new BsonWriter(fileStream))
                            {
                                File.SetAttributes(_path, FileAttributes.Normal);
                                JsonSerializer jsonSerializer = JsonSerializer.Create(_serializerSettings);
                                jsonSerializer.Serialize(bsonWriter, _data);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to write file at: " + _path);
                    Console.WriteLine(ex.ToString());
                    Monitor.Exit(_lock);
                    result = false;
                    return result;
                }
                result = true;
            }
            return result;
        }

        public void Put(string name, object value)
        {
            lock (_lock)
            {
                _data[name] = value;
                if (AutoSave)
                    Save(true);
            }
        }

        public T Get<T>(string name, T defaultValue)
        {
            T result;
            lock (_lock)
            {
                try
                {
                    object obj;
                    if (_data.TryGetValue(name, out obj))
                    {
                        if (obj is T)
                            result = (T)(obj);
                        else
                            result = (T)(Convert.ChangeType(obj, typeof(T)));
                    }
                    else
                        result = defaultValue;
                }
                catch
                {
                    result = defaultValue;
                }
            }
            return result;
        }

        public void Get<T>(string name, ref T currentValue)
        {
            currentValue = Get<T>(name, currentValue);
        }
    }
}