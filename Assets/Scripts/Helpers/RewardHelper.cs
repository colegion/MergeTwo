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
            foreach (var temp in _capacity)
            {
                _remainingCounts.TryAdd(temp.itemToProduce, temp.produceCount);
            }
        }

        public BaseItemConfig GetRandomItemToProduce()
        {
            var randomIndex = Random.Range(0, _capacity.Count);
            var produceItem = _capacity[randomIndex].itemToProduce;
            
            while (_remainingCounts[produceItem] == 0)
            {
                randomIndex = Random.Range(0, _capacity.Count);
                produceItem = _capacity[randomIndex].itemToProduce;
            }
            
            return produceItem;
        }

        public void DecreaseRemainingCount(BaseItemConfig spawnedItemConfig)
        {
            _remainingCounts[spawnedItemConfig]--;
        }

        public bool IsEmpty()
        {
            bool cooldownActive = true;
            foreach (var config in _remainingCounts)
            {
                if (config.Value > 0) 
                {
                    cooldownActive = false;
                    break;  
                }
            }
            return cooldownActive;
        }
    }
}
