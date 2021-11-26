using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    //TODO: Add namespace
    public class ObstacleDamagePlayerInteraction : PlayerObstacleInteraction
    {
        [SerializeField] private float _speedLossMultiplier = 0;

        protected override void Interact(Collider collider)
        {
            collider.attachedRigidbody.GetComponent<CarDamage>().Damage(_speedLossMultiplier);

            MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;            
        }
    }
}
