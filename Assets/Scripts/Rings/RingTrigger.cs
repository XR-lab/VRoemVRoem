using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class RingTrigger : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        //Set the color to green when hitting the player
        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody.CompareTag(Tags.PLAYER))
            {
                _renderer.material.SetColor("_Color", Color.green);
            }
        }
    }
}
