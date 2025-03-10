using System;
using Helpers;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;

namespace Tile
{
    public class SpecialTile : BaseTile
    {
        private SpecialItemConfig _config;

        public static event Action<ItemType, int> OnCurrencyGathered;
        public override void ConfigureSelf(BaseItemConfig config, int x, int y)
        {
            base.ConfigureSelf(config, x, y);
            _config = (SpecialItemConfig)config;
        }
    
        public override void OnTap()
        {
            OnCurrencyGathered?.Invoke(_config.itemType, _config.rewardAmount);
            PlayerInventory.Instance.IncreaseCurrency(_config.itemType, _config.rewardAmount);
            GameController.Instance.ReturnPoolableToPool(this);
        }
        
    }
}
