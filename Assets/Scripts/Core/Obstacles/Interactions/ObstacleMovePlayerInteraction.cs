using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Vehicle;

//TODO: Add Namespace
public class ObstacleMovePlayerInteraction : PlayerObstacleInteraction
{
    [SerializeField] private float _posX = 1;
    [SerializeField] private bool _addCurrentPositionX = false;

    protected override void Interact(Collider col)
    {
        float x = _posX;

        if (_addCurrentPositionX)
        {
            x += transform.position.x;
        }

        col.attachedRigidbody.GetComponent<SimpleMovementCar>().SetOverridenTargetPoint(x);
    }
}
