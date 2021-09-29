using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJOPriset.Models
{
    public class SettingJson
    {
        [JsonIgnore]
        private static readonly string SAVE_FILE_PATH = Path.Combine(Environment.CurrentDirectory, "UserData", "NJOPrisetSongs.json");
        public virtual ConcurrentDictionary<string, int> Songs { get; set; } = new ConcurrentDictionary<string, int>();
        public void Save()
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(SAVE_FILE_PATH, json);
        }
        public static SettingJson Load()
        {
            if (!File.Exists(SAVE_FILE_PATH)) {
                new SettingJson().Save();
            }
            var json = File.ReadAllText(SAVE_FILE_PATH);
            return JsonConvert.DeserializeObject<SettingJson>(json);
        }
    }
}
