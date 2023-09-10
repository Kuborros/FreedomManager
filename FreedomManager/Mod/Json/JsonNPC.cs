using System.Text.Json.Serialization;

namespace FreedomManager.Mod.Json
{
    public class JsonNPC
    {
        public string UID { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string scene { get; set; }
        public string bundlePath { get; set; }
        public int species { get; set; }
        public int home { get; set; }
        public int dialogue { get; set; }
    }
}

