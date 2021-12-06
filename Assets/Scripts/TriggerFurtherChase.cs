using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRLab.VRoem.Core;

public class TriggerFurtherChase : MonoBehaviour
{
    private PoliceManager _policeManager;
    private bool _done = false;

    // Start is called before the first frame update
    void Start()
    {
        _policeManager = FindObjectOfType<PoliceManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_done && other.attachedRigidbody.CompareTag(Tags.PLAYER))
        {
            _done = true;
            _policeManager.ActivateReplacements();
        }
    }
}
