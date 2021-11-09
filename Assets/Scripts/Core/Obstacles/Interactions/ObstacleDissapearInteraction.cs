using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDissapearInteraction : PlayerObstacleInteraction
{
    protected override void Interact(Collider collider)
    {
        Destroy(gameObject);
    }
}
