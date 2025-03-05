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
        public BaseStepConfig step;
        public BaseItemConfig nextItem;

        public void Initialize()
        {
            
        }
    }
}