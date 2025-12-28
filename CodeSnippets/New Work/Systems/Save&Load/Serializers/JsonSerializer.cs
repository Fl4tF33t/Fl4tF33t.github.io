using System;
using UnityEngine;

namespace Systems {
    public class JsonSerializer : IDataSerializer {
        public string Serialize(object data) => JsonUtility.ToJson(data, true);
        public object Deserialize(string data, Type type) => JsonUtility.FromJson(data, type);
    }
}