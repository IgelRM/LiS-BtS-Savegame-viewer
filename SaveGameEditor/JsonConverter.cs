using System.Text;
using Newtonsoft.Json;

namespace SaveGameEditor
{
    public static class JsonConverter
    {
        /// <summary>
        ///  Convert given encoded bytes array to a json object 
        /// </summary>
        /// <param name="data">Bytes array object to be converted</param>
        /// <returns>Json object</returns>
        public static object DecodeFileContentToJson(byte[] data)
        {
            var decodedContent = DeckNineXorEncoder.Encode(data);
            var jsonString = Encoding.UTF8.GetString(decodedContent);
            return JsonConvert.DeserializeObject(jsonString);
        }

        /// <summary>
        /// Convert given json object to an encoded bytes array
        /// </summary>
        /// <param name="json">Json object to be converted</param>
        /// <returns>Encrypted bytes array</returns>
        public static byte[] EncodeJsonToFileContent(object json)
        {
            var jsonString = JsonConvert.SerializeObject(json, Formatting.Indented);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            var hash = DeckNineXorEncoder.ComputeContentHash(jsonBytes);
            var encodedContent = DeckNineXorEncoder.Encode(jsonBytes);

            var newFile = new byte[DeckNineXorEncoder.ContentlessHeaderLength + encodedContent.Length];

            int offset = 0;
            int i;
            for (i = 0; i < DeckNineXorEncoder.EncryptedFileSignature.Length + offset; i++)
            {
                newFile[i] = DeckNineXorEncoder.EncryptedFileSignature[i - offset];
            }
            offset = i;
            while (i < hash.Length + offset)
            {
                newFile[i] = hash[i - offset];
                i++;
            }
            offset = i;
            while (i < encodedContent.Length + offset)
            {
                newFile[i] = encodedContent[i - offset];
                i++;
            }

            return newFile;
        }
    }
}
