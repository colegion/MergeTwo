using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Helpers
{
    public class ItemConfigManager : MonoBehaviour
    {
        // A dictionary to map (ItemType, level) to the corresponding ItemConfig
        private Dictionary<(ItemType, int), ItemConfig> itemConfigLookup;

        // List of Addressable Asset references to ItemConfig objects
        public List<AssetReference> itemConfigReferences;

        private void Start()
        {
            // Initialize the dictionary
            itemConfigLookup = new Dictionary<(ItemType, int), ItemConfig>();

            // Load all ItemConfig assets asynchronously using Addressables
            LoadItemConfigs();
        }

        private void LoadItemConfigs()
        {
            // Start loading all ItemConfig assets asynchronously
            foreach (var reference in itemConfigReferences)
            {
                reference.LoadAssetAsync<ItemConfig>().Completed += OnItemConfigLoaded;
            }
        }

        // Callback when an ItemConfig is loaded
        private void OnItemConfigLoaded(AsyncOperationHandle<ItemConfig> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var itemConfig = handle.Result;

                // Add the loaded ItemConfig to the dictionary
                itemConfigLookup[(itemConfig.itemType, itemConfig.level)] = itemConfig;

                // Optionally log the loading process for debugging purposes
                Debug.Log($"Loaded ItemConfig: {itemConfig.itemType}, Level: {itemConfig.level}");
            }
            else
            {
                Debug.LogError("Failed to load ItemConfig.");
            }
        }

        // Retrieve an ItemConfig based on ItemType and Level
        public ItemConfig GetItemConfig(ItemType itemType, int level)
        {
            return itemConfigLookup.GetValueOrDefault((itemType, level)); 
        }
    }
}