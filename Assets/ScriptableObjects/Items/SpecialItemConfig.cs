using Helpers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpecialItemConfig", menuName = "ScriptableObjects/SpecialItemConfig")]
    public class SpecialItemConfig : BaseItemConfig
    {
        public int rewardAmount;
    }
}
