namespace Utils {
    public abstract class Timer {
        protected readonly float initialTime;
        public delegate void TimerStart();
        public delegate void TimerStop();
        public delegate void TimerComplete();
        
        public event TimerStart OnStart;
        public event TimerStop OnStop;
        public event TimerComplete OnComplete;
        
        protected float Time { get; set; }
        public bool IsRunning { get; protected set; }
        public virtual float Progress => initialTime <= 0 ? 1f : Time / initialTime;

        protected Timer(float initialTime = 0) {
            this.initialTime = initialTime;
            Time = initialTime;
        }

        public void Start() {
            if (IsRunning) return;

            OnStart?.Invoke();
            IsRunning = true;
        }

        public void Stop() {
            if (!IsRunning) return;

            OnStop?.Invoke();
            IsRunning = false;
        }

        public abstract void Tick(float deltaTime);
        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;
        public void Complete() => OnComplete?.Invoke();
        public void Reset() => Time = initialTime;
    }

    public class CountdownTimer : Timer {
        public override float Progress => 1 - (Time / initialTime);

        public CountdownTimer(float initialTime = 0) : base(initialTime) {
        }

        public override void Tick(float deltaTime) {
            if (!IsRunning) return;

            Time -= deltaTime;
            if (Time <= 0) {
                Time = 0;
                Stop();
                Complete();
            } 
        }
    }

    public class StopWatchTimer : Timer {
        public override float Progress => Time / limit;
        private readonly float limit;

        public StopWatchTimer(float initialTime = 0, float limit = float.Epsilon) : base(initialTime) {
            this.limit = limit;
        }


        public override void Tick(float deltaTime) {
            if (!IsRunning) return;

            Time += deltaTime;
            if (Time >= limit) {
                Time = limit;
                Stop();
                Complete();
            }
        }
    }
}