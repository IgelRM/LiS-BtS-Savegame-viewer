using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace savefiledecoder
{
    class DecodeEncode
    {
        static private byte[] s_FileHeader = new byte[4]   { 81, 55, 110, 170 };

        private static byte[] s_key = new byte[1] { 0 };

        public static void SetKey(byte[] key)
        {
            s_key = key;
        }

        public static byte[] Decode(byte [] encoded)
        {
            bool headerFound = true;
            // test if the file starts with the expected header
            for (int i = 0; i < s_FileHeader.Length; i++)
            {
                if (encoded[i] != s_FileHeader[i])
                {
                    headerFound = false;
                    break;
                }
            }

            int contentOffset = 0;
            if (headerFound)
                contentOffset = 20;  // 4 byte header, 16 byte md5sum of content..
            byte[] decoded = Decrypt(encoded, contentOffset);
             return decoded;
        }
    
        private static byte[] Decrypt(byte[] encoded, int contentOffset)
        {
            byte[] content = new byte[encoded.Length - contentOffset];

            // simple xor encryption
            for (int i = 0; i < content.Length; i++)
            {
                content[i] = (byte)( encoded[i+contentOffset] ^ s_key[i % s_key.Length]);
            }

            return content;
        }
    }


}
