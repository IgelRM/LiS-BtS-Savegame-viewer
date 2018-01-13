using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace savefiledecoder
{
    public static class DeckNineXorEncoder
    {
        private static readonly MD5 ContentHash = MD5.Create();

        public static byte[] Key { get; private set; } = { 0 };
        public static readonly byte[] EncryptedFileSignature = {81, 55, 110, 170};
        public static readonly int ContentlessHeaderLength = EncryptedFileSignature.Length + ContentHash.HashSize/8;

        /// <summary>
        /// Load repeating key from a given file
        /// </summary>
        /// <param name="assemblyPath">Assembly-CSharp.dll file path</param>
        public static void ReadKeyFromFile(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException($"Unable to find file {assemblyPath}. Save data can't be decoded.");
            }

            try
            {
                var ass = Assembly.Load(File.ReadAllBytes(assemblyPath));
                var t = ass.GetType("T_3EF937CB");
                var keyField = t.GetField("_18AFCD9AB", BindingFlags.Static | BindingFlags.NonPublic);
                Key = (byte[]) keyField.GetValue(null);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to read a key from a given file {assemblyPath}. Save data can't be decoded.", ex);
            }
        }

        /// <summary>
        /// Compute hash for a given array of bytes
        /// </summary>
        /// <param name="content">Array of bytes to be processed</param>
        /// <returns>Computed hash</returns>
        public static byte[] ComputeContentHash(byte[] content)
        {
            return ContentHash.ComputeHash(content);
        }

        /// <summary>
        /// Repeatedly encode and decode given data. Cuts off service header.
        /// </summary>
        /// <param name="data">Array of bytes to be processed</param>
        /// <returns>Decoded data for encoded input and vice versa</returns>
        public static byte[] Encode(byte[] data)
        {
            var signatureFound = false;
            if (data.Length > ContentlessHeaderLength)
            {
                signatureFound = true;
                // test if the file starts with the expected header
                for (int i = 0; i < EncryptedFileSignature.Length; i++)
                {
                    if (data[i] != EncryptedFileSignature[i])
                    {
                        signatureFound = false;
                        break;
                    }
                }
            }

            var contentOffset = signatureFound ? ContentlessHeaderLength : 0;
            return Encrypt(data, contentOffset);
        }

        /// <summary>
        /// Apply a XOR operation to a data array using a key to repeatedly encode and decode data
        /// </summary>
        /// <param name="data">Array of bytes to be processed</param>
        /// <param name="contentOffset">Offset where real content data starts</param>
        /// <returns></returns>
        private static byte[] Encrypt(byte[] data, int contentOffset)
        {
            var content = new byte[data.Length - contentOffset];

            // simple xor encryption
            for (int i = 0; i < content.Length; i++)
            {
                content[i] = (byte) (data[i + contentOffset] ^ Key[i%Key.Length]);
            }

            return content;
        }
    }
}
