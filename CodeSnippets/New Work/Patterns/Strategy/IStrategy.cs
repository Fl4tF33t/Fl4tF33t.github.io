namespace Patterns {
    public interface IStrategy<out T> {
        public T Execute();
    }
}