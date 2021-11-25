using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{


    //TODO: Add namespace
    public class ObstacleDamagePlayerInteraction : PlayerObstacleInteraction
    {
        ObjectHitTracker _hitTracker;
        private void Start()
        {
            _hitTracker = FindObjectOfType<ObjectHitTracker>();
        }


        protected override void Interact(Collider collider)
        {
            collider.attachedRigidbody.GetComponent<CarDamage>().Damage();

            MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;

            _hitTracker.objectHitCounter += 1;
        }
    }
}
