using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;

namespace Helpers
{
    public class RewardHelper
    {
        private readonly List<RewardEntry> _capacity;
        private readonly Dictionary<BaseItemConfig, int> _remainingCounts = new Dictionary<BaseItemConfig, int>();

        public RewardHelper(List<RewardEntry> capacity)
        {
            _capacity = capacity;
            PopulateCapacities();
        }
        
        public void PopulateCapacities()
        {
            _remainingCounts.Clear();
            foreach (var entry in _capacity)
            {
                _remainingCounts[entry.itemToProduce] = entry.produceCount;
            }
        }

        public BaseItemConfig GetRandomItemToProduce()
        {
            var availableItems = GetAvailableItems();

            if (availableItems.Count == 0)
            {
                Debug.LogWarning("No available items to produce!");
                return null;
            }

            int randomIndex = Random.Range(0, availableItems.Count);
            return availableItems[randomIndex];
        }

        public void DecreaseRemainingCount(BaseItemConfig itemConfig)
        {
            if (!_remainingCounts.ContainsKey(itemConfig))
            {
                Debug.LogError($"Item {itemConfig.name} not found in remaining counts.");
                return;
            }

            _remainingCounts[itemConfig] = Mathf.Max(0, _remainingCounts[itemConfig] - 1);
        }

        public bool IsEmpty()
        {
            foreach (var count in _remainingCounts.Values)
            {
                if (count > 0)
                    return false;
            }
            return true;
        }

        private List<BaseItemConfig> GetAvailableItems()
        {
            var availableItems = new List<BaseItemConfig>();

            foreach (var pair in _remainingCounts)
            {
                if (pair.Value > 0)
                {
                    availableItems.Add(pair.Key);
                }
            }

            return availableItems;
        }
    }
}
