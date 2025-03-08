using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects.Orders
{
    [CreateAssetMenu(fileName = "OrderConfig", menuName = "ScriptableObjects/OrderConfig")]
    public class OrderConfig : ScriptableObject
    {
        public List<BaseItemConfig> requests;
        public bool hasCompleted;
        public int rewardAmount;
    }
}
