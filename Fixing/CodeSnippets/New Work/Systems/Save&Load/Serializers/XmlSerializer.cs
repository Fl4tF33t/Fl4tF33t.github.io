using System;
using System.IO;
using System.Xml.Serialization;

namespace Systems {
    public class XmlSerializer : IDataSerializer {
        public string Serialize(object data) {
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(data.GetType());
            using var writer = new StringWriter();
            xml.Serialize(writer, data);
            return writer.ToString();
        }

        public object Deserialize(string data, Type type) {
            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(type);
            using var reader = new StringReader(data);
            return xml.Deserialize(reader);
        }
    }
}