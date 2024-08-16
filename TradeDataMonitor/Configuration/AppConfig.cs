using Newtonsoft.Json;
using System.IO;

namespace TradeMonitor.Configuration
{
    public class AppConfig
    {
        public string InputDirectory { get; set; }
        public int MonitoringFrequencySeconds { get; set; }
        public List<string> EnabledLoaders { get; set; }

        public static AppConfig Load(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<AppConfig>(json);
        }

        public void Save(string path)
        {
            string json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}