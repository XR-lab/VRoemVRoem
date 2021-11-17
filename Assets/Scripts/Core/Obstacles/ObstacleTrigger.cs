using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class ObstacleTrigger : MonoBehaviour
    {
        private ObjectHitTracker _hitTracker;

        private void Start()
        {
            _hitTracker = FindObjectOfType<ObjectHitTracker>();
        }
        
        private void OnTriggerEnter(Collider other) 
        {
            if (other.attachedRigidbody.CompareTag(Tags.PLAYER)) 
            {
                _hitTracker.objectHitCounter += 1;
            }
        }
    }
}
