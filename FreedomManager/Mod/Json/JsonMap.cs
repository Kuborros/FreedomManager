using System.Text.Json.Serialization;


namespace FreedomManager.Mod.Json
{
    internal class JsonMap
    {
        public string UID { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string version { get; set; }
        public string bundlePath { get; set; }
    }
}
