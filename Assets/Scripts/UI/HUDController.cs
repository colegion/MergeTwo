using System.Collections.Generic;
using Helpers;
using Pool;
using UnityEngine;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private List<CurrencyHUD> currencies;

        private PoolController _poolController;
        private ItemConfigManager _itemConfigManager;
        private const int SpritePlaceHolderConfigLevel = 0;
        public void IncreaseCurrency(ItemType type, int amount)
        {
            var target = currencies.Find(c => c.GetCurrencyType() == type);
            
            target.IncreaseCurrency(amount);
        }

        private void AnimateCurrencyClaim(ItemType type, int amount)
        {
            if (_poolController == null) _poolController = ServiceLocator.Get<PoolController>();
            if (_itemConfigManager == null) _itemConfigManager = ServiceLocator.Get<ItemConfigManager>();
            var item = _itemConfigManager.GetItemConfig(type, SpritePlaceHolderConfigLevel);

            for (int i = 0; i < amount; i++)
            {
                var tempTrail = _poolController.GetPooledObject(PoolableTypes.TrailObject);
                
            }
        }
    }
}
