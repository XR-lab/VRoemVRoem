using System;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class CollectableCounter : MonoBehaviour
    {
        private Dictionary<Pickup.PickupType, int> _pickupCountMap = new Dictionary<Pickup.PickupType, int>();
        private void Start()
        {
            //TODO: convert  to forloop 
            _pickupCountMap.Add(Pickup.PickupType.Repair, 0);
            _pickupCountMap.Add(Pickup.PickupType.Speedboost, 0);
        }

        public void AddPickupCount(Pickup.PickupType type)
        {
            if (!_pickupCountMap.ContainsKey(type))
            {
                Debug.Log("Pickup type is not added to the CollectableCounter");
                return;
            }

            _pickupCountMap[type]++;
        }

        public int GetTotalPickupCount()
        {
            int count = 0;

            foreach (KeyValuePair<Pickup.PickupType,int> pair in _pickupCountMap)
            {
                count += _pickupCountMap[pair.Key];
            }

            return count;
        }
    }
}


