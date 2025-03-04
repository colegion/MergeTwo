using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public class Utilities : MonoBehaviour
    {
        public static int GetMaxLevelByType(ItemType type)
        {
            switch (type)
            {
                case ItemType.Coin: case ItemType.Energy:
                    return 3;
                default: return 5;
            }
        }
    }

    [Serializable]
    public enum ItemType
    {
        Pasta,
        Pizza,
        Wine,
        Steak,
        Pie,
        Energy,
        Coin,
        VegetableProducer,
        Chest
    }

    public enum PoolableTypes
    {
        BaseTile,
        
    }

    [Serializable]
    public class LevelData
    {
        public int boardWidth;
        public int boardHeight;
        public List<TileData> tiles;
        
    }

    [Serializable]
    public class TileData
    {
        public int xCoord;
        public int yCoord;
        public ItemType itemType;
        public int itemLevel;
    }

    [Serializable]
    public class CapacityConfig
    {
        public int stepLevel;
        public List<ItemType> types;
        public List<CapacityEntry> capacityEntries;
        public float durationForRecharge;
    }

    [Serializable]
    public class ChestCapacityConfig
    {
        public int stepLevel;
        public List<ChestCapacityEntry> chestCapacityEntries;
        public float durationToUnlock;
    }

    [Serializable]
    public class StepConfig
    {
        public int level;
        public Sprite itemSprite;
        public bool isMaxLevel;
    }

    [Serializable]
    public class ProducerStepConfig
    {
        public List<CapacityConfig> capacityConfigs;
    }

    [Serializable]
    public class SpecialStepConfig
    {
        public List<int> rewardAmount;
    }

    [Serializable]
    public class CapacityEntry
    {
        public int itemLevel;
        public int itemCount;
    }

    [Serializable]
    public class ChestCapacityEntry : CapacityEntry
    {
        public ItemType itemType;
    }
    
}
