using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class ObstacleTrigger : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        //Set the color to green when hitting the player
        ////private void OnTriggerEnter(Collider other)
        ////{
        ////    if (other.attachedRigidbody.CompareTag(Tags.PLAYER))
        ////    {
        ////        print("HIT");
        ////        //_renderer.material.SetColor("_Color", Color.green);
        ////       // FindObjectOfType<ObjectHitTracker>().objectHitCounter += 1;
        ////    }
        ////}
        private void OnCollisionEnter(Collision collision) 
        {
            if (collision.gameObject.CompareTag(Tags.PLAYER)) {
                Debug.LogError(collision.gameObject.tag);
                //_renderer.material.SetColor("_Color", Color.green);
                //FindObjectOfType<ObjectHitTracker>().objectHitCounter += 1;
            }
        }
    }
}
