using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstacleInteraction : MonoBehaviour
{
    private bool interacting = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!interacting && collision.gameObject.CompareTag("Player"))
        {
            interacting = true;
            Interact(collision);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!interacting && col.attachedRigidbody.CompareTag("Player"))
        {
            interacting = true;
            Interact(col);
        }
    }

    protected virtual void Interact(Collision collision)
    {
        Debug.Log("Interacting with Player");
    }

    protected virtual void Interact(Collider collider)
    {
        Debug.Log("Interacting with Player");
    }
}
