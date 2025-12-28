using System;

namespace Systems {
    public interface IDataSerializer {
        public string Serialize(object data);
        public object Deserialize(string data, Type type);
    }
}