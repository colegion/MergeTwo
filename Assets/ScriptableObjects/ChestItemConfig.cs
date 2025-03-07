using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChestItemConfig", menuName = "ScriptableObjects/ChestItemConfig")]
    public class ChestItemConfig : BaseItemConfig
    {
        public Sprite lockedSprite;
        public ChestCapacityConfig chestRewards;
        public float durationToUnlock;
    }
}
