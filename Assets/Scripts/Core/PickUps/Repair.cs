using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class Repair : PickUp
    {
        //je hebt dit nodig om de pick up af te laten spelen
        public override void PowerUp()
        {
            repair();
            base.PowerUp();
        }
        private void repair()
        {
            Debug.Log("Repair Repair");
            counter.repairCount++;
        }
    }
}
