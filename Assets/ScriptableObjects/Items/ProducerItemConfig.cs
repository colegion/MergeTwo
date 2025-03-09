using System.Collections.Generic;
using Helpers;
using ScriptableObjects.Items;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProducerItemConfig", menuName = "ScriptableObjects/ProducerItemConfig")]
    public class ProducerItemConfig : BaseItemConfig
    {
        public bool canProduce;
        public int produceCost;
        public RewardItemsContainer producerCapacity;
        public float durationForRecharge;
    }
}
