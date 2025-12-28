namespace Utils {
    public class FPSCounter {
        private float timer;
        private int frames;

        public float FPS { get; private set; }

        public void Update(float deltaTime) {
            frames++;
            timer += deltaTime;

            if (timer >= 1f) {
                FPS = frames;
                frames = 0;
                timer = 0f;
            }
        }
    }
}