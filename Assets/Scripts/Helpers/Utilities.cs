using System;
using System.Collections.Generic;
using ScriptableObjects;
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
        Producer,
        Chest
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
    public class ProducerCapacityConfig
    {
        public BaseItemConfig itemToProduce;
        public int produceCount;
    }

    [Serializable]
    public class ChestCapacityConfig
    {
        public List<ChestRewards> chestRewards;
    }

    [Serializable]
    public class ChestRewards
    {
        public BaseItemConfig rewardItem;
        public int rewardCount;
    }

    [Serializable]
    public class BaseStepConfig
    {
        public int level;
        public Sprite itemSprite;
        public bool isMaxLevel;
        private ItemType itemType;
        public ItemType ItemType { get; set; }
    }

    [Serializable]
    public class ProducerStepConfig
    {
        public List<ProducerCapacityConfig> capacityConfigs;
    }
}
