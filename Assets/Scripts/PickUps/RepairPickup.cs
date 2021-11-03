using UnityEngine;

namespace XRLab.VRoem.Core
{ public class RepairPickup : Pickup
    {
        protected override void OnPickupTriggered()
        {
            Repair();
        }
    
        private void Repair() {
            Debug.Log("Repair Repair");
            AddPickupCount();
        }
    }
}

