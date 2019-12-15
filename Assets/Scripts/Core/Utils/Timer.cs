using JetBrains.Annotations;
using UnityEngine;

namespace Core.Utils
{
    public enum TimerEvent
    {
        End,
        Tick,
    }

    public class Timer : MonoBehaviour
    {
        public delegate void ChangeState();

        [UsedImplicitly]
        private void Awake()
        {
            IsCompleted = false;
            TimeElapsed = 0;
            IsRunning = false;
        }

        public event ChangeState EndEvent;

        public event ChangeState TickEvent;

        public bool IsRunning;

        public float TimeElapsed;

        public bool IsCompleted;

        public float TotalTime;

        public float FinishTime;

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
            FinishTime += Time.deltaTime;

            if (TickEvent != null)
                TickEvent();

            if (!(TimeElapsed >= TotalTime))
                return;

            TimeElapsed = 0;
            IsRunning = false;
            IsCompleted = true;

            if (EndEvent != null)
                EndEvent();
        }
    }
}
