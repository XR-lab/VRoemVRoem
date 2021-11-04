using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRLab.VRoem.Core
{
    public class PickUp : MonoBehaviour
    {
        private GameObject _counterObject;
        protected CollectableCounter counter;
        private void Awake()
        {
            _counterObject = GameObject.FindWithTag("Counter");
            if (_counterObject == null)
            {
                Debug.LogError("Noo bad cant find counterobject");
                return;
            }

            counter = _counterObject.GetComponent<CollectableCounter>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PowerUp();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PowerUp();
                counter.totaalCount++;
                // destroy pick up
                Destroy(this);
            }
        }

        public virtual void PowerUp()
        {
            Debug.Log("Knak");
        }

    }
}
