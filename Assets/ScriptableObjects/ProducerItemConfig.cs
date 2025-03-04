using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProducerItemConfig", menuName = "ScriptableObjects/ProducerItemConfig")]
    public class ProducerItemConfig : BaseItemConfig
    {
        public List<ProducerStepConfig> producerCapacity;
    }
}
