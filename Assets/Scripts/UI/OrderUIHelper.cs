using System.Collections.Generic;
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
            var requests = config.requests;
            for (int i = 0; i < requests.Count; i++)
            {
                orderImages[i].sprite = requests[i].step.itemSprite;
                orderImages[i].enabled = true;
            }
        }

        private void ToggleImages(bool toggle)
        {
            orderImages.ForEach(i => i.enabled = toggle);
        }
    }
}
