using IniParser.Model;
using IniParser.Parser;
using System;
using System.IO;

namespace FreedomManager
{
    internal class ModConfig
    {

        SectionDataCollection sections { get; set; }


        public ModConfig(string filename) {

            if (File.Exists(filename))
            {
                try
                {
                    var parser = new IniDataParser();
                    parser.Configuration.CommentString = "#";
                    IniData data = parser.Parse(File.ReadAllText(filename));

                    sections = data.Sections;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
