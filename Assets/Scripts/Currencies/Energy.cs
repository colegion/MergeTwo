using System.Collections;
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
            
            if (_amount == 0) _amount = maxEnergy;
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
                    Save();
                }
            }
        }
    }
}