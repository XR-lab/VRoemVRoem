using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

//TODO: Add namespace, fix const stings. Colision Enter And Trigger??
public class PlayerObstacleInteraction : MonoBehaviour
{
    private bool _interacting = false;
    private bool _interactingTrigger = false;
    private bool _interactedWithPolice = false;
    private bool _hasBreakInteraction = false;

    private void Start()
    {
        _hasBreakInteraction = GetComponent<ObstacleBreakInteraction>() != null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_interacting && collision.gameObject.CompareTag("Player"))
        {
            _interacting = true;
            Interact(collision);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!_interactingTrigger && col.attachedRigidbody != null && col.attachedRigidbody.CompareTag("Player"))
        {
            _interactingTrigger = true;
            Interact(col);
        }

        if (!_interactedWithPolice && !_interacting && col.gameObject.layer == 12)
        {
            _interactedWithPolice = true;
            _interacting = true;
            _interactingTrigger = true;
            RunOver();
        }
    }

    protected virtual void Interact(Collision collision)
    {
        Debug.Log("Interacting with " + collision.gameObject.name);
    }

    protected virtual void Interact(Collider collider)
    {
        Debug.Log("Interacting with " + collider.gameObject.name);
    }

    protected virtual void RunOver()
    {
        Debug.Log("Running over with a police car!");
    }
}
