using System;
using System.Collections;
using Currencies;
using Helpers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimerUIHelper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timer;
        private CountdownTimer _countdown;
        private int _interval;
        
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void InitializeTimer(int time)
        {
            _interval = time;
            _countdown = new CountdownTimer(_interval);
            StartCoroutine(ProcessInterval());
        }

        private IEnumerator ProcessInterval()
        {
            var temp = _interval;
            while (true)
            {
                while (temp > 0)
                {
                    timer.text = _countdown.GetFormattedTime();
                    temp--;
                    _countdown.SetTime(temp);
                    yield return new WaitForSeconds(1f);
                }

                temp = _interval;
            }
            
            yield return null;
        }

        private void AddListeners()
        {
            Energy.OnRechargeBegin += InitializeTimer;
        }

        private void RemoveListeners()
        {
            Energy.OnRechargeBegin -= InitializeTimer;
        }
    }
}
