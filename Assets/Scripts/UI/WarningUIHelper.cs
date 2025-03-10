using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WarningUIHelper : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private Transform target;
        [SerializeField] private TextMeshProUGUI warningField;
        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void ConfigureWarning(string info)
        {
            Reset();
            Sequence sequence = DOTween.Sequence();
            warningField.text = info;
            sequence.Append(warningField.DOFade(1, 0.15f).SetEase(Ease.OutBack));
            sequence.Join(warningField.transform.DOMove(target.position, 0.8f).SetEase(Ease.OutBack));
            sequence.OnComplete(Reset);
        }

        private void Reset()
        {
            warningField.text = "";
            warningField.transform.position = origin.position;
        }

        private void AddListeners()
        {
            GameController.OnWarningNeeded += ConfigureWarning;
        }

        private void RemoveListeners()
        {
            GameController.OnWarningNeeded += ConfigureWarning;
        }
    }
}
