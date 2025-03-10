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
        [SerializeField] private Transform target;
        [SerializeField] private TextMeshProUGUI currencyAmountField;
        
        private int _currentAmount;

        public void SetCurrency(int amount)
        {
            currencyAmountField.text = $"{amount}";
            _currentAmount = amount;
        }

        public void IncreaseCurrency(int amount)
        {
            Sequence sequence = DOTween.Sequence();
            int targetAmount = _currentAmount + amount;
            DOTween.Kill(currencyAmountField);
            
            sequence.Append(DOTween.To(() => _currentAmount, x =>
                {
                    _currentAmount = x;
                    currencyAmountField.text = _currentAmount.ToString();
                }, targetAmount, 0.15f)
                .SetEase(Ease.OutQuad));

            sequence.Join(target.DOPunchScale(new Vector3(1.04f, 1.04f, 1.04f), 0.15f).SetEase(Ease.OutQuad));
        }

        public ItemType GetCurrencyType()
        {
            return currencyType;
        }

        public Transform GetTarget()
        {
            return target;
        }
    }
}
