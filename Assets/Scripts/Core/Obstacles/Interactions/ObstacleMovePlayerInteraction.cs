using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Vehicle;

//TODO: Add Namespace
public class ObstacleMovePlayerInteraction : PlayerObstacleInteraction
{
    [SerializeField] private Vector2 _pos;
    [SerializeField] private bool _addCurrentPosition = false;

    protected override void Interact(Collider col)
    {
        Vector2 pos = _pos;

        if (_addCurrentPosition)
        {
            _pos += new Vector2(transform.position.x, transform.position.y);
        }

        Transform player = col.attachedRigidbody.transform;

        Vector3 teleportPos = _pos;
        teleportPos.z = player.position.z;

        player.transform.position = teleportPos;
    }
}
