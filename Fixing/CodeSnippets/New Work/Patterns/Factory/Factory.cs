namespace Patterns {
    public abstract class Factory<T> : IFactory<T> {
        public abstract T Create();
    }
}

