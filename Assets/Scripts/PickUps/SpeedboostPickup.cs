using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class SpeedboostPickup : Pickup
    {
        protected override void OnPickupTriggered()
        {
            SpeedBoost();
        }
    
        private void SpeedBoost() {
            Debug.Log("Speed Speed");
            AddPickupCount();
        }
    }
}


