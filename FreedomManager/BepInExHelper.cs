using FreedomManager.Ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomManager
{
    public static class BepInExHelper
    {
        public const string HookDllName = "winhttp.dll";
        public const string ConfigName = "BepInEx/config/BepInEx.cfg";

        private static bool? _installed, _consoleEnabled;

        public static bool IsInstalled
        {
            get
            {
                if (_installed == null)
                {
                    _installed = File.Exists(HookDllName);
                }
                return _installed.Value;
            }
        }

        public static bool IsConsoleEnabled
        {
            get
            {
                if (!IsInstalled)
                {
                    return false;
                }

                if (!_consoleEnabled.HasValue)
                {
                    _consoleEnabled = ReadIsConsoleEnabled();
                }
                return _consoleEnabled.Value;
            }
            set
            {
                if (_consoleEnabled.HasValue && _consoleEnabled.Value == value)
                {
                    return;
                }

                WriteIsConsoleEnabled(value);
                _consoleEnabled = value;
            }
        }

        public static bool ToggleIsConsoleEnabled()
        {
            try
            {
                var file = IniReader.Read(ConfigName, Encoding.UTF8);
                var property = file["Logging.Console"].Properties["Enabled"];
                bool newValue = !bool.Parse(property.Value);
                property.Value = newValue.ToString();
                IniWriter.Write(file, ConfigName, Encoding.UTF8);

                _consoleEnabled = newValue;
                return newValue;
            }
            catch
            {
                _consoleEnabled = null;
                throw;
            }
        }

        private static bool ReadIsConsoleEnabled()
        {
            try
            {
                var file = IniReader.Read(ConfigName, Encoding.UTF8, new IniReaderOptions { ParseComments = false });
                return bool.Parse(file["Logging.Console"]["Enabled"]);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void WriteIsConsoleEnabled(bool value)
        {
            var file = IniReader.Read(ConfigName, Encoding.UTF8);
            file["Logging.Console"]["Enabled"] = value.ToString();
            IniWriter.Write(file, ConfigName, Encoding.UTF8);
        }

        [Obsolete("Should not exist - this class should handle all (un)installation logic, including clearing cached values", false)]
        public static void Refresh()
        {
            _installed = null;
            _consoleEnabled = null;
        }
    }
}
