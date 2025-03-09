using UnityEngine;

namespace Helpers
{
    public class CountdownTimer
    {
        private float _timeInSeconds;

        public CountdownTimer(int interval)
        {
            _timeInSeconds = interval;
        }

        private int GetMinutes()
        {
            return Mathf.FloorToInt(_timeInSeconds / 60);
        }

        private int GetSeconds()
        {
            return Mathf.FloorToInt(_timeInSeconds % 60);
        }
        
        public string GetFormattedTime()
        {
            int minutes = GetMinutes();
            int seconds = GetSeconds();
            return $"{minutes:D2}:{seconds:D2}";
        }
        
        public void SetTime(int seconds)
        {
            _timeInSeconds = seconds;
        }

    }
}