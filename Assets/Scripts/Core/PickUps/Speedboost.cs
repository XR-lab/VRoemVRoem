using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class Speedboost : PickUp
    {
        private XRLab.VRoem.Vehicle.CarController _controller;
        private SpeedManager _speedManger;

        private void Start() {
            _controller = FindObjectOfType<XRLab.VRoem.Vehicle.CarController>();
            _speedManger = FindObjectOfType<SpeedManager>();
        }
        //je hebt dit nodig om de pick up af te laten spelen
        public override void PowerUp()
        {
            speedBoost();
            base.PowerUp();
        }

        private void speedBoost()
        {
            Debug.Log("Speed Speed");
            _controller._boosting = true;
            _speedManger.CalculateModifedSpeed(2);
            counter.speedBoostCount++;
        }
    }
}
