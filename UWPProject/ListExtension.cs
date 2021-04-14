using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace UWPProject
{
    public static class ListExtension
    {
        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> result = new List<T>(list);
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = result.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = result[k];
                result[k] = result[n];
                result[n] = value;
            }

            provider.Dispose();

            return result;
        }
    }
}
