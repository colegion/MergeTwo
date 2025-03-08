using System;
using Tile;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemInfoPlate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoField;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemLevel;

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
                ToggleFields(false);
            }
            else
            {
                ToggleFields(true);
                var config = tile.GetItemConfig();
                itemName.text = $"{config.step.level}";
            }
        }

        private void ToggleFields(bool value)
        {
            infoField.enabled = !value;
            itemName.enabled = value;
            itemLevel.enabled = value;
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
