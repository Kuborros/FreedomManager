using System.Text.Json.Serialization;


namespace FreedomManager.Mod.Json
{
    internal class JsonMap
    {
        internal string Author { get; set; }
        internal string Name { get; set; }

        [JsonConstructor]
        internal JsonMap(string author, string name)
        {
            Author = author;
            Name = name;
        }
    }
}
