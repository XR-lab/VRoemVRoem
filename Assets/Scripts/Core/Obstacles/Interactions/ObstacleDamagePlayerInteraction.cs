using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamagePlayerInteraction : PlayerObstacleInteraction
{
    protected override void Interact(Collider collider)
    {
        collider.attachedRigidbody.GetComponent<CarDamage>().Damage();
    }
}
