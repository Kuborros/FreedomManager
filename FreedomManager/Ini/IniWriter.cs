using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FreedomManager.Ini
{
    public static class IniWriter
    {
        private static readonly IniWriterOptions DefaultOptions = new IniWriterOptions();

        private static void WriteComments(TextWriter writer, IEnumerable<string> comments)
        {
            foreach (string comment in comments)
            {
                writer.WriteLine(comment);
            }
        }

        private static void WriteComments(TextWriter writer, IniElement element)
        {
            if (element.HasComments)
            {
                WriteComments(writer, element.Comments);
            }
        }

        private static void WriteSection(TextWriter writer, IniSection section, bool writeHeader)
        {
            WriteComments(writer, section);

            if (writeHeader)
            {
                writer.Write('[');
                writer.Write(section.Name);
                writer.WriteLine(']');
            }

            foreach (var property in section.Properties)
            {
                WriteComments(writer, property);
                writer.Write(property.Key);
                writer.Write(" = ");
                writer.WriteLine(property.Value);
            }
        }

        public static void Write(IniFile file, TextWriter writer, IniWriterOptions options)
        {
            try
            {
                WriteSection(writer, file.DefaultSection, false);
                foreach (var section in file.Sections)
                {
                    WriteSection(writer, section, true);
                }
                
                if (file.HasTrailingComments)
                {
                    WriteComments(writer, file.TrailingComments);
                }
            }
            finally
            {
                if (!options.LeaveWriterOpen)
                {
                    writer.Dispose();
                }
            }
        }

        public static void Write(IniFile file, TextWriter writer) => Write(file, writer, DefaultOptions);

        public static void Write(IniFile file, Stream stream, Encoding encoding, IniWriterOptions options)
            => Write(file, new StreamWriter(stream, encoding), options);

        public static void Write(IniFile file, Stream stream, IniWriterOptions options) => Write(file, stream, Encoding.UTF8, options);

        public static void Write(IniFile file, Stream stream, Encoding encoding) => Write(file, stream, encoding, DefaultOptions);

        public static void Write(IniFile file, Stream stream) => Write(file, stream, Encoding.UTF8, DefaultOptions);

        public static void Write(IniFile file, string filepath, Encoding encoding, IniWriterOptions options)
        {
            using (var writer = new StreamWriter(filepath, false, encoding))
            {
                Write(file, writer, options);
            }
        }

        public static void Write(IniFile file, string filepath, IniWriterOptions options) => Write(file, filepath, Encoding.UTF8, options);

        public static void Write(IniFile file, string filepath, Encoding encoding) => Write(file, filepath, encoding, DefaultOptions);

        public static void Write(IniFile file, string filepath) => Write(file, filepath, Encoding.UTF8, DefaultOptions);

        public static string WriteToString(IniFile file, IniWriterOptions options)
        {
            using (var writer = new StringWriter())
            {
                Write(file, writer, options);
                return writer.ToString();
            }
        }

        public static string WriteToString(IniFile file) => WriteToString(file, DefaultOptions);
    }

    public class IniWriterOptions
    {
        public bool LeaveWriterOpen { get; set; } = false;
    }
}
