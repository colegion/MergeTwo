using System;
using DG.Tweening;
using Helpers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrencyHUD : MonoBehaviour
    {
        [SerializeField] private ItemType currencyType;
        [SerializeField] private TextMeshProUGUI currencyAmountField;

        private int _currentAmount;
        
        public void SetCurrency(int amount)
        {
            currencyAmountField.text = $"{amount}";
            _currentAmount = amount;
        }

        public void IncreaseCurrency(int amount)
        {
            int targetAmount = _currentAmount + amount;
            DOTween.Kill(currencyAmountField);
            
            DOTween.To(() => _currentAmount, x =>
                {
                    _currentAmount = x;
                    currencyAmountField.text = _currentAmount.ToString();
                }, targetAmount, 0.5f)
                .SetEase(Ease.OutQuad);
        }

        public ItemType GetCurrencyType()
        {
            return currencyType;
        }
    }
}
