using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChestItemConfig", menuName = "ScriptableObjects/ChestItemConfig")]
    public class ChestItemConfig : BaseItemConfig
    {
        public List<ChestCapacityConfig> capacityConfigs;
    }
}
