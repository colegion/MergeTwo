using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Helpers
{
    public class ItemConfigManager : MonoBehaviour
    {
        private List<BaseItemConfig> _itemConfigs;
        public List<AssetReference> itemConfigReferences;

        private void Start()
        {
            _itemConfigs = new List<BaseItemConfig>();
            
            LoadItemConfigs();
        }

        private void LoadItemConfigs()
        {
            foreach (var reference in itemConfigReferences)
            {
                reference.LoadAssetAsync<BaseItemConfig>().Completed += OnItemConfigLoaded;
                Debug.LogWarning("Loaded reference " + reference);
            }
        }
        
        private void OnItemConfigLoaded(AsyncOperationHandle<BaseItemConfig> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var itemConfig = handle.Result;
                Debug.LogWarning("Item config loaded: " + itemConfig);
                itemConfig.Initialize();
                _itemConfigs.Add(itemConfig);
            }
            else
            {
                Debug.LogError("Failed to load ItemConfig.");
            }
        }

        public BaseStepConfig GetItemConfig(ItemType itemType, int level)
        {
            var config = _itemConfigs.Find(c => c.itemType == itemType);
            return config.steps.Find(s => s.level == level);
        }
    }
}