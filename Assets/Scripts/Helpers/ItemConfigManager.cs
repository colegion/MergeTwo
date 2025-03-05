using System;
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

        private int _pendingLoads = 0; 

        private void Start()
        {
            _itemConfigs = new List<BaseItemConfig>();
            LoadItemConfigs();
        }

        private void LoadItemConfigs()
        {
            _pendingLoads = itemConfigReferences.Count;

            foreach (var reference in itemConfigReferences)
            {
                reference.LoadAssetAsync<BaseItemConfig>().Completed += OnItemConfigLoaded;
            }
        }

        private void OnItemConfigLoaded(AsyncOperationHandle<BaseItemConfig> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var itemConfig = handle.Result;
                itemConfig.Initialize();
                _itemConfigs.Add(itemConfig);
            }
            else
            {
                Debug.LogError("Failed to load ItemConfig.");
            }

            _pendingLoads--;

            if (_pendingLoads == 0)
            {
                ServiceLocator.Register(this);
                GameController.Instance.LoadLevel();
                Debug.LogWarning("All item configs loaded.");
            }
        }

        public BaseItemConfig GetItemConfig(ItemType itemType, int level)
        {
            var config = _itemConfigs.Find(c => c.itemType == itemType && c.step.level == level);
            return config;
        }
    }
}