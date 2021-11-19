using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Add namespace
public class ObstacleDamagePlayerInteraction : PlayerObstacleInteraction
{
    protected override void Interact(Collider collider)
    {
        collider.attachedRigidbody.GetComponent<CarDamage>().Damage();

        MoneySystem.currentMonney = MoneySystem.currentMonney - MoneySystem.MonneyToLose;
    }
}
