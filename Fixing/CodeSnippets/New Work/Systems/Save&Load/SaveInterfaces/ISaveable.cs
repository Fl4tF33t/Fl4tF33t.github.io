namespace Systems {
    public interface ISaveable {
        public string UniqueID { get; }

        public object GetSaveData();
        public void LoadData(object data);
    }

}