using System;
using System.Text;

namespace Systems {
    public class XorEncryptedSerializer : IDataSerializer {
        private readonly IDataSerializer serializer;
        private readonly byte[] keyBytes;

        public XorEncryptedSerializer(IDataSerializer serializer, string key) {
            this.serializer = serializer;
            keyBytes = Encoding.UTF8.GetBytes(key);
        }

        public string Serialize(object data) {
            string plain = serializer.Serialize(data);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plain);
            byte[] encrypted = Xor(plainBytes);
            return Convert.ToBase64String(encrypted);
        }

        public object Deserialize(string data, Type type) {
            byte[] encrypted = Convert.FromBase64String(data);
            byte[] plainBytes = Xor(encrypted);
            string plain = Encoding.UTF8.GetString(plainBytes);
            return serializer.Deserialize(plain, type);
        }

        private byte[] Xor(byte[] input) {
            var output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++) {
                output[i] = (byte)(input[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return output;
        }
    }
}