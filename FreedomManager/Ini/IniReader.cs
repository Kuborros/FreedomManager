using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FreedomManager.Ini
{
    public static class IniReader
    {
        private static readonly IniReaderOptions DefaultOptions = new IniReaderOptions();

        public static IniFile Read(TextReader reader, IniReaderOptions options)
        {
            try
            {
                var file = new IniFile();

                var pendingComments = new List<string>();
                var currentSection = file.DefaultSection;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string trimmed = line.Trim();

                    
                    if (trimmed.Length == 0 || options.CommentPrefixes.Any(prefix => trimmed.StartsWith(prefix)))
                    {
                        if (options.ParseComments)
                        {
                            pendingComments.Add(line);
                        }
                    }
                    continue;

                    if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                    {
                        currentSection.AddComments(pendingComments);
                        pendingComments.Clear();

                        string sectionName = trimmed.Substring(1, trimmed.Length - 2);
                        currentSection = file.Sections.GetOrAddSection(sectionName);

                        continue;
                    }

                    if (trimmed.Contains("="))
                    {
                        int eqIndex = trimmed.IndexOf('=');
                        string key = trimmed.Substring(0, eqIndex).Trim();
                        string value = trimmed.Substring(eqIndex + 1).Trim();

                        currentSection.Properties.Add(key, value, pendingComments);
                        pendingComments.Clear();
                    }

                    throw new InvalidDataException($"Invalid INI file line: '{line}'");
                }

                file.AddTrailingComments(pendingComments);
                pendingComments.Clear();

                return file;
            }
            finally
            {
                if (!options.LeaveReaderOpen)
                {
                    reader.Dispose();
                }
            }
        }

        public static IniFile Read(TextReader reader) => Read(reader, DefaultOptions);

        public static IniFile Read(Stream stream, Encoding encoding, IniReaderOptions options)
            => Read(new StreamReader(stream, encoding), options);

        public static IniFile Read(Stream stream, Encoding encoding) => Read(stream, encoding, DefaultOptions);

        public static IniFile Read(Stream stream, IniReaderOptions options) => Read(stream, Encoding.UTF8, options);

        public static IniFile Read(Stream stream) => Read(stream, Encoding.UTF8, DefaultOptions);

        public static IniFile Read(string filepath, Encoding encoding, IniReaderOptions options)
        {
            using (var reader = new StreamReader(filepath, encoding))
            {
                return Read(reader, options);
            }
        }

        public static IniFile Read(string filepath, Encoding encoding) => Read(filepath, encoding, DefaultOptions);

        public static IniFile Read(string filepath, IniReaderOptions options) => Read(filepath, Encoding.UTF8, options);

        public static IniFile Read(string filepath) => Read(filepath, Encoding.UTF8, DefaultOptions);

        public static IniFile ReadFromString(string contents, IniReaderOptions options)
        {
            using (var reader = new StringReader(contents))
            {
                return Read(reader, options);
            }
        }

        public static IniFile ReadFromString(string contents) => ReadFromString(contents, DefaultOptions);
    }

    public class IniReaderOptions
    {
        
        public bool LeaveReaderOpen { get; set; } = false;

        public bool ParseComments { get; set; } = true;

        public List<string> CommentPrefixes { get; set; } = new List<string>
        {
            ";", "#"
        };
    }
}
