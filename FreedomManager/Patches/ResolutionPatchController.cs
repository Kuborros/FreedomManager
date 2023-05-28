using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace FreedomManager.Patches
{
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
            int resolution = BitConverter.ToInt32(bytes, index);

            Resolution res = (Resolution)((resolution / 640) - 1);
            enabled = (res != Resolution.x360);
            return res;

        }

        public void setIntResolution(Resolution res)
        {
            if (res == currentRes) return;

            byte[] newW = BitConverter.GetBytes(baseW * ((int)res + 1));
            byte[] newH = BitConverter.GetBytes(baseH * ((int)res + 1));

            byte[] bytes = File.ReadAllBytes("FP2_Data/sharedassets0.assets");

            byte[] replace = newW.Concat(newH).ToArray();

                int index = FindPattern(bytes, pattern);
                for (int i = index, replaceIndex = 0; i < bytes.Length && replaceIndex < replace.Length; i++, replaceIndex++)
                {
                    bytes[i] = replace[replaceIndex];
                }
                File.WriteAllBytes("FP2_Data/sharedassets0.assets", bytes);
                Console.WriteLine("Pattern found at offset {0} and replaced.", index);

        }

        private static int FindPattern(byte[] source, byte[] pattern)
        {
            for (int i = 1113300; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    return i + pattern.Length;
                }
            }
            return 0;
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
