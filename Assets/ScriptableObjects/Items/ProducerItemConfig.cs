using Helpers;
using UnityEngine;

namespace ScriptableObjects.Items
{
    [CreateAssetMenu(fileName = "ProducerItemConfig", menuName = "ScriptableObjects/ProducerItemConfig")]
    public class ProducerItemConfig : BaseItemConfig
    {
        public bool canProduce;
        public int produceCost;
        public RewardItemsContainer producerCapacity;
        public int durationForRecharge;
    }
}
