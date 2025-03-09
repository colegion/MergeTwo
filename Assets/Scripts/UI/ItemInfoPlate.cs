using System;
using Tile;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemInfoPlate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoField;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void HandleOnTap(BaseTile tile)
        {
            if (tile == null)
            {
                infoField.text = "Select item to see info";
            }
            else
            {
                var config = tile.GetItemConfig();
                infoField.text = $"{config.itemName} (Level: {config.step.level})";
            }
        }
        

        private void AddListeners()
        {
            GameController.OnUserTapped += HandleOnTap;
        }

        private void RemoveListeners()
        {
            GameController.OnUserTapped += HandleOnTap;
        }
    }
}
