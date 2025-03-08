using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChestItemConfig", menuName = "ScriptableObjects/ChestItemConfig")]
    public class ChestItemConfig : BaseItemConfig
    {
        public Sprite unlockedSprite;
        public RewardItemsContainer chestRewards;
        public float durationToUnlock;
    }
}
