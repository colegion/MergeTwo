using System;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjects.Orders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OrderUIHelper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI orderField;
        [SerializeField] private List<Image> orderImages;
        
        public void ConfigureSelf(OrderConfig config, int orderIndex)
        {
            ToggleImages(false);
            orderField.text = $"Order #{orderIndex}";
            orderField.gameObject.SetActive(true);
            var requests = config.requests;
            for (int i = 0; i < requests.Count; i++)
            {
                orderImages[i].sprite = requests[i].step.itemSprite;
                orderImages[i].enabled = true;
            }
        }

        public void OnOrderCompleted(Action onComplete)
        {
            transform.DOShakeScale(0.25f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        private void ToggleImages(bool toggle)
        {
            orderImages.ForEach(i => i.enabled = toggle);
        }

        public void DisableSelf()
        {
            ToggleImages(false);
            orderField.gameObject.SetActive(false);
        }
    }
}
