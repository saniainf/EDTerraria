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
            this._path = path;
            this.UseBson = useBson;
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
            lock (this._lock)
            {
                if (!File.Exists(this._path))
                {
                    result = false;
                }
                else
                {
                    try
                    {
                        if (!this.UseBson)
                        {
                            string text = File.ReadAllText(this._path);
                            this._data = JsonConvert.DeserializeObject<Dictionary<string, object>>(text, this._serializerSettings);
                        }
                        else
                        {
                            using (FileStream fileStream = File.OpenRead(this._path))
                            {
                                using (BsonReader bsonReader = new BsonReader(fileStream))
                                {
                                    JsonSerializer jsonSerializer = JsonSerializer.Create(this._serializerSettings);
                                    this._data = jsonSerializer.Deserialize<Dictionary<string, object>>(bsonReader);
                                }
                            }
                        }
                        if (this._data == null)
                        {
                            this._data = new Dictionary<string, object>();
                        }
                        if (this.OnLoad != null)
                        {
                            this.OnLoad(this);
                        }
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
            lock (this._lock)
            {
                try
                {
                    if (this.OnSave != null)
                    {
                        this.OnSave(this);
                    }
                    if (!createFile && !File.Exists(this._path))
                    {
                        result = false;
                        return result;
                    }
                    Directory.GetParent(this._path).Create();
                    if (!createFile)
                    {
                        File.SetAttributes(this._path, FileAttributes.Normal);
                    }
                    if (!this.UseBson)
                    {
                        File.WriteAllText(this._path, JsonConvert.SerializeObject(_data, null, _serializerSettings));
                        File.SetAttributes(this._path, FileAttributes.Normal);
                    }
                    else
                    {
                        using (FileStream fileStream = File.Create(this._path))
                        {
                            using (BsonWriter bsonWriter = new BsonWriter(fileStream))
                            {
                                File.SetAttributes(this._path, FileAttributes.Normal);
                                JsonSerializer jsonSerializer = JsonSerializer.Create(this._serializerSettings);
                                jsonSerializer.Serialize(bsonWriter, this._data);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to write file at: " + this._path);
                    Console.WriteLine(ex.ToString());
                    Monitor.Exit(this._lock);
                    result = false;
                    return result;
                }
                result = true;
            }
            return result;
        }
        public void Put(string name, object value)
        {
            lock (this._lock)
            {
                this._data[name] = value;
                if (this.AutoSave)
                {
                    this.Save(true);
                }
            }
        }
        public T Get<T>(string name, T defaultValue)
        {
            T result;
            lock (this._lock)
            {
                try
                {
                    object obj;
                    if (this._data.TryGetValue(name, out obj))
                    {
                        if (obj is T)
                        {
                            result = (T)((object)obj);
                        }
                        else
                        {
                            result = (T)((object)Convert.ChangeType(obj, typeof(T)));
                        }
                    }
                    else
                    {
                        result = defaultValue;
                    }
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
            currentValue = this.Get<T>(name, currentValue);
        }
    }
}