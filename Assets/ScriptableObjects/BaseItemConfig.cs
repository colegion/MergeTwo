using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BaseItemConfig", menuName = "ScriptableObjects/BaseItemConfig")]
    public class BaseItemConfig : ScriptableObject
    {
        public ItemType itemType;
        public int level;
        public int maxLevel;
        public Sprite itemSprite;
        public bool IsMaxLevel => level >= maxLevel;
        public BaseItemConfig nextLevelItem;
        
        public BaseItemConfig GetNextLevel()
        {
            return IsMaxLevel ? null : nextLevelItem;
        }
        
        public bool IsIdentical(BaseItemConfig other) => itemType == other.itemType && level == other.level;
    }
}