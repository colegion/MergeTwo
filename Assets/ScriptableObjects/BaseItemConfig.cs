using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BaseItemConfig", menuName = "ScriptableObjects/BaseItemConfig")]
    public class BaseItemConfig : ScriptableObject
    {
        public ItemType itemType;
        public List<BaseStepConfig> steps;

        public void Initialize()
        {
            steps.ForEach(s => s.ItemType = itemType);
        }

        public BaseStepConfig GetStepByLevel(int level)
        {
            return steps.Find(s => s.level == level);
        }
    }
}