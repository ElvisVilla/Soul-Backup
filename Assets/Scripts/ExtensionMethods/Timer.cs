using System;
using System.Collections;
using UnityEngine;

namespace Bissash
{
    public class Timer
    {
        private float seconds;
        private float currentSeconds;

        private bool performOnce;
        public event Action OnceTimerEndEvent;

        public Timer Tick(float secondsToRun, bool reset = false)
        {
            if (seconds != secondsToRun)
                seconds = secondsToRun;

            currentSeconds -= Time.deltaTime;

            if (IsTimeComplete() && OnceTimerEndEvent != null)
            {
                OnceTimerEndEvent();

                if (reset == true)
                    ResetTimer();
            }

            return this;
        }

        /// <summary>
        /// Retorna los segundos actuales en el temporizador.
        /// </summary>
        /// <returns></returns>
        public float GetCurrentSeconds()
        {
            return currentSeconds;
        }

        /// <summary>
        /// Reinicia los segundos transcurridos en el timer.
        /// </summary>
        public void ResetTimer()
        {
            currentSeconds = seconds;
        }

        /// <summary>
        /// Retorna true si el tiempo transcurrido sobre pasa el limite de tiempo.
        /// </summary>
        /// <returns></returns>
        public bool IsTimeComplete()
        {
            return currentSeconds <= 0;
        }

        public void OnCompleteCallback(Action action)
        {
            if (IsTimeComplete())
                action?.Invoke();
        }

        public void OnTimerEnd(float seconds, Action action)
        {
            Tick(seconds);
            if (IsTimeComplete())
            {
                action();
                ResetTimer();
            }
        }
    }
}



