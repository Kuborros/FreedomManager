using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FreedomManager.Patches
{

    //Based on original work by @JohnnyonFlame on FP2res (https://github.com/JohnnyonFlame/fp2res)

    internal class ResolutionPatchController
    {

        public bool enabled = false;
        const int baseW = 640;
        const int baseH = 360;
        public Resolution currentRes;

        /*
        640x360
        1280x720
        1920x1080
        2560x1440
        3200x1800
        3840x2160
        4480x2520
        */

        public enum Resolution
        {
            x360,
            x720,
            x1080,
            x1440,
            x1800,
            x2160,
            x2520
        }

        byte[] pattern = ConvertHexStringToByteArray("506978656C2041727420427566666572"); //Pixel Art Buffer


        public ResolutionPatchController()
        {
            if (!File.Exists("FP2_Data/sharedassets0.assets")) return;

            if (!File.Exists("FP2_Data/sharedassets0.assets.backup"))
            {
                File.Copy("FP2_Data/sharedassets0.assets", "FP2_Data/sharedassets0.assets.backup");
            }
            currentRes = getCurrentResolution();
        }

        public Resolution getCurrentResolution()
        {
            byte[] bytes = File.ReadAllBytes("FP2_Data/sharedassets0.assets");

            int index = FindPattern(bytes, pattern);
            if (index == -1) return Resolution.x360;

            int resolution = BitConverter.ToInt32(bytes, index);

            Resolution res = (Resolution)((resolution / 640) - 1);
            enabled = (res != Resolution.x360);
            return res;

        }

        public bool setIntResolution(Resolution res)
        {
            if (res == currentRes) return true;

            byte[] newW = BitConverter.GetBytes(baseW * ((int)res + 1));
            byte[] newH = BitConverter.GetBytes(baseH * ((int)res + 1));

            byte[] bytes = File.ReadAllBytes("FP2_Data/sharedassets0.assets");

            byte[] replace = newW.Concat(newH).ToArray();

            int index = FindPattern(bytes, pattern);
            if (index == -1) return false;

            for (int i = index, replaceIndex = 0; i < bytes.Length && replaceIndex < replace.Length; i++, replaceIndex++)
            {
                bytes[i] = replace[replaceIndex];
            }

            File.WriteAllBytes("FP2_Data/sharedassets0.assets", bytes);
            Console.WriteLine("Pattern found at offset {0} and replaced.", index);

            return true;

        }

        private static int FindPattern(byte[] source, byte[] pattern)
        {
            return source.AsSpan().IndexOf(pattern) + pattern.Length;
        }

        private static byte[] ConvertHexStringToByteArray(string hexString)
        {
            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return data;
        }
    }
}
