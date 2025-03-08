using UI;
using UnityEngine;

namespace Currencies
{
    public abstract class Currency : MonoBehaviour
    {
        [SerializeField] protected CurrencyHUD currencyField;
        protected int _amount;
        protected string _saveKey;

        protected virtual void Start()
        {
            Load();
            currencyField.SetCurrency(_amount);
        }

        public void SetAmount(int amount)
        {
            _amount = amount;
            Save();
        }

        public void Increment()
        {
            _amount++;
            Save();
        }

        public void Decrement()
        {
            if (_amount > 0)
            {
                _amount--;
                Save();
            }
        }

        public void IncreaseByAmount(int amount)
        {
            _amount += amount;
            Save();
        }

        public int GetAmount()
        {
            return _amount;
        }

        protected void Save()
        {
            PlayerPrefs.SetInt(_saveKey, _amount);
            PlayerPrefs.Save();
        }

        protected virtual void Load()
        {
            _amount = PlayerPrefs.GetInt(_saveKey, 0);
        }
    }
}