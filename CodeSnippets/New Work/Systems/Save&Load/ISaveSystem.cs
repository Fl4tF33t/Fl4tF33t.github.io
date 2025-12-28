using System;
using System.IO;

namespace Systems {
    public interface ISaveSystem {
        public void Save(string filename, object data);
        public bool Load(string filename, Type type, out object data);
        // Use below if Generic is know in compile time, eg, loading a single large file "SaveData"
        //public T Load<T>(string filename);
        public void DeleteAll();
    }

    public class FileSaveSystem : ISaveSystem {
        private const string FOLDER_NAME = "SaveData";
        private readonly string path;
        private readonly IDataSerializer serializer;
        public FileSaveSystem(string directory, IDataSerializer serializer) {
            path = Path.Combine(directory, FOLDER_NAME);
            this.serializer = serializer;
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }

        public void Save(string filename, object data) {
            var serializedData = serializer.Serialize(data);
            if (string.IsNullOrEmpty(serializedData))
                return;
            try {
                string location = Path.Combine(path, filename);
                File.WriteAllText(location, serializedData);
            }
            catch (Exception e) {
                throw new Exception("Error saving file: " + path, e);
            }
        }
        
        public bool Load(string filename, Type type, out object data) {
            string location = Path.Combine(path, filename);
            
            if (!File.Exists(location)){
                data = null;
                return false;
            }

            try {
                string unserializedData = File.ReadAllText(location);
                data = serializer.Deserialize(unserializedData, type);
                return true;
            }
            catch (Exception e) {
                throw new IOException("Unable to Deserialize data", e);
            }
        }
        
        // public T Load<T>(string filename) {
        //     string location = Path.Combine(path, filename);
        //     if (!File.Exists(location)) return default(T);
        //
        //     string unserializedData = File.ReadAllText(location);
        //     return (T)serializer.Deserialize(unserializedData, typeof(T));
        // }

        public void DeleteAll() {
            if (Directory.Exists(path)) {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }
    }
    
}