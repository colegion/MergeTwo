using System.Collections;
using UI;
using UnityEngine;

namespace Currencies
{
    public class Energy : Currency
    {
        [SerializeField] private int maxEnergy = 100;
        [SerializeField] private int rechargeTime = 60;

        protected override void Start()
        {
            _saveKey = "energy";
            base.Start();
            
            StartCoroutine(RegenerateEnergy());
        }

        private IEnumerator RegenerateEnergy()
        {
            while (true)
            {
                yield return new WaitForSeconds(rechargeTime);
                
                if (_amount < maxEnergy)
                {
                    _amount++;
                    currencyField.SetCurrency(_amount);
                    Save();
                }
            }
        }
        
        protected override void Load()
        {
            _amount = PlayerPrefs.GetInt(_saveKey, maxEnergy);
        }
    }
}