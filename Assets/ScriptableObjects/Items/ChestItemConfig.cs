using Helpers;
using UnityEngine;

namespace ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "ChestItemConfig", menuName = "ScriptableObjects/ChestItemConfig")]
    public class ChestItemConfig : BaseItemConfig
    {
        public Sprite unlockedSprite;
        public RewardItemsContainer chestRewards;
        public int durationToUnlock;
    }
}
