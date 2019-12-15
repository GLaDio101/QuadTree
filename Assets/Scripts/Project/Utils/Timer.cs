using JetBrains.Annotations;
using UnityEngine;

namespace Project.Utils
{
    public delegate void TimerEvent(Timer timer);

    public class Timer : MonoBehaviour
    {
        [UsedImplicitly]
        private void Awake()
        {
            IsCompleted = false;
            TimeElapsed = 0;
            IsRunning = false;
        }

        public TimerEvent OnTimerEnd { get; set; }

        public TimerEvent OnTimerTick { get; set; }

        public bool IsRunning;

        public float TimeElapsed;

        public bool IsCompleted;

        public float TotalTime;

        public void ResetTime()
        {
            TimeElapsed = 0;
            IsRunning = false;
            IsCompleted = false;
        }

        public void PauseTime()
        {
            IsRunning = false;
        }

        public void ResumeTime()
        {
            IsRunning = true;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (!IsRunning)
                return;

            TimeElapsed += Time.deltaTime;

            if(OnTimerTick != null)
                OnTimerTick(this);

            if (!(TimeElapsed >= TotalTime))
                return;

            TimeElapsed = 0;
            IsRunning = false;
            IsCompleted = true;

            if (OnTimerEnd != null)
                OnTimerEnd(this);
        }
    }
}
