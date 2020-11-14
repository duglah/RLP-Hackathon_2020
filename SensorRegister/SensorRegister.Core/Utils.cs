using System;
using System.Linq;

namespace SensorRegister.Core
{
    public class Utils
    {
        private static Random random = new Random();
        public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomByteString(int byteCount)
        {
            return new string(Enumerable.Repeat("0123456789ABCDEF", byteCount * 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}