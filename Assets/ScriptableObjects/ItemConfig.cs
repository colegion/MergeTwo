using Helpers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObjects/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        public ItemType itemType;
        public int level;
        public int maxLevel;
        public Sprite itemSprite;
        public bool IsMaxLevel => level >= maxLevel;
        public ItemConfig nextLevelItem;
        
        public ItemConfig GetNextLevel()
        {
            return IsMaxLevel ? null : nextLevelItem;
        }
    }
}