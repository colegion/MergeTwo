using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProducerItemConfig", menuName = "ScriptableObjects/ProducerItemConfig")]
    public class ProducerItemConfig : BaseItemConfig
    {
        public bool canProduce;
        public ProducerStepConfig producerCapacity;
        public float durationForRecharge;
    }
}
