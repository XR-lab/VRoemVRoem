using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class Speedboost : PickUp
    {
        

        //je hebt dit nodig om de pick up af te laten spelen
        public override void PowerUp()
        {
            speedBoost();
            base.PowerUp();
        }

        private void speedBoost()
        {
            Debug.Log("Speed Speed");
            counter.speedBoostCount++;
        }
    }
}
