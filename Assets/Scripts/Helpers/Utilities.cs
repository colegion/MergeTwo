using System;
using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public class Utilities : MonoBehaviour
    {
        public static PoolableTypes GetPoolableType(ItemType type)
        {
            switch (type)
            {
                case ItemType.Coin : case ItemType.Energy:
                    return PoolableTypes.SpecialTile;
                case ItemType.Chest:
                    return PoolableTypes.Chest;
                case ItemType.VegetableProducer:
                    return PoolableTypes.Producer;
                default:
                    return PoolableTypes.BaseTile;
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
        SpecialTile,
        Chest,
        TrailObject,
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
    public class RewardEntry
    {
        public BaseItemConfig itemToProduce;
        public int produceCount;
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
    public class RewardItemsContainer
    {
        public List<RewardEntry> capacityConfigs;
    }
}
