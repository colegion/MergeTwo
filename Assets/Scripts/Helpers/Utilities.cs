using System;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class Utilities : MonoBehaviour
    {
        
    }

    [Serializable]
    public enum ItemType
    {
        Food,
        Construction,
        Energy,
        Coin,
        Producer,
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
        public ItemType itemType;
        public int itemLevel;
        public int itemCount;
    }
}
