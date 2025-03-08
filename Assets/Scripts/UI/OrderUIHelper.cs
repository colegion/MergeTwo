using System.Collections.Generic;
using ScriptableObjects.Orders;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OrderUIHelper : MonoBehaviour
    {
        [SerializeField] private List<Image> orderImages;
        
        public void ConfigureSelf(OrderConfig config)
        {
            var requests = config.requests;
            for (int i = 0; i < requests.Count; i++)
            {
                orderImages[i].sprite = requests[i].step.itemSprite;
            }
        }
    }
}
