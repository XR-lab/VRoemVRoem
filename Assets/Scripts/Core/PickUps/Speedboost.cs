using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Vehicle;

namespace XRLab.VRoem.Core
{
    public class Speedboost : PickUp
    {
        private XRLab.VRoem.Vehicle.CarController _controller;
        private SpeedManager _speedManger;
        private CarController _carController;

        private void Start() {
            _controller = FindObjectOfType<XRLab.VRoem.Vehicle.CarController>();
            _speedManger = FindObjectOfType<SpeedManager>();
            _carController = FindObjectOfType<CarController>();
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
            _controller.BoostTime(3);
            _carController._carBoostAudio.Play();
            _controller._boosting = true;
            _speedManger.CalculateModifedSpeed(2.2f);
            counter.speedBoostCount++;

            Destroy(gameObject);
        }
    }
}
