using Helpers;
using UnityEngine;

namespace ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "BaseItemConfig", menuName = "ScriptableObjects/BaseItemConfig")]
    public class BaseItemConfig : ScriptableObject
    {
        public string itemName;
        public ItemType itemType;
        public BaseStepConfig step;
        public BaseItemConfig nextItem;

        public void Initialize()
        {
            step.ItemType = itemType;
        }
    }
}